using TerminalVelocity;
public class Rat : PhysicsObject, IAttackMove, IDefensiveMove, IMovementAbility, IAttackble
{

    public int HP { get => 34; set {} }

    public void Attack(IAttackble target)
    {
        target.HP -= 1;
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
        throw new NotImplementedException();
    }
}