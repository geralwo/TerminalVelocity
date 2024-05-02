using TerminalVelocity;

public interface IBrain
{
    public enum State {};
    public void Move(Vec2i _position);
    public void InteractWith<T>(T target);
    public int AD {get;set;}

    public int HP {get;set;}
    public Guid id {get;set;}
}