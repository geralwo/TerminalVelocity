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
    /// <summary>
    /// This is the screen buffer that gets drawn to the terminal
    /// </summary>
    private Dictionary<Vec2i, SceneObject> screenBuffer = new Dictionary<Vec2i, SceneObject>();
    private List<SceneObject> registered_buffer = new List<SceneObject>();
    /// <summary>
    /// This is the singleton instance of the RenderServer
    /// </summary>
    private static RenderServer Instance
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
    /// <summary>
    /// Adds an item to the registerd Buffer which makes it possible to render the SceneObject.
    /// </summary>
    /// <param name="obj">SceneObject obj</param>
    /// <returns>True if added successfully; false when not</returns>
    public static bool AddItem(SceneObject obj)
    {
        if (Instance.registered_buffer.Contains(obj))
        {
            return false;
        }
        Instance.registered_buffer.Add(obj);
        return true;
    }
    /// <summary>
    /// Removes SceneObject from the RenderSever
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool RemoveItem(SceneObject obj)
    {
        if (Instance.registered_buffer.Contains(obj))
        {
            Instance.registered_buffer.Remove(obj);
            return true;
        }
        return false;
    }
    /// <summary>
    /// Returns the number of items drawn last frame.
    /// </summary>
    /// <returns></returns>
    public static int Count()
    {
        return Instance.screenBuffer.Count;
    }
    /// <summary>
    /// Draws the screenBuffer to the terminal
    /// </summary>
    public static void DrawBuffer()
    {
        Instance.Render();
        foreach (var idx in Instance.screenBuffer.Keys.Where(idx => idx.x >= 0 && idx.y >= 0 && idx.x <= Console.WindowWidth && idx.y <= Console.WindowHeight))
        {
            Console.SetCursorPosition(idx.x, idx.y);
            Instance.screenBuffer.TryGetValue(idx, out SceneObject? v);
            if (v != null)
            {
                Console.ForegroundColor = v.ForegroundColor;
                Console.BackgroundColor = v.BackgroundColor;
                // Console.Write(v.name);
                // Console.Write(v.GetType());
                // Console.Write(v.ZIndex);
                Console.Write(v.Display);
                Console.ResetColor();
            }
        }
    }
    /// <summary>
    /// Creates the screenBuffer to be rendered in RenderServer.DrawBuffer()<br />
    /// We create a new_screen_buffer object.<br />
    /// We go through each object in the RegisteredBuffer and check if SceneObject.Visible == true and if its inside of the Screen<br />
    /// and add it to the new_screen_buffer. If there is already a SceneObject in the buffer at the position, we check which<br />
    /// SceneObject has the higher z-index to decide what to draw.<br />
    /// With the old screen_buffer we check if the position existed before and if its the same item on the same position.
    /// If it is, we remove the item from old scree_buffer.<br />
    /// Having in the screen_buffer now only items that are different from before,<br />
    /// we can later go through the old screen_buffer and remove pixels that are not valid any more.
    /// </summary>
    private void Render()
    {
        var new_screen_buffer = new Dictionary<Vec2i, SceneObject>();
        var rBufferCopy = new List<SceneObject>(registered_buffer);
        foreach(var obj in rBufferCopy.Where(obj => obj.Visible && obj.Position.IsInBoundsOf(Game.Settings.Engine.WindowSize)))
        //foreach (var obj in rBufferCopy.Where(obj => obj.Visible && obj.Position.x >= 0 && obj.Position.y >= 0 && obj.Position.x < Console.WindowWidth && obj.Position.y < Console.WindowHeight))
        {
            new_screen_buffer.TryAdd(obj.Position, obj);
            if(new_screen_buffer.TryGetValue(obj.Position, out SceneObject v))
            {
                if(obj.ZIndex > v.ZIndex)
                    new_screen_buffer[obj.Position] = obj;
                // if the old screen buffer has a key for this position 
                // and the object in the new buffer is the same, 
                // we dont need to render it again because its already there   
                if (screenBuffer.ContainsKey(obj.Position) && obj.id == v.id)
                {
                    screenBuffer.Remove(obj.Position);
                }
            }
        }
        
        
        // clean up screen by removing pixels left in the old screen buffer
        foreach (var pos in screenBuffer.Keys)
        {
            Console.SetCursorPosition(pos.x, pos.y);
            for (int i = 0; i < screenBuffer[pos].Display.Length; i++) // bug: this is a refernce to an object. the display length could have changed
            {
                Console.ResetColor();
                Console.Write(" ");
            }
        }
        // now we just add all pixels to the buffer to render
        // todo: we dont need to add all pixels, only the pixels that have changed
        screenBuffer = new_screen_buffer;
    }

    public static void ClearScene()
    {
        Console.Clear();
        Instance.screenBuffer = new Dictionary<Vec2i, SceneObject>();
        Instance.registered_buffer = new List<SceneObject>();
    }
}
