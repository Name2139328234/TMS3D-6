using Closures;
using UnityEngine;



public class ProjectileAcceleration : MonoBehaviour
{
    [SerializeField] private ProjectileMovement _target;
    [SerializeField] private Stat _acñeleration;

    private StatModifier _previousFrameModifier;
    private float _accumulatedAcceleration;



    void Update()
    {
        _accumulatedAcceleration += _acñeleration * Time.deltaTime;

        if (_previousFrameModifier != null)
            _target.Speed.RemoveModifier(_previousFrameModifier);

        _previousFrameModifier = StatModifier.Addition(_accumulatedAcceleration);
        _target.Speed.AddModifier(_previousFrameModifier);
    }
}
