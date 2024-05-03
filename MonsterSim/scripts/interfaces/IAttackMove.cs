using TerminalVelocity;

public interface IAttackMove
{
    int AD { get; set; }
    void Attack(ICreature target);

    void AttackWith(int slot, ICreature? target);
}
