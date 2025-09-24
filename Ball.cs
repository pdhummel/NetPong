using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;


namespace Pong {
    public class Ball {
        Microsoft.Xna.Framework.Rectangle rect;

        private PongGame game;

        public Ball(PongGame game)
        {
            this.game = game;
            rect = new Rectangle(Globals.WIDTH / 2 - 20, Globals.HEIGHT / 2 - 20, 40, 40);
        }

        public void Update(GameTime gameTime, Paddle player1, Paddle player2)
        {
            if (player1 == null || player2 == null)
            {
                return;
            }

            if (game.Client != null)
            {
                rect.X = game.Client.GameState.BallX;
                rect.Y = game.Client.GameState.BallY;
            }
        }


        public void Draw()
        {
            Globals.spriteBatch?.Draw(Globals.pixel, rect, Color.White);
        }
    }
}
