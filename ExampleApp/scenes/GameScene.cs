using System.Runtime.InteropServices;
using TerminalVelocity;


public class GameScene : Scene
{
    Player p = new Player(Vec2i.ONE, EscapeRoomSettings.PlayerChar);
    Level l = new Level(EscapeRoomSettings.RoomSize.x, EscapeRoomSettings.RoomSize.y);
    public GameScene(string _name) : base(_name)
    {
        name = "game_scene;";
        p.Visible = true;
        p.Position = l.player_spawn;
        p.name = "player";
        p.BackgroundColor = ConsoleColor.White;
        p.Color = ConsoleColor.Red;
        p.ZIndex = 10;

        AddChild(l);
        AddChild(p);
        InputEnabled = true;
        ProcessEnabled = true;
    }

    public override void OnProcess()
    {
        if (Game.CurrentScene == this && l.ready)
        {
            if (p.Position == l.key?.Position)
            {
                if (l.door != null && l.key != null)
                {
                    l.door.Dispose();
                    l.key.Dispose();
                }
                else
                {
                    if (l.door == null)
                        throw new Exception("door not found");
                    else if (l.key == null)
                        throw new Exception("key not found");
                }
            }
            if (p.Position == l.door_spawn)
            {
                Game.CurrentScene = new WinScreen();
            }

        }


    }

    public override void OnInput(ConsoleKey key)
    {
        if (Game.CurrentScene == this)
        {
            switch (key)
            {
                case ConsoleKey.Escape:
                    Game.CurrentScene = new MainMenu();
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
