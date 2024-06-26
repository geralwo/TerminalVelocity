using System.Runtime.InteropServices;
using TerminalVelocity;

public class Ork : PhysicsObject, IAttackMove, IDefensiveMove, IMovementAbility, ICreature
{
    private int hp = 100;

    public int HP
    {
        get => hp;
        set
        {
            hp = value;
            if (hp < 0)
            {
                Dispose();
            }
        }
    }
    public int AD { get; set; } = 33;

    public string Name => name;

    Guid ICreature.id => id;

    public int MovementSpeed => 1;

    private List<Action> Attacks = new List<Action>();

    public Ork() : base()
    {
        BackgroundColor = ConsoleColor.DarkGreen;
        Color = ConsoleColor.Green;
        Display = "ô";
        Attacks.Add(() => { new GroundSlam<Ork>(Position, new Vec2i(3, 3), this); });
        Attacks.Add(() =>
        {
            var dir = Console.ReadKey(true);
            new FireBall<Ork>(Position, Vec2i.FromCKI(dir), this);
        });
    }
    int AttackActionIndex = 0;

    public void Attack()
    {
        Attacks[AttackActionIndex].Invoke();
    }

    public void AttackWith(int slot = 0)
    {
        AttackActionIndex = slot;
        Attack();
    }
    public void DefensiveMove(ref int damage)
    {
        damage -= 1;
    }

    public void MovementAbility()
    {
        Velocity *= 2;
    }

    public void TakeDamage(int damage, out int hpLeft)
    {
        DefensiveMove(ref damage);
        if (HP < 0)
            Dispose();
        HP -= damage;
        hpLeft = HP;
    }

    public void Attack(ICreature target)
    {
        Attack();
    }

    public void Move(Vec2i _direction)
    {
        Velocity += _direction;
    }

    public void InteractWith<T>(T target)
    {
        throw new NotImplementedException();
    }

    public void MoveToPosition(Vec2i position)
    {
        Velocity = position.Normalized * MovementSpeed;
    }

    public void AttackWith(int slot, ICreature? target)
    {
        Attacks[slot].Invoke();
    }
}
