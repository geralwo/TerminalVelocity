namespace TerminalVelocity;
public struct CollisionInfo
{
    // CollisionInfo holds all the objects and information about the collision
    public bool is_valid = true;

    public List<PhysicsObject> colliders = new List<PhysicsObject>();

    public CollisionInfo()
    {
    }
}
