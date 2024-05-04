using TerminalVelocity;
public class StripePattern : SceneObject
{
    bool Vertical;
    public StripePattern(bool vertical)
    {
        Visible = false;
        Vertical = vertical;
    }

    public override void OnStart()
    {
        var start = Game.RunTime.ElapsedMilliseconds;
        var rxc = Console.BackgroundColor;
        for (int x = 0; x < Game.Settings.Engine.WindowSize.x; x++)
        {
            rxc = Game.GetRandomConsoleColor();
            for (int y = 0; y < Game.Settings.Engine.WindowSize.y; y++)
            {
                SceneObject random = new SceneObject(" ");
                random.Position = new Vec2i(x, y);
                random.BackgroundColor = rxc;
                random.ProcessAction = () =>
                {
                    if (Game.RunTime.ElapsedMilliseconds - start > 2.5 * 1000) Vertical = false;
                    if (Vertical)
                    {
                        if (!random.Position.IsInBoundsOf(Game.Settings.Engine.WindowSize))
                            random.Position = new Vec2i(0, random.Position.y);
                        random.Position += Vec2i.RIGHT;
                    }
                    else
                    {
                        if (!random.Position.IsInBoundsOf(Game.Settings.Engine.WindowSize))
                            random.Position = new Vec2i(random.Position.x, 0);
                        random.Position += Vec2i.DOWN;
                    }
                };
                random.ProcessEnabled = true;
                AddChild(random);
            }
        }

    }
}
