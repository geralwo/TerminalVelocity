namespace TerminalVelocity;
public class Game
{
    public static event ProcessEvent? ProcessTick;
    public delegate void ProcessEvent();

    public static bool quitting = false;

    public static GameSettings Settings = new GameSettings();

    private static Scene root = new Scene();
    private static Scene current_scene = new Scene();
    public static Scene CurrentScene
    {
        get { return current_scene; }
        set
        {
            if (current_scene != null)
            {
                CurrentScene.unload();
                root.remove_child(CurrentScene);
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
        root.Visible = false;
        _init_console();
    }

    private bool frame_completed = false;
    public void Run()
    {
        System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
        while (!quitting)
        {
            ProcessTick?.Invoke();
            Input.get_input();
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
        Thread.Sleep(1000 / Game.Settings.MaxFps);
    }


    private void _init_console()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.CursorVisible = false;
        Console.Clear();
    }

    public static void Beep(int freq, int milliseconds)
    {
        if (System.OperatingSystem.IsWindows())
        {
            if (Game.Settings.audio_enabled)
                Console.Beep(freq, milliseconds);
        }

    }

    public static void Beep(int freq, int milliseconds, bool force = false)
    {
        if (System.OperatingSystem.IsWindows())
        {
            if (Game.Settings.audio_enabled)
                Console.Beep(freq, milliseconds);
        }
        else if (force)
        {
            Console.Beep();
        }

    }

    public static void Beep()
    {
        if (Game.Settings.audio_enabled)
            Console.Beep();
    }

    public static event OnKeyPressed? KeyPressed;
    public delegate void OnKeyPressed(ConsoleKey key);
}

