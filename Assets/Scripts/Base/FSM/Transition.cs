


public class Transition : ITransition
{
    public IState To { get => _to; }
    public IPredicate Condition { get => _condition; }

    private IState _to;
    private IPredicate _condition;



    public Transition(IState to, IPredicate condition)
    {
        _to = to;
        _condition = condition;
    }
}
