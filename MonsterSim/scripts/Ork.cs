using TerminalVelocity;

public class Ork : PhysicsObject, IAttackMove, IDefensiveMove, IMovementAbility, IAttackble
{
    private int hp = 100;

    public int HP {
        get => hp;
        set {
            hp = value;
            if(HP < 0)
            {
                Display = "X";
            }
        }
    }
    public int AD = 33;
    public Ork()
    {
        BackgroundColor = ConsoleColor.DarkGreen;
        ForegroundColor = ConsoleColor.Green;
        Display = "ô";
    }


    public void Attack(IAttackble target)
    {
        target.HP -= AD;
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