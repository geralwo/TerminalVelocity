using TerminalVelocity;

public interface ICharacter : IAttackMove, IMovementAbility, IAttackble, IDefensiveMove
{
    string Name { get; }
    Guid id { get; }
    int HP { get; }

    public void MoveToPosition(Vec2i position);

    int MovementSpeed { get; }
}