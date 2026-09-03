using UnityEngine;



public class HitsDestroy : MonoBehaviour
{
    [SerializeField] private HitRegistration _hitreg;
    [SerializeField] private Stat _hitsMax;

    private int _hitsLeft;



    void Start()
    {
        _hitsLeft = _hitsMax.GetRoundedvalue();
        _hitreg.OnHit += AttemptDestroy;
    }



    private void AttemptDestroy(RaycastHit2D _)
    {
        _hitsLeft--;

        if (_hitsLeft <= 0)
            Destroy(gameObject);
    }
}
