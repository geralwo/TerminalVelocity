using System.Data.SqlTypes;
using TerminalVelocity;
public class MainScene : Scene
{
    public MainScene(string _name) : base(_name)
    {
        Player<Ork> player= new Player<Ork>(Vec2i.ZERO, new Ork());
        add_child(player.Character);
        // Rat rat = new Rat(Vec2i.ONE * 5);
        // rat.ProcessAction += () => {         
        //     if(Game.FrameCount % 60 == 0)
        //     {
        //         rat.Velocity = Vec2i.RIGHT;
        //     }
        // };
        // Rat rat2 = new Rat(new Vec2i(40,5));
        // rat2.Display = ".::-";
        // rat2.ProcessAction += () => {         
        //     if(Game.FrameCount % 60 == 0)
        //     {
        //         rat2.Velocity = Vec2i.LEFT;
        //     }
        // };
        // add_child(rat);
        // add_child(rat2);
        PhysicsObject staticObject = new PhysicsObject(new Vec2i(2,0));
        staticObject.name = "staticBody";
        add_child(staticObject);
        InputEnabled = true;
    }
    public override void OnInput(ConsoleKey key)
    {
        base.OnInput(key);
    }
}