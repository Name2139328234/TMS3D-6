using ObservableCollections;
using System;
using UnityEngine;



public class WeaponInventory : MonoBehaviour
{
    public ObservableDictionary<WeaponInfo, int> AvailableWeapons { get => _availableAmount; }

    private ObservableDictionary<WeaponInfo, int> _availableAmount;



    public void Add(WeaponInfo weaponInfo, int amount)
    {
        if (!_availableAmount.ContainsKey(weaponInfo))
            _availableAmount.Add(weaponInfo, amount);
        else
            _availableAmount[weaponInfo] += amount;
    }
    public void Remove(WeaponInfo info, int amount)
    {
        if (_availableAmount[info] < amount)
            throw new Exception($"Not enough stored weapons of kind {info.Kind} and level {info.Level} to remove {amount} of them");

        _availableAmount[info] -= amount;
    }
}
