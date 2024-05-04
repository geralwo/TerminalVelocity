using TerminalVelocity;
using TerminalVelocity.UI;
public class SelectBoxTest : Scene
{
    SelectBox s = new SelectBox();
    public SelectBoxTest(string _name = "SelectBoxTest") : base(_name)
    {
        s.Gap = Vec2i.DOWN; // only down is tested
        s.FlowDirection = Vec2i.DOWN; // only down is tested
        SceneObject action = new SceneObject("action");
        action.Visible = true;
        action.ProcessAction = () => s.AddChild(new SceneObject("x"));
        SceneObject exit = new SceneObject("exit");
        exit.Visible = true;
        exit.ProcessAction = () => Game.Quit = true;
        s.AddChild(action);
        s.AddChild(exit);
        AddChild(s);
        InputEnabled = true;
    }

    public override void OnInput(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.DownArrow:
                s.Next();
                break;
            case ConsoleKey.UpArrow:
                s.Previous();
                break;
            case ConsoleKey.Enter:
                Game.Beep();
                s.Select();
                break;
            default:
                break;
        }
    }

}
