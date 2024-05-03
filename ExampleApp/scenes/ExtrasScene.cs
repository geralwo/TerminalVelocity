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
        s.pad_and_recenter();
        AddChild(s);
    }

    public override void OnInput(ConsoleKey key)
    {
		if(Game.CurrentScene == this)
		{
			switch(key)
			{
				case ConsoleKey.DownArrow:
					s.next();
					break;
				case ConsoleKey.UpArrow:
					s.previous();
					break;
				case ConsoleKey.Enter:
                    Game.Beep();
                    s.select();
					break;
				default:
					break;
			}
		}   
    }
}