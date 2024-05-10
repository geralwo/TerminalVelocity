using TerminalVelocity;

Game g = new Game();
Game.LogLevel = LogLevel.Game;
//Game.CurrentScene = new MainScene("mainscene");
g.Run(new DungeonScene("mainscene"));

Game.PrintEngineStats();
