using TerminalVelocity;

public interface IAI
{
    public enum State {};
    public void Move(Vec2i _position);
    public void InteractWith<T>(T target);
}