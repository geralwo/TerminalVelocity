using TerminalVelocity;
namespace TerminalVelocity;
public class Scene : SceneObject
{
	public Scene(string _name)
	{
		name = _name;
		Visible = false;
	}
	public override void OnInput(ConsoleKey key)
	{
		if (key == ConsoleKey.Escape)
			Game.Quit = true;
	}
	public virtual void unload()
	{
		Console.Clear();
	}
}
