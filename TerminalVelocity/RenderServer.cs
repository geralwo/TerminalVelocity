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
        render();
        foreach (var idx in screen_buffer.Keys.Where(idx => idx.x >= 0 && idx.y >= 0 && idx.x <= Console.WindowWidth && idx.y <= Console.WindowHeight))
        {
            Console.SetCursorPosition(idx.x, idx.y);
            // Console.ForegroundColor = screen_buffer[idx].ForegroundColor;
            // Console.BackgroundColor = screen_buffer[idx].BackgroundColor;

            screen_buffer.TryGetValue(idx, out SceneObject v); // rider said this is better
            if (v != null)
            {
                Console.ForegroundColor = v.ForegroundColor;
                Console.BackgroundColor = v.BackgroundColor;
                Console.Write(v.Display);
            }

            // Console.Write(screen_buffer[idx].Display);
            // Console.Write(screen_buffer[idx].name);
            // Console.Write(screen_buffer[idx].GetType());
            // Console.Write(screen_buffer[idx].ZIndex);
            Console.ResetColor();
        }

    }

    public void render()
    {
        var new_screen_buffer = new Dictionary<Vec2i, SceneObject>();

        var rBufferCopy = new List<SceneObject>(registered_buffer);

        foreach (var obj in rBufferCopy.Where(obj => obj.Visible && obj.Position.x >= 0 && obj.Position.y >= 0 && obj.Position.x < Console.WindowWidth && obj.Position.y < Console.WindowHeight))
        {
            new_screen_buffer.TryAdd(obj.Position, obj);
            {
                // if the current obj.ZIndex is higher than the pixel in the buffer we overwrite it
                if (obj.ZIndex > new_screen_buffer[obj.Position].ZIndex)
                {
                    new_screen_buffer[obj.Position] = obj;
                }
            }
            // if the old screen buffer has a key for this position 
            // and the object in the new buffer is the same, 
            // we dont need to render it again because its already there
            if (screen_buffer.ContainsKey(obj.Position) && obj.id == new_screen_buffer[obj.Position].id)
            {
                // the obj did not move, so we remove it from the new screen buffer
                screen_buffer.Remove(obj.Position);
            }
        }


        // clean up screen by removing pixels left in the old screen buffer
        foreach (var pos in screen_buffer.Keys)
        {
            Console.SetCursorPosition(pos.x, pos.y);
            for (int i = 0; i < screen_buffer[pos].Display.Length; i++) // bug: this is a refernce to an object. the display length could have changed
            {
                Console.Write(" ");
            }
            if(pos.x > Console.WindowWidth || pos.y > Console.WindowHeight)
            {
                Console.SetCursorPosition(pos.x,pos.y);
                Console.Write(" ");
            }
        }
        // now we just add all pixels to the buffer to render
        // todo: we dont need to add all pixels, only the pixels that have changed
        screen_buffer = new_screen_buffer;
    }

    internal void clear_scene()
    {
        Console.Clear();
        screen_buffer = new Dictionary<Vec2i, SceneObject>();
        registered_buffer = new List<SceneObject>();
    }
}
