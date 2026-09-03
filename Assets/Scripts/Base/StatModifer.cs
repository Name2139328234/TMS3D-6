using Closures;
using System;



public class StatModifier
{
    public Func<float, float> Modification { get => _modification; }
    public int Order { get => _order; }

    private Func<float, float> _modification;
    private int _order;



    private StatModifier(Func<float, float> modification, int order)
    {
        _modification = modification;
        _order = order;
    }



    public static StatModifier Addition(float value)
    {
        return new StatModifier(Closure.Func<float, float, float>(value, (original, added) => original + added).AsFunc(), 0);
    }
    public static StatModifier Multiplication(float value)
    {
        return new StatModifier(Closure.Func<float, float, float>(value, (original, multiplier) => original * multiplier).AsFunc(), 1);
    }
}

