namespace TerminalVelocity.AI;

public class AI<T>  where T : SceneObject, IAI
{
    public T body;
    public AI(T _body)
    {
        this.body = _body;
    }
}