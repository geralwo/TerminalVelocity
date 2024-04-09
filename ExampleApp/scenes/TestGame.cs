using TerminalVelocity;


public class GameScene : Scene
{
    Player p = new Player(Vec2i.ONE, Game.Settings.player_char);
    Level l = new Level(Game.Settings.lvl_size.x, Game.Settings.lvl_size.y);
    PhysicsObject d;
    SceneObject k;
    public GameScene()
    {
        p.Visible = true;
        l.Visible = true;
        l.name = "level";
        InputEnabled = true;
        ProcessEnabled = true;
        add_child(p);
        add_child(l);
        p.Position = l.player_spawn;
        p.BackgroundColor = ConsoleColor.White;
        p.ForegroundColor = ConsoleColor.Red;
        p.ZIndex = 10;
        d = l.GetNodeById(l.door_guid) as PhysicsObject;
        k = l.GetNodeById(l.key_guid);

        SceneObject ztest = new SceneObject(new Vec2i(7,7),"Z");
        ztest.ZIndex = 0;
        ztest.Visible = true;
    }

    public override void OnProcess()
    {
        if(Game.CurrentScene == this)
        {
            if(p.Position == k.Position)
            {
                if (k != null && d != null)
                {
                    d.Dispose();
                    k.Dispose();
                }
                else
                {
                    throw new Exception("door or key not found");
                }
            }
        }


    }

    public override void OnInput(ConsoleKey key)
    {
        if(Game.CurrentScene == this && key == ConsoleKey.Escape)
            Game.quitting = true;
        if(Game.CurrentScene == this && key == ConsoleKey.Enter)
            Game.CurrentScene = new GameScene();
    }

    private void Sound()
    {
        Game.Beep(1940, 100);
        Game.Beep(2180, 233);
        Game.Beep(1556, 400);
    }
}