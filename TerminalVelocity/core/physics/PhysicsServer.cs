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

		CollisionTree = new QuadTree(Vec2i.ZERO, Game.Settings.Engine.WindowSize);
		foreach (var obj in Instance.Colliders)
		{

			TerminalVelocity.core.Debug.AddImportantEntry($"Going through {obj.name} -> p:{obj.Position} v:{obj.Velocity} IsColliding:{obj.IsColliding}", Instance);

			// in Step() we go through each object and insert it into the quad tree.
			// we want to only check for collisions on objects that have a velocity != 0
			TerminalVelocity.core.Debug.AddDebugEntry($"Trying to insert {obj.CollisionShape.Length} shape(s) into CollisionTree", Instance);
			var collisionShape = obj.CollisionShape;
			for (int i = 0; i < collisionShape.Length; i++)
			{
				var posInTree = obj.Position + collisionShape[i];
				TerminalVelocity.core.Debug.AddDebugEntry("Inserting Shape " + collisionShape[i] + " into " + posInTree, Instance);
				CollisionTree.Insert(posInTree, obj);
			}
		}
		foreach (var obj in Instance.Colliders)
		{
			obj.IsColliding = false;
			if (obj.Velocity != Vec2i.ZERO)
				checkCollision(obj);
			obj.Position += obj.Velocity;
			obj.Velocity = obj.Velocity.StepToZero();
		}
	}

	// private static void checkCollision(PhysicsObject obj)
	// {
	// 	TerminalVelocity.core.Debug.AddImportantEntry("Checking collison for " + obj.name, Instance);
	// 	CollisionInfo colinfo = new CollisionInfo();
	// 	//obj.BackgroundColor = ConsoleColor.Blue;
	// 	colinfo.colliders.Add(obj);
	// 	var newPosition = obj.Position + obj.Velocity;
	// 	// We check for every position in the obj.CollisionShape
	// 	foreach (Vec2i _colShapeOffset in obj.CollisionShape)
	// 	{
	// 		var positionToCheck = _colShapeOffset + newPosition;
	// 		TerminalVelocity.core.Debug.AddDebugEntry($"Checking position {positionToCheck} for collisions", Instance);
	// 		if (CollisionTree.Query(positionToCheck, out PhysicsObject[] queryResult))
	// 		{
	// 			TerminalVelocity.core.Debug.AddDebugEntry($"CollisionTree returns result for position {positionToCheck}", Instance);
	// 			foreach (var res in queryResult)
	// 			{
	// 				TerminalVelocity.core.Debug.AddDebugEntry($"{res.name}", Instance);
	// 			}
	// 			foreach (PhysicsObject collider in queryResult)
	// 			{
	// 				if (collider.id != obj.id)
	// 				{
	// 					TerminalVelocity.core.Debug.AddImportantEntry($"OBJECT COLLISION: obj {obj.name} collided with {collider.name} at {obj.Position}", obj);
	// 					colinfo.colliders.Add(collider);
	// 					obj.Velocity = Vec2i.ZERO;
	// 					obj.IsColliding = true;
	// 					collider.IsColliding = true;
	// 					collider.OnCollision(colinfo);
	// 					obj.OnCollision(colinfo);
	// 					break;
	// 				}
	// 			}
	// 		}
	// 	}
	// }

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

				TerminalVelocity.core.Debug.AddDebugEntry($"Checking position {positionToCheck} for collisions", Instance);

				if (CollisionTree.Query(positionToCheck, out PhysicsObject[] queryResult))
				{
					// Process collision results
					foreach (PhysicsObject collider in queryResult)
					{
						if (collider.id != obj.id)
						{
							TerminalVelocity.core.Debug.AddImportantEntry($"OBJECT COLLISION: obj {obj.name} {obj.Position} collided with {collider.name} {collider.Position} trying to go to {obj.Position + obj.Velocity}", obj);
							colinfo.colliders.Add(collider);
							if(collider is not PhysicsArea && !obj.CollisionIgnoreFilter.Contains(collider.name))
							{
								obj.Velocity = Vec2i.ZERO;
								obj.IsColliding = true;
								collider.IsColliding = true;
							}
							obj.OnCollision(colinfo);
							collider.OnCollision(colinfo);
							break; // No need to continue if collision is detected
						}
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
			Game.CurrentScene.RemoveChild(quadTreeVisuals);
			quadTreeVisuals = null;
		}
	}

}