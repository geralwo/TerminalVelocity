using TerminalVelocity;

public class Ork : PhysicsObject, IAttackMove, IDefensiveMove, IMovementAbility, IAttackble
{
    private int hp = 100;

    public int HP {
        get => hp;
        set {
            hp = value;
        }
    }
    public int AD = 33;
    public Ork() : base()
    {
        BackgroundColor = ConsoleColor.DarkGreen;
        Color = ConsoleColor.Green;
        Display = "ôk";

    }


    public void Attack(IAttackble target)
    {
        target.TakeDamage(AD);
    }

    public void DefensiveMove()
    {
        throw new NotImplementedException();
    }

    public void Move()
    {
        throw new NotImplementedException();
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
    }
}