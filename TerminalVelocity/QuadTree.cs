namespace TerminalVelocity;

public class QuadTree
{
    public AABB Aabb;
    public int Capacity;
    public List<SceneObject> Items = new List<SceneObject>();
    public bool Divided { get; private set; } = false;
    public QuadTree[]? SubTrees;
    
    public QuadTree(Vec2i _position, Vec2i _size, int _capacity)
    {
        Capacity = _capacity;
        Aabb.Position = _position;
        Aabb.Size = _size;
    }

    public bool Insert(SceneObject obj)
    {
        if (!Aabb.Contains(obj.Position))
            return false;
        if (!Divided && Items.Count < Capacity)
        {
            Items.Add(obj);
            return true;
        }
        if(!Divided)
            Subdivide();
        
        foreach (QuadTree qt in SubTrees)
        {
            if(qt.Insert(obj))
            {
                return true;
            }
        }
        return false;
    }

private void Subdivide()
{
    SubTrees = new QuadTree[4];
    var half_dim = Aabb.Size / 2;
    
    var remainderX = Aabb.Size.x % 2;
    var remainderY = Aabb.Size.y % 2;
    
    for (var i = 0; i < 4; i++)
    {
        var offset = new Vec2i(
            (i & 1) * half_dim.x,
            ((i >> 1) & 1) * half_dim.y
        );

        // we do this for odd divisions so the childs cover the whole parent when an axis is odd
        var childWidth = half_dim.x + (i % 2 == 0 ? remainderX : 0);
        var childHeight = half_dim.y + (i >= 2 ? remainderY : 0);

        var child_pos = Aabb.Position + offset;
        SubTrees[i] = new QuadTree(child_pos, new Vec2i(childWidth, childHeight), Capacity);
    }

    Divided = true;
}

    public SceneObject visualize(int z = 0)
    {
        var vis = new SceneObject();
        vis.Visible = false;
        var ab = this.Aabb.GetVisual(z);
        vis.add_child(ab);
        //vis.add_child(new SceneObject(this.Aabb.Center,Items.Count.ToString()));
        vis.add_child(new SceneObject(this.Aabb.Center,Aabb.Size.ToString()));
        if(Divided)
        {
            for(int i=0;i<4;i++)
            {
                vis.add_child(SubTrees[i].visualize(z + 1));
            }
        }
        return vis;
    }
}