using TerminalVelocity;

public interface ICreature : IAttackMove, IMovementAbility, IDefensiveMove, IAttackable
{
    string Name { get; }
    Guid id { get; }
    int HP { get; }

    public void MoveToPosition(Vec2i position);

    int MovementSpeed { get; }
}
