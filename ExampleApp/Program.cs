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

        Console.ResetColor();
        Console.Clear();
        Console.WriteLine($"total frames   : {Game.FrameCount}");
        Console.WriteLine($"total run time : {Game.RunTime} ms");
        Console.WriteLine($"average fps    : {Game.FrameCount / (Game.RunTime / 1000)}");
        Console.WriteLine($"render items   : { RenderServer.Instance.Count() }");
        Console.WriteLine("press any thing to exit");
    }
}