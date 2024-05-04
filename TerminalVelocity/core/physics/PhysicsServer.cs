namespace TerminalVelocity;
public class PhysicsServer
{
    public static event OnCollision? OnCollisionEvent;
    public delegate void OnCollision(PhysicsObject collider);
    private static PhysicsServer singleton = new PhysicsServer();
    private List<PhysicsObject> Colliders = new List<PhysicsObject>();
    private static QuadTree CollisionTree = new QuadTree();
    public static PhysicsServer Instance
    {
        get
        {
            if (singleton == null)
            {
                singleton = new PhysicsServer();
            }
            return singleton;
        }
    }
    private PhysicsServer()
    {
    }

    public static bool AddCollider(PhysicsObject obj)
    {
        if (obj == null || Instance.Colliders.Contains(obj))
        {
            return false;
        }
        Instance.Colliders.Add(obj);
        return true;

    }

    public static bool AddCollider(PhysicsArea obj)
    {
        foreach (Vec2i area_coord in obj.CollisionShape)
        {
            if (obj == null || Instance.Colliders.Contains(obj))
            {
                return false;
            }
            Instance.Colliders.Add(obj);
        }
        return true;
    }

    public static bool RemoveCollider(PhysicsObject obj)
    {
        if (Instance.Colliders.Contains(obj))
        {
            Instance.Colliders.Remove(obj);
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool RemoveCollider<T>(T obj) where T : SceneObject
    {
        if (obj is PhysicsObject po)
        {
            return Instance.Colliders.Remove(po);
        }
        if (obj is PhysicsArea pa)
        {
            return Instance.Colliders.Remove(pa);
        }
        return false;
    }
    public static void Step()
    {
        lock (Instance)
        {
            int timer = Game.RunTime;
            CollisionTree = new QuadTree(Vec2i.ZERO, Game.Settings.Engine.WindowSize);
            var colDup = new List<PhysicsObject>(Instance.Colliders);
            foreach (var obj in colDup)
            {
                if (obj == null) continue;
                core.Debug.AddImportantEntry($"Going through {obj.name} -> p:{obj.Position} v:{obj.Velocity} IsColliding:{obj.IsColliding}", Instance);
                // in Step() we go through each object and insert it into the quad tree.
                // we want to only check for collisions on objects that have a velocity != 0
                core.Debug.AddImportantEntry($"Trying to insert {obj.CollisionShape.Length} shape(s) into CollisionTree", Instance);
                var collisionShape = obj.CollisionShape;
                for (int i = 0; i < collisionShape.Length; i++)
                {
                    var posInTree = obj.Position + collisionShape[i];
                    core.Debug.AddImportantEntry("Inserting Shape " + collisionShape[i] + " into " + posInTree, Instance);
                    CollisionTree.Insert(posInTree, obj);
                }
            }
            foreach (var obj in colDup)
            {
                obj.IsColliding = false;
                if (obj.Velocity != Vec2i.ZERO)
                    checkCollision(obj);
                obj.Position += obj.Velocity;
                obj.Velocity = obj.Velocity.StepToZero();
            }
            core.Debug.AddImportantEntry($"Step took {Game.RunTime - timer}µs", Instance);
        }
    }

    private static void checkCollision(PhysicsObject obj)
    {
        TerminalVelocity.core.Debug.AddImportantEntry("Checking collision for " + obj.name, Instance);

        // Keep track of points that have already been queried
        var queriedPositions = new HashSet<Vec2i>();
        var newPosition = obj.Position + obj.Velocity;
        CollisionInfo colinfo = new CollisionInfo();
        //obj.BackgroundColor = ConsoleColor.Blue;
        colinfo.colliders.Add(obj);
        foreach (Vec2i shapeOffset in obj.CollisionShape)
        {
            var positionToCheck = shapeOffset + newPosition;

            // Only query if this position hasn't been checked already
            if (!queriedPositions.Contains(positionToCheck))
            {
                queriedPositions.Add(positionToCheck);

                core.Debug.AddImportantEntry($"Checking {positionToCheck} for collisions", Instance);
                if (CollisionTree.Query(positionToCheck, out PhysicsObject[] queryResult))
                {
                    // Process collision results
                    foreach (PhysicsObject collider in queryResult)
                    {
                        if (collider.id != obj.id)
                        {
                            if (obj.CollisionIgnoreFilter.Contains(collider.name)) continue;
                            var ignoreNames = obj.CollisionIgnoreFilter.Where(x => true);
                            core.Debug.AddImportantEntry($"OBJECT COLLISION: obj {obj.name} {obj.Position} [{string.Join(", ", ignoreNames)}] collided with {collider.name} {collider.Position} trying to go to {obj.Position + obj.Velocity}", obj);
                            colinfo.colliders.Add(collider);
                            if (collider is not PhysicsArea)
                            {
                                obj.Velocity = Vec2i.ZERO;
                                obj.IsColliding = true;
                                collider.IsColliding = true;
                            }
                            obj.OnCollision(colinfo);
                            collider.OnCollision(colinfo);
                            if (obj is not PhysicsArea)
                                break; // No need to continue if collision is detected
                        }
                    }
                }
            }
        }
    }

    public static void CheckCollision(PhysicsArea obj)
    {
        TerminalVelocity.core.Debug.Log("Checking area collision for " + obj.name, Instance);

        // Keep track of points that have already been queried
        var queriedPositions = new HashSet<Vec2i>();
        var newPosition = obj.Position + obj.Velocity;
        CollisionInfo colinfo = new CollisionInfo();
        //obj.BackgroundColor = ConsoleColor.Blue;
        colinfo.colliders.Add(obj);
        foreach (Vec2i shapeOffset in obj.CollisionShape)
        {
            var positionToCheck = shapeOffset + newPosition;

            // Only query if this position hasn't been checked already
            if (!queriedPositions.Contains(positionToCheck))
            {
                queriedPositions.Add(positionToCheck);

                core.Debug.AddImportantEntry($"[called from GameLogic] Checking {obj.name} {positionToCheck} for collisions", Instance);

                if (CollisionTree.Query(positionToCheck, out PhysicsObject[] queryResult))
                {
                    // Process collision results
                    foreach (PhysicsObject collider in queryResult)
                    {
                        if (collider.id != obj.id)
                        {
                            if (obj.CollisionIgnoreFilter.Contains(collider.name)) continue;
                            var ignoreNames = obj.CollisionIgnoreFilter.Where(x => true);
                            core.Debug.AddImportantEntry($"OBJECT COLLISION: obj {obj.name} {obj.Position} [{string.Join(", ", ignoreNames)}] collided with {collider.name} {collider.Position}", obj);
                            colinfo.colliders.Add(collider);
                            if (collider is not PhysicsArea)
                            {
                                obj.Velocity = Vec2i.ZERO;
                                obj.IsColliding = true;
                                collider.IsColliding = true;
                            }
                            obj.OnCollision(colinfo);
                            collider.OnCollision(colinfo);
                            if (obj is not PhysicsArea)
                                break; // No need to continue if collision is detected
                        }
                    }
                }
            }
        }
    }

    public static void Raycast(Vec2i from, Vec2i to, out CollisionInfo result)
    {
        var colResult = new CollisionInfo();
        Vec2i.GetLine(from, to).ForEach(x =>
        {
            if (CollisionTree.Query(x, out PhysicsObject[] queryResult))
                foreach (var obj in queryResult)
                {
                    colResult.colliders.Add(obj);
                }
        });
        result = colResult;
    }

    public struct CollisionInfo
    {
        // CollisionInfo holds all the objects and information about the collision
        public bool is_valid = true;

        public List<PhysicsObject> colliders = new List<PhysicsObject>();

        public CollisionInfo()
        {
        }
    }

    private static SceneObject? quadTreeVisuals;
    public static void ToggleQuadTreeVisuals()
    {
        if (quadTreeVisuals == null)
            quadTreeVisuals = CollisionTree.Visualize();
        else
        {
            Game.CurrentScene.RemoveChild(quadTreeVisuals);
            quadTreeVisuals = null;
        }
    }

}
