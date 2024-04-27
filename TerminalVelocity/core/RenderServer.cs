using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Media;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Linq;

namespace TerminalVelocity;

public class RenderServer
{
    private static RenderServer singleton = new RenderServer();
    private Dictionary<Vec2i, SceneObject> screen_buffer = new Dictionary<Vec2i, SceneObject>();
    private List<SceneObject> registered_buffer = new List<SceneObject>();
    public static RenderServer Instance
    {
        get
        {
            if (singleton == null)
            {
                singleton = new RenderServer();
            }
            return singleton;
        }
    }
    private RenderServer()
    {
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
        if (registered_buffer.Contains(obj))
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
        Render();
        foreach (var idx in screen_buffer.Keys.Where(idx => idx.x >= 0 && idx.y >= 0 && idx.x <= Console.WindowWidth && idx.y <= Console.WindowHeight))
        {
                Console.SetCursorPosition(idx.x, idx.y);
            screen_buffer.TryGetValue(idx, out SceneObject? v);
            if (v != null)
            {
                // Console.Write(v.name);
                // Console.Write(v.GetType());
                // Console.Write(v.ZIndex);
                Console.Write(v.Display);
            }
            Console.ResetColor();
        }

    }

    public void Render()
    {
        var new_screen_buffer = new Dictionary<Vec2i, SceneObject>();
        var rBufferCopy = new List<SceneObject>(registered_buffer);
        foreach (var obj in rBufferCopy.Where(obj => obj.Visible && obj.Position.x >= 0 && obj.Position.y >= 0 && obj.Position.x < Console.WindowWidth && obj.Position.y < Console.WindowHeight))
        {
            new_screen_buffer.TryAdd(obj.Position, obj);
            // if the current obj.ZIndex is higher than the pixel in the buffer we overwrite it
            // if (obj.ZIndex > new_screen_buffer[obj.Position].ZIndex)
            if(new_screen_buffer.TryGetValue(obj.Position, out SceneObject v))
            {
                if(obj.ZIndex > v.ZIndex)
                    new_screen_buffer[obj.Position] = obj;
                // if the old screen buffer has a key for this position 
                // and the object in the new buffer is the same, 
                // we dont need to render it again because its already there   
                if (screen_buffer.ContainsKey(obj.Position) && obj.id == v.id)
                {
                    screen_buffer.Remove(obj.Position);
                }
            }
        }
        
        
        // clean up screen by removing pixels left in the old screen buffer
        foreach (var pos in screen_buffer.Keys)
        {
            Console.SetCursorPosition(pos.x, pos.y);
            for (int i = 0; i < screen_buffer[pos].Display.Length; i++) // bug: this is a refernce to an object. the display length could have changed
            {
                Console.ResetColor();
                Console.Write(" ");
            }
        }
        // now we just add all pixels to the buffer to render
        // todo: we dont need to add all pixels, only the pixels that have changed
        screen_buffer = new_screen_buffer;
    }

    internal void ClearScene()
    {
        Console.Clear();
        screen_buffer = new Dictionary<Vec2i, SceneObject>();
        registered_buffer = new List<SceneObject>();
    }
}
