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
    public Vec2i Velocity { get; set; } = new Vec2i();
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
        set { 
            solid = value;
            if(solid)
            {
                PhysicsServer.Instance.add_collider(this);
            } 
            else
            {
                PhysicsServer.Instance.remove_collider(this);
            }
        }
    }
    /// <summary>
    /// Uses the objects Velocity to move and collide with the world.<br />
    /// Let's you add velocity when calling it with a Vec2i _direction.
    /// </summary>
    /// <param name="_direction">Vec2i _directions adds Velocity to Velocity</param>
    /// <returns>True if the object moved freely, False when it collides</returns>
    public bool MoveAndCollide(Vec2i _direction)
    {
        Velocity += _direction;
        PhysicsServer.CollisionInfo col = PhysicsServer.Instance.colliding(this);
        if(col.colliders.Count == 0)
        {
            Teleport(Velocity);
            Velocity = Vec2i.ZERO; // this is currently needed because the velocity is not affected by anything
            return true;
        }
        foreach (var colItem in col.colliders.Where(colItem => colItem != this))
        {
            if (colItem is PhysicsArea)
            {
                Teleport(Velocity);
                Velocity = Vec2i.ZERO;
                return true;
                
            }
            else if (colItem.IsSolid)
            {
                Velocity = Vec2i.ZERO;
                return false;
            }
        }
        
        throw new Exception($"something strange happened in move_and_collide: {this.name} {this.GetType()}");
    }
    /// <summary>
    /// Uses the objects Velocity to move and collide with the world.
    /// </summary>
    /// <returns>True if it moved freely, False when it collides</returns>
    public bool MoveAndCollide()
    {
        if (Velocity == Vec2i.ZERO)
            return true;
        PhysicsServer.CollisionInfo col = PhysicsServer.Instance.colliding(this);
        
        if(col.colliders.Count == 0)
        {
            Position += Velocity;
            //move(Velocity);
            Velocity = Vec2i.ZERO; // this is currently needed because the velocity is not affected by anything
            return true;
        }
        foreach (var colItem in col.colliders.Where(colItem => colItem != this))
        {
            if (colItem is PhysicsArea)
            {
                Teleport(Velocity);
                Velocity = Vec2i.ZERO;
                return true;
                
            }
            else if (colItem.IsSolid)
            {
                Velocity = Vec2i.ZERO;
                return false;
            }
        }
        
        return true;
    }
    /// <summary>
    /// Teleports object to position
    /// </summary>
    /// <param name="_direction"></param>
    public void Teleport(Vec2i _direction)
    {
        Position +=  _direction;
    }

    public PhysicsObject() : base()
    {
        PhysicsServer.Instance.add_collider(this);
    }

    public PhysicsObject(Vec2i _position, string _icon) : this()
    {
        Position = _position;
        Display = _icon;
    }
    /// <summary>
    /// A function to override when you want to do things with the objects involved in the collision<br />
    /// PhysicsServer.CollisionInfo is passed as Parameter
    /// </summary>
    /// <param name="collisionInfo"></param>
    public virtual void OnCollision(PhysicsServer.CollisionInfo collisionInfo)
    {
        return;
    }
}
