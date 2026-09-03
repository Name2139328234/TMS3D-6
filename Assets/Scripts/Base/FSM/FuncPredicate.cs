using System;



public class FuncPredicate : IPredicate
{
    private Func<bool> _func;



    public FuncPredicate(Func<bool> func)
    {
        _func = func;
    }
    public bool Evaluate()
    {
        return _func();
    }
}
public class FuncPredicate<T> : IPredicate //no way to achieve varying generic arguments without class overload
{
    private T _arg;
    private Func<T, bool> _predicate;



    public FuncPredicate(T arg, Func<T, bool> predicate)
    {
        _arg = arg;
        _predicate = predicate;
    }
    public bool Evaluate()
    {
        return _predicate(_arg);
    }
}
