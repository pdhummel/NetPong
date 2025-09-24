using Pong;

Form1 form = new();
form.Show();
//Application.Run(form);
//var game = new Game1(form.GetDrawSurface());
var game = new PongGame();
form.SetGameObject(game);
Server server = new();
form.SetServer(server);
Client client = new();
form.SetClient(client);
game.Client = client;
game.Run();
Console.WriteLine("Program started");
