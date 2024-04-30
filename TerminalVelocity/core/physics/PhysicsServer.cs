using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Media;
using System.Reflection.Metadata;
using System.Net.Mime;
using System.Security.Cryptography;

namespace TerminalVelocity;

public class PhysicsServer
{
	public static event OnCollision? OnCollisionEvent;
	public delegate void OnCollision(PhysicsObject collider);
	private static PhysicsServer singleton = new PhysicsServer();
	private List<PhysicsObject> objects = new List<PhysicsObject>();
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
		if (obj == null || Instance.objects.Contains(obj))
		{
			return false;
		}
		Instance.objects.Add(obj);
		return true;

	}

	public static bool RemoveCollider(PhysicsObject obj)
	{
		if (Instance.objects.Contains(obj))
		{
			Instance.objects.Remove(obj);
			return true;
		}
		else
		{
			return false;
		}
	}
	private static List<SceneObject> debugCols = new List<SceneObject>();
	public static void Step()
	{
		foreach (var debug_col in debugCols)
		{
			Game.CurrentScene.remove_child(debug_col);
		}
		debugCols = new List<SceneObject>();
		CollisionTree = new QuadTree(Vec2i.ZERO, Game.Settings.Engine.WindowSize, 4);
		foreach (var obj in Instance.objects)
		{
			foreach (Vec2i colpos in obj.CollisionShape)
			{
				// SceneObject colDebug = new SceneObject(" ");
				// colDebug.BackgroundColor = ConsoleColor.Blue;
				// colDebug.Position = colpos + obj.Position;
				// if (obj.Velocity == Vec2i.ZERO)
				// 	colDebug.BackgroundColor = ConsoleColor.Green;
				// debugCols.Add(colDebug);
				CollisionTree.Insert(colpos + obj.Position, obj);
			}

			// foreach (var debug_col in debugCols)
			// {
			// 	Game.CurrentScene.add_child(debug_col);
			// }
			if (obj.Velocity == Vec2i.ZERO)
				continue;
			checkCollision(obj);
		}
	}

	private static void checkCollision(PhysicsObject obj)
	{
		bool hasCollision = false;
		CollisionInfo colinfo = new CollisionInfo();
		colinfo.colliders.Add(obj);
		// We check for every position in the obj.CollisionShape
		foreach (Vec2i shapepos in obj.CollisionShape)
		{
			Vec2i nextPos = shapepos + obj.Position + obj.Velocity;
			var results = CollisionTree.Query(nextPos);
			foreach (PhysicsObject result in results)
			{
				if (result.id != obj.id)
				{
					foreach (Vec2i potentialColPosition in result.CollisionShape)
					{
						if (nextPos == result.Position + potentialColPosition)
						{
							// Collision detected
							colinfo.colliders.Add(result);
							hasCollision = true;
							var tmp_colinfo = new CollisionInfo();
							tmp_colinfo.colliders.Add(obj);
							result.OnCollision(tmp_colinfo);
							break;
						}
					}
				}
			}
		}

		if (!hasCollision)
		{
			// update position if no collision was detected
			obj.Position += obj.Velocity;
			obj.Velocity = Vec2i.ZERO;
			obj.IsColliding = false;
		}
		else
		{
			obj.IsColliding = true;
			obj.Velocity = Vec2i.ZERO;
			obj.OnCollision(colinfo);
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
		if(quadTreeVisuals == null)
			quadTreeVisuals = CollisionTree.Visualize();
		else
		{
			Game.CurrentScene.remove_child(quadTreeVisuals);
			quadTreeVisuals = CollisionTree.Visualize();
		}
		Game.CurrentScene.add_child(quadTreeVisuals);
	}

}