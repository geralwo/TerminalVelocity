namespace TerminalVelocity;

public class QuadTreeTest : Scene
{
    private AABB aabb;

    private QuadTree qt;

    private SceneObject qt_vis;
    
    SceneObject cursor = new SceneObject(Vec2i.ZERO,"x");
    
    public QuadTreeTest(string _name = "QuadTreeTest") : base(_name)
    {
        cursor.ZIndex = 10;
        InputEnabled = true;
        qt = new QuadTree(Vec2i.ZERO, Game.Settings.Engine.WindowSize, 8);
        qt_vis = qt.Visualize();
        AddChild(qt_vis);
        AddChild(cursor);
        cursor.center_xy();
    }
    
    public override void OnInput(ConsoleKey key)
    {
        switch(key)
        {
            case ConsoleKey.UpArrow:
                cursor.Position += Vec2i.UP;
                break;
            case ConsoleKey.DownArrow:
                cursor.Position += Vec2i.DOWN;
                break;
            case ConsoleKey.LeftArrow:
                cursor.Position += Vec2i.LEFT;
                break;
            case ConsoleKey.RightArrow:
                cursor.Position += Vec2i.RIGHT;
                break;
            case ConsoleKey.Enter:
                PhysicsObject n = new PhysicsObject(cursor.Position);
                n.Color = Game.GetRandomConsoleColor();
                if(qt.Insert(n.Position,n))
                {
                    RemoveChild(qt_vis);
                    AddChild(n);
                    qt_vis = qt.Visualize();
                }
                break;
            case ConsoleKey.Escape:
                Game.CurrentScene = new MainMenu();
                break;
        }
    }
}