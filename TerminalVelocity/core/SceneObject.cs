namespace TerminalVelocity;
public class SceneObject : IDisposable
{

    /// <summary>
    /// The Action you execute when using ProcessEnabled in override void OnProcess()
    /// </summary>
    public Action? ProcessAction;
    /// <summary>
    /// The Action you execute when using InputEnabled in override void OnInput(ConsoleKey)
    /// </summary>
    public Action? InputAction;
    /// <summary>
    /// Unique identifier
    /// </summary>
    public readonly Guid id;
    /// <summary>
    /// The name of the object. The default is ClassName+id
    /// </summary>
    public string name;
    /// <summary>
    /// used for GC in SceneObject.Dispose(bool)
    /// </summary>
    private bool _disposed;
    private bool process_enabled;
    /// <summary>
    /// Setting ProcessEnabled to true subscribes the object to the Game.ProcessTick event or unsubscribes it when setting it to false
    /// Function:
    /// public override void OnProcess()
    /// </summary>
    public bool ProcessEnabled
    {
        get => process_enabled;
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
    /// <summary>
    /// Setting InputEnabled to true subscribes the object to the Input.KeyPressed event or unsubscribes it when setting it to false
    /// Function:
    /// public override void OnInput(ConsoleKey)
    /// </summary>
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
    /// <summary>
    /// When setting break_text to true it creates new SceneObjects for every new line
    /// </summary>
    private bool break_text = false;
    private string display = "";
    /// <summary>
    /// Getting the value sets the console colors and return the string.
    /// Setting the value sets the value and if break_text is true splits into new child per newline
    /// </summary>
    public virtual string Display
    {
        get
        {
            return display;
        }
        set
        {
            if (break_text && value.Contains('\n'))
            {
                string[] str = value.Split('\n');
                int longest_str = str.OrderByDescending(s => s.Length).First().Length;

                int diff = longest_str - str[0].Length;

                this.Position = this.Position; // recenter after finding longest string
                display = str[0].PadRight(longest_str);

                for (int i = 1; i < str.Length; i++)
                {
                    SceneObject subText = new SceneObject(str[i].PadRight(longest_str));
                    subText.Position = Vec2i.DOWN * i;
                    subText.Color = this.Color;
                    subText.BackgroundColor = this.BackgroundColor;
                    subText.name = $"{this.name}_{i}";

                    this.AddChild(subText);
                }
            }
            else
            {
                display = value;
            }
        }
    }

    private int zIndex = 1;
    /// <summary>
    /// Sets the z-index. Higher values get rendered infront.
    /// </summary>
    public int ZIndex
    {
        get => zIndex;
        set
        {
            if (value == 0)
                return;
            zIndex = value;
        }
    }


    private ConsoleColor foregroundColor = Console.ForegroundColor;
    /// <summary>
    /// Sets the foreground color for the object
    /// </summary>
    public ConsoleColor Color
    {
        get { return foregroundColor; }
        set
        {
            foregroundColor = value;
            if (break_text)
            {
                Children.ForEach(child => child.Color = Color);
            }
        }
    }

    private ConsoleColor backgroundColor = Console.BackgroundColor;
    /// <summary>
    /// Sets the background color for the object
    /// </summary>
    public ConsoleColor BackgroundColor
    {
        get { return backgroundColor; }
        set
        {
            backgroundColor = value;
            if (break_text)
            {
                Children.ForEach(child => child.BackgroundColor = BackgroundColor);
            }
        }
    }


    private bool visible = true;
    /// <summary>
    /// If Visible is true the object is rendered
    /// </summary>
    public bool Visible
    {
        get => visible;
        set
        {
            visible = value;
            if (visible)
            {
                RenderServer.AddItem(this);
            }
            else
            {
                RenderServer.RemoveItem(this);
            }
        }
    }
    /// <summary>
    /// Reference to the parent of this object
    /// </summary>
    public SceneObject? Parent = null;
    /// <summary>
    /// Contains possible children objects
    /// </summary>
    public List<SceneObject> Children = new List<SceneObject>();

    public bool TopLevel = false;
    private Vec2i position = Vec2i.ZERO;
    /// <summary>
    /// Represents the global position
    /// </summary>
    public Vec2i Position
    {
        get => position;
        set
        {
            Vec2i offset = position - value;
            position = value;
            Children.Where(child => !child.TopLevel).ToList().ForEach(child => child.Position -= offset);
        }
    }

    public Vec2i GlobalPosition
    {
        get => GetGlobalPosition();
    }

    private Vec2i GetGlobalPosition()
    {
        var globalPosition = Vec2i.ZERO;
        globalPosition += this.Position;
        var currentParent = Parent;
        while (currentParent != null)
        {
            globalPosition += currentParent.Position;
            currentParent = currentParent.Parent;
        }
        return globalPosition;
    }

    public SceneObject()
    {
        InputEnabled = false;
        ProcessEnabled = false;
        id = Guid.NewGuid();
        name = this.GetType().ToString() + id.ToString();
        Position = position;
        ZIndex = zIndex;
        Visible = visible;
    }
    /// <summary>
    /// If break_text is true, every newline creates a new child.
    /// </summary>
    /// <param name="breakText">bool break_text</param>
    public SceneObject(bool breakText) : this()
    {
        break_text = breakText;
    }
    /// <summary>
    /// Sets the appearence and z-index
    /// </summary>
    /// <param name="_display">string</param>
    /// <param name="_zindex">int</param>
    public SceneObject(string _display, int _zindex) : this()
    {
        display = _display;
        ZIndex = _zindex;
    }
    /// <summary>
    /// Sets the Position
    /// </summary>
    /// <param name="_pos"></param>
    public SceneObject(Vec2i _pos) : this()
    {
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
    /// <summary>
    /// Adds the object as child and sets the position and z-index relative to the parent.
    /// Also then executes the OnStart() method.
    /// </summary>
    /// <param name="_child">SceneObject</param>
    /// <returns></returns>
    public virtual bool AddChild<T>(T _child) where T : SceneObject
    {
        _child.Parent = this;
        _child.Position += this.position;
        _child.ZIndex += this.ZIndex;
        _child.Visible = _child.Visible;
        // if (_child is PhysicsObject physicsObject)
        //     _ = physicsObject.IsSolid; // call the getter to set collisionshape
        Children.Add(_child);
        _child.OnStart();
        return true;
    }
    /// <summary>
    /// OnStart() gets executed when an object is added to a parent object (SceneObject, Scene)
    /// </summary>
    public virtual void OnStart()
    {
        Children.ForEach(child => child.OnStart());
        Visible = Visible; // hack: else nothing is visible
    }

    public bool RemoveChild(SceneObject _child)
    {
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
    /// <summary>
    /// Returns the root Scene
    /// </summary>
    /// <returns>SceneObject scene_object</returns>
    public SceneObject get_root()
    {
        if (this.Parent == null)
        {
            return this;
        }
        else
        {
            return Parent.get_root();
        }
    }
    /// <summary>
    /// Tries to return the object with the specified id
    /// </summary>
    /// <param name="_id"></param>
    /// <returns>SceneObject or null</returns>
    public SceneObject? GetNodeById(Guid _id)
    {
        if (this.id == _id)
            return this;
        foreach (var _child in Children)
        {
            if (_child.id == _id)
                return _child;
        }
        return Parent?.GetNodeById(_id);
    }
    /// <summary>
    /// Tries to return the first object with the specified name
    /// </summary>
    /// <param name="_name"></param>
    /// <returns>SceneObject or null</returns>
    public SceneObject? GetNodeByName(string _name)
    {
        if (this.name == _name)
            return this;
        foreach (var _child in Children)
        {
            if (_child.name == _name)
                return _child;
        }
        return Parent?.GetNodeByName(_name);
    }

    /// <summary>
    /// Gets invoked on every Game.ProcessTick
    /// </summary>
    public virtual void OnProcess()
    {
        ProcessAction?.Invoke();
    }
    /// <summary>
    /// Gets invoked on every Input.KeyPressed event
    /// </summary>
    /// <param name="key">ConsoleKey key</param>
    public virtual void OnInput(ConsoleKey key)
    {
        InputAction?.Invoke();
    }
    /// <summary>
    /// Centers element relative to screen width and places it on the specified y position
    /// </summary>
    /// <param name="y">int y represents the y coordinate on the screen</param>
    public void center_x(int y = 0)
    {
        Position = new Vec2i((Console.WindowWidth / 2) - this.Display.Length / 2, this.Position.y);
    }
    /// <summary>
    /// Centers the element relative to the screen on the y axis and places it on the specified x position
    /// </summary>
    /// <param name="x">int x represents the x position on the screen</param>
    public void center_y(int x = 0)
    {
        Position = new Vec2i(this.Position.x, Console.WindowHeight / 2);
    }
    /// <summary>
    /// Centers the element on the screen on both axis
    /// </summary>
    /// <param name="offset">int offset is unused</param>
    public void center_xy(int offset = 0)
    {
        center_x();
        center_y();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~SceneObject()
    {
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
            PhysicsServer.RemoveCollider(this);
        }
        InputEnabled = false;
        ProcessEnabled = false;
        Visible = false;
        Children.Clear();
        _disposed = true;
    }
}
