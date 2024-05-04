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

    public string Name => throw new NotImplementedException();

    Guid ICreature.id => throw new NotImplementedException();

    public int MovementSpeed => throw new NotImplementedException();

    public void Attack(ICreature target)
    {
        target.TakeDamage(AD, out var _targetHP);
        TerminalVelocity.core.Debug.Log($"Rat attacks {target.Name} => HP left:{_targetHP}", this);
    }

    public override void OnCollision(PhysicsServer.CollisionInfo collisionInfo)
    {
        var target = collisionInfo.colliders.Where(collider => collider.id != this.id)?.First() as ICreature;
        if (target != null)
            Attack(target);
    }

    public void DefensiveMove(ref int number)
    {
        next_goal_position = Vec2i.Random(Game.Settings.Engine.WindowSize);
        HP += 5;
        MovementAbility();
    }
    Vec2i next_goal_position = Vec2i.Random(Game.Settings.Engine.WindowSize);
    bool flip = false;
    public void MovementAbility()
    {

        if (Game.RunTime.ElapsedMilliseconds % 500 < 15)
        {
            if (Position == next_goal_position)
                next_goal_position = Vec2i.Random(Game.Settings.Engine.WindowSize);
            Velocity = Position.DirectionTo(next_goal_position).Normalized;
            if (Velocity.x < 0) Display = ".::-";
            else Display = "-::.";
        }

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
