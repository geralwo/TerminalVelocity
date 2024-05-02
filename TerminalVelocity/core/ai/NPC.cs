using System.Runtime.CompilerServices;

namespace TerminalVelocity.AI;
using TerminalVelocity.AI;
public abstract class NPC<T> : IStateMachine where T : PhysicsObject
{
    public T Body;
    public NPC(T _body)
    {
        Body = _body;
    }
}