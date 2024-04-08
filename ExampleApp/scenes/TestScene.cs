using TerminalVelocity;

public class TestScene : Scene
{
    public SceneObject fpsMeter = new SceneObject();

    public SceneObject menuItems = new SceneObject();


    public TestScene()
    {
        ProcessEnabled = true;
        add_child(fpsMeter);
    }


    public override void OnProcess()
    {
        if(Game.FrameCount % 2 == 0)
        {
            fpsMeter.Display = ((int)((float)Game.FrameCount / ((float)Game.RunTime / 1000f))).ToString();
        }
        
    }
}