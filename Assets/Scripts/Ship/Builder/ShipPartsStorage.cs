using System;
using UnityEngine;



[CreateAssetMenu(fileName = "ShipPartsStorage", menuName = "Custom/ShipPartsStorage", order = 0)]
public class ShipPartsStorage : ScriptableObject
{
    public PartObject[] AvailableParts { get => _availableParts; }

    [SerializeField] private PartObject[] _availableParts;



    public PartObject Get(PartKind partKind)
    {
        foreach (var part in _availableParts)
            if (part.Kind == partKind)
                return part;

        throw new Exception($"No part of kind {partKind} found");
    }
}



[Serializable]
public struct PartObject
{
    public PartKind Kind;
    public GameObject Part;
}
