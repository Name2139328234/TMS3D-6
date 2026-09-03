using System;
using UnityEngine;



[Serializable]
public struct WeaponInfo//Unity can't serialize records or readonly structs. Do not introduce any methods to change kind or level of instance of this struct, since that will break "GetHashCode" method
{
    public WeaponKind Kind { get => _kind; }
    public int Level { get => _level; }

    [SerializeField] private WeaponKind _kind;
    [SerializeField] private int _level;



    public WeaponInfo(WeaponKind kind, int level)
    {
        _kind = kind;
        _level = level;
    }

    

    public override readonly int GetHashCode() => HashCode.Combine(_kind, _level);
    public override readonly bool Equals(object obj)
    {
        return obj is WeaponInfo info &&
               _kind == info._kind &&
               _level == info._level;
    }
    public static bool operator ==(WeaponInfo left, WeaponInfo right)
    {
        return (left.Kind == right.Kind && left.Level == right.Level);
    }
    public static bool operator !=(WeaponInfo left, WeaponInfo right)
    {
        return !(left == right);
    }
}
