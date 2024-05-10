using TerminalVelocity;
public class Projectile<T> : PhysicsArea where T : SceneObject
{

    public T Owner;
    public Vec2i TargetPosition;
    public List<Vec2i> PathPoints;
    public Vec2i ProjectileSpeed;
    public Vec2i Direction;
    private IEnumerator<Vec2i> currentPathIndex;
    int MoveTimer = 500;
    public Projectile(Vec2i _startPosition, Vec2i _direction, T _owner, int _speed = 500) : base(_startPosition, Vec2i.ONE)
    {
        MoveTimer = _speed;
        Owner = _owner;
        Direction = _direction;
        Position = _startPosition;
        PathPoints = Vec2i.GetLine(Position, Position + _direction);
        currentPathIndex = PathPoints.AsEnumerable().GetEnumerator();
        BackgroundColor = ConsoleColor.Cyan;
        ProcessEnabled = true;
        TopLevel = true;
        ProcessAction += () =>
        {
            if (Game.RunTime.ElapsedMilliseconds % MoveTimer == 0)
                currentPathIndex.MoveNext();
            Velocity = Direction;
        };
        CollisionAction += (CollisionInfo c) =>
        {
            TerminalVelocity.core.Debug.Log($"{this.name} collided with {c.Colliders[0].name}", this);
        };
        currentPathIndex.MoveNext();
    }
}
