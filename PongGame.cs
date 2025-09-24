using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Color = Microsoft.Xna.Framework.Color;
using System.Text.Json;

namespace Pong;

public class PongGame : Game
{
    private GraphicsDeviceManager _graphics;
    private readonly IntPtr drawSurface;
    Paddle? leftPaddle;
    Paddle? rightPaddle;
    public Client? Client { get; set; }
    Ball ball;
    SpriteFont? font;

    public PongGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = Globals.WIDTH;
        _graphics.PreferredBackBufferHeight = Globals.HEIGHT;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        ball = new Ball(this);
    }

    public PongGame(IntPtr drawSurface) : this()
    {
        this.drawSurface = drawSurface;
        _graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);
        Control? control = Control.FromHandle(Window.Handle);
        if (control != null)
        {
            control.VisibleChanged += new EventHandler(PongGame_VisibleChanged);
        }
        

    }

    public void CreateLeftPaddle()
    {
        leftPaddle = new Paddle(this, "left");
    }

    public void CreateRightPaddle()
    {
        rightPaddle = new Paddle(this, "right");
    }

    void graphics_PreparingDeviceSettings(object? sender, PreparingDeviceSettingsEventArgs e)
    {
        e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = drawSurface;
    }

    private void PongGame_VisibleChanged(object? sender, EventArgs e)
    {
        Control? control = Control.FromHandle(Window.Handle);
        if (control != null && control.Visible == true)
        {
            control.Visible = false;
        }
    }

    protected override void Initialize()
    {
        // Add your initialization logic here
        base.Initialize();
    }

    protected override void LoadContent()
    {
        Globals.spriteBatch = new SpriteBatch(GraphicsDevice);

        Globals.pixel = new Texture2D(GraphicsDevice, 1, 1);
        Globals.pixel.SetData<Microsoft.Xna.Framework.Color>(new Microsoft.Xna.Framework.Color[] { Microsoft.Xna.Framework.Color.White });
        font = Content.Load<SpriteFont>("Score");
    }


    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        leftPaddle?.Update(gameTime);
        rightPaddle?.Update(gameTime);
        if (leftPaddle != null && rightPaddle != null)
            ball.Update(gameTime, leftPaddle, rightPaddle);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        Globals.spriteBatch?.Begin();
        // TODO: Add your drawing code here     
        Globals.spriteBatch?.DrawString(font, Client?.GameState.LeftScore.ToString(), new Vector2(100, 50), Color.White);
        Globals.spriteBatch?.DrawString(font, Client?.GameState.RightScore.ToString(), new Vector2(Globals.WIDTH - 112, 50), Color.White);
        leftPaddle?.Draw();
        rightPaddle?.Draw();
        ball.Draw();
        Globals.spriteBatch?.End();

        base.Draw(gameTime);
    }

    public void SendActionToServer(PongAction action)
    {
        string leftOrRight = Client?.ClientIdentifier == null ? "left" : Client.ClientIdentifier;
        string jsonString = JsonSerializer.Serialize(action);
        Client?.SendData(leftOrRight, jsonString);
        if (!"move".Equals(action.Type))
            Console.WriteLine("SendActionToServer(): Paddle Update " + jsonString);
    }

    public int GetPaddleY(string leftOrRight)
    {
        if (Client == null)
        {
            return 0;
        }
        return Client.GameState.GetPaddleY(leftOrRight);
    }



}
