using System.Diagnostics;

namespace TerminalVelocity;
using TerminalVelocity.core;
public class EngineSettings
{
    public Vec2i WindowSize = new Vec2i(Console.WindowWidth, Console.WindowHeight);
    public bool AudioEnabled = false;
    public long MaxFps = 1000 / 32;

    public Debug.LogLevel Log = Debug.LogLevel.Game;
}
