using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponInfo Info { get => _info; }

    [SerializeField] private WeaponInfo _info;
}
