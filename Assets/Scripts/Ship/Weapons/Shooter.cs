using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;



public class Shooter : MonoBehaviour
{
    public event Action<GameObject> OnShot;

    [SerializeField] private Transform _shootPoint;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private int _reloadTurnsMax = 1;
    [SerializeField] private int _burst = 1;
    [SerializeField] private float _burstInnerReloadSeconds;

    private CancellationTokenSource _cts = new();
    private bool _isReloaded = true;
    private int _reloadTurns;



    void OnDestroy()
    {
        _cts.Cancel();
        _cts.Dispose();
    }


    
    public async UniTask<bool> TryShoot()
    {
        if (!_isReloaded)
            return false;

        await ShootBurst();

        return true;
    }



    private async UniTask ShootBurst()
    {
        var token = _cts.Token;

        for (int i = 0; i < _burst; i++)
        {

            var projectileGO = Instantiate(_projectile, _shootPoint.position, _shootPoint.rotation);

            OnShot?.Invoke(projectileGO);

            await UniTask.Delay(TimeSpan.FromSeconds(_burstInnerReloadSeconds), cancellationToken: token);
        }

        _isReloaded = false;
        _reloadTurns = _reloadTurnsMax;
    }
    private void Reload()
    {
        _reloadTurns--;

        if (_reloadTurns <= 0) 
            _isReloaded = true;
    }
}
