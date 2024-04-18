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
        foreach (var idx in screen_buffer.Keys.Where(idx => idx.x >= 0 && idx.y >= 0 && idx.x <= Console.WindowWidth && idx.y <= Console.WindowHeight))
        {
            Console.SetCursorPosition(idx.x, idx.y);
            // Console.ForegroundColor = screen_buffer[idx].ForegroundColor;
            // Console.BackgroundColor = screen_buffer[idx].BackgroundColor;

            screen_buffer.TryGetValue(idx, out SceneObject v);
            if (v != null)
            {
                Console.ForegroundColor = v.ForegroundColor;
                Console.BackgroundColor = v.BackgroundColor;
            }

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
        foreach (var pixel in registered_buffer.Where(pixel => pixel.Visible))
        {
            if(screen_buffer.ContainsKey(pixel.Position) && pixel.id == screen_buffer[pixel.Position].id) // obj did not move, so we remove it from the old screen to reduce cleanup loops
            {
                screen_buffer.Remove(pixel.Position);
            }

            new_screen_buffer.TryAdd(pixel.Position, pixel);

            if(pixel.ZIndex > new_screen_buffer[pixel.Position].ZIndex)
            {
                new_screen_buffer[pixel.Position] = pixel;
            }
        }
        foreach (var pos in screen_buffer.Keys)
        {
            Console.SetCursorPosition(pos.x,pos.y);
            for(int i = 0; i < screen_buffer[pos].Display.Length; i++)
            {
                Console.Write(" ");
            }
        }

        screen_buffer = new_screen_buffer;
    }
}
