namespace TerminalVelocity;
public struct Settings
{
    public EngineSettings Engine;
    public GameSettings? User;
    public Settings()
    {
        Engine = new EngineSettings();
    }
}
    