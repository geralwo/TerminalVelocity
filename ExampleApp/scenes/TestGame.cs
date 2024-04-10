using TerminalVelocity;


public class GameScene : Scene
{
    Player p = new Player(Vec2i.ONE, TestGameSettings.player_char);
    Level l = new Level(TestGameSettings.level_size.x,TestGameSettings.level_size.y);
    public GameScene(string _name) : base(_name)
    {
        name = "game_scene;";
        p.Visible = true;
        InputEnabled = true;
        ProcessEnabled = true;
        p.Position = l.player_spawn;
        p.BackgroundColor = ConsoleColor.White;
        p.ForegroundColor = ConsoleColor.Red;
        p.ZIndex = 10;

        SceneObject ztest = new SceneObject(new Vec2i(7,7),"Z");
        ztest.name = "ztest";
        ztest.ZIndex = 0;
        ztest.Visible = true;

        add_child(l);
        add_child(p);
        add_child(ztest);
    }

    public override void OnProcess()
    {
        if(Game.CurrentScene == this)
        {
            if(p.Position == l.key.Position)
            {
                if (l.door != null && l.key != null)
                {
                    l.door.Dispose();
                    l.key.Dispose();
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
        if(Game.CurrentScene == this)
        {
            switch(key)
            {
                case ConsoleKey.Escape:
                    Game.CurrentScene = new SettingsMenu();
                    return;
                case ConsoleKey.Enter:
                    Game.CurrentScene = new GameScene("game scene after enter");
                    return;
                default:
                    return;
            }
        }
    }
}