using TerminalVelocity;

public class FireBall<T> : Projectile<T> where T : SceneObject
{
    public FireBall(Vec2i _startPosition, Vec2i _direction, T _owner) : base(_startPosition, _direction, _owner)
    {
        CollisionIgnoreFilter.Add("FireBallExplosion");
        CollisionAction += (CollisionInfo c) =>
        {
            PhysicsServer.RemoveCollider(this);
            RenderServer.RemoveItem(this);
            TerminalVelocity.common.Logger.Log($"fireball hit {c.Colliders[0].name}", this);
            var explosion = new GroundSlam<Ork>(Vec2i.ZERO, Vec2i.ONE * 3, _owner as Ork);
            explosion.name = "FireBallExplosion";
            explosion.TopLevel = true;
            var startTime = Game.RunTime.ElapsedMilliseconds;
            AddChild(explosion);
        };
    }
}
