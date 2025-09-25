using LiteNetLib;
using LiteNetLib.Utils;
using System.Text.Json;

namespace Pong;

public class Server
{
    private NetManager? server;

    //Dictionary<string, NetPeer> peers = new Dictionary<string, NetPeer>();

    private EventBasedNetListener? listener;
    private Thread? serverThread;
    private bool isRunning = false;
    private string? key;
    private int maxPeers;
    readonly float moveSpeed = 500f;
    long lastMilliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    long lastScoreMilliseconds = 0;
    private int right = 1;
    private int top = 1;
    private readonly int ballMoveSpeed = 200;
    private readonly GameState gameState = new();

    public void StartAsHost(int port, int maxPeers, string key)
    {
        this.maxPeers = maxPeers;
        this.key = key;
        listener = new EventBasedNetListener();

        // Set up event handlers for connection/data
        listener.ConnectionRequestEvent += OnConnectionRequest;
        listener.PeerConnectedEvent += OnPeerConnected;
        listener.NetworkReceiveEvent += OnNetworkReceive;
        listener.PeerDisconnectedEvent += OnPeerDisconnected;

        server = new NetManager(listener)
        {
            UnsyncedEvents = true
        };

        // Start the server manager
        server.Start(port);
        isRunning = true;

        // Create and start the new thread for the server's polling loop
        serverThread = new Thread(new ThreadStart(ServerLoop))
        {
            IsBackground = true // Ensures thread closes with the main app
        };
        serverThread.Start();
    }

    private void ServerLoop()
    {
        Console.WriteLine("ServerLoop(): Server polling");
        // This is the server's polling loop, which runs continuously on its own thread.
        while (isRunning)
        {
            server?.PollEvents();
            Thread.Sleep(15); // Adjust sleep time to control CPU usage.
            MoveBall();

            NetDataWriter writer = new NetDataWriter();
            string jsonString = JsonSerializer.Serialize(this.gameState);
            writer.Put(jsonString);
            if (server != null)
            {
                foreach (NetPeer peer in server.ConnectedPeerList)
                {
                    peer.Send(writer, DeliveryMethod.ReliableOrdered);
                }
            }
        }
    }

    private void MoveBall()
    {
        if (server?.ConnectedPeersCount < 2)
        {
            return;
        }
        long nowMilliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        long elapsedMilliseconds = nowMilliseconds - lastMilliseconds;
        lastMilliseconds = nowMilliseconds;
        //int deltaSpeed = (int)(ballMoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
        int deltaSpeed = (int)(ballMoveSpeed * (float)elapsedMilliseconds/1000);
        gameState.BallX += right * deltaSpeed;
        gameState.BallY += top * deltaSpeed;

        int leftPaddleRightX = Paddle.leftPaddleX + 20;
        int leftPaddleTopY = gameState.GetPaddleY("left");
        int leftPaddleBottomY = gameState.GetPaddleY("left") + Paddle.sizeY;
        int ballLeftX = gameState.BallX - 20;
        int ballTopY = gameState.BallY - 20;
        int ballBottomY = gameState.BallY + 20;
        //if (player1.rect.Right > rect.Left && rect.Top > player1.rect.Top && rect.Bottom < player1.rect.Bottom)
        if (leftPaddleRightX > ballLeftX && ballTopY > leftPaddleTopY && ballBottomY < leftPaddleBottomY)
        {
            right = 1;
        }

        int rightPaddleLeftX = Paddle.rightPaddleX - 20;
        int rightPaddleTopY = gameState.GetPaddleY("right");
        int rightPaddleBottomY = gameState.GetPaddleY("right") + Paddle.sizeY;
        int ballRightX = gameState.BallX + 20;
        //if (player2.rect.Left < rect.Right && rect.Top > player2.rect.Top && rect.Bottom < player2.rect.Bottom)
        if (rightPaddleLeftX < ballRightX && ballTopY > rightPaddleTopY && ballBottomY < rightPaddleBottomY)
        {
            right = -1;
        }

        if (gameState.BallY < 0)
        {
            top *= -1;
        }

        int height = 40;
        if (gameState.BallY > Globals.HEIGHT - height)
        {
            top *= -1;
        }

        if (lastScoreMilliseconds == 0)
        {
            lastScoreMilliseconds = nowMilliseconds;
        }
        long ellapsedSinceLastScore = nowMilliseconds - lastScoreMilliseconds;
        if (gameState.BallX < 0)
        {
            gameState.BallX = Globals.WIDTH / 2 - 20;
            gameState.BallY = Globals.HEIGHT / 2 - 20;
            if (ellapsedSinceLastScore > 0)
            {
                gameState.RightScore += 1;
                lastScoreMilliseconds = nowMilliseconds;
                Console.WriteLine("MoveBall(): right scored");
            }    
        }

        int width = 40;
        if (gameState.BallX > Globals.WIDTH - width)
        {
            gameState.BallX = Globals.WIDTH / 2 - 20;
            gameState.BallY = Globals.HEIGHT / 2 - 20;
            if (ellapsedSinceLastScore > 0)
            {
                gameState.LeftScore += 1;
                lastScoreMilliseconds = nowMilliseconds;
                Console.WriteLine("MoveBall(): left scored");
            }

        }
        
    }


