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
        banner.Color = ConsoleColor.Black;
        banner.BackgroundColor = ConsoleColor.Red;
		banner.Display = """                                                  
 _____                        _____               
|   __|___ ___ ___ ___ ___   | __  |___ ___ _____ 
|   __|_ -|  _| .'| . | -_|  |    -| . | . |     |
|_____|___|___|__,|  _|___|  |__|__|___|___|_|_|_|
                  |_|
""";
		banner.center_xy();
		banner.Position = banner.Position + Vec2i.UP * 8;
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
            Game.Quit = true;
		};
        selectables.pad_and_recenter();
		add_child(selectables);
		selectables.center_xy();
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
