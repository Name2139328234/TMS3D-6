using UnityEngine;

public class TargetData
{
    public bool IsEmpty { get => _target == null; }
    public Vector3 Position { get => _target.position; }

    private Transform _target;



    public TargetData(Transform target)
    {
        _target = target;
    }
}