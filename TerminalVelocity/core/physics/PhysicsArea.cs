namespace TerminalVelocity;
public class PhysicsArea : PhysicsObject
{
    //public bool IsSolid = false;
    public AABB Area;
    public new Vec2i[] CollisionShape
    {
        get => GetLocalAreaCoordinates();
    }
    public PhysicsArea(Vec2i _position, Vec2i _size)
    {
        IsSolid     = false;
        Display     = " ";
        BackgroundColor = ConsoleColor.Magenta;
        Position    = _position;
        Area        = new AABB(Position, _size);
        PhysicsServer.AddCollider(this);
    }

    private Vec2i[] GetLocalAreaCoordinates()
    {
        var coords = new List<Vec2i>();
        for (int x = 0; x < Area.Size.x; x++)
        {
            for (int y = 0; y < Area.Size.y; y++)
            {
                coords.Add(new Vec2i(x-Area.Size.x / 2, y-Area.Size.y / 2));
            }
        }
        return coords.ToArray();
    }
}