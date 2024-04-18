using TerminalVelocity;
public class SettingsScene : Scene
{
    public Vec2i lvl_size = new Vec2i(40,20);
    public string player_char = "*";
    public bool audio_enabled = false;
    private int menuIndex = 1;


    // These could be simplified into a MenuItem class
    private SceneObject title_player_char;
    private SceneObject value_player_char;
  
    private SceneObject title_lvl_size;
    private SceneObject value_lvl_size;

    private SceneObject title_audio_enabled;
    private SceneObject value_audio_enabled;

    public HashSet<String>custom_data = new HashSet<string>();
    private bool getting_input = false;

    public SettingsScene(string _name = "SettingsScene") : base(_name)
    {
        lvl_size = EscapeRoomSettings.RoomSize;
        player_char = EscapeRoomSettings.PlayerChar;
        audio_enabled = Game.Settings.Engine.AudioEnabled;
        InputEnabled = true;
        ProcessEnabled = true;

        title_lvl_size = new SceneObject("Level Size:");
        value_lvl_size = new SceneObject(title_lvl_size.Position + Vec2i.DOWN);
        value_lvl_size.name = "settings_lvl_size";
        value_lvl_size.ProcessAction += get_input;
        value_lvl_size.Display = lvl_size.ToString();


        title_player_char = new SceneObject(value_lvl_size.Position + Vec2i.DOWN,"Player char:");
        value_player_char = new SceneObject(title_player_char.Position + Vec2i.DOWN);
        value_player_char.Display = player_char.ToString();
        value_player_char.name = "settings_player_char";
        value_player_char.ProcessAction += get_input;

        title_audio_enabled = new SceneObject(value_player_char.Position + Vec2i.DOWN, "Audio enabled:");
        value_audio_enabled = new SceneObject(title_audio_enabled.Position + Vec2i.DOWN, audio_enabled.ToString());
        value_audio_enabled.name = "settings_audio_enabled";
        value_audio_enabled.ProcessAction += get_input;



        title_lvl_size.ForegroundColor = ConsoleColor.Green;
        value_lvl_size.ForegroundColor = ConsoleColor.Red;

        title_player_char.ForegroundColor = ConsoleColor.Green;
        value_player_char.ForegroundColor = ConsoleColor.Red;

        add_child(title_lvl_size);
        add_child(value_lvl_size);

        add_child(title_player_char);
        add_child(value_player_char);

        add_child(title_audio_enabled);
        add_child(value_audio_enabled);

        string legend_text = "Esc to go back to main menu\nEnter to change highlighted value";
        SceneObject legend = new SceneObject(false);

        legend.Position = new Vec2i(0, Console.WindowHeight - 3);
        legend.Display = legend_text;
        add_child(legend);
    }

