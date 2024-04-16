namespace TerminalVelocity;

public struct AABB
{
    public Vec2i Position = Vec2i.ZERO;
    public Vec2i Size = Vec2i.ZERO;

    public Vec2i End => Position + Size;

    public Vec2i Center => Position + Size / 2;

    public AABB(){}

    public AABB(AABB _aabb)
    {
        Position = _aabb.Position;
        Size = _aabb.Size;
    }

    public AABB(Vec2i _position, Vec2i _size)
    {
        Position = _position;
        Size = _size;
    }

    public AABB(int x, int y, int w, int h)
    {
        Position = new Vec2i(x, y);
        Size = new Vec2i(w, h);
    }

    public bool Contains(Vec2i _pos)
    {
        return _pos.x >= Position.x && _pos.x <= End.x &&
               _pos.y >= Position.y && _pos.y <= End.y;
    }
    
    public bool Contains(AABB _aabb)
    {
        return _aabb.Position.x >= Position.x && _aabb.End.x <= End.x &&
               _aabb.Position.y >= Position.y && _aabb.End.y <= End.y;
    }
    
    public SceneObject GetVisual()
    {
        var bounds = new SceneObject();
        ConsoleColor rndc = Game.GetRandomConsoleColor();
        bounds.Visible = false;
        for (int y = 0; y < Size.y; y++)
        {
            for (int x = 0; x < Size.x; x++)
            {
                if (y == 0 || x == 0 || x == Size.x -1 || y == Size.y - 1)
                {
                    Vec2i offset = new Vec2i(x, y);
                    Vec2i global_pos = Position + offset;
                    SceneObject line = new SceneObject(global_pos,":");
                    line.BackgroundColor = rndc;
                    line.ZIndex = -10;
                    bounds.add_child(line);
                }
                
            }
        }
        var center = new SceneObject("o");
        center.BackgroundColor = ConsoleColor.Black;
        center.Position = Center;
        bounds.add_child(center);
        return bounds;
    }
    
}