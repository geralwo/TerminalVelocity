using TerminalVelocity;
public class MainMenu : Scene
{
	int menuIndex = 0;
	SelectBox selectables = new SelectBox();
    SceneObject play = new SceneObject(" play ");
    SceneObject settings = new SceneObject(" settings ");
    SceneObject exit = new SceneObject(" exit ");

	SceneObject extras = new SceneObject(" extras ");
    public MainMenu(string _name = "SettingsMenu") : base(_name)
	{   
        name = _name;
        Visible = false;
        selectables.name = "selectables";
		selectables.ZIndex = 100;
		selectables.HighlightBackgroundColor = ConsoleColor.Red;
		selectables.Gap = Vec2i.ZERO;
        selectables.add_child(play);
        selectables.add_child(settings);
		selectables.add_child(extras);
        selectables.add_child(exit);
        InputEnabled = true;
        name = "main_menu";

		play.name = "main_menu_play";
		settings.name = "main_menu_settings";
		exit.name = "main_menu_exit";


		SceneObject banner = new SceneObject(true);
        banner.name = "banner";
        banner.ForegroundColor = ConsoleColor.Red;
        banner.BackgroundColor = ConsoleColor.Black;
        banner.Display = "___________                                   \r\n\\_   _____/ ______ ____ _____  ______   ____  \r\n |    __)_ /  ___// ___\\\\__  \\ \\____ \\_/ __ \\ \r\n |        \\\\___ \\\\  \\___ / __ \\|  |_> >  ___/ \r\n/_______  /____  >\\___  >____  /   __/ \\___  >\r\n        \\/     \\/     \\/     \\/|__|        \\/ \r\n__________                        ________    \r\n\\______   \\ ____   ____   _____   \\_____  \\   \r\n |       _//  _ \\ /  _ \\ /     \\   /  ____/   \r\n |    |   (  <_> |  <_> )  Y Y  \\ /       \\   \r\n |____|_  /\\____/ \\____/|__|_|  / \\_______ \\  \r\n        \\/                    \\/          \\/";
		banner.center_x();
		add_child(banner);	
		
		extras.ProcessAction = () => Game.CurrentScene = new ExtrasScene();
		play.ProcessAction = () => Game.CurrentScene = new GameScene("new game");
		settings.ProcessAction = () =>
		{
			Game.Beep(1337, 500);
            Game.CurrentScene = new SettingsScene();
        };
		
		
		exit.ProcessAction = () =>
		{
			Game.Beep(633, 300);
            Game.Beep(553, 200);
            Game.Beep(513, 500);
            Game.quitting = true;
		};
        selectables.pad_and_recenter();
		add_child(selectables);
    }

    public override void OnInput(ConsoleKey key)
    {
		if(Game.CurrentScene == this)
		{
			switch(key)
			{
				case ConsoleKey.DownArrow:
					selectables.next();
					break;
				case ConsoleKey.UpArrow:
					selectables.previous();
					break;
				case ConsoleKey.Enter:
                    Game.Beep();
                    selectables.select();
					break;
				default:
					break;
			}
		}   
    }
}