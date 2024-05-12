using System.Diagnostics;
using TerminalVelocity;

public class GroundSlam<T> : PhysicsArea where T : ICreature
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
            if (flash)
            {
                new Thread(() =>
                {
                    BackgroundColor = ConsoleColor.Red;
                    Thread.Sleep(125);
                    if (flash)
                        flash = false;
                }).Start();
            }
            else
            {
                new Thread(() =>
                {
                    BackgroundColor = ConsoleColor.Yellow;
                    Thread.Sleep(150);
                    if (!flash)
                        flash = true;

                }).Start();
            }
        };
        timer.Start();
        EffectLocation = center;
        EffectSize = radius;
        Creator = creator;
        CollisionAction += OnCollision;
    }

    private void OnCollision(CollisionInfo collisionInfo)
    {
        foreach (PhysicsObject collision in collisionInfo.Colliders.Where(x => x.id != this.id && x.id != Creator.id))
        {
            if (collision is ICreature attackble)
            {
                attackble.TakeDamage((int)(Creator.AD * 0.3), out _);
                TerminalVelocity.common.Logger.Log($"{collision.name} takes dmg from {Creator.Name} by {this.name}", Creator);
            }
        }
    }
}
