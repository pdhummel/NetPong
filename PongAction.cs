using Microsoft.Xna.Framework;
namespace Pong;

public class PongAction(string clientIdentifier, string direction, GameTime gameTime)
{

    public string ClientIdentifier
    { get; set; } = clientIdentifier;
    public string Direction { get; set; } = direction;

    public GameTime GameTime { get; set; } = gameTime;

    public string Type { get; set; } = "move";
}