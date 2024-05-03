using TerminalVelocity;
using TerminalVelocity.core;

Game g = new Game();
Debug.CurrentLogLevel = Debug.LogLevel.Important;
//Game.CurrentScene = new MainScene("mainscene");
//g.Run(new MainScene("mainscene"));
g.Run(new Menu());

Game.PrintEngineStats();