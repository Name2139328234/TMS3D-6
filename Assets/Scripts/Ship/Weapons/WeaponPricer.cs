using ObservableCollections;
using UnityEngine;



public class WeaponPricer
{
    private WeaponPlacer _placer;
    private WeaponInventory _inventory;



    public WeaponPricer(WeaponPlacer placer, WeaponInventory inventory)
    {
        _placer = placer;
        _inventory = inventory;

        _placer.OnBuild += Pay;
        _placer.OnUnbuild += Refund;
    }

    

    private void Pay(WeaponInfo info)
    {
        _inventory.Remove(info, 1);
    }
    private void Refund(WeaponInfo info)
    {
        _inventory.Add(info, 1);
    }
}
