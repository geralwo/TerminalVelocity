using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Media;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Drawing;

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

    public int Count()
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
            // Console.Write(screen_buffer[idx].name);
            // Console.Write(screen_buffer[idx].GetType());
            // Console.Write(screen_buffer[idx].ZIndex);
            Console.ResetColor();
        }
        render();
    }

    internal void clear_scene()
    {
        Console.Clear();
        screen_buffer = new Dictionary<Vec2i, SceneObject>();
        registered_buffer = new List<SceneObject>();
    }
        
    public void render()
    {
        var new_screen_buffer = new Dictionary<Vec2i, SceneObject>();
        foreach(SceneObject pixel in registered_buffer)
        {
            if(!new_screen_buffer.ContainsKey(pixel.Position))
            {
                new_screen_buffer.Add(pixel.Position, pixel);
            }
            else if(pixel.ZIndex > new_screen_buffer[pixel.Position].ZIndex)
            {
                new_screen_buffer[pixel.Position] = pixel;
            }
        }
        foreach(Vec2i pos in screen_buffer.Keys) // go through old buffer to clean changed pixels;
        {
            #region screen edge bug
            //if we dont do this, on the positive x and y we dont properly remove the obj when it goes out of bounds
            if(pos.x < 0 || pos.y < 0)
                continue;
            if(pos.x >= Console.WindowWidth || pos.y >= Console.WindowHeight)
            {
                Console.SetCursorPosition(pos.x,pos.y);
                Console.Write(" ");
            }
            #endregion
            
            if(!new_screen_buffer.ContainsKey(pos))
            {
                Console.SetCursorPosition(pos.x,pos.y);
                for(int i = 0; i < screen_buffer[pos].Display.Length; i++)
                {
                    Console.Write(" ");
                }

            }
        }
        screen_buffer = new_screen_buffer;
    }
}
