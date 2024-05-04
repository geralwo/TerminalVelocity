using TerminalVelocity;

public class RandomMovingPixel : SceneObject
{
    public RandomMovingPixel(string name = "RandomMovingPixel")
    {

    }

    public override void OnStart()
    {
        for (int x = 0; x < Game.Settings.Engine.WindowSize.x; x++)
        {
            var rxc = Game.GetRandomConsoleColor();
            for (int y = 0; y < Game.Settings.Engine.WindowSize.y; y++)
            {
                var randomDest = Vec2i.Random(Game.Settings.Engine.WindowSize);
                long start = Game.RunTime.ElapsedMilliseconds;
                SceneObject random = new SceneObject(" ");
                random.Position = new Vec2i(x, y);
                random.BackgroundColor = rxc;
                random.ProcessAction = () =>
                {
                    if (Game.RunTime.ElapsedMilliseconds - start < 1.5 * 1000)
                        random.Position = random.Position.StepToPosition(randomDest);
                    else if (Game.RunTime.ElapsedMilliseconds - start > 1.5 * 1000 && Game.RunTime.ElapsedMilliseconds - start < 3.5 * 1000)
                        random.Position = random.Position.StepToPosition(Game.Settings.Engine.WindowSize / 2);
                    else if (Game.RunTime.ElapsedMilliseconds - start > 3.5 * 1000)
                        random.Position = random.Position.StepToZero(1);
                };
                random.ProcessEnabled = true;
                AddChild(random);
            }
        }
    }
}
