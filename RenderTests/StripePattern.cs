using TerminalVelocity;

public class StripePattern : Scene
{
    public StripePattern(int _duration) : base("StripePatternScene")
    {
        InputEnabled = true;
        ProcessAction = () =>
        {
            if (Game.RunTimeSW.Elapsed.TotalMilliseconds > _duration * 1000) Game.Quit = true;
        };
    }

    public override void OnStart()
    {
        for (int x = 0; x < Game.Settings.Engine.WindowSize.x; x++)
        {
            var xc = Game.GetRandomConsoleColor();
            for (int y = 0; y < Game.Settings.Engine.WindowSize.y; y++)
            {
                SceneObject random = new SceneObject(" ");
                random.Position = new Vec2i(x, y);
                random.BackgroundColor = xc;
                random.ProcessEnabled = true;
                random.ProcessAction = () =>
                {
                    if (!random.Position.IsInBoundsOf(Game.Settings.Engine.WindowSize))
                        random.Position = new Vec2i(0, random.Position.y);
                    random.Position += Vec2i.RIGHT;
                };
                AddChild(random);
            }
        }
        ProcessEnabled = true;
        base.OnStart();
    }
}
