using TerminalVelocity;
public class Rat : PhysicsObject, IAttackMove, IDefensiveMove, IMovementAbility, IAttackble
{
    public Rat(Vec2i _position, string _icon = "-::.")
    {
        BackgroundColor = ConsoleColor.Gray;
        Color = ConsoleColor.Black;
        Position = _position;
        Display = _icon;
        ProcessEnabled = true;
        name ="rat";
    }
    int hp = 33;
    public int HP { 
        get => hp; 
        set {
            hp = value;
            if (HP <= 0)
            {
                Dispose();
                TerminalVelocity.core.Debug.Log($"{this.name} died",this);
            }
        } 
    }

    public override void OnProcess()
    {
        if(this.Velocity.x < 0)
            Display = ".::-";
        else if (this.Velocity.x > 0)
            Display = "-::.";
    }

    public int AD { get; set;} = 3;
    public void Attack(IAttackble target)
    {
       target.TakeDamage(AD, out _);
    }

    public override void OnCollision(PhysicsServer.CollisionInfo collisionInfo)
    {
        var target = collisionInfo.colliders.Where(collider => collider.id != this.id)?.First() as IAttackble;
        if(target != null)
            Attack(target);  
    }

    public void DefensiveMove(ref int number)
    {
        throw new NotImplementedException();
    }
    Vec2i next_goal_position = Vec2i.Random(Game.Settings.Engine.WindowSize);
    bool flip = false;
    public void MovementAbility()
    {
        // if(Position == next_goal_position)
        //     next_goal_position = Vec2i.Random(Game.Settings.Engine.WindowSize);
        if(Game.RunTime % 1000 < 15)
        {
            if(flip)
            {
                Velocity -= Vec2i.Random(2);
                flip = !flip;
            }
            else
            {
                Velocity += Vec2i.Random(2);
                flip = !flip;
            }

        }
            
    }

    public void TakeDamage(int damage, out int hpLeft)
    {
        HP -= damage;
        hpLeft = HP;
    }
}