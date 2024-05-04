using TerminalVelocity;
Game g = new Game();
TerminalVelocity.core.Debug.CurrentLogLevel = LogLevel.Important;
//Game.CurrentScene = new RenderTestScene("RenderTestScene");
// g.Run(new RenderTestScene("RenderTestScene"));
// g.Run(new StripePattern());
//g.Run(new RandomNoisePattern(1));
g.Run(new RenderTestsScene());
Game.PrintEngineStats();
string path = Directory.GetCurrentDirectory();
string filePath = Path.Combine(path, $"RenderTestResult.txt");
using (StreamWriter sw = File.CreateText(filePath))
{
    sw.WriteLine(Game.EngineStats);
}

public class RenderTestsScene : Scene
{
    public int perTestTime = 5;
    private int testSceneIndex = 0;
    private Scene[] testScenes;
    public RenderTestsScene() : base("RenderTestMainScene")
    {
        InputEnabled = true;
        ProcessEnabled = true;
        testScenes = new Scene[3];
        testScenes[0] = new RandomNoisePattern(5, 1);
    }

    public override void OnStart()
    {
        Game.CurrentScene = testScenes[0];
    }
}
