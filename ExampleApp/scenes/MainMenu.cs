using TerminalVelocity;
public class MainMenu : Scene
{
	int menuIndex = 0;
	SceneObject selectables = new SceneObject();
    SceneObject play = new SceneObject(" play ");
    SceneObject settings = new SceneObject(" settings ");
    SceneObject exit = new SceneObject(" exit ");
    public MainMenu(string _name = "SettingsMenu") : base(_name)
	{   
        name = _name;
        Visible = false;
        selectables.name = "selectables";
		selectables.ZIndex = 100;
        selectables.Visible = false;
        selectables.add_child(play);
        selectables.add_child(settings);
        selectables.add_child(exit);
        InputEnabled = true;
        ProcessEnabled = true;
        name = "main_menu";

		play.name = "main_menu_play";
		settings.name = "main_menu_settings";
		exit.name = "main_menu_exit";


		SceneObject banner = new SceneObject(true);
        banner.name = "banner";
        banner.ForegroundColor = ConsoleColor.Red;
        banner.BackgroundColor = ConsoleColor.Black;
        banner.Display = "___________                                   \r\n\\_   _____/ ______ ____ _____  ______   ____  \r\n |    __)_ /  ___// ___\\\\__  \\ \\____ \\_/ __ \\ \r\n |        \\\\___ \\\\  \\___ / __ \\|  |_> >  ___/ \r\n/_______  /____  >\\___  >____  /   __/ \\___  >\r\n        \\/     \\/     \\/     \\/|__|        \\/ \r\n__________                        ________    \r\n\\______   \\ ____   ____   _____   \\_____  \\   \r\n |       _//  _ \\ /  _ \\ /     \\   /  ____/   \r\n |    |   (  <_> |  <_> )  Y Y  \\ /       \\   \r\n |____|_  /\\____/ \\____/|__|_|  / \\_______ \\  \r\n        \\/                    \\/          \\/  ";
		banner.center_x();
		add_child(banner);

		play.center_xy();
		play.BackgroundColor = ConsoleColor.White;
		play.ForegroundColor = ConsoleColor.Red;
		play.ProcessAction = () => Game.CurrentScene = new GameScene("play button game scene");
		
		
		settings.ProcessAction = () =>
		{
			Game.Beep(1337, 500);
            Game.CurrentScene = new SettingsScene();
        };
		
		settings.Position = play.Position + Vec2i.DOWN;
		settings.ForegroundColor = play.ForegroundColor;
		settings.BackgroundColor = play.BackgroundColor;
		
		
		exit.ProcessAction = () =>
		{
			Game.Beep(633, 300);
            Game.Beep(553, 200);
            Game.Beep(513, 500);
            Game.quitting = true;
		};
		exit.Position = settings.Position + Vec2i.DOWN;
		exit.BackgroundColor = play.BackgroundColor;
		exit.ForegroundColor = play.ForegroundColor;


        // pad and recenter items
        int exit_length = exit.Display.Length;
        int settings_length = settings.Display.Length;
        int play_length = play.Display.Length;
        int longest_str = Math.Max(exit_length, settings_length);
        longest_str = Math.Max(play_length, longest_str);

        play.Display = play.Display.PadRight(longest_str);
        exit.Display = exit.Display.PadRight(longest_str);
        settings.Display = settings.Display.PadRight(longest_str);

        
		add_child(selectables);
    }

    public override void OnProcess()
    {
		if(Game.CurrentScene  == this)
		{
			for(int i=0;i < selectables.Children.Count;i++)
			{
				if (i == menuIndex)
				{
					selectables.Children[i].BackgroundColor = ConsoleColor.Red;
					selectables.Children[i].ForegroundColor = ConsoleColor.Black;
				}
				else
				{
                    selectables.Children[i].BackgroundColor = ConsoleColor.White;
                    selectables.Children[i].ForegroundColor = ConsoleColor.Red;
                }
			}
		}
		
    }

    public override void OnInput(ConsoleKey key)
    {
		if(Game.CurrentScene == this)
		{
			switch(key)
			{
				case ConsoleKey.DownArrow:
					if(menuIndex < selectables.Children.Count -1)
						menuIndex++;
						break;
				case ConsoleKey.UpArrow:
					if(menuIndex > 0)
						menuIndex--;
						break;
				case ConsoleKey.Enter:
                    Game.Beep();
                    selectables.Children[menuIndex].ProcessAction.Invoke();
					break;
				default:
					break;
			}
		}   
    }
}