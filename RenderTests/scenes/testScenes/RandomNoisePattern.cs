using TerminalVelocity;
public class RandomNoisePattern : SceneObject
{
    int offset = 1;
    public RandomNoisePattern(int _offset)
    {
        Visible = false;
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
    }
}
