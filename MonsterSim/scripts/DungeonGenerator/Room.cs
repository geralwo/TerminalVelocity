using TerminalVelocity;
public class Room : SceneObject
{
    public Vec2i Size;
    public List<SceneObject> Doors = new();
    public SceneObject[] Walls;
    public AABB Box;
    public Room(Vec2i _size)
    {
        Visible = false;
        Size = _size + Vec2i.New(2, 2);
        Box = new AABB(Position, Size);
        Walls = new SceneObject[Size.x * Size.y - ((Size.x - 1) * (Size.y - 1))];
    }

    private void constructWalls()
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                if (y == 0 || x == 0 || x == Size.x - 1 || y == Size.y - 1)
                {
                    var position = new Vec2i(x, y);
                    var wall = new PhysicsObject(position);
                    wall.BackgroundColor = ConsoleColor.Green;
                    Walls.Append(wall);
                    AddChild(wall);
                }
            }
        }
    }

    public override void OnStart()
    {
        constructWalls();
        base.OnStart();
    }

    private Vec2i randomDoorPosition()
    {
        Random random = new Random();
        int side = random.Next(4);
        var result = Vec2i.ZERO;
        switch (side)
        {
            case 0: // Top
                result = new Vec2i(random.Next(1, Size.x - 1), 0);
                break;
            case 1: // Right
                result = new Vec2i(Size.x - 1, random.Next(1, Size.y - 1));
                break;
            case 2: // Bottom
                result = new Vec2i(random.Next(1, Size.x - 1), Size.y - 1);
                break;
            case 3: // Left
                result = new Vec2i(0, random.Next(1, Size.y - 1));
                break;
        }
        return result;
    }
}
