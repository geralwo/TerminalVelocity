using TerminalVelocity;
public class Program
{
    public static void Main(string[] args)
    {
        Game game = new Game();
        TestScene scene = new TestScene();
        Game.CurrentScene = scene;
        game.Run();

        Console.ResetColor();
        Console.Clear();
        Console.WriteLine($"total frames   : {Game.FrameCount}");
        Console.WriteLine($"total run time : {Game.RunTime}ms");
        Console.WriteLine($"average fps    : {(int)((float)Game.FrameCount / ((float)Game.RunTime / 1000f))}");
        Console.WriteLine($"render items   : { RenderServer.Instance.count() }");
        Console.ReadKey(true);
    }
}