using System.Net.Security;
using System.Reflection.Metadata;
using TerminalVelocity;
public class TestScene : Scene
{
    SceneObject fpsMeter;
    SceneObject hello;
    public SceneObject menuItems = new SceneObject();

    
    public TestScene()
    {
        fpsMeter         = new SceneObject("hi");
        fpsMeter.Visible = true;
        ProcessEnabled   = true;
        InputEnabled     = true;
        fpsMeter.Foreground = ConsoleColor.White;
        fpsMeter.Background = ConsoleColor.Red;
        hello   = new SceneObject("Jeööo");
        hello.Position      = new Vec2i(3, 3);
        hello.Visible       = true;
        hello.InputEnabled = true;
        hello.InputAction = () => { this.Position += Vec2i.DOWN; };
        
        add_child(fpsMeter);
        add_child(hello);
    }


    public override void OnProcess()
    {
        if(Game.FrameCount % 2 == 0)
            // fpsMeter.Display = ((int)((float)Game.FrameCount / ((float)Game.RunTime / 1000f))).ToString();
            fpsMeter.Display = hello.Position.ToString();
            
    }

    public override void OnInput(ConsoleKey key)
    {
        if (key == ConsoleKey.LeftArrow)
        {
            hello.Position += Vec2i.DOWN;
        }

        if(key == ConsoleKey.Escape)
            Game.quitting = true;
    }
}