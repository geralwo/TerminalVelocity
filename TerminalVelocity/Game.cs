namespace TerminalVelocity;
public class Game
{
    public static event ProcessEvent? ProcessTick;
    public delegate void ProcessEvent();

    public static bool quitting = false;

    public static Settings Settings = new Settings();

    private static Scene root = new Scene("root");
    private static Scene current_scene = new Scene("empty default Scene");
    public static Scene CurrentScene
    {
        get { return current_scene; }
        set
        {
            if (current_scene != null)
            {
                CurrentScene.unload();
                root.remove_child(CurrentScene);
                RenderServer.Instance.clear_scene();
                GC.Collect();
            }
            current_scene = value;
            root.add_child(CurrentScene);
        }
    }


    public static int FrameCount = 0;
    public static int RunTime = 0;
    public Game()
    {
        Console.CancelKeyPress += delegate {
            Console.Clear();
        };
        root.Visible = false;
        _init_console();

    }
    public void Run()
    {
        System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
        while (!quitting)
        {
            Input.get_input();
            ProcessTick?.Invoke();
            render();
            RunTime = (int)stopwatch.ElapsedMilliseconds;
        }

        _finished();
    }

    private static void _finished()
    {
        Console.Clear(); // when we quit be nice and clear screen
        Console.CursorVisible = true;
    }

    private void render()
    {
        RenderServer.Instance.DrawBuffer();
        FrameCount++;
        Thread.Sleep(1000 / Game.Settings.Engine.MaxFps); // needs to be because
    }


    private void _init_console()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.CursorVisible = false;
        Console.Clear();
        Console.SetCursorPosition(0,0);
    }

    public static void Beep(int freq, int milliseconds)
    {
        if (System.OperatingSystem.IsWindows())
        {
            if (Game.Settings.Engine.AudioEnabled)
                Console.Beep(freq, milliseconds);
        }

    }

    public static void Beep()
    {
        if (Game.Settings.Engine.AudioEnabled)
            Console.Beep();
    }
    
    public static ConsoleColor GetRandomConsoleColor(ConsoleColor exclude = ConsoleColor.Black)
    {
        Random rng = new Random();
        var consoleColors = Enum.GetValues(typeof(ConsoleColor));
        ConsoleColor random_color = (ConsoleColor)consoleColors.GetValue(rng.Next(consoleColors.Length));
        if(random_color == exclude)
        {
            return GetRandomConsoleColor(exclude);
        }
        return random_color;
    }
}