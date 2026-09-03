using UnityEngine;



public class ShooterLayer : ShooterInfoProvider
{
    void Start()
    {
        _shooter.OnShot += AttachLayer;
    }



    private void AttachLayer(GameObject obj)
    {
        obj.GetComponent<ProjectileArgs>().Add(typeof(LayerMask), (LayerMask)Physics2D.GetLayerCollisionMask(gameObject.layer));
    }
}
