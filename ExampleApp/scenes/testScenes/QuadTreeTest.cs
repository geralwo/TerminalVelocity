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
        // aabb.Position = Vec2i.ZERO;
        // aabb.Size = Vec2i.ONE * 4;
        // add_child(aabb.DrawBounds());
        // AABB naabb = new AABB();
        // naabb.Position = new Vec2i(1, 1);
        // naabb.Size = new Vec2i(8, 2);
        // if (aabb.Contains(naabb))
        // {
        //     add_child(naabb.DrawBounds());
        // }
        // if (aabb.Contains(naabb.Position))
        // {
        //     add_child(naabb.DrawBounds());
        // }

        qt = new QuadTree(Vec2i.ZERO, Game.Settings.Engine.WindowSize, 8);
        qt_vis = qt.visualize();
        add_child(qt_vis);
        add_child(cursor);
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
                SceneObject n = new SceneObject(cursor.Position," ");
                n.BackgroundColor = Game.GetRandomConsoleColor();
                if(qt.Insert(n))
                {
                    remove_child(qt_vis);
                    add_child(n);
                    qt_vis = qt.visualize();
                }
                break;
            case ConsoleKey.Escape:
                Game.CurrentScene = new MainMenu();
                break;
        }
    }
}