using System.Buffers;
using System.Diagnostics;
using TerminalVelocity;

public class Projectile : PhysicsArea, IProjectile
{
    public int lifeTimeInSeconds = 2;
    public Vec2i[] TargetPositions;
    private Vec2i TargetPosition;
    
    private SceneObject Owner;

    public Projectile(Vec2i _startPosition,Vec2i _targetPosition, SceneObject _owner, int _speed = 1, string _display=".") : base(_startPosition,Vec2i.ONE)
    {
        Owner = _owner;
        Display = _display;
        Position = _startPosition;
        Velocity = Position.DirectionTo(_targetPosition);
        TargetPositions = Vec2i.GetLine(Position,_targetPosition).ToArray();
        TargetPosition = _targetPosition;
        ProcessEnabled = true;
        Stopwatch timer = new Stopwatch();
        // Create an enumerator to iterate through the list
        var _iterator = TargetPositions.GetEnumerator();

        // Move to the first item
        if (_iterator.MoveNext())
        {
            Position = (Vec2i)_iterator.Current;
        }
        ProcessAction += () =>
        {
            //if(Position == TargetPosition) Dispose();
            if(timer.ElapsedMilliseconds > lifeTimeInSeconds * 1000)
            {
                Dispose();
            }
            if (_iterator.MoveNext())
            {
                Position = (Vec2i)_iterator.Current;
            }
        };
        timer.Start();
    }

    public override void OnCollision(PhysicsServer.CollisionInfo collision)
    {

    }

    public Vec2i Direction => Position.DirectionTo(TargetPosition);

    List<string> IProjectile.CollisionIgnoreFilter => new List<string>();

    public Guid ProjectileOwner => throw new NotImplementedException();

    SceneObject IProjectile.ProjectileOwner => Owner;
}