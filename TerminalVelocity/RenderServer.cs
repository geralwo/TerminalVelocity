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

    private SortedSet<int> Z_INDICES = new SortedSet<int>();
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
        Z_INDICES.Add(obj.ZIndex);
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
            if(idx.x < 0 || idx.y < 0 || idx.x > Console.WindowWidth || idx.y > Console.WindowHeight)
                continue;
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
        foreach(int i in Z_INDICES)
        {
            
        }
        foreach(SceneObject pixel in registered_buffer)
        {
            if(!screen_buffer_copy.ContainsKey(pixel.Position))
            {
                screen_buffer_copy.Add(pixel.Position, pixel);
                continue;
            }
            screen_buffer_copy[pixel.Position] = pixel;
        }
                

        foreach(Vec2i pos in screen_buffer.Keys)
        {
            if(pos.x < 0 || pos.y < 0 || pos.x > Console.WindowWidth || pos.y > Console.WindowHeight)
                continue;
            if(!screen_buffer_copy.ContainsKey(pos))
            {
                Console.SetCursorPosition(pos.x,pos.y);
                //Console.BackgroundColor = ConsoleColor.Red;
                Console.Write(" ");
            }
            else
            {
                if(screen_buffer_copy[pos].ZIndex > screen_buffer[pos].ZIndex)
                {
                    Console.SetCursorPosition(pos.x,pos.y);
                    //Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write(" ");
                }
            }
        }
        screen_buffer = screen_buffer_copy;
        
    }
}
