using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace TerminalVelocity
{
    public class RenderServer
    {
        private static readonly Lazy<RenderServer> singleton = new(() => new RenderServer());
        
        /// <summary>
        /// This is the screen buffer that gets drawn to the terminal
        /// </summary>
        private ConcurrentDictionary<Vec2i, Pixel> screenBuffer = new();
        
        /// <summary>
        /// This is the buffer containing objects to render
        /// </summary>
        private List<SceneObject> registered_buffer = new();
        
        private readonly object registeredBufferLock = new();
        
        private RenderServer() { }
        
        public static RenderServer Instance => singleton.Value;

        public static bool AddItem(SceneObject obj)
        {
            var instance = Instance;
            lock (instance.registeredBufferLock)
            {
                if (instance.registered_buffer.Contains(obj))
                {
                    return false;
                }
                instance.registered_buffer.Add(obj);
                return true;
            }
        }

        public static bool RemoveItem(SceneObject obj)
        {
            var instance = Instance;
            lock (instance.registeredBufferLock)
            {
                return instance.registered_buffer.Remove(obj);
            }
        }

        public static int Count()
        {
            return Instance.screenBuffer.Count;
        }

        private bool frame_completed = true;

        public static bool IsReady => Instance.frame_completed;

        public static int FrameTimeInMicroseconds
        {
            get => Instance.frameTime?.Elapsed.Microseconds ?? 0;
        }

        private Stopwatch? frameTime;

        public static void DrawBuffer()
        {
            var instance = Instance;
            lock (instance)
            {
                instance.frame_completed = false;
                instance.frameTime = Stopwatch.StartNew();
                instance.Render();

                foreach (var kvp in instance.screenBuffer)
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

                instance.frameTime.Stop();
                instance.frame_completed = true;
                Game.FrameCount++;
                core.Debug.AddImportantEntry($"Frame {Game.FrameCount} completed FrameTime: {instance.frameTime.Elapsed.Microseconds}µs", instance);
            }
        }

        private void Render()
        {
            var new_screenBuffer = new ConcurrentDictionary<Vec2i, Pixel>();
            
            lock (registeredBufferLock)
            {
                var rBufferCopy = new List<SceneObject>(registered_buffer);

                foreach (var obj in rBufferCopy.Where(obj => obj.Visible && obj.Position.IsInBoundsOf(Game.Settings.Engine.WindowSize - 1)))
                {
                    if (obj is PhysicsArea area)
                    {
                        var p = new Pixel(' ', obj);

                        foreach (var local_coord in area.CollisionShape)
                        {
                            var screenPosition = obj.Position + local_coord;

                            if (!screenPosition.IsInBoundsOf(Game.Settings.Engine.WindowSize)) continue;

                            if (!new_screenBuffer.TryAdd(screenPosition, p))
                            {
                                if (obj.ZIndex > new_screenBuffer[screenPosition].ZIndex)
                                {
                                    new_screenBuffer[screenPosition] = p;
                                }
                            }
                        }
                    }

                    for (int i = 0; i < obj.Display.Length; i++)
                    {
                        var screen_coord = obj.Position + Vec2i.RIGHT * i;
                        var pixel = new Pixel(obj.Display[i], obj);

                        if (!new_screenBuffer.TryAdd(screen_coord, pixel))
                        {
                            if (pixel.ZIndex > new_screenBuffer[screen_coord].ZIndex)
                            {
                                new_screenBuffer[screen_coord] = pixel;
                            }
                        }
                    }
                }
            }
            
            // Clear old pixels that are no longer part of the new screen buffer
            foreach (var oldPixelCoord in screenBuffer.Keys)
            {
                if (!new_screenBuffer.ContainsKey(oldPixelCoord))
                {
                    Console.SetCursorPosition(oldPixelCoord.x, oldPixelCoord.y);
                    Console.Write(" ");
                }
            }

            screenBuffer = new_screenBuffer;
        }

        public static void ClearScene()
        {
            var instance = Instance;
            lock (instance)
            {
                Console.Clear();
                instance.screenBuffer = new ConcurrentDictionary<Vec2i, Pixel>();
                lock (instance.registeredBufferLock)
                {
                    instance.registered_buffer = new List<SceneObject>();
                }
            }
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
                else
                {
                    ForegroundColor = ConsoleColor.Gray;
                    BackgroundColor = ConsoleColor.Black;
                    Display = _display;
                    ZIndex = 0;
                }
                Owner = _owner;
            }

            public static bool operator ==(Pixel a, Pixel b)
            {
                return a.ForegroundColor == b.ForegroundColor &&
                       a.BackgroundColor == b.BackgroundColor &&
                       a.Display == b.Display;
            }

            public static bool operator !=(Pixel a, Pixel b)
            {
                return !(a == b);
            }

            public override bool Equals(object? obj)
            {
                if (obj is Pixel other)
                {
                    return this.ForegroundColor == other.ForegroundColor &&
                           this.BackgroundColor == other.BackgroundColor &&
                           this.Display == other.Display;
                }
                return false;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(this.ForegroundColor, this.BackgroundColor, this.Display);
            }
        }
    }
}
