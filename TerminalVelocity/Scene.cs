using TerminalVelocity;
namespace TerminalVelocity;
public class Scene : SceneObject
{
    public ConsoleColor SceneBackgroundColor = ConsoleColor.Black;
	public Scene()
	{
	}

    public virtual void unload()
	{
		Console.Clear();
	}
}