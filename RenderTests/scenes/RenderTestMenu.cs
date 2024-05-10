using TerminalVelocity;

public class RenderTestMenu : StartScene
{
    public RenderTestMenu(string _name = "RenderTestMenu") : base(_name)
    {
        // SceneObject Start = new SceneObject("Start Tests");
        // Start.ProcessAction = () =>Game.CurrentScene = new RenderTest();
        // MenuList.AddChild(Start, 0);
    }
    public override void OnStart()
    {
        base.OnStart();
        Game.CurrentScene = new RenderTest();
    }

}
