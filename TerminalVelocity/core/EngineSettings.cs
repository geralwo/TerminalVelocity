namespace TerminalVelocity;
public class EngineSettings
{
    public Vec2i WindowSize = new Vec2i(Console.WindowWidth, Console.WindowHeight);
    public bool AudioEnabled = false;
    public float MaxFps = 1000 / 120;
    public LogLevel LogLevel = LogLevel.Game;
}
