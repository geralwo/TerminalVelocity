using System.Buffers;
using System.Diagnostics;
using TerminalVelocity;

public class Projectile : PhysicsArea, IProjectile
{
    
    private SceneObject Owner;
    public List<Vec2i> Path; 
    // the owner is in charge of removing this object
    public Projectile(Vec2i _startPosition, Vec2i _endPosition, SceneObject _owner, Vec2i _size) : base(_startPosition,_size)
    {
        Owner = _owner;
        Path = Vec2i.GetLine(_startPosition, _endPosition);
        Position = _startPosition;
        ProcessEnabled = true;
        int nextPosition = 0;
        int spawnTime = Game.RunTime;
        int lifeTime = 0;
        ProcessAction += () =>
        {
            lifeTime += Game.RunTime - spawnTime;
            if (nextPosition < Path.Count - 1)
                Position = Path[nextPosition];
            if(lifeTime > 1000)
                Dispose();
            nextPosition++;
        };
    }

    SceneObject IProjectile.ProjectileOwner => Owner;

    List<string> IProjectile.CollisionIgnoreFilter => CollisionIgnoreFilter;
}