using TerminalVelocity;
Game g = new Game();
Game.CurrentScene = new RenderTestScene("RenderTestScene");
g.Run();
Game.PrintEngineStats();

public class RenderTestScene : Scene
{
    private List<SceneObject> testObjects = new List<SceneObject>();
    public RenderTestScene(string _name) : base(_name)
    {
        InputEnabled = true;
        ProcessEnabled = true;
    }

    public override void OnStart()
    {
        Game.CurrentScene = generateTestPattern(1);
    }

    Scene generateTestPattern(int offset)
    {
        var testPatternScene = new Scene("TestPattern");
        testPatternScene.InputEnabled = true;
        testPatternScene.ProcessEnabled = true;
        testPatternScene.ProcessAction += () =>
        {
            if (Game.RunTime > 5 * 1000)
            {
                Game.CurrentScene = generateStripePattern();
            }
        };
        if (offset == 0)
            offset = 1;
        for (int x = 0; x < Game.Settings.Engine.WindowSize.x; x++)
        {
            for (int y = 0; y < Game.Settings.Engine.WindowSize.y; y++)
            {
                if (x % offset == 0 && y % offset == 0)
                {
                    SceneObject random = new SceneObject(" ");
                    random.ProcessEnabled = true;
                    random.ProcessAction += () => { random.BackgroundColor = Game.GetRandomConsoleColor(); };
                    random.Position = new Vec2i(x, y);
                    random.BackgroundColor = Game.GetRandomConsoleColor();
                    testPatternScene.add_child(random);
                }
            }
        }

        return testPatternScene;
    }

    Scene generateStripePattern()
    {
        var testWavePatternScene = new Scene("WavePatternScene");
        testWavePatternScene.InputEnabled = true;
        testWavePatternScene.ProcessEnabled = true;
        testWavePatternScene.ProcessAction += () =>
        {
            if (Game.RunTime > 5 * 1000 && Game.RunTime < 10 * 1000)
            {
                foreach (SceneObject child in testWavePatternScene.Children)
                {
                    if(child.Position <= Game.Settings.Engine.WindowSize && child.Position >= Vec2i.ZERO)
                        child.Position += Vec2i.RIGHT;
                    else
                    {
                        child.Position = new Vec2i(0, child.Position.y);
                    }
                }
            }
            else if (Game.RunTime > 10 * 1000 && Game.RunTime < 13 * 1000)
            {
                foreach (SceneObject child in testWavePatternScene.Children)
                {
                    if(child.Position <= Game.Settings.Engine.WindowSize && child.Position >= Vec2i.ZERO)
                        child.Position = child.Position.StepToZero();
                }
            }
            else if (Game.RunTime > 13 * 1000 && Game.RunTime < 15 * 1000)
            {
                foreach (SceneObject child in testWavePatternScene.Children)
                {
                    if(child.Position <= Game.Settings.Engine.WindowSize && child.Position >= Vec2i.ZERO)
                        child.Position = child.Position.StepToPosition(Vec2i.Random(Game.Settings.Engine.WindowSize.y));
                }
            }
            else Game.Quit = true;
        };
        for (int x = 0; x < Game.Settings.Engine.WindowSize.x; x++)
        {
            var rxc = Game.GetRandomConsoleColor();
            for (int y = 0; y < Game.Settings.Engine.WindowSize.y; y++)
            {
                SceneObject random = new SceneObject(" ");
                random.Position = new Vec2i(x, y);
                random.BackgroundColor = rxc;
                testWavePatternScene.add_child(random);
            }
        }
        return testWavePatternScene;
    }

}
