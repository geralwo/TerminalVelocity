using TerminalVelocity;
public class RandomNoisePattern : Scene
{
    private int offset = 1;
    public RandomNoisePattern(int _duration, int _offset) : base("RandomNoisePattern")
    {
        if (_offset <= 0)
            _offset = 1;
        offset = _offset;
        InputEnabled = true;
        ProcessAction = () =>
        {
            if (Game.RunTimeSW.Elapsed.TotalMilliseconds > _duration * 1000)
                Game.CurrentScene = new StripePattern(_duration * 2);
        };
    }

    public override void OnStart()
    {
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
                    AddChild(random);
                }
            }
        }
        ProcessEnabled = true;
        base.OnStart();
    }
}
