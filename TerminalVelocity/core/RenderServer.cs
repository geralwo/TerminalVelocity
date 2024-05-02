using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Media;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace TerminalVelocity;

public class RenderServer
{
    private static RenderServer singleton = new RenderServer();
    /// <summary>
    /// This is the screen buffer that gets drawn to the terminal
    /// </summary>
    private Dictionary<Vec2i, Pixel> screenBuffer = new();
    private List<SceneObject> registered_buffer = new();

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
    private bool frame_completed = true;
    public static bool IsReady
    {
        get => Instance.frame_completed;
    }

    private int frameTimeInMs;
    public static int FrameTimeInMs {
        get => Instance.frameTimeInMs;
    }

    private Stopwatch frameTime;
    public static void DrawBuffer()
    {
            Instance.frame_completed = false;
            Instance.frameTime = System.Diagnostics.Stopwatch.StartNew();
            Instance.Render();
            for (int x=0; x < Game.Settings.Engine.WindowSize.x; x++)
            {
                for (int y = 0; y < Game.Settings.Engine.WindowSize.y; y++)
                {
                    var screen_coord = new Vec2i(x, y);
                    Console.SetCursorPosition(x,y);
                    if (Instance.screenBuffer.TryGetValue(screen_coord, out var v))
                    {
                        Console.ForegroundColor = v.ForegroundColor;
                        Console.BackgroundColor = v.BackgroundColor;
                        Console.Write(v.Owner?.name);
                        // Console.Write(v.GetType());
                        // Console.Write(v.ZIndex);
                        // Console.Write(v.Display);
                        Console.ResetColor();
                    }
                }
            }
            Instance.frameTimeInMs = (int)Instance.frameTime.ElapsedMilliseconds;
            Instance.frame_completed = true;
            Game.FrameCount++;
            TerminalVelocity.core.Debug.AddImportantEntry($"Frame {Game.FrameCount} completed FrameTime: {FrameTimeInMs}ms",Instance);
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
        var new_screenBuffer = new Dictionary<Vec2i, Pixel>();
        var rBufferCopy = new List<SceneObject>(registered_buffer);
        // we go through each object in the registered buffer.
        foreach(var obj in rBufferCopy.Where(obj => obj.Visible && obj.Position.IsInBoundsOf(Game.Settings.Engine.WindowSize)))
        {
            // for the length of the string we create pixels
            for (int i = 0; i < obj.Display.Length; i++)
            {
                var screen_coord = obj.Position + Vec2i.RIGHT * i;
                var pixel = new Pixel(obj.Display[i],obj);
                // we add the pixel to the new screen buffer
                // if that fails, there is already a pixel
                // and we check if the pixel.ZIndex is higher than
                // the z-index of the current pixel
                if (new_screenBuffer.TryAdd(screen_coord, pixel)) continue;
                if (pixel.ZIndex > new_screenBuffer[screen_coord].ZIndex)
                {
                    // z-index of current obj is higher, so we set the pixel to the new value
                    new_screenBuffer[screen_coord] = pixel;
                }
            }
        }
        // now we can go through the old screen buffer to see if any pixel are different
        // from the new buffer.
        // if a pixel is different it is because something has moved. so we need to find out
        // what the pixel should look like now. if there is no object to render, we write ' '
        // with the appropriate colors. else we draw the pixel of the object.
        foreach (Vec2i oldPixelCoord in screenBuffer.Keys)
        {
            if (oldPixelCoord.IsInBoundsOf(Game.Settings.Engine.WindowSize))
            {
                if (!new_screenBuffer.TryGetValue(oldPixelCoord, out _))
                {
                    if (oldPixelCoord.IsInBoundsOf(Game.Settings.Engine.WindowSize - 1)) // on windows the max needs to be one else it crashes?
                    Console.SetCursorPosition(oldPixelCoord.x,oldPixelCoord.y);
                    Console.Write(" ");
                }
            }
        }
        screenBuffer = new_screenBuffer;
    }

    public static void ClearScene()
    {
        Console.Clear();
        Instance.screenBuffer = new Dictionary<Vec2i, Pixel>();
        Instance.registered_buffer = new List<SceneObject>();
    }

    private struct Pixel
    {
        public readonly ConsoleColor ForegroundColor;
        public readonly ConsoleColor BackgroundColor;
        public readonly char Display;
        public readonly int ZIndex;
        public readonly SceneObject? Owner;

        public Pixel(char _display, SceneObject? _owner = null)
        {
            if (_owner != null)
            {
                ForegroundColor = _owner.Color;
                BackgroundColor = _owner.BackgroundColor;
                Display = _display;
                ZIndex = _owner.ZIndex;
            }
            Owner = _owner;
        }

        public static bool operator ==(Pixel a, Pixel b)
        {
            return a.ForegroundColor == b.ForegroundColor && a.BackgroundColor == b.BackgroundColor &&
                   a.Display == b.Display;
        }
        
        public static bool operator !=(Pixel a, Pixel b)
        {
            return !(a == b);
        }
        public static Pixel Default => new Pixel(' ');
    }
}
