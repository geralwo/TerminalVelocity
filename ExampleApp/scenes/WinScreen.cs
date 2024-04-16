namespace TerminalVelocity;

public class WinScreen : Scene
{
    SceneObject banner;
    public WinScreen(string _name = "WinScreen") : base(_name)
    {
        banner = new SceneObject(true);
        banner.Display = "\n Congratulations! \n You have made it! \n Press any key to go to the main menu \n";
        banner.center_xy();
        banner.Position += Vec2i.UP;
        add_child(banner);
        InputEnabled = true;
        ProcessEnabled = true;
    }

    public override void OnInput(ConsoleKey key)
    {
        Game.CurrentScene = new MainMenu();
    }

    public override void OnProcess()
    {
        banner.BackgroundColor = Game.GetRandomConsoleColor(ConsoleColor.White);
        Thread.Sleep(333);
    }
}