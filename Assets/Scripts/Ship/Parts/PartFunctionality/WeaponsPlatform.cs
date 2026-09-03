using Cysharp.Threading.Tasks;
using System;
using UnityEngine;



public class WeaponsPlatform : MonoBehaviour
{
    public event Action<Transform> OnTargeted;

    public Weapon Weapon { get => _weapon; }

    private Weapon _weapon;
    private Shooter _shooter;
    private WeaponRotator _aimer;
    private Transform _lastTarget;
    private Vector3 _lastTargetPos;

    

    void Update()
    {
        if (_aimer == null)
            return;

        if (_lastTarget != null)
            _aimer.Aim(_lastTarget.position);
        else if (_lastTargetPos != default)
            _aimer.Aim(_lastTargetPos);
    }



    public void Fire()
    {
        if (_shooter == null) 
            return;

        _shooter.TryShoot().Forget();
    }
    public void Aim(Transform target)
    {
        _lastTarget = target;

        OnTargeted?.Invoke(target);
    }
    public void Aim(Vector3 pos)
    {
        _lastTargetPos = pos;
    }
    public void SetWeapon(GameObject weapon)
    {
        if (_weapon != null)
            throw new Exception("Tried to place weapon into a busy slot");

        _weapon = weapon.GetComponent<Weapon>();
        _shooter = weapon.GetComponent<Shooter>();
        _aimer = weapon.GetComponent<WeaponRotator>();
    }
    public Weapon RemoveWeapon()
    {
        var result = _weapon;

        _weapon = null;
        _shooter = null;
        _aimer = null;

        return result;
    }
}
