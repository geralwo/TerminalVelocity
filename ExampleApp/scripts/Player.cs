using System;
using TerminalVelocity;
public class Player : PhysicsObject
{
    int playerSpeed = 1;
    public Player(Vec2i _pos, string _display)
    {
        Position = _pos;
        Display = _display;
        InputEnabled = true;
        ProcessEnabled = true;
        name = "player";
    }
    public override void OnProcess()
    {
        MoveAndCollide();
    }

    public override void OnInput(ConsoleKey key)
    {
      switch(key)
      {
        case ConsoleKey.UpArrow:
          if(this.Velocity == Vec2i.ZERO)
            Velocity = Vec2i.UP * playerSpeed;
          break;
        case ConsoleKey.DownArrow:
          if (this.Velocity == Vec2i.ZERO)
             Velocity = Vec2i.DOWN * playerSpeed;
          break;
        case ConsoleKey.LeftArrow:
          if (this.Velocity == Vec2i.ZERO)
              Velocity = Vec2i.LEFT * playerSpeed;
          break;
        case ConsoleKey.RightArrow:
          if (this.Velocity == Vec2i.ZERO)
              Velocity = Vec2i.RIGHT * playerSpeed;
          break;
        case ConsoleKey.T:
          this.Position = Vec2i.ONE * 5;
          break;
      }
    }
}