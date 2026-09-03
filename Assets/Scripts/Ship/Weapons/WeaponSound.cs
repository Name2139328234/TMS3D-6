using Closures;
using UnityEngine;



public class WeaponSound : MonoBehaviour
{
    [SerializeField] private Shooter _weapon;
    [SerializeField] private AudioSource _sound;



    void Start()
    {
        _weapon.OnShot += Play;
    }



    private void Play(GameObject _)
    {
        _sound.Play();
    }
}
