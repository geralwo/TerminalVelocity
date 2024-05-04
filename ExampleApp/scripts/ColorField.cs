using TerminalVelocity;

public class ColorField : PhysicsArea
{
    public ColorField(ConsoleColor _color, Vec2i _position) : base(_position, Vec2i.ONE)
    {
        BackgroundColor = _color;
        Display = " ";
        Position = _position;
        name = _color.ToString();
    }

    public override void OnCollision(PhysicsServer.CollisionInfo collisionInfo)
    {
        collisionInfo.colliders.FindAll(obj => obj != this).ForEach(obj =>
        {
            obj.CollisionIgnoreFilter.Clear();
            obj.BackgroundColor = this.BackgroundColor;
            obj.CollisionIgnoreFilter.Add(this.BackgroundColor.ToString());
            obj.Color = ConsoleColor.Black;
        });
    }
}
