using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Media;
using System.Reflection.Metadata;

namespace TerminalVelocity;

public class PhysicsServer
{
	private static PhysicsServer singleton = new PhysicsServer();
	private List<PhysicsObject> objects = new List<PhysicsObject>();
	public static PhysicsServer Instance { 
		get {
            if (singleton == null)
            {
                singleton = new PhysicsServer();
            }
            return singleton;
        }
	}
	private PhysicsServer(){
	}

	public bool add_collider(PhysicsObject obj)
	{
		if (objects.Contains(obj))
		{
            return false;
        }

		objects.Add(obj);
		return true;
		
	}

	public bool remove_collider(PhysicsObject obj)
	{
		if (objects.Contains(obj))
		{
			objects.Remove(obj);
			return true;
		} else
		{
			return false;
		}
	}


	public CollisionInfo colliding(PhysicsObject obj) // bad name
    {
        Vec2i target_position = obj.Position + obj.Velocity;

		CollisionInfo collisionInfo = new CollisionInfo();

        List<PhysicsObject> objectsDup = new List<PhysicsObject>(objects); // You need to Copy the List else it crashes

        foreach (PhysicsObject other_obj in objectsDup)
        {
            if (target_position == other_obj.Position && obj != other_obj)
            {
				if(obj.CollisionIgnoreFilter.Contains(other_obj.name))
				{
					return collisionInfo;
				}


				//obj.Background = ConsoleColor.Red; // debug collision
				collisionInfo.colliders.Add(obj);
				collisionInfo.colliders.Add(other_obj);

                obj.CollisionAction?.Invoke();
                other_obj.CollisionAction?.Invoke();

				obj.on_collision(collisionInfo);
				other_obj.on_collision(collisionInfo);
            }
        }
        //obj.Background = ConsoleColor.Black; // debug collision
        return collisionInfo;
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
}
