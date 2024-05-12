namespace TerminalVelocity;
public class PhysicsServer
{
    public static event OnCollision? OnCollisionEvent;
    public delegate void OnCollision(CollisionInfo colInfo);
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
            return false;
        Instance.Colliders.Add(obj);
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
            return false;
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
        lock (Instance.Colliders)
        {
            System.Diagnostics.Stopwatch time = System.Diagnostics.Stopwatch.StartNew();
            CollisionTree = new QuadTree(Vec2i.ZERO, Game.Settings.Engine.WindowSize);
            var colDup = new List<PhysicsObject>(Instance.Colliders);
            foreach (var obj in colDup)
            {
                if (obj == null) continue;
                common.Logger.AddPhysicsEntry($"Iterating over {obj.name} -> p:{obj.Position} v:{obj.Velocity} IsColliding:{obj.IsColliding}", Instance);
                // in Step() we go through each object and insert it into the quad tree.
                // we want to only check for collisions on objects that have a velocity != 0
                common.Logger.AddPhysicsEntry($"Trying to insert {obj.CollisionShape.Length} shape(s) into CollisionTree", Instance);
                var collisionShape = obj.CollisionShape;
                for (int i = 0; i < collisionShape.Length; i++)
                {
                    var posInTree = obj.Position + collisionShape[i];
                    common.Logger.AddPhysicsEntry("Trying to insert Shape " + collisionShape[i] + " into " + posInTree, Instance);
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
            time.Stop();
            common.Logger.AddPhysicsEntry($"Step took {time.Elapsed.Microseconds}µs", Instance);
        }
    }

    private static void checkCollision(PhysicsObject obj)
    {
        TerminalVelocity.common.Logger.AddPhysicsEntry("Checking collision for " + obj.name, Instance);

        // Keep track of points that have already been queried
        var queriedPositions = new HashSet<Vec2i>();
        var newPosition = obj.Position + obj.Velocity;

        foreach (Vec2i shapeOffset in obj.CollisionShape)
        {
            var positionToCheck = shapeOffset + newPosition;

            // Only query if this position hasn't been checked already
            if (!queriedPositions.Contains(positionToCheck))
            {
                queriedPositions.Add(positionToCheck);

                common.Logger.AddPhysicsEntry($"Checking {positionToCheck} for collisions", Instance);
                if (CollisionTree.Query(positionToCheck, out PhysicsObject[] queryResult))
                {
                    // Process collision results
                    foreach (var collider in queryResult)
                    {
                        if (collider.id != obj.id)
                        {
                            if (obj.CollisionIgnoreFilter.Contains(collider.name)) continue;

                            var ignoreNames = obj.CollisionIgnoreFilter.Where(x => true);
                            common.Logger.AddPhysicsEntry($"OBJECT COLLISION: obj {obj.name} {obj.Position} [{string.Join(", ", ignoreNames)}] collided with {collider.name} {collider.Position} trying to go to {obj.Position + obj.Velocity}", obj);
                            if (collider is not PhysicsArea)
                            {
                                if (obj is not PhysicsArea)
                                    obj.Velocity = Vec2i.ZERO;
                                obj.IsColliding = true;
                                collider.IsColliding = true;
                            }

                            CollisionInfo collider_info = new CollisionInfo();
                            collider_info.Colliders.Add(obj);
                            collider.CollisionAction?.Invoke(collider_info);
                            CollisionInfo obj_info = new CollisionInfo();
                            obj_info.Colliders.Add(collider);
                            obj.CollisionAction?.Invoke(obj_info);
                            if (obj is not PhysicsArea)
                                break; // No need to continue if collision is detected
                        }
                    }
                }
            }
        }
    }

    // public static void CheckCollision(PhysicsArea obj)
    // {
    //     core.Debug.Log($"Checking {obj.name} for collisions", Instance);
    //     // Keep track of points that have already been queried
    //     var queriedPositions = new HashSet<Vec2i>();
    //     var newPosition = obj.Position + obj.Velocity;
    //     foreach (Vec2i shapeOffset in obj.CollisionShape)
    //     {
    //         var positionToCheck = shapeOffset + newPosition;

    //         // Only query if this position hasn't been checked already
    //         if (!queriedPositions.Contains(positionToCheck))
    //         {
    //             queriedPositions.Add(positionToCheck);

    //             if (CollisionTree.Query(positionToCheck, out PhysicsObject[] queryResult))
    //             {
    //                 // Process collision results
    //                 foreach (PhysicsObject collider in queryResult)
    //                 {
    //                     if (collider.id != obj.id)
    //                     {
    //                         CollisionInfo colinfo = new CollisionInfo();
    //                         colinfo.Colliders.Add(obj);
    //                         if (obj.CollisionIgnoreFilter.Contains(collider.name)) continue;
    //                         var ignoreNames = obj.CollisionIgnoreFilter.Where(x => true);
    //                         core.Debug.Log($"OBJECT COLLISION: obj {obj.name} {obj.Position} [{string.Join(", ", ignoreNames)}] collided with {collider.name} {collider.Position}", obj);
    //                         colinfo.Colliders.Add(collider);
    //                         if (collider is not PhysicsArea)
    //                         {
    //                             obj.Velocity = Vec2i.ZERO;
    //                             obj.IsColliding = true;
    //                             collider.IsColliding = true;
    //                         }
    //                         obj.CollisionAction?.Invoke(colinfo);
    //                         collider.CollisionAction?.Invoke(colinfo);
    //                         if (obj is not PhysicsArea)
    //                             break; // No need to continue if collision is detected
    //                     }
    //                 }
    //             }
    //         }
    //     }
    // }

    public static void Raycast(Vec2i from, Vec2i to, out CollisionInfo result)
    {
        var colResult = new CollisionInfo();
        Vec2i.GetLine(from, to).ForEach(x =>
        {
            if (CollisionTree.Query(x, out PhysicsObject[] queryResult))
                foreach (var obj in queryResult)
                {
                    colResult.Colliders.Add(obj);
                }
        });
        result = colResult;
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
