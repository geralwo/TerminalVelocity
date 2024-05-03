using TerminalVelocity;
internal class Menu : StartScene
{
    SceneObject playButton = new SceneObject("Play");
    public Menu(string _name = "MonsterSimMainMenu") : base (_name)
    {
        MenuEntries.AddChild(playButton,0);
        playButton.ProcessAction += () => { Game.CurrentScene = new PlayScene("MonsterSimPlayScene"); };
    }
}
