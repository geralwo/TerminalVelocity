using TerminalVelocity;

public class Ork : Player, IAttackMove, IDefensiveMove, IMovementAbility
{
    public int HP;
    public int AD;
    public Ork(Vec2i _pos, string _display) : base(_pos, _display)
    {
        Position = _pos;
        Display = _display;

    }

    public void Attack(SceneObject target)
    {
        throw new NotImplementedException();
    }

    public void DefensiveMove()
    {
        throw new NotImplementedException();
    }

    public void Move()
    {
        throw new NotImplementedException();
    }
}