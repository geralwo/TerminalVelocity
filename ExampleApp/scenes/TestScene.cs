using System.Reflection.Metadata;
using TerminalVelocity;
public class TestScene : Scene
{
    public SceneObject fpsMeter;

    public SceneObject menuItems = new SceneObject();


    public TestScene()
    {
        fpsMeter         = new SceneObject("hi");
        fpsMeter.Visible = true;
        ProcessEnabled   = true;
        InputEnabled     = true;
        fpsMeter.Foreground = ConsoleColor.White;
        fpsMeter.Background = ConsoleColor.Red;
        add_child(fpsMeter);
    }


    public override void OnProcess()
    {
        if(Game.FrameCount % 2 == 0)
            fpsMeter.Display = ((int)((float)Game.FrameCount / ((float)Game.RunTime / 1000f))).ToString();
    }

    public override void OnInput(ConsoleKey key)
    {

        if(key == ConsoleKey.Escape)
            Game.quitting = true;
    }
}