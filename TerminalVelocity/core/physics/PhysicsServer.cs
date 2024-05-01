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
	public static void Step()
	{
		// foreach (var debug_col in debugCols)
		// {
		// 	Game.CurrentScene.remove_child(debug_col);
		// }
		//debugCols = new List<SceneObject>();
		CollisionTree = new QuadTree(Vec2i.ZERO, Game.Settings.Engine.WindowSize, 1);
		foreach (var obj in Instance.Colliders)
		{
			// in Step() we go through each object and insert it into the quad tree.
			// we want to only check for collisions on objects that have a velocity != 0
			if (obj.Velocity != 0)
			{
				checkCollision(obj);
			}
			foreach (Vec2i _colShapeOffset in obj.CollisionShape)
			{
				var posInTree = obj.Position + _colShapeOffset;
				if(CollisionTree.Insert(posInTree, obj))
					obj.Color = ConsoleColor.DarkMagenta;
			}
		}
	}

	private static void checkCollision(PhysicsObject obj)
	{
        CollisionInfo colinfo = new CollisionInfo();
		//obj.BackgroundColor = ConsoleColor.Blue;
		colinfo.colliders.Add(obj);
		var newPosition = obj.Position + obj.Velocity;
		// We check for every position in the obj.CollisionShape
		bool collisionDetected = false;
		foreach (Vec2i _colShapeOffset in obj.CollisionShape)
		{
			var positionToCheck = _colShapeOffset + newPosition;
			if (CollisionTree.Query(positionToCheck, out SceneObject[] queryResult))
			{
				foreach(PhysicsObject collider in queryResult)
				{
					if(collider.id != obj.id)
					{
						colinfo.colliders.Add(collider);
						collider.BackgroundColor = ConsoleColor.Red;
						collider.OnCollision(colinfo);
						obj.BackgroundColor = ConsoleColor.Red;
						obj.Velocity = Vec2i.ZERO;
						collisionDetected = true;
						break;
					}
				}
			}
		}
		if (!collisionDetected)
		{
			obj.IsColliding = false;
			obj.Position = newPosition;
			obj.Velocity = obj.Velocity.StepToZero();
		}
		else
		{
			obj.Position = obj.Position;
			obj.IsColliding = true;
			obj.Velocity = Vec2i.ZERO;
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