namespace TerminalVelocity;
public class PhysicsArea : PhysicsObject
{
    //public bool IsSolid = false;
    public PhysicsArea()
    {
        IsSolid = false;
        PhysicsServer.AddCollider(this);
    }
}