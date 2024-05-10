using TerminalVelocity;
public class Rat : PhysicsObject, IAttackMove, IDefensiveMove, IMovementAbility, ICreature
{
    public Rat(Vec2i _position, string _icon = "-::.")
    {
        BackgroundColor = ConsoleColor.Gray;
        Color = ConsoleColor.Black;
        Position = _position;
        Display = _icon;
        ProcessEnabled = true;
        name = "rat";
        CollisionAction += OnCollision;
        next_goal_position = new Path2D(Position, Vec2i.Random(Game.Settings.Engine.WindowSize));
    }
    int hp = 33;
    public int HP
    {
        get => hp;
        set
        {
            hp = value;
            if (HP <= 0)
            {
                Dispose();
                TerminalVelocity.core.Debug.Log($"{this.name} died", this);
            }
        }
    }

    public int AD { get; set; } = 25;

    public string Name => name;

    Guid ICreature.id => throw new NotImplementedException();

    public int MovementSpeed => throw new NotImplementedException();

    public void Attack(ICreature target)
    {
        target.TakeDamage(AD, out var _targetHP);
        TerminalVelocity.core.Debug.Log($"{this.name} attacks {target.Name}.HP -> {_targetHP}", this);
    }

    private void OnCollision(CollisionInfo collisionInfo)
    {
        var target = collisionInfo.Colliders.Where(collider => collider.id != this.id)?.First() as ICreature;
        if (target != null)
            Attack(target);
    }
    Path2D next_goal_position;
    public override void OnStart()
    {
        next_goal_position = new Path2D(Position, Vec2i.Random(Game.Settings.Engine.WindowSize));
        base.OnStart();
    }

    public void DefensiveMove(ref int number)
    {
        next_goal_position = new Path2D(Position, Vec2i.Random(Game.Settings.Engine.WindowSize));
        HP += 5;
        MovementAbility();
    }

    public void MovementAbility()
    {

        // if (Game.RunTime.ElapsedMilliseconds % 500 < 15)
        // {
        if (Position == next_goal_position.End)
            next_goal_position = new Path2D(Position, Vec2i.Random(Game.Settings.Engine.WindowSize));
        Velocity = Position.DirectionTo(next_goal_position.GetNextPoint()).Normalized;
        if (Velocity.x < 0) Display = ".::-";
        else Display = "-::.";
        // }

    }

    public void TakeDamage(int damage, out int hpLeft)
    {
        HP -= damage;
        hpLeft = HP;
        DefensiveMove(ref hpLeft);
    }

    public void AttackWith(int slot, ICreature? target)
    {
        throw new NotImplementedException();
    }

    public void MoveToPosition(Vec2i position)
    {
        Position = position;
    }
}
