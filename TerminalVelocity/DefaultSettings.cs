using TerminalVelocity;

public class DefaultSettings : GameSettings
{
    public string player_char = "@";
    public Vec2i lvl_size = new Vec2i(16,16);

    public int MaxFps = 60;

    public bool AudioEnabled = false;
    public DefaultSettings()
    {
    }
} 