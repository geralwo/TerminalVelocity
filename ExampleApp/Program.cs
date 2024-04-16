using TerminalVelocity;
namespace ExampleApp;
public class Program
{
    public static void Main(string[] args)
    {
        Game game = new Game();
        Scene scene = new MainMenu();
        //Scene scene = new QuadTreeTest();
        //Scene scene = new SelectBoxTest();
        Game.CurrentScene = scene;

        game.Run();

        Console.ResetColor();
        Console.Clear();
        Console.WriteLine($"total frames   : {Game.FrameCount}");
        Console.WriteLine($"total run time : {Game.RunTime} ms");
        Console.WriteLine($"average fps    : {(int)(Game.FrameCount / (Game.RunTime / 1000))}");
        Console.WriteLine($"render items   : { RenderServer.Instance.Count() }");
    }
}