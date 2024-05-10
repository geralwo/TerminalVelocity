using TerminalVelocity;

public class FireBall<T> : Projectile<T> where T : SceneObject
{
    public FireBall(Vec2i _startPosition, Vec2i _direction, T _owner) : base(_startPosition, _direction, _owner)
    {
        CollisionAction += (CollisionInfo c) =>
        {
            var explosion = new GroundSlam<Ork>(Vec2i.ZERO, Vec2i.ONE * 3, _owner as Ork);
            explosion.name = "FireBallExplosion";
            CollisionIgnoreFilter.Add("FireBallExplosion");
            explosion.TopLevel = true;
            var startTime = Game.RunTime.ElapsedMilliseconds;
            AddChild(explosion);
        };
    }
}
