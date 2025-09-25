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
            if (game.Client?.ClientIdentifier == null || !game.Client.ClientIdentifier.Equals(leftOrRight))
            {
                return;
            }

            float deadZone = 0.2f;
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            float leftThumbstickY = gamepadState.ThumbSticks.Left.Y;
            float rightThumbstickY = gamepadState.ThumbSticks.Right.Y;
            GamePadDPad dpad = GamePad.GetState(PlayerIndex.One).DPad;

            // Since I have a MSI Claw and sometime use it docked like a PC,
            // second controller support is useful.
            GamePadState gamepadState2 = GamePad.GetState(PlayerIndex.Two);
            float leftThumbstickY2 = gamepadState2.ThumbSticks.Left.Y;
            float rightThumbstickY2 = gamepadState2.ThumbSticks.Right.Y;
            GamePadDPad dpad2 = GamePad.GetState(PlayerIndex.Two).DPad;

            KeyboardState kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) ||
                kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up) ||
                (Math.Abs(leftThumbstickY) > deadZone && leftThumbstickY > 0) ||
                (Math.Abs(rightThumbstickY) > deadZone && rightThumbstickY > 0) ||
                dpad.Up == Microsoft.Xna.Framework.Input.ButtonState.Pressed ||
                (Math.Abs(leftThumbstickY2) > deadZone && leftThumbstickY2 > 0) ||
                (Math.Abs(rightThumbstickY2) > deadZone && rightThumbstickY2 > 0) ||
                dpad2.Up == Microsoft.Xna.Framework.Input.ButtonState.Pressed
               )
            {
                PongAction action = new PongAction(leftOrRight, "up", gameTime);
                game.SendActionToServer(action);
            }
            if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) ||
                kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down) ||
                (Math.Abs(leftThumbstickY) > deadZone && leftThumbstickY < 0) ||
                (Math.Abs(rightThumbstickY) > deadZone && rightThumbstickY < 0) ||
                dpad.Down == Microsoft.Xna.Framework.Input.ButtonState.Pressed ||
                (Math.Abs(leftThumbstickY2) > deadZone && leftThumbstickY2 < 0) ||
                (Math.Abs(rightThumbstickY2) > deadZone && rightThumbstickY2 < 0) ||
                dpad2.Down == Microsoft.Xna.Framework.Input.ButtonState.Pressed
               )
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