    private void StopServer()
    {
        isRunning = false;
        if (serverThread != null && serverThread.IsAlive)
        {
            serverThread.Join(); // Wait for the server thread to finish gracefully
        }
        server?.Stop();
    }

    private void Update()
    {
        server?.PollEvents();
    }

    private void OnDestroy()
    {
        server?.Stop();
    }

    // --- LiteNetLib Event Handlers ---
    private void OnConnectionRequest(ConnectionRequest request)
    {
        Console.WriteLine($"OnConnectionRequest(): Incoming connection request to Server from: {request.RemoteEndPoint}");
        // In a real application, you would add validation here.
        if (server?.ConnectedPeersCount < maxPeers)
        {
            request.AcceptIfKey(this.key);
            Console.WriteLine("OnConnectionRequest(): connection accepted by Server");
        }
        else
        {
            request.Reject();
            Console.WriteLine("OnConnectionRequest(): connection rejected by Server");
        }
    }

    private void OnPeerConnected(NetPeer peer)
    {
        Console.WriteLine($"OnPeerConnected(): Peer connected to Server: {peer.Address}");
    }

    private void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
    {
        var jsonString = reader.GetString();
        reader.Recycle(); // Free up the data reader
        Pong.PongAction? action =
                JsonSerializer.Deserialize<Pong.PongAction>(jsonString);
        if (action == null || (action != null && !"move".Equals(action.Type)))
            Console.WriteLine($"OnNetworkReceive(): Server [Received] from {peer.Address}: {jsonString}");
        if ("left".Equals(action?.ClientIdentifier) &&
            "up".Equals(action.Direction) &&
            gameState.LeftPaddleTopY > 0 &&
            action.GameTime != null
           )
        {
            gameState.LeftPaddleTopY -= (int)(moveSpeed * (float)action.GameTime.ElapsedGameTime.TotalSeconds);
            if (gameState.LeftPaddleTopY < 0)
                gameState.LeftPaddleTopY = 0;
        }
        if ("left".Equals(action?.ClientIdentifier) &&
            "down".Equals(action.Direction) &&
            gameState.LeftPaddleTopY < Globals.HEIGHT - Paddle.sizeY &&
            action.GameTime != null
           )
        {
            gameState.LeftPaddleTopY += (int)(moveSpeed * (float)action.GameTime.ElapsedGameTime.TotalSeconds);
            if (gameState.LeftPaddleTopY > Globals.HEIGHT - Paddle.sizeY)
            {
                gameState.LeftPaddleTopY = Globals.HEIGHT - Paddle.sizeY;
            }
        }
        if ("right".Equals(action?.ClientIdentifier) &&
            "up".Equals(action.Direction) &&
            gameState.LeftPaddleTopY > 0 &&
            action.GameTime != null
           )
        {
            gameState.RightPaddleTopY -= (int)(moveSpeed * (float)action.GameTime.ElapsedGameTime.TotalSeconds);
            if (gameState.RightPaddleTopY < 0)
                gameState.RightPaddleTopY = 0;
        }
        if ("right".Equals(action?.ClientIdentifier) &&
            "down".Equals(action.Direction) &&
            gameState.LeftPaddleTopY < Globals.HEIGHT - Paddle.sizeY &&
            action.GameTime != null
           )
        {
            gameState.RightPaddleTopY += (int)(moveSpeed * (float)action.GameTime.ElapsedGameTime.TotalSeconds);
            if (gameState.RightPaddleTopY > Globals.HEIGHT - Paddle.sizeY)
            {
                gameState.RightPaddleTopY = Globals.HEIGHT - Paddle.sizeY;
            }
        }
    }

    private void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        Console.WriteLine($"OnPeerDisconnected(): Peer disconnected: {peer.Address} from Server. Reason: {disconnectInfo.Reason}");
    }

}


