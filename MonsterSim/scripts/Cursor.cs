using TerminalVelocity;

public class Cursor
{
    public int X = Console.CursorTop;
    public int Y = Console.CursorLeft;

    public Vec2i CursorPosition
    {
        get => new Vec2i(X, Y);
        private set
        {
                X = value.x;
                Y = value.y;
        }
    }

    public void MoveCursorTo(Vec2i _position)
    {
        CursorPosition = _position;
    }
}