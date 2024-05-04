using TerminalVelocity;
public class RenderTest : Scene
{
    public SceneObject[] RenderTests;
    int testIndex = -1;
    int testDuration = 5;
    public RenderTest(string _name = "RenderTestMain") : base(_name)
    {
        RenderTests = new SceneObject[3];
        InputEnabled = true;
        ProcessEnabled = true;
        RenderTests[0] = new RandomNoisePattern(2);
        RenderTests[1] = new StripePattern(true);
        RenderTests[2] = new RandomMovingPixel();
    }

    public override void OnProcess()
    {
        if (Game.RunTime.ElapsedMilliseconds < (testDuration * 1) * 1000 && testIndex == -1)
        {
            testIndex = 0;
            AddChild(RenderTests[0]);
        }
        else if (Game.RunTime.ElapsedMilliseconds > (testDuration * 1) * 1000 && testIndex == 0)
        {
            RemoveChild(RenderTests[0]);
            testIndex = 1;
            AddChild(RenderTests[1]);
        }
        else if (Game.RunTime.ElapsedMilliseconds > (testDuration * 2) * 1000 && testIndex == 1)
        {
            RemoveChild(RenderTests[1]);
            testIndex = 2;
            AddChild(RenderTests[2]);
        }
        else if (Game.RunTime.ElapsedMilliseconds > (testDuration * 3) * 1000 && testIndex == 2) Game.Quit = true;
    }
}
