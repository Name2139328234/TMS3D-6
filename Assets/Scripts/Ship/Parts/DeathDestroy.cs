using R3;
using UnityEngine;



public class DeathDestroy : MonoBehaviour
{
    [SerializeField] private Health _health;



    void Start()
    {
        _health.Value
            .Where(health => health <= 0)
            .Subscribe(DestroySelf)
            .AddTo(gameObject);
    }



    private void DestroySelf(float _)//ugly, but no heap allocation
    {
        Destroy(gameObject);
    }
}
