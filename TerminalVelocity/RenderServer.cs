using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Media;
using System.Reflection.Metadata;

namespace TerminalVelocity;

public class RenderServer
{
	private static RenderServer singleton = new RenderServer();
	private Dictionary<Vec2i,SceneObject> objects = new Dictionary<Vec2i,SceneObject>();
	public static RenderServer Instance { 
		get {
            if (singleton == null)
            {
                singleton = new RenderServer();
            }
            return singleton;
        }
	}
	private RenderServer(){
	}

	public bool AddItem(SceneObject obj)
	{
        if(objects.ContainsKey(obj.Position))
        {
            if(objects[obj.Position].ZIndex < obj.ZIndex)
                objects[obj.Position] = obj;
            return true;
        }
        objects.Add(obj.Position, obj);
		return true;
		
	}

	public bool RemoveItem(SceneObject obj)
	{
		return true;
	}

    public int count()
    {
        return objects.Count;
    }

    public void DrawBuffer()
    {
        foreach(Vec2i idx in objects.Keys)
        {
            Console.SetCursorPosition(idx.x, idx.y);
            var d = objects[idx].Icon;
            Console.Write(RenderServer.Instance.objects[idx].Display);
        }
        Game.FrameCount++;
    }
}
