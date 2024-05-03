using TerminalVelocity;

public interface ICreature : IAttackMove, IMovementAbility, IDefensiveMove
{
    string Name { get; }
    Guid id { get; }
    int HP { get; }

    public void MoveToPosition(Vec2i position);

    int MovementSpeed { get; }
    public void TakeDamage(int damage, out int _HPremaining);
}
