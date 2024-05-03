using TerminalVelocity;

public interface IProjectile
{
    public Vec2i Position { get; }
    public SceneObject ProjectileOwner { get; } // the type could be more generic
    public List<string> CollisionIgnoreFilter { get; }
}