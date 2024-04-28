using TerminalVelocity;

public interface IAttackble
{
    int HP { get; set; }
    void TakeDamage(int damage);
}