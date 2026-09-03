using UnityEngine;



public class HitDamage : MonoBehaviour
{
    [SerializeField] private HitRegistration _hitreg;
    [SerializeField] private Stat _damage;
    [SerializeField] private DamageKind _kind;



    void Start()
    {
        _hitreg.OnHit += ProcessHit;
    }



    private void ProcessHit(RaycastHit2D hit)
    {
        var health = hit.collider.GetComponent<Health>();
        var modifiers = hit.collider.GetComponent<DamageModifiers>();

        if (health)
            health.ApplyDamage(modifiers != null ? modifiers.Modify(_damage, _kind) : _damage);
    }
}