    private void get_input()
    {
        getting_input = true;
        Game.Beep(1100,700);
        Console.SetCursorPosition(Children[menuIndex].Position.x, Children[menuIndex].Position.y);
        Console.BackgroundColor = ConsoleColor.Green;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.CursorVisible = true;

        // setting for level size
        if (Children[menuIndex].name == value_lvl_size.name)
        {
            // asking for x input
            x_input_loop:
            Console.Clear();
            Console.SetCursorPosition(Children[menuIndex].Position.x, Children[menuIndex].Position.y - 1);
            Console.WriteLine($"pls enter the value for the x dimension [min: 10, max : {Console.WindowWidth}]");
            Console.SetCursorPosition(Children[menuIndex].Position.x, Children[menuIndex].Position.y);
            string? x_dim = Console.ReadLine();
            int ix_dim = EscapeRoomSettings.RoomSize.x;
            if (x_dim != null)
            {
                if (int.TryParse(x_dim, out int x_dim_int) && x_dim_int > 0 && x_dim_int <= Console.WindowWidth)
                {
                    ix_dim = x_dim_int;
                    Game.Beep(2254, 200);
                }
                else
                {
                    Game.Beep(500, 750);
                    goto x_input_loop;
                }
            }
            
            // asking for y input
            y_input_loop:
            Console.Clear();
            Console.SetCursorPosition(Children[menuIndex].Position.x, Children[menuIndex].Position.y - 1);
            Console.WriteLine($"pls enter the value for the y dimension [min: 10, max : {Console.WindowHeight}]");
            Console.SetCursorPosition(Children[menuIndex].Position.x, Children[menuIndex].Position.y);
            string? y_dim = Console.ReadLine();
            int iy_dim = EscapeRoomSettings.RoomSize.y;
            if (y_dim != null)
            {
                if(int.TryParse(y_dim, out int y_dim_int) && y_dim_int > 0 && y_dim_int <= Console.WindowHeight)
                {
                    iy_dim = y_dim_int;
                    Game.Beep(2254, 200);
                }

                else
                {
                    Game.Beep(500, 750);
                    goto y_input_loop;
                }
            }
            

            // Setting values in settings
            Vec2i new_size = new Vec2i(ix_dim, iy_dim);
            Children[menuIndex].Display = new_size.ToString();
            EscapeRoomSettings.RoomSize = new_size;
            Console.Clear();
        }

        // setting for player character
        
        if (Children[menuIndex].name == value_player_char.name)
        {
            Children[menuIndex].BackgroundColor = ConsoleColor.White;
            Children[menuIndex].ForegroundColor = ConsoleColor.Black;
            char input = Console.ReadKey().KeyChar;
            if (!char.IsControl(input))
            {
                Game.Beep(2254, 200);
                player_char = input.ToString();
                Children[menuIndex].BackgroundColor = ConsoleColor.Green;
                Children[menuIndex].ForegroundColor = ConsoleColor.Black;
                Children[menuIndex].Display = player_char.ToString();
            }
            else
            {
                get_input();
            }
            EscapeRoomSettings.PlayerChar = player_char;
            Console.ResetColor();
            Console.Clear();
        }

        // setting for audio enabling
        if (Children[menuIndex].name == value_audio_enabled.name)
        {
            audio_enabled = !audio_enabled;
            Children[menuIndex].Display = audio_enabled.ToString();
            Game.Settings.Engine.AudioEnabled = audio_enabled;
            Game.Beep(2254, 200);
            Console.Clear();
        }
        Console.CursorVisible = false;
        Console.ResetColor();
        getting_input = false;
    }

    public override void OnProcess()
    {
        if (Game.CurrentScene == this)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                if (i == menuIndex)
                {
                    Children[i].ForegroundColor = ConsoleColor.Black;
                    Children[i].BackgroundColor = ConsoleColor.Green;
                }
                else
                {
                    Children[i].ForegroundColor = ConsoleColor.White;
                    Children[i].BackgroundColor = ConsoleColor.Black;
                }
            }
        }
    }

    public override void OnInput(ConsoleKey key)
    {
        if (Game.CurrentScene == this)
        {
            switch (key)
            {
                case ConsoleKey.DownArrow:
                    if (!getting_input && menuIndex < Children.Count - 2)
                    {
                        menuIndex += 2;
                    }
                    else
                    {
                        Game.Beep();
                    }
                        
                    break;
                case ConsoleKey.UpArrow:
                    if (!getting_input && menuIndex > 1)
                    {
                        menuIndex -= 2;
                    }
                    else
                    {
                        Game.Beep();
                    }
                        
                    break;
                case ConsoleKey.Enter:
                    if(!getting_input)
                    {
                        if(Children[menuIndex].name != value_audio_enabled.name) Game.Beep(1337,133);
                        Children[menuIndex].ProcessAction.Invoke();
                    }
                        
                    break;
                case ConsoleKey.Escape:
                    Game.Beep(1100, 200);
                    Game.Beep(800, 600);
                    Game.CurrentScene = new MainMenu();
                    break;
                default:
                    break;
            }
        }
    }
}
