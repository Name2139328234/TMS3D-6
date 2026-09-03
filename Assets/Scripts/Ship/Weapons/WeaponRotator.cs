using UnityEngine;



public class WeaponRotator : MonoBehaviour
{
    public void Aim(Vector3 target)
    {
        var rot = transform.eulerAngles;
        rot.z = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        transform.eulerAngles = rot;
    }
}
