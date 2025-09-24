using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SharpDX.X3DAudio;

namespace Pong {
    public class Paddle {
        public Microsoft.Xna.Framework.Rectangle rect;
        private string leftOrRight;

        public const int sizeY = 120;

        public const int leftPaddleX = 10;

        public const int rightPaddleX = Globals.WIDTH - 60;

        public PongGame game;

        public Paddle(PongGame game, string leftOrRight)
        {
            this.game = game;
            this.leftOrRight = leftOrRight;
            int x = leftPaddleX;
            if ("right".Equals(leftOrRight))
            {
                x = rightPaddleX;
            }
            int topY = game.GetPaddleY(leftOrRight);
            rect = new Microsoft.Xna.Framework.Rectangle(x, topY, 40, sizeY);
        }
        public void Update(GameTime gameTime)
        {
            if (game.Client?.ClientIdentifier == null || ! game.Client.ClientIdentifier.Equals(leftOrRight))
            {
                return;
            }
            KeyboardState kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) || kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                PongAction action = new PongAction(leftOrRight, "up", gameTime);
                game.SendActionToServer(action);
            }
            if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) || kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                PongAction action = new PongAction(leftOrRight, "down", gameTime);
                game.SendActionToServer(action);
            }
        }

        public void Draw()
        {
            rect.Y = game.GetPaddleY(leftOrRight);
            Globals.spriteBatch?.Draw(Globals.pixel, rect, Microsoft.Xna.Framework.Color.White);
        }
    }
}
