namespace TerminalVelocity;
public class Game
{
    /// <summary>
    /// This event gets fired every execution of Game.Run()
    /// </summary>
    public static event ProcessEvent? ProcessTick;
    public delegate void ProcessEvent();

    /// <summary>
    /// If Game.Quit is set to true, the game loop breaks and the Game is terminated
    /// </summary>
    public static bool Quit = false;
    public static LogLevel LogLevel = LogLevel.None; // init global logger to no logging
    /// <summary>
    /// Partial implementation of a Settings class to use for basic engine needs and game needs<br />
    /// - not finished -
    /// </summary>
    public static Settings Settings = new Settings();
    /// <summary>
    /// The root scene of the game. Every object is a child of this.
    /// <br /> You are not supposed to interact with this.
    /// </summary>
    private static Scene root = new Scene("root");
    private static Scene current_scene = new Scene("empty default Scene");
    /// <summary>
    /// Reference to the current executing scene.
    /// When changing this value, the old scene gets disposed, the screen is cleared<br />
    /// and the new value is added as a child to the root and started.
    /// </summary>
    public static Scene CurrentScene
    {
        get { return current_scene; }
        set
        {
            if (current_scene != null)
            {
                CurrentScene.unload();
                root.RemoveChild(CurrentScene);
                RenderServer.ClearScene();
                GC.Collect();
            }
            current_scene = value;
            root.AddChild(CurrentScene);
        }
    }
    /// <summary>
    /// Frames drawn since starting the game.
    /// </summary>
    public static int FrameCount = 0;
    /// <summary>
    /// RunTime in milliseconds since starting the game.
    /// </summary>
    public static int RunTime = 0;
    /// <summary>
    /// Not implemented
    /// </summary>
    public long DeltaTime = 0;
    public Game()
    {
        Console.CancelKeyPress += delegate
        {
            Console.Clear();
        };
        root.Visible = false;
        _init_console();

    }
    /// <summary>
    /// Main Game loop <br />
    /// Input is its own thread to enable ConsoleReads.
    /// PhysicsServer MainThread
    /// RenderServer MainThread but with locks
    /// </summary>
    public void Run(Scene _startScene)
    {
        core.Debug.CurrentLogLevel = Game.LogLevel;
        Game.CurrentScene = _startScene;
        System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
        Input.StartInputListener();
        while (!Quit)
        {
            var frameStart = stopwatch.ElapsedMilliseconds;
            PhysicsServer.Step();
            ProcessTick?.Invoke();
            render();
            var frameEnd = stopwatch.ElapsedMilliseconds;
            var frameDuration = frameEnd - frameStart;

            if (frameDuration < Game.Settings.Engine.MaxFps)
            {
                Thread.Sleep((int)(Game.Settings.Engine.MaxFps - frameDuration));
            }

            RunTime = (int)stopwatch.ElapsedMilliseconds;

        }
        TerminalVelocity.core.Debug.PrintToFile();
        _finished();
    }
    /// <summary>
    /// Clean up method for quitting the game
    /// </summary>
    private static void _finished()
    {
        Console.CursorVisible = true;
        Console.Clear(); // when we quit be nice and clear screen
    }
    /// <summary>
    /// Calls RenderServer.DrawBuffer, adds one to the frame count and sleeps for an amount of milliseconds to limit FPS
    /// </summary>
    private void render()
    {
        if (RenderServer.IsReady)
        {
            RenderServer.DrawBuffer();
        }
        //Thread.Sleep(1000 / Game.Settings.Engine.MaxFps); // needs to be because
    }


    private void _init_console()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.CursorVisible = false;
        Console.Clear();
        Console.SetCursorPosition(0, 0);
    }
    /// <summary>
    /// Cross platform Beep function.
    /// Only plays a sound when the platform is windows and Game.AudioEnabled is true.
    /// </summary>
    /// <param name="freq"></param>
    /// <param name="milliseconds"></param>
    public static void Beep(int freq, int milliseconds)
    {
        if (System.OperatingSystem.IsWindows())
        {
            if (Game.Settings.Engine.AudioEnabled)
                Console.Beep(freq, milliseconds);
        }

    }
    /// <summary>
    /// Beeps when Game.AudioEnabled is true
    /// </summary>
    public static void Beep()
    {
        if (Game.Settings.Engine.AudioEnabled)
            Console.Beep();
    }
    /// <summary>
    /// Returns a random ConsoleColor
    /// </summary>
    /// <param name="exclude">ConsoleColor exclude</param>
    /// <returns>A random ConsoleColor excluding exclude</returns>
    public static ConsoleColor GetRandomConsoleColor(ConsoleColor exclude = ConsoleColor.Black)
    {
        Random rng = new Random();
        var consoleColors = Enum.GetValues(typeof(ConsoleColor));
#pragma warning disable CS8605 // Unboxing a possibly null value.
        ConsoleColor random_color = (ConsoleColor)consoleColors.GetValue(rng.Next(consoleColors.Length));
#pragma warning restore CS8605 // Unboxing a possibly null value.
        if (random_color == exclude)
        {
            return GetRandomConsoleColor(exclude);
        }
        return random_color;
    }
    /// <summary>
    /// Prints basic engine stats:<br />
    /// - Game.FrameCount<br />
    /// - Game.RunTime<br />
    /// - Game.FrameCount / Game.RunTime / 1000<br />
    /// - RenderServer.Count -> the amount of objects drawn last frame
    /// </summary>
    public static void PrintEngineStats()
    {
        Console.ResetColor();
        Console.Clear();
        Console.WriteLine(EngineStats);
    }

    public static string EngineStats
    {
        get
        {
            string str = "";
            str += $"{DateTime.Now}\n";
            str += $"total frames   : {Game.FrameCount}\n";
            str += $"total run time : {Game.RunTime} ms\n";
            str += $"average fps    : {Game.FrameCount / (Game.RunTime / 1000)}\n";
            str += $"render items   : {RenderServer.Count()}\n";
            str += $"last frameTime : {RenderServer.FrameTimeInMicroseconds}µs\n";
            str += $"window size    : {Game.Settings.Engine.WindowSize}\n";
            return str;
        }
    }
}
