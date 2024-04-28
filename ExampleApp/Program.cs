using TerminalVelocity;
namespace ExampleApp;
public class Program
{
    public static void Main(string[] args)
    {
        Game game = new Game();
        Scene scene = new MainMenu();
        Game.CurrentScene = scene;

        game.Run();
        Game.PrintEngineStats();
    }
}