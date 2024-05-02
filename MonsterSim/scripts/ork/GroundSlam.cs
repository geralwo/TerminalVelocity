using System.Diagnostics;
using System.Runtime.InteropServices;
using TerminalVelocity;

public class GroundSlam : PhysicsArea
{
    public Vec2i EffectLocation;
    public Vec2i EffectSize;

    public readonly SceneObject Creator;
    public GroundSlam(Vec2i center, Vec2i radius, SceneObject creator) : base(center, radius)
    {
        Stopwatch timer = new Stopwatch();
        ProcessEnabled = true;
        ProcessAction += () =>
        {
            if (timer.ElapsedMilliseconds > 1000)
            {
                this.Dispose();
            }
            if (timer.ElapsedMilliseconds % 200 < 15)
            {
                BackgroundColor = Game.GetRandomConsoleColor();
                PhysicsServer.CheckCollision(this);
            }
        };
        timer.Start();
        EffectLocation = center;
        EffectSize = radius;
        Creator = creator;
    }

    public override void OnCollision(PhysicsServer.CollisionInfo collisionInfo)
    {
        foreach (PhysicsObject collision in collisionInfo.colliders.Where(x => x.id != this.id && x.id != Creator.id))
        {
            if(collision is IAttackble attackble)
            {
                TerminalVelocity.core.Debug.Log($"{collision.name} takes dmg from {this.name}",Creator);
                attackble.TakeDamage(50, out _);
            }
        }
    }
}