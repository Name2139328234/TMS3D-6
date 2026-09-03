using ObservableCollections;
using System;
using UnityEngine;



public class Ship : MonoBehaviour
{
    public Action<Ship> OnDead;

    public ObservableDictionary<Vector3Int, GameObject> Parts { get => _parts; }

    private ObservableDictionary<Vector3Int, GameObject> _parts = new();



    public bool IsOccupied(Vector3Int position)
    {
        return _parts.ContainsKey(position);
    }
    public void AddPart(Vector3Int position, GameObject part)
    {
        _parts.Add(position, part);
        //part.GetComponent<Health>().OnDie += RemoveDeadPart;
    }
    public GameObject RemovePart(Vector3Int position)
    {
        GameObject part = _parts[position];

        _parts.Remove(position);

        if (_parts.Count == 0)
            OnDead?.Invoke(this);

        return part;
    }
}
