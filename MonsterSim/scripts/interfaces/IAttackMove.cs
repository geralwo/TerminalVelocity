using TerminalVelocity;

public interface IAttackMove
{
    int AD {get;set;}
    void Attack(IAttackble target);

    void AttackWith(int slot, IAttackble? target);
}