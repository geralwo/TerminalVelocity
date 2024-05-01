namespace TerminalVelocity;

public class PhysicsObject : SceneObject
{
    /// <summary>
    /// A function tha gets executed when a collision happens
    /// </summary>
    public Action? CollisionAction;
    /// <summary>
    /// Objects on the same layer collide - not implemented
    /// </summary>
    public int CollisionLayer = 0;
    /// <summary>
    /// A list of object names for which collision should be ignored. If an object's name matches any entry in this list, it will not collide with this object.
    /// </summary>
    public List<string> CollisionIgnoreFilter = new List<string>();
    /// <summary>
    ///  The Velocity of this object
    /// </summary>
    public Vec2i Velocity { get; set; } = Vec2i.ZERO;
    /// <summary>
    /// Mass property. Not used at the moment.
    /// </summary>
    public float Mass = 1.0f;
    private bool solid = true;
    /// <summary>
    /// If IsSolid is true the object is added to the PhysicsServer, else it gets removed from the PhysicsServer
    /// </summary>
    public bool IsSolid
    {
        get => solid;
        set
        {
            solid = value;
            if (solid)
            {
                PhysicsServer.AddCollider(this);
            }
            else
            {
                PhysicsServer.RemoveCollider(this);
            }
        }
    }
    private Vec2i[]? collisionShape;
    public Vec2i[] CollisionShape
    {
        get => CreateCollisionShape();
    }
    private string display = "█";
    public override string Display
    {
        get => display;
        set
        {
            // collision with no space makes no sense
            if (string.IsNullOrEmpty(value))
                throw new Exception("Display cannot be empty when creating CollisionShape.");
            display = value;
        }
    }

    public bool IsColliding = false;
    public PhysicsObject()
    {
        IsSolid = solid;
    }


    public PhysicsObject(Vec2i _position) : this()
    {
        Position = _position;
    }
    /// <summary>
    /// Creates an Vec2i Array of offsets that represent the shape of the object
    /// </summary>
    /// <returns>Vec2i[Display.Length]</returns>
    /// <exception cref="Exception"></exception>
    public Vec2i[] CreateCollisionShape()
    {
        // return shape if the old shape still fits
        if (collisionShape != null && collisionShape.Length == Display.Length)
            return collisionShape;
        // create new shape
        collisionShape = new Vec2i[Display.Length];
        for (int i = 0; i < Display.Length; i++)
        {
            collisionShape[i] = Vec2i.RIGHT * i;
        }
        return collisionShape;
    }

    public void Teleport(Vec2i _direction)
    {
        Position += _direction;
    }
    /// <summary>
    /// A function to override when you want to do things on collision.<br />
    /// PhysicsServer.CollisionInfo is a struct containing the objects involved in the collision
    /// </summary>
    /// <param name="collisionInfo"></param>
    public virtual void OnCollision(PhysicsServer.CollisionInfo collisionInfo)
    {
        CollisionAction?.Invoke();
    }
}
