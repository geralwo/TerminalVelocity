using System.Diagnostics;
using System.Runtime.InteropServices;
using TerminalVelocity;

public class GroundSlam<T> : PhysicsArea where T : ICharacter
{
    public Vec2i EffectLocation;
    public Vec2i EffectSize;

    public readonly T Creator;
    public GroundSlam(Vec2i center, Vec2i radius, T creator) : base(center, radius)
    {
        Stopwatch timer = new Stopwatch();
        ProcessEnabled = true;
        var flash = true;
        ProcessAction += () =>
        {
            if (timer.ElapsedMilliseconds > 500)
            {
                this.Dispose();
            }
            if (timer.ElapsedMilliseconds % 50 < 15)
            {
                PhysicsServer.CheckCollision(this);
            }
            if(flash)
            {
                new Thread(() => {
                    BackgroundColor = ConsoleColor.Red;
                    Thread.Sleep(133);
                    if(flash)
                        flash = false;
                }).Start();
            }
            else
            {
                new Thread(() => {
                    BackgroundColor = ConsoleColor.Yellow;
                    Thread.Sleep(150);
                    if(!flash)
                        flash = true;

                }).Start();
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
                attackble.TakeDamage((int)(Creator.AD * 0.3), out _);
            }
        }
    }
}