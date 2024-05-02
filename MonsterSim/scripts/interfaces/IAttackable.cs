using TerminalVelocity;

public interface IAttackble
{
    Vec2i Position { get; set;}
    int HP { get; set; }
    void TakeDamage(int damage, out int hpRemaining);
}