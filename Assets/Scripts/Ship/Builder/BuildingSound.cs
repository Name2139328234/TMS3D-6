using Closures;
using UnityEngine;



public class BuildingSound : MonoBehaviour
{
    [SerializeField] private AudioSource _buildSound;
    [SerializeField] private AudioSource _unbuildSound;

    private ShipBlockPricer _pricer;



    public void Initialize(ShipBlockPricer pricer)
    {
        _pricer = pricer;
        _pricer.OnBlockPayed += Closure.Action(_buildSound, sound => sound.Play()).AsAction();
        _pricer.OnBlockRefunded += Closure.Action(_unbuildSound, sound => sound.Play()).AsAction();
    }
}
