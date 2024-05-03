namespace TerminalVelocity;
using TerminalVelocity.UI;
public class StartScene : Scene
{
    public SelectBox MenuEntries        = new SelectBox();
    private SceneObject quitGameButton  = new SceneObject("Exit");
    public StartScene(string _name) : base(_name)
    {
        InputEnabled = true;
        AddChild(MenuEntries);
        quitGameButton.ProcessAction += () =>
        {
            Game.Quit = true;
        };

        MenuEntries.AddChild(quitGameButton);
    }

    public override void OnInput(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.Enter:
                MenuEntries.Select();
                return;
            case ConsoleKey.UpArrow:
                MenuEntries.Previous();
                return;
            case ConsoleKey.DownArrow:
                MenuEntries.Next();
                return;
            default:
                return;
        }
    }
}
