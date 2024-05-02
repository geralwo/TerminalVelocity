using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace TerminalVelocity
{
    public class QuadTree
    {
        public static int DefaultCapacity = 16;
        public AABB Aabb { get; private set; } // Ensure this property is properly set
        public int Capacity { get; set; } = DefaultCapacity; // Settable property
        public Dictionary<Vec2i,PhysicsObject> Items { get; private set; } = new Dictionary<Vec2i,PhysicsObject>();
        public bool Divided { get; private set; } = false; // Private setter to prevent external modification
        public QuadTree[] SubTrees { get; private set; } = new QuadTree[4]; // Avoid null pointer issues

        // Default constructor that initializes the AABB to avoid NullReferenceException
        public QuadTree()
        {
            Aabb = new AABB(); // Default AABB initialization, adjust if needed
        }

        public QuadTree(Vec2i position, Vec2i size, int capacity)
        {
            Capacity = capacity;
            Aabb = new AABB(position, size); // Ensure AABB is initialized
        }
        public QuadTree(Vec2i position, Vec2i size)
        {
            Aabb = new AABB(position, size); // Ensure AABB is initialized
        }

        public bool Insert(Vec2i position, PhysicsObject obj)
        {
            if (!Aabb.Contains(obj.Position))
            {
                TerminalVelocity.core.Debug.AddDebugEntry($"Position {obj.Position} is out of bounds for object {obj.name}  <- {Aabb.Center}", this);
                return false;
            }

            if (!Divided && Items.Count < Capacity)
            {
                if(Items.TryAdd(position,obj))
                {
                    TerminalVelocity.core.Debug.AddImportantEntry($"Inserted object {obj.name} at {position} <- {Aabb.Center}", this);
                    return true;
                }
                else Subdivide();
            }

            if (!Divided)
            {
                Subdivide();
            }

            // Insert into appropriate sub-quadtree
            foreach (var subtree in SubTrees)
            {
                if (subtree.Aabb.Contains(obj.Position) && subtree.Insert(position, obj))
                {
                    return true;
                }
            }

            TerminalVelocity.core.Debug.AddDebugEntry($"failed to insert object {obj.name} <- {Aabb.Center}", this);
            return false;
        }

        private void Subdivide()
        {
            var halfSize = Aabb.Size / 2;

            var remainderX = Aabb.Size.x % 2;
            var remainderY = Aabb.Size.y % 2;

            for (var i = 0; i < 4; i++)
            {
                var offset = new Vec2i(
                    (i & 1) * halfSize.x,
                    ((i >> 1) & 1) * halfSize.y
                );

                var childWidth = halfSize.x + (i % 2 == 0 ? remainderX : 0);
                var childHeight = halfSize.y + (i >= 2 ? remainderY : 0);

                var childPosition = Aabb.Position + offset;

                SubTrees[i] = new QuadTree(childPosition, new Vec2i(childWidth, childHeight), Capacity);
            }

            Divided = true;
        }

        public SceneObject Visualize(int z = 0)
        {
            var vis = new SceneObject { Visible = false };
            var visAabb = Aabb.GetVisual(z);
            vis.AddChild(visAabb);
            foreach (var item in Items.Keys)
            {
                SceneObject i = new SceneObject("x");
                i.Color = ConsoleColor.Green;
                i.Position = Items[item].Position;
                vis.AddChild(i);
            }
            // Add quadtree size and information
            vis.AddChild(new SceneObject(Aabb.Center, Aabb.Size.ToString()));

            if (Divided)
            {
                for (int i = 0; i < 4; i++)
                {
                    vis.AddChild(SubTrees[i].Visualize(z + 1));
                }
            }

            return vis;
        }

        public bool Query(Vec2i position, out PhysicsObject[] queryResult)
        {

            var result = new List<PhysicsObject>();
            if (Aabb.Contains(position))
            {
                if(Items.ContainsKey(position))
                    result.Add(Items[position]);

                if (Divided)
                {
                    foreach (var subtree in SubTrees)
                    {
                        if (subtree.Query(position, out var subtreeResults))
                        {
                            result.AddRange(subtreeResults);
                        }
                    }
                }
            }

            queryResult = result.ToArray();
            TerminalVelocity.core.Debug.AddDebugEntry($"Query result length: {queryResult.Length}", this);
            return queryResult.Length > 0;
        }
    }
}
