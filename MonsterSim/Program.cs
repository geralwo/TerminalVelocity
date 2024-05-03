using TerminalVelocity;

Game g = new Game();
Game.Settings.Engine.Log = TerminalVelocity.core.Debug.LogLevel.Important;
//Game.CurrentScene = new MainScene("mainscene");
g.Run(new MainScene("mainscene"));

Game.PrintEngineStats();
