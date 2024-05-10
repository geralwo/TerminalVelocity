using TerminalVelocity;

public class ColorField : PhysicsArea
{
    public ColorField(ConsoleColor _color, Vec2i _position) : base(_position, Vec2i.ONE)
    {
        BackgroundColor = _color;
        Display = " ";
        Position = _position;
        name = _color.ToString();
        CollisionAction += (CollisionInfo collisionInfo) =>
        {
            collisionInfo.Colliders.FindAll(obj => obj != this).ForEach(obj =>
            {
                obj.CollisionIgnoreFilter.Clear();
                obj.BackgroundColor = this.BackgroundColor;
                obj.CollisionIgnoreFilter.Add(this.BackgroundColor.ToString());
                obj.Color = ConsoleColor.Black;
            });
        };
    }
}
