using TerminalVelocity;
Game g = new Game();
Game.LogLevel = LogLevel.Important;
g.Run(new RenderTestMenu());
Game.PrintEngineStats();
string path = Directory.GetCurrentDirectory();
string filePath = Path.Combine(path, $"RenderTestResult.txt");
using (StreamWriter sw = File.CreateText(filePath))
{
    sw.WriteLine(Game.EngineStats);
}
