using TerminalVelocity;
using TerminalVelocity.UI;
public class ExtrasScene : Scene
{
    SelectBox s = new SelectBox();
    public ExtrasScene(string _name = "ExtrasScene") : base(_name)
    {
        InputEnabled = true;
        SceneObject qtTest = new SceneObject("QuadTree Test");
        SceneObject selectBoxTest = new SceneObject("SelectBox Test");

        qtTest.ProcessAction = () => Game.CurrentScene = new QuadTreeTest();
        selectBoxTest.ProcessAction = () => Game.CurrentScene = new SelectBoxTest();
        s.AddChild(qtTest);
        s.AddChild(selectBoxTest);
        s.PadAndRecenter();
        AddChild(s);
    }

    public override void OnInput(ConsoleKey key)
    {
        if (Game.CurrentScene == this)
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
}
