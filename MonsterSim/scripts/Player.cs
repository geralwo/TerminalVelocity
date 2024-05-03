using System;
using Microsoft.VisualBasic;
using TerminalVelocity;
public class Player<T> : IDisposable where T : PhysicsObject, IAttackMove, IDefensiveMove, IMovementAbility, ICreature
{
    public T Character { get; private set; } // aka RigidBody
    int playerSpeed = 1;
    /// <summary>
    /// Represents a generic player class.
    /// T must inherit from PhysicsObejct and implement:
    ///   IAttackMove,
    ///   IAttackAble,
    ///   IDefensiveMove,
    ///   IMovementAbility
    /// </summary>
    /// <param name="_position">Vec2i _position</param>
    /// <param name="_character">T _character</param>
    public Player(Vec2i _position, T _character)
    {
        Character = _character;
        Character.Position = _position;
        Character.ProcessEnabled = true;
        Character.name = "playerBody";
        Character.ProcessAction += OnProcess;
        Character.ZIndex = 10;
        Input.KeyPressed += OnInput;
    }

    void OnProcess()
    {
        if (Character.HP < 0)
        {
            Dispose();
        }
    }

    void OnInput(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.UpArrow:
                if (Character.Velocity == Vec2i.ZERO)
                {
                    Character.Velocity = Vec2i.UP * playerSpeed;

                    {

                    }
                }
                break;
            case ConsoleKey.DownArrow:
                if (Character.Velocity == Vec2i.ZERO)
                {
                    Character.Velocity = Vec2i.DOWN * playerSpeed;
                    if (Input.LastKeyPressed.Modifiers.HasFlag(ConsoleModifiers.Shift))
                        Character.MovementAbility();
                }
                break;
            case ConsoleKey.LeftArrow:
                if (Character.Velocity == Vec2i.ZERO)
                    Character.Velocity = Vec2i.LEFT * playerSpeed;
                if (Input.LastKeyPressed.Modifiers.HasFlag(ConsoleModifiers.Shift))
                    Character.MovementAbility();
                break;
            case ConsoleKey.RightArrow:
                if (Character.Velocity == Vec2i.ZERO)
                    Character.Velocity = Vec2i.RIGHT * playerSpeed;
                if (Input.LastKeyPressed.Modifiers.HasFlag(ConsoleModifiers.Shift))
                    Character.MovementAbility();
                break;
            case ConsoleKey.Q:
                Character.AttackWith(0, Character);
                break;
            case ConsoleKey.W:
                Character.AttackWith(1, Character);
                break;
            case ConsoleKey.R:
                //Character.GetNodeByName("rat").BackgroundColor = ConsoleColor.Red;
                PhysicsServer.ToggleQuadTreeVisuals();
                break;
        }
    }
    public void Dispose()
    {
        Character.Dispose();
    }
}
