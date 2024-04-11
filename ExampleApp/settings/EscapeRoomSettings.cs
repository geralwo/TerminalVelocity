using TerminalVelocity;
public class EscapeRoomSettings
{
    public EscapeRoomSettings()
    {}

    public static Vec2i RoomSize = new Vec2i(Console.WindowHeight,Console.WindowHeight > Console.WindowWidth ? Console.WindowWidth : Console.WindowHeight);
    public static string PlayerChar = "*";
}