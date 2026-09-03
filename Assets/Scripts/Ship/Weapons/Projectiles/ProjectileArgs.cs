using System;
using System.Collections.Generic;
using UnityEngine;



public class ProjectileArgs : MonoBehaviour
{
    private Dictionary<Type, object> _args = new();

    

    public ProjectileArgs Add(Type type, object obj)
    {
        if (!type.IsInstanceOfType(obj))
            throw new Exception($"{obj} isn't of type {type}");

        _args.Add(type, obj);

        return this;
    }
    public bool TryGet<T>(out T result)
    {
        if (!_args.ContainsKey(typeof(T)))
        {
            result = default;
            return false;
        }

        result = (T)_args[typeof(T)];
        return true;
    }
}
