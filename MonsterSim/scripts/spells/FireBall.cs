using TerminalVelocity;
public class FireBall : Projectile
{
    public FireBall(Vec2i _position, Vec2i _targetPosition, int damage, SceneObject _owner) : base (_position, _targetPosition, _owner, Vec2i.ONE)
    {

    }
}