using TerminalVelocity;

public interface IProjectile
{
    public Vec2i Direction { get; }
    public Vec2i Position { get; }
    public SceneObject ProjectileOwner { get; }
    public List<string> CollisionIgnoreFilter { get; }
}