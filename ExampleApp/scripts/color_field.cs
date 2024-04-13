using TerminalVelocity;

public class ColorField : PhysicsArea
{
    public static SceneObject[] fence;
    public ColorField(ConsoleColor _color, Vec2i _position)
    {
        BackgroundColor = _color;
        Position = _position;
        Display = " ";
    }

    public override void on_collision(PhysicsServer.CollisionInfo collisionInfo)
    {
        collisionInfo.colliders.FindAll(obj => obj != this).ForEach(obj => {
            obj.BackgroundColor = this.BackgroundColor;
            obj.CollisionIgnoreFilter.Add("keyfence");
        });
    }
}