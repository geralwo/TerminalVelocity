﻿using System.ComponentModel.Design.Serialization;
using System.Data.Common;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace TerminalVelocity;
public class SceneObject : IDisposable 
{

    public Action? ProcessAction;
    public Action? InputAction;
    public Guid id;
    public string name;
    private bool _disposed;
    private bool process_enabled;
    public bool ProcessEnabled
    {
        get { return process_enabled; }
        set
        {
            process_enabled = value;
            if (process_enabled)
            {
                Game.ProcessTick += OnProcess;
            }
            else
            {
                Game.ProcessTick -= OnProcess;
            }
            
        }
    }
    private bool input_enabled;
    public bool InputEnabled
    {
        get => input_enabled;
        set
        {
            input_enabled = value;
            if (input_enabled)
            {
                Input.KeyPressed += OnInput;
            }
            else
            {
                Input.KeyPressed -= OnInput;
            }
        }
    }
    private bool break_text = false;
    private string display = " ";
    public virtual string Display
    {
        get => display;
        set {
            if(break_text && value.Contains('\n'))
            {
                string[] str = value.Split('\n');
                int longest_str = str.OrderByDescending( s => s.Length ).First().Length;

                int diff = longest_str - str[0].Length;
                
                this.Position = this.Position + new Vec2i(-diff,0); // if a subtext is longer than the first element, we offset the position so it centers better; workaround for scene menus
                display = str[0].PadRight(longest_str);
                
                for(int i = 1; i < str.Length; i++)
                {
                    SceneObject subText = new SceneObject(str[i].PadRight(longest_str));
                    subText.Position    = Vec2i.DOWN * i;
                    subText.ForegroundColor  = this.ForegroundColor;
                    subText.BackgroundColor  = this.BackgroundColor;
                    subText.name        = $"{this.name}_{i}";

                    this.add_child(subText);
                }
            }
            else
            {
                display = value;
            }
        }
    }

    private int zIndex = 1;
    public int ZIndex
    {
        get => zIndex; 
        set {
            if(value == 0)
                return;
            zIndex = value;
        }
    }


    private ConsoleColor foregroundColor = ConsoleColor.White;
    public ConsoleColor ForegroundColor { 
        get { return foregroundColor; }
        set
        {
            foregroundColor = value;
            if(break_text)
            {
                Children.ForEach(child => child.ForegroundColor = ForegroundColor);
            }
        }
     }

    private ConsoleColor backgroundColor = ConsoleColor.Magenta;
    public ConsoleColor BackgroundColor { 
        get { return backgroundColor; }
        set
        {
            backgroundColor = value;
            if(break_text)
            {
                Children.ForEach(child => child.BackgroundColor = BackgroundColor);
            }
        }
     }


    private bool visible = true;
    public bool Visible
    {
        get => visible;
        set {
            visible = value;
            if(visible)
            {
                RenderServer.Instance.AddItem(this);
            }
            else
            {
                RenderServer.Instance.RemoveItem(this);
            }
        }
    }
   
    public SceneObject? parent = null;
    public List<SceneObject> Children = new List<SceneObject>();

    private Vec2i position = Vec2i.ZERO;
    public Vec2i Position
    {
        get => position;
        set 
        {
            Vec2i offset = position - value;
            position = value;
            Children.ForEach(child => child.Position -= offset);
        }
    }
    public SceneObject() {
        InputEnabled = false;
        ProcessEnabled = false;
        id = Guid.NewGuid();
        name = this.GetType().ToString() + id.ToString();
        Position = position;
        ZIndex = zIndex;
        Visible = visible;
    }

    public SceneObject(bool breakText) : this()
    {
        break_text = breakText;
    }
    public SceneObject(string _icon, int _zindex) : this() {
        display = _icon; 
        ZIndex = _zindex;
    }
    public SceneObject(Vec2i _pos) : this() { 
        Position = _pos; 
    }
    public SceneObject(Vec2i _position, string _icon) : this()
    {
        display = _icon;
        Position = _position;
    }

    public SceneObject(string _icon) : this()
    {
        Display = _icon;
    }
    public bool add_child(SceneObject _child)
    {
        _child.parent = this;
        _child.Position += this.position;
        _child.ZIndex += this.ZIndex;
        _child.Visible = _child.Visible;
        Children.Add(_child);
        _child.OnStart();
        return true;
    }

    public virtual void OnStart()
    {
        Children.ForEach(child => child.OnStart());
        Visible = Visible; // hack: else nothing is visible
    }

    public bool remove_child(SceneObject _child) {
        if (_child == this)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write($"You can't free yourself: {this.name}");
            throw new Exception("Circular ref?");
        }
        
        _child.Dispose();
        return Children.Remove(_child); // maybe put into OnRemove function?
    }

    public SceneObject get_root()
    {
        if(this.parent == null)
        {
            return this;
        } 
        else
        {
            return parent.get_root();
        }
    }

    public SceneObject? GetNodeById(Guid _id)
    {
        if(this.id == _id ) { return this; }
        Children.ForEach(child => { child.GetNodeById(_id); });
        return parent?.GetNodeById(_id);
    }


    public virtual void OnProcess(){
        ProcessAction?.Invoke();
    }

    public virtual void OnInput(ConsoleKey key){
        InputAction?.Invoke();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~SceneObject() {
        Dispose(false);
    }
    // dispose pattern https://gist.github.com/matthewaburton/5455227
    private void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing)
        {
            if (this.Children.Count > 0)
            {
                List<SceneObject> cc = new List<SceneObject>(this.Children);
                foreach (var child in cc)
                {
                    child.Dispose();
                }
            }
            if (this is PhysicsObject || this is PhysicsArea)
            {
                PhysicsServer.Instance.remove_collider((PhysicsObject)this);
            }
        }
        InputEnabled    = false;
        ProcessEnabled  = false;
        Visible = false;
        Children.Clear();
        _disposed       = true;
    }

    public void center_x(int y = 0)
    {
        Position = new Vec2i((Console.WindowWidth / 2) - this.Display.Length / 2, this.Position.y);
    }

    public void center_y(int x = 0)
    {
        Position = new Vec2i(this.Position.x, Console.WindowHeight / 2);
    }

    public void center_xy(int offset = 0)
    {
        center_x();
        center_y();
    }
}
