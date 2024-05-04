namespace TerminalVelocity;

public class StartScene : Scene
{
    public UI.SelectBox MenuList = new UI.SelectBox();
    public StartScene(string _name) : base(_name)
    {
        InputEnabled = true;
        SceneObject ExitButton = new SceneObject("Exit");
        ExitButton.ProcessAction = () => Game.Quit = true;
        MenuList.AddChild(ExitButton);
    }

    public override void OnInput(ConsoleKey key)
    {
        if (Game.CurrentScene == this)
        {
            switch (key)
            {
                case ConsoleKey.DownArrow:
                    MenuList.Next();
                    break;
                case ConsoleKey.UpArrow:
                    MenuList.Previous();
                    break;
                case ConsoleKey.Enter:
                    Game.Beep();
                    MenuList.Select();
                    break;
                default:
                    break;
            }
        }
    }
}
