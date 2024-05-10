namespace TerminalVelocity;
/// <summary>
/// Holds information about the objects that collided (excludes self)
/// Members:<br />
/// - Colliders List<PhysicsObject><br/>
/// </summary>
public struct CollisionInfo
{
    public List<PhysicsObject> Colliders = new List<PhysicsObject>();

    public CollisionInfo()
    {
    }
}
