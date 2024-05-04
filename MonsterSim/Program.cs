using TerminalVelocity;

Game g = new Game();
Game.LogLevel = LogLevel.Important;
//Game.CurrentScene = new MainScene("mainscene");
g.Run(new MainScene("mainscene"));

Game.PrintEngineStats();
