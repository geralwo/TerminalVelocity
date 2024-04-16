using TerminalVelocity;

public class ColorField : PhysicsArea
{
    public ColorField(ConsoleColor _color, Vec2i _position)
    {
        BackgroundColor = _color;
        Position = _position;
        Display = " ";
    }

    public override void on_collision(PhysicsServer.CollisionInfo collisionInfo)
    {
        collisionInfo.colliders.FindAll(obj => obj != this).ForEach(obj => {
            obj.CollisionIgnoreFilter.Clear();
            obj.BackgroundColor = this.BackgroundColor;
            obj.CollisionIgnoreFilter.Add(this.BackgroundColor.ToString());
            obj.ForegroundColor = ConsoleColor.Black;
        });
    }
}