using Cysharp.Threading.Tasks;
using System;
using UnityEngine;



public class TimeDestroy : MonoBehaviour
{
    [SerializeField] private float _deathTimeSeconds;



    void Start()
    {
        Die().Forget();
    }



    private async UniTask Die()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_deathTimeSeconds), cancellationToken: gameObject.GetCancellationTokenOnDestroy());

        Destroy(gameObject);
    }
}
