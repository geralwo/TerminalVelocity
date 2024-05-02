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

    public static int FrameTimeInMicroseconds {
        get => Instance.frameTime.Elapsed.Microseconds;
    }

    private Stopwatch? frameTime;
    public static void DrawBuffer()
    {
        Instance.frame_completed = false;
        Instance.frameTime = System.Diagnostics.Stopwatch.StartNew();
        Instance.Render();

        foreach (var kvp in Instance.screenBuffer)
        {
            var screen_coord = kvp.Key;
            var pixel = kvp.Value;

            if (screen_coord.IsInBoundsOf(Game.Settings.Engine.WindowSize))
            {
                Console.SetCursorPosition(screen_coord.x, screen_coord.y);
                Console.ForegroundColor = pixel.ForegroundColor;
                Console.BackgroundColor = pixel.BackgroundColor;
                Console.Write(pixel.Display);
                Console.ResetColor();
            }
        }
        Instance.frame_completed = true;
        Game.FrameCount++;
        core.Debug.AddImportantEntry($"Frame {Game.FrameCount} completed FrameTime: {Instance.frameTime.Elapsed.Microseconds}µs", Instance);
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
        foreach (var obj in rBufferCopy.Where(obj => obj.Visible && obj.Position.IsInBoundsOf(Game.Settings.Engine.WindowSize)))
        {
            if(obj is PhysicsArea area)
            {
                var p = new Pixel(' ',obj);
                foreach(var local_coord in area.CollisionShape)
                {
                    var screenPosition = obj.Position + local_coord;
                    if(!screenPosition.IsInBoundsOf(Game.Settings.Engine.WindowSize)) continue;
                    if (new_screenBuffer.TryAdd(screenPosition, p)) continue;
                    if (obj.ZIndex > new_screenBuffer[screenPosition].ZIndex)
                    {
                        // z-index of current obj is higher, so we set the pixel to the new value
                        new_screenBuffer[screenPosition] = p;
                    }
                }
            }
            // for the length of the string we create pixels
            for (int i = 0; i < obj.Display.Length; i++)
            {
                var screen_coord = obj.Position + Vec2i.RIGHT * i;
                var pixel = new Pixel(obj.Display[i], obj);
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
                if (!new_screenBuffer.TryGetValue(oldPixelCoord, out Pixel p))
                {
                    //if (oldPixelCoord.IsInBoundsOf(Game.Settings.Engine.WindowSize - 1)) // on windows the max needs to be one else it crashes?
                    Console.SetCursorPosition(oldPixelCoord.x, oldPixelCoord.y);
                    Console.Write(" ");
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
            return a.ForegroundColor == b.ForegroundColor && a.BackgroundColor == b.BackgroundColor && a.Display == b.Display;
        }

        public static bool operator !=(Pixel a, Pixel b)
        {
            return !(a == b);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Pixel other)
                return this.ForegroundColor == other.ForegroundColor && this.BackgroundColor == other.BackgroundColor && this.Display == other.Display;
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.BackgroundColor, this.ForegroundColor, this.Display);
        }
        public static Pixel Default => new Pixel(' ');
    }
}
