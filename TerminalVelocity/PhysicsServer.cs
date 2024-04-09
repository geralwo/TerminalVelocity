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
				
				//obj.Background = ConsoleColor.Red; // debug collision
				collisionInfo.obj.Add(obj);
				collisionInfo.obj.Add(other_obj);

                obj.CollisionAction?.Invoke();
                other_obj.CollisionAction?.Invoke();
            }
        }
        //obj.Background = ConsoleColor.Black; // debug collision
        return collisionInfo;
    }

	public struct CollisionInfo
	{
		// CollisionInfo should hold all the objects and information about the collision
		public List<PhysicsObject> obj = new List<PhysicsObject>();

        public CollisionInfo()
        {
        }
    }
}
