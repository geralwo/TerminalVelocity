using TerminalVelocity;
public class Program
{
    public static void Main(string[] args)
    {
        Game game = new Game();
        TestScene scene = new TestScene();
        Game.CurrentScene = scene;
        game.Run();

        Console.ResetColor();
        Console.Clear();
        Console.WriteLine($"total frames   : {Game.FrameCount}");
        Console.WriteLine($"total run time : {Game.RunTime} ms");
        Console.WriteLine($"average fps    : {(int)((float)Game.FrameCount / ((float)Game.RunTime / 1000f))}");
        Console.WriteLine($"render items   : { RenderServer.Instance.count() }");
        Console.ReadKey(true);
    }


    public class TestScene : Scene
    {
        SceneObject fpsMeter;
        SceneObject Player;
        SelectBox selectBox;

        
        public TestScene()
        {
            fpsMeter         = new SceneObject("hi");
            ProcessEnabled   = true;
            InputEnabled     = true;

            fpsMeter.ForegroundColor = ConsoleColor.White;
            fpsMeter.BackgroundColor = ConsoleColor.Red;
            Player   = new SceneObject("@");
            Player.Position      = new Vec2i(3, 3);
            Player.Visible       = true;
            Player.InputEnabled = true;
            Player.InputAction = () => { this.Position += Vec2i.DOWN; };
            
            add_child(fpsMeter);
            add_child(Player);
            fpsMeter.Visible = true;
        }


        public override void OnProcess()
        {
            
                
        }

        public override void OnInput(ConsoleKey key)
        {
            fpsMeter.Display = Player.Position.ToString();
            if (key == ConsoleKey.LeftArrow)
            {
                Player.Position += Vec2i.DOWN;
            }

            if(key == ConsoleKey.Escape)
                Game.quitting = true;
        }
}

    public class SelectBox : SceneObject
    {

    }
}