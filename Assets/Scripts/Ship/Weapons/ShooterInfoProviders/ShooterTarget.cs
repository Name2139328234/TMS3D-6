using UnityEngine;



public class ShooterTarget : ShooterInfoProvider
{
    private Transform _lastTarget;



    void Start()
    {
        GetComponentInParent<WeaponsPlatform>().OnTargeted += SaveLastTargetTransform;
        _shooter.OnShot += AttachTransformData;
    }



    private void AttachTransformData(GameObject projectile)
    {
        projectile.GetComponent<ProjectileArgs>().Add(typeof(TargetData), new TargetData(_lastTarget));
    }
    private void SaveLastTargetTransform(Transform target)
    {
        _lastTarget = target;
    }
}
