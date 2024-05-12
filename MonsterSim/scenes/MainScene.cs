using TerminalVelocity;
public class DungeonScene : Scene
{
    public DungeonScene(string _name) : base(_name)
    {
        int RatCount = 55;
        int MoveTimer = 500;
        Player<Ork> player = new Player<Ork>(Vec2i.ZERO, new Ork());
        // for (int i = 0; i < RatCount; i++)
        // {
        //     Rat rat = new Rat(Vec2i.Random(Game.Settings.Engine.WindowSize));
        //     rat.name = $"rat{i}";
        //     rat.ProcessAction += () =>
        //     {
        //         if (Game.RunTime.ElapsedMilliseconds % MoveTimer < 16)
        //             rat.MovementAbility();
        //     };
        //     AddChild(rat);
        // }
        InputEnabled = true;
        var l = new Level(Vec2i.New(50, 25), 8);
        AddChild(l);
        AddChild(player.Character);
    }
    public override void OnInput(ConsoleKey key)
    {
        base.OnInput(key);
    }
}
