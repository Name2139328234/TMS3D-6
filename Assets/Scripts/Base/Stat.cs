using System;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class Stat
{
    public float ModifiedValue
    {
        get
        {
            if (_isDirty == true)
            {
                float val = _value;

                foreach (var modifier in _modifiers)
                {
                    val = modifier.Modification(val);
                }

                _modifiedValueCache = val;
                _isDirty = false;
            }

            return _modifiedValueCache;
        }
        set
        {
            if (_isDirty == true)
            {
                float val = _value;

                foreach (var modifier in _modifiers)
                {
                    val = modifier.Modification(val);
                }

                _modifiedValueCache = val;
                _isDirty = false;
            }

            _value += value * (_value / _modifiedValueCache);
        }
    }
    public float RawValue { get => _value; set => _value = value; }
    public bool IsPositive { get => _isPositive; }

    [SerializeField] private float _value;//beware, changing the name of this variable requires a drawer change too
    [SerializeField] private bool _isPositive = true;

    private List<StatModifier> _modifiers = new();
    private float _modifiedValueCache;
    private bool _isDirty = true;



    public Stat(float startValue)
    {
        _value = startValue;
    }
    public void AddModifier(StatModifier modifier)
    {
        _modifiers.Add(modifier);
        _isDirty = true;
    }
    public void RemoveModifier(StatModifier modifier)
    {
        _modifiers.Remove(modifier);
        _isDirty = true;
    }
    public int GetRoundedvalue(bool isAlwaysRoundedDown = true)
    {
        float value = ModifiedValue;

        if (isAlwaysRoundedDown)
            return Mathf.FloorToInt(value);
        else
            return Mathf.RoundToInt(value);
    }



    public static implicit operator float(Stat stat) => stat.ModifiedValue;
}
