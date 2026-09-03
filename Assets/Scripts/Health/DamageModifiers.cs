using System;
using System.Collections.Generic;
using UnityEngine;



public class DamageModifiers : MonoBehaviour
{
    [SerializeField] private List<DamageModifier> _modifiers = new();



#if UNITY_EDITOR
    void OnValidate()
    {
        foreach (DamageKind kind in Enum.GetValues(typeof(DamageKind)))
        {
            bool isAvailable = false;
            
            foreach (DamageModifier modifier in _modifiers)
            {
                if (modifier.Kind == kind)
                {
                    isAvailable = true;
                    break;
                }
            }

            if (isAvailable)
                continue;

            _modifiers.Add(new DamageModifier(kind, 1f));
        }
    }
#endif



    public float Modify(float amount, DamageKind kind)
    {
        foreach (var modifier in _modifiers)
            if (modifier.Kind == kind)
                return amount * modifier.Modification;

        Debug.LogWarning($"No modifier for damage of kind {kind} found, assuming it is unmodified, returning the value that was recieved unchanged");
        return amount;
    }



    [Serializable]
    public struct DamageModifier
    {
        public DamageKind Kind;
        public Stat Modification;



        public DamageModifier(DamageKind kind, float modification)
        {
            Kind = kind;
            Modification = new Stat(modification);
        }
    }
}
