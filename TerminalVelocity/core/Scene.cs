using TerminalVelocity;
namespace TerminalVelocity;
public class Scene : SceneObject
{
    public ConsoleColor SceneBackgroundColor = ConsoleColor.Black;
	public Scene(string _name)
	{
		name = _name;
		Visible = false;
	}
    public override void OnInput(ConsoleKey key)
    {
        if (key == ConsoleKey.Escape)
			Game.quitting = true;
    }
    public virtual void unload()
	{
		Console.Clear();
	}
}