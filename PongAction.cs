using Microsoft.Xna.Framework;
namespace Pong;

public class PongAction
{
    public string? ClientIdentifier { get; set; }
    public string? Direction { get; set; }

    public GameTime? GameTime { get; set; }

    public string Type { get; set; } = "move";

    public PongAction()
    {
        
    }

    public PongAction(string clientIdentifier)
    {
        ClientIdentifier = clientIdentifier;
    }

    public PongAction(string clientIdentifier, string type)
    {
        ClientIdentifier = clientIdentifier;
        Type = type;
    }

    public PongAction(string clientIdentifier, string direction, GameTime gameTime)
    {
        ClientIdentifier = clientIdentifier;
        Direction = direction;
        GameTime = gameTime;
    }


}