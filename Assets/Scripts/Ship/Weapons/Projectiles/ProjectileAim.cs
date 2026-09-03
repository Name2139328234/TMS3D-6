using UnityEngine;



public class ProjectileAim : MonoBehaviour
{
    [SerializeField] private ProjectileArgs _args;
    [Header("degrees per second")]
    [SerializeField] private Stat _turnSpeed;

    private TargetData _target;



    void Start()
    {
        _args.TryGet(out _target);
    }
    void Update()
    {
        if (_target.IsEmpty)
            return;

        var angles = transform.eulerAngles;
        float desiredAngle = Mathf.Atan2(_target.Position.y - transform.position.y, _target.Position.x - transform.position.x);
        angles.z = Mathf.MoveTowardsAngle(angles.z, desiredAngle, _turnSpeed * Time.deltaTime);
        transform.eulerAngles = angles;
    }
}
