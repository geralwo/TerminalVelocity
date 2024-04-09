using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Media;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace TerminalVelocity;

public class RenderServer
{
	private static RenderServer singleton = new RenderServer();
	private Dictionary<Vec2i, SceneObject> screen_buffer = new Dictionary<Vec2i, SceneObject>();
    private List<SceneObject> registered_buffer = new List<SceneObject>();
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

        if (registered_buffer.Contains(obj))
        {
            return false;
        }
        registered_buffer.Add(obj);
        return true;
	}

	public bool RemoveItem(SceneObject obj)
	{
        if(registered_buffer.Contains(obj))
        {
            registered_buffer.Remove(obj);
            return true;
        }
        return false;

	}

    public int count()
    {
        return screen_buffer.Count;
    }

    public void DrawBuffer()
    {
        

        foreach(Vec2i idx in screen_buffer.Keys)
        {   
            Console.SetCursorPosition(idx.x, idx.y);
            Console.ForegroundColor = screen_buffer[idx].ForegroundColor;
            Console.BackgroundColor = screen_buffer[idx].BackgroundColor;

            Console.Write(screen_buffer[idx].Display);
            Console.ResetColor();
        }
        render();
        }
        
    public void render()
    {
        var screen_buffer_copy = new Dictionary<Vec2i, SceneObject>();
        foreach(SceneObject pixel in registered_buffer)
        {
            if(!screen_buffer_copy.ContainsKey(pixel.Position))
            {
                screen_buffer_copy.Add(pixel.Position, pixel);
                continue;
            }
            if(pixel.ZIndex > screen_buffer_copy[pixel.Position].ZIndex)
            {
                screen_buffer_copy[pixel.Position] = pixel;
            }
        }
        foreach(Vec2i pos in screen_buffer.Keys)
        {
            if(!screen_buffer_copy.ContainsKey(pos))
            {
                Console.SetCursorPosition(pos.x,pos.y);
                //Console.BackgroundColor = ConsoleColor.Red;
                Console.Write(" ");
            }
        }
        screen_buffer = screen_buffer_copy;
        
    }
}
