using System;
using UnityEngine;



public class WeaponPlacer
{
    public event Action<WeaponInfo> OnBuild;
    public event Action<WeaponInfo> OnUnbuild;

    private WeaponsInfoStorage _infos;
    private Ship _ship;



    public WeaponPlacer(WeaponsInfoStorage infos, Ship ship)
    {
        _infos = infos;
        _ship = ship;
    }
    public void Build(WeaponInfo info, Vector3Int platformPosition)
    {
        WeaponsPlatform target = _ship.Parts[platformPosition].GetComponent<WeaponsPlatform>();
        var spawned = UnityEngine.Object.Instantiate(_infos.GetPrefab(info), target.transform.position, target.transform.rotation, target.transform);
        spawned.layer = target.gameObject.layer;
        target.SetWeapon(spawned);

        OnBuild?.Invoke(info);
    }
    public void Unbuild(Vector3Int platformPosition)
    {
        WeaponsPlatform target = _ship.Parts[platformPosition].GetComponent<WeaponsPlatform>();
        var weapon = target.RemoveWeapon();
        var info = weapon.Info;
        UnityEngine.Object.Destroy(weapon.gameObject);
        OnUnbuild?.Invoke(info);
    }
}
