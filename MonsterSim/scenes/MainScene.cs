using TerminalVelocity;

public class MainScene : Scene
{
    public MainScene(string _name) : base(_name)
    {
        add_child(new Ork(Vec2i.ONE,"ô"));
        InputEnabled = true;
    }

    public override void OnInput(ConsoleKey key)
    {
        base.OnInput(key);
    }
}