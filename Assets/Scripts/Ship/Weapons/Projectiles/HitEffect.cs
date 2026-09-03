using UnityEngine;



public class HitEffect : MonoBehaviour
{
    [SerializeField] private HitRegistration _hitreg;
    [SerializeField] private GameObject _effectPrefab;



    void Start()
    {
        _hitreg.OnHit += SpawnEffect;
    }



    private void SpawnEffect(RaycastHit2D hit)
    {
        Instantiate(_effectPrefab, hit.point, Quaternion.identity);
    }
}
