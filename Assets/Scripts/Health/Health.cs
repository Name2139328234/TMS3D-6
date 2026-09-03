using R3;
using UnityEngine;

public class Health : MonoBehaviour
{
    public ReadOnlyReactiveProperty<float> Value { get => _value; }
    public Stat MaxValue { get => _maxValue; }

    [SerializeField] private Stat _maxValue;

    private ReactiveProperty<float> _value = new();
    private bool _isDead;//failsafe for multiple deadly attacks on the same frame



    public void ApplyDamage(float amount)
    {
        if (_isDead)
        {
            Debug.LogWarning($"{gameObject.name} tried to die multiple times in the same frame. Secondary death attempt prevented");
            return;
        }

        if (_value.Value - amount < 0)
            amount = _maxValue;

        _value.Value -= amount;

        if (_value.Value <= 0)
            _isDead = true;
    }
    public void ApplyHeal(float amount)
    {
        if (_isDead)
        {
            Debug.LogWarning($"{gameObject.name} tried to be healed after it dies. Healing attempt prevented");
            return;
        }

        if (_value.Value + amount < _maxValue)
            amount = _maxValue - _value.Value;

        _value.Value += amount;
    }
}
