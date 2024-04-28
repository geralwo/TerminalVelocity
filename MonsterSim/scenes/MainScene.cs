using System.Data.SqlTypes;
using TerminalVelocity;
public class MainScene : Scene
{
    public MainScene(string _name) : base(_name)
    {
        Player<Ork> player= new Player<Ork>(Vec2i.ZERO, new Ork());
        add_child(player.Character);
        InputEnabled = true;
    }
    public override void OnInput(ConsoleKey key)
    {
        base.OnInput(key);
    }
}