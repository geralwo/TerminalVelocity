using TerminalVelocity;
namespace ExampleApp;
public class Program
{
    public static void Main(string[] args)
    {
        GameSettings settings = new TestGameSettings();
        Game game = new Game(settings);
        Scene scene = new SettingsMenu();
        Game.CurrentScene = scene;
        Game.Settings.User = settings;
        game.Run();

        Console.ResetColor();
        Console.Clear();
        Console.WriteLine($"total frames   : {Game.FrameCount}");
        Console.WriteLine($"total run time : {Game.RunTime} ms");
        Console.WriteLine($"average fps    : {(int)((float)Game.FrameCount / ((float)Game.RunTime / 1000f))}");
        Console.WriteLine($"render items   : { RenderServer.Instance.Count() }");
        Console.ReadKey(true);
    }
}