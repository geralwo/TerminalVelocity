using TerminalVelocity;
namespace ExampleApp;
public class Program
{
    public static void Main(string[] args)
    {
        Game game = new Game();
        Game.LogLevel = LogLevel.Important;
        game.Run(new MainMenu());
        Game.PrintEngineStats();
    }
}
