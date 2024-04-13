using TerminalVelocity;

public class UiTestScene : Scene
{
    public UiTestScene(string _name = "UI_TEST") : base(_name)
    {   
        UI.Window window= new UI.Window(20,10);
        window.BackgroundColor = ConsoleColor.Red;
        window.Visible = true;
    }
}