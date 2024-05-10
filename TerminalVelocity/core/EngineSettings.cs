namespace TerminalVelocity;
public class EngineSettings
{
    public Vec2i WindowSize = new Vec2i(Console.WindowWidth, Console.WindowHeight);
    public bool AudioEnabled = false;
    public long MaxFps = 1000 / 120;
    public LogLevel LogLevel = LogLevel.Game;

    public EngineSettings() 
    {
        if(System.OperatingSystem.IsWindows())
        {
            WindowSize -= 1;
        }
    }
}
