using UnityEngine;



public class Damage : MonoBehaviour
{
    [SerializeField] private Stat _baseAmount;
    [SerializeField] private DamageKind _kind;



    public void Deal(Health health, DamageModifiers modifiers = null)
    {
        health.ApplyDamage(modifiers != null ? modifiers.Modify(_baseAmount, _kind) : _baseAmount);
    }
}
