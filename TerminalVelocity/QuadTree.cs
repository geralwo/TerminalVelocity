namespace TerminalVelocity;

public class QuadTree
{
    public AABB Aabb;
    public int Capacity;
    public List<SceneObject> Items = new List<SceneObject>();
    public bool Divided { get; private set; } = false;
    public QuadTree[] SubTrees;
    
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
        for(var i = 0; i < 4;i++)
        {
            #region bitwise expl
            // (i & 1) is a bitwise AND operation between the integer variable i and the constant 1.
            // In binary representation, 1 is 00000001. When you perform a bitwise AND operation with any number and 1, it will keep only the least significant bit of that number.
            // Here's what happens:
            //     If the least significant bit of i is 1, the result will be 1.
            //     If the least significant bit of i is 0, the result will be 0.
            // In essence, (i & 1) checks if i is odd or even. If i is odd, the result will be 1; if i is even, the result will be 0.


            //     ((i >> 1) & 1) is a bit more complex but follows a similar logic.
            //     - i >> 1: This is a bitwise right shift operation. It shifts all the bits of i one place to the right.
            //      For example:
            //          If i is 5 (101 in binary), i >> 1 would result in 2 (10 in binary).
            //          If i is 8 (1000 in binary), i >> 1 would result in 4 (100 in binary).
            //     - & 1: This is a bitwise AND operation with 1, similar to the explanation in the previous message.
            //      So, ((i >> 1) & 1) essentially checks the bit that is shifted out by the right shift operation. It checks if the bit that was originally in the second least significant position of i (after shifting) is 1 or 0.
            //      If it's 1, the result will be 1.
            //      If it's 0, the result will be 0.

            // This operation is commonly used to extract specific bits from a binary representation. In the context of your original code, it's used to determine if the second least significant bit of i is set or not.
            #endregion
           var offset = new Vec2i(
                (i & 1) * half_dim.x - half_dim.x,
                ((i >> 1) & 1) * half_dim.y - half_dim.y
            );
           var child_pos = Aabb.Position + offset + half_dim;
           SubTrees[i] = new QuadTree(child_pos,half_dim,Capacity);
        }
        Divided = true;
    }

    public SceneObject visualize()
    {
        var vis = new SceneObject();
        vis.Visible = false;
        vis.add_child(this.Aabb.GetVisual());
        if(Divided)
        {
            for(int i=0;i<4;i++)
            {
                vis.add_child(SubTrees[i].visualize());
            }
        }
        return vis;
    }
}