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
	//private static List<SceneObject> debugCols = new List<SceneObject>();
	static string debugMsg = "";
	public static void Step()
	{
        // foreach (var debug_col in debugCols)
        // {
        // 	Game.CurrentScene.remove_child(debug_col);
        // }
        //debugCols = new List<SceneObject>();

        CollisionTree = new QuadTree(Vec2i.ZERO, Game.Settings.Engine.WindowSize, 8);
		foreach (var obj in Instance.Colliders)
		{
            debugMsg = "";
            debugMsg += $"[{obj.name}: {obj.Position} {obj.Velocity} {obj.IsColliding}]\n";

            // in Step() we go through each object and insert it into the quad tree.
            // we want to only check for collisions on objects that have a velocity != 0
            if (obj.IsColliding)
			{
				obj.Velocity = Vec2i.ZERO;
			}
			if (obj.Velocity != 0)
			{
				checkCollision(obj);
				obj.Position += obj.Velocity;
				obj.Velocity = obj.Velocity.StepToZero();
			}
            debugMsg += $"  inserting {obj.CollisionShape.Length} shape(s) into CollisionTree\n";
            foreach (Vec2i _colShapeOffset in obj.CollisionShape)
            {
                var posInTree = obj.Position + _colShapeOffset;
                debugMsg += "    Shape: " + _colShapeOffset + " => " + (_colShapeOffset + obj.Position) + $"into {posInTree}\n";
                if (CollisionTree.Insert(posInTree, obj)) ;
                //	obj.Color = ConsoleColor.DarkMagenta;
            }

            debugMsg += "\n";
            TerminalVelocity.core.Debug.AddDebugEntry(debugMsg);
		}
	}

	private static void checkCollision(PhysicsObject obj)
	{
		debugMsg += "\n  checking collison for" + obj.name + "\n";
        CollisionInfo colinfo = new CollisionInfo();
		//obj.BackgroundColor = ConsoleColor.Blue;
		colinfo.colliders.Add(obj);
		var newPosition = obj.Position + obj.Velocity;
		// We check for every position in the obj.CollisionShape
		foreach (Vec2i _colShapeOffset in obj.CollisionShape)
		{
            var positionToCheck = _colShapeOffset + newPosition;
            debugMsg += $"    checking position {positionToCheck}\n";
			if (CollisionTree.Query(positionToCheck, out SceneObject[] queryResult))
			{
                debugMsg += $"collision tree gives result for {positionToCheck}";
                foreach (PhysicsObject collider in queryResult)
				{
					if(collider.id != obj.id)
					{
						debugMsg += "\n[!] obj collided!";
						colinfo.colliders.Add(collider);
						obj.IsColliding = true;
						collider.OnCollision(colinfo);
						obj.OnCollision(colinfo);
						break;
					}
				}
			}
		}
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
			Game.CurrentScene.remove_child(quadTreeVisuals);
			quadTreeVisuals = null;
		}
	}

}