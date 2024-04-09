using TerminalVelocity;
namespace TerminalVelocity;
public class Scene : SceneObject
{
    public ConsoleColor SceneBackgroundColor = ConsoleColor.Black;
	public Scene()
	{
		Visible = false;
	}

    public virtual void unload()
	{
		Console.Clear();
	}
}