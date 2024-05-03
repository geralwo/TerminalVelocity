using TerminalVelocity;
public class PlayScene : Scene
{
    public PlayScene(string _name) : base(_name)
    {
        Player<Ork> player= new Player<Ork>(Vec2i.ZERO, new Ork());
        AddChild(player.Character);
        Rat rat = new Rat(Vec2i.ONE * 5);
        rat.ProcessAction += () => {         
            rat.MovementAbility();
        };
        Rat rat2 = new Rat(new Vec2i(40,5));
        rat2.name = "rat2";
        rat2.ProcessAction += () => {         
            rat2.MovementAbility();
        };
        AddChild(rat);
        AddChild(rat2);
        InputEnabled = true;
    }
    public override void OnInput(ConsoleKey key)
    {
        base.OnInput(key);
    }
}