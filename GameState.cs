namespace Pong;

public class GameState
{

    public int LeftPaddleTopY { get; set; } = Globals.HEIGHT / 2 - Paddle.sizeY/2; 
    public int RightPaddleTopY { get; set; } = Globals.HEIGHT / 2 - Paddle.sizeY/2; 
    public int BallX { get; set; } = Globals.WIDTH / 2 - 20;
    public int BallY { get; set; } = Globals.HEIGHT / 2 - 20;
    public int LeftScore { get; set; } = 0;
    public int RightScore { get; set; } = 0;



    public GameState()
    {
    }

    public GameState(int ballX, int ballY)
    {
        this.BallX = ballX;
        this.BallY = ballY;
    }

    public GameState(int leftY, int rightY, int ballX, int ballY)
    {
        this.LeftPaddleTopY = leftY;
        this.RightPaddleTopY = rightY;
        this.BallX = ballX;
        this.BallY = ballY;
    }

    public int GetPaddleY(string leftOrRight)
    {
        if ("left".Equals(leftOrRight))
        {
            return this.LeftPaddleTopY;
        }
        else
        {
            return this.RightPaddleTopY;
        }
    }
}