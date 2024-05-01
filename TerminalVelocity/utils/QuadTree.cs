
namespace TerminalVelocity;

public class QuadTree
{
    public AABB Aabb;
    public int Capacity;
    public Dictionary<Vec2i,SceneObject> Items = new Dictionary<Vec2i,SceneObject>();
    public bool Divided { get; private set; } = false;
    public QuadTree[] SubTrees = new QuadTree[4];

    public QuadTree()
    {
    }
    public QuadTree(Vec2i _position, Vec2i _size, int _capacity)
    {
        Capacity = _capacity;
        Aabb.Position = _position;
        Aabb.Size = _size;
    }

    public bool Insert(Vec2i _position, SceneObject obj)
    {
        if (!Aabb.Contains(obj.Position))
            return false;
        if (!Divided && Items.Count < Capacity)
        {
            Items.TryAdd(obj.Position,obj);
            return true;
        }
        if (!Divided)
            Subdivide();

        foreach (QuadTree qt in SubTrees)
        {
            if (qt.Insert(obj.Position,obj))
            {
                return true;
            }
        }
        return false;
    }

    private void Subdivide()
    {
        var half_dim = Aabb.Size / 2;

        var remainderX = Aabb.Size.x % 2;
        var remainderY = Aabb.Size.y % 2;

        for (var i = 0; i < 4; i++)
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

    public SceneObject Visualize(int z = 0)
    {
        var vis = new SceneObject();
        vis.Visible = false;
        var ab = this.Aabb.GetVisual(z);
        vis.add_child(ab);
        //vis.add_child(new SceneObject(this.Aabb.Center,Items.Count.ToString()));
        vis.add_child(new SceneObject(this.Aabb.Center, Aabb.Size.ToString()));
        if (Divided)
        {
            for (int i = 0; i < 4; i++)
            {
                vis.add_child(SubTrees[i].Visualize(z + 1));
            }
        }
        return vis;
    }

    public bool Query(Vec2i position, out SceneObject[] queryResult)
    {
        List<SceneObject> queryResultList = new List<SceneObject>();

        if (Aabb.Contains(position))
        {
            if (Items.TryGetValue(position, out SceneObject? sceneObject))
            {
                queryResultList.Add(sceneObject);
            }

            if (Divided)
            {
                foreach (var subtree in SubTrees)
                {
                    if (subtree.Query(position, out SceneObject[] subTreeResult))
                    {
                        queryResultList.AddRange(subTreeResult);
                    }
                }
            }
        }

        queryResult = queryResultList.ToArray();
        return queryResult.Length > 0;
    }
}