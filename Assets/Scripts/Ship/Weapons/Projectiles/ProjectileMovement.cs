using UnityEngine;



public class ProjectileMovement : MonoBehaviour
{
    public Stat Speed { get => _speed; }

    [SerializeField] private Stat _speed;



    void Update()
    {
        transform.position += _speed * Time.deltaTime * transform.up;
    }
}
