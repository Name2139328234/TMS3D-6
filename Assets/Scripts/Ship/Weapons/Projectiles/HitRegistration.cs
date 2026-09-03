using System;
using System.Collections.Generic;
using UnityEngine;



public class HitRegistration : MonoBehaviour
{
    public Action<RaycastHit2D> OnHit;

    [SerializeField] private ProjectileMovement _hitDistanceSource;
    [SerializeField] private ProjectileArgs _args;
    [Header("defines if this can hit same collider twice")]
    [SerializeField] private bool _isMultiHit;

    private List<Collider2D> _hits;
    private LayerMask _targets;



    void Start()
    {
        _args.TryGet(out _targets);
        if (!_isMultiHit)
            _hits = new();
    }
    void Update()
    {
        var hit = Physics2D.Raycast(transform.position, transform.up, _hitDistanceSource.Speed * Time.deltaTime, _targets);

        if (hit && (_isMultiHit || !_hits.Contains(hit.collider)))
        {
            if (!_isMultiHit)
                _hits.Add(hit.collider);

            OnHit?.Invoke(hit);
        }
    }
}
