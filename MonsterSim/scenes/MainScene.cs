using System.Data.SqlTypes;
using TerminalVelocity;
public class MainScene : Scene
{
    public MainScene(string _name) : base(_name)
    {
        int RatCount = 5;
        Player<Ork> player = new Player<Ork>(Vec2i.ZERO, new Ork());
        AddChild(player.Character);
        for (int i = 0; i < RatCount; i++)
        {
            Rat rat = new Rat(Vec2i.Random(Game.Settings.Engine.WindowSize));
            rat.ProcessAction += () =>
            {
                rat.MovementAbility();
            };
            AddChild(rat);
        }
        InputEnabled = true;
    }
    public override void OnInput(ConsoleKey key)
    {
        base.OnInput(key);
    }
}
