using TerminalVelocity;

public class Ork : PhysicsObject, IAttackMove, IDefensiveMove, IMovementAbility, IAttackble, IBrain
{
    private int hp = 100;

    public int HP {
        get => hp;
        set {
            hp = value;
            if(hp < 0) {
                Dispose();
            }
        }
    }
    public int AD {get;set;} = 33;
    Guid IBrain.id { get => id; set {} }

    public Ork() : base()
    {
        BackgroundColor = ConsoleColor.DarkGreen;
        Color = ConsoleColor.Green;
        Display = "ô";
    }

    public void Attack()
    {
        new GroundSlam<Ork>(Position,new Vec2i(5,3),this);
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
        HP -= damage;
        hpLeft = HP;
    }

    public void Attack(IAttackble target)
    {
        Attack();
    }

    public void Move(Vec2i _position)
    {
        throw new NotImplementedException();
    }

    public void InteractWith<T>(T target)
    {
        throw new NotImplementedException();
    }
}