namespace TerminalVelocity;

public class PhysicsObject : SceneObject
{
    public Action? CollisionAction;
    public int layer = 0;
    private Vec2i velocity = new Vec2i();
    public List<string> CollisionIgnoreFilter = new List<string>();
    public Vec2i Velocity
    {
        get {return velocity;}
        set {
            velocity = value;
        }
    }
    public float mass = 1.0f;
    private bool solid = true;
    public bool IsSolid
    {
        get { return solid; }
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
    public bool move_and_collide(Vec2i _direction)
    {
        Velocity += _direction;
        PhysicsServer.CollisionInfo col = PhysicsServer.Instance.colliding(this);
        if(col.colliders.Count == 0)
        {
            move(Velocity);
            Velocity = Vec2i.ZERO; // this is currently needed because the velocity is not affected by anything
            return true;
        }
        foreach (var colItem in col.colliders) // should this be in physics server?
        {
            if(colItem == this)
                continue;
            if (colItem is PhysicsArea)
            {
                move(Velocity);
                velocity = Vec2i.ZERO;
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

    public bool move_and_collide()
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
        foreach (var colItem in col.colliders)
        {
            if(colItem == this)
                continue;
            if (colItem is PhysicsArea)
            {
                move(Velocity);
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

    public void move(Vec2i _direction)
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

    public virtual void on_collision(PhysicsServer.CollisionInfo collisionInfo)
    {
        return;
    }
}
