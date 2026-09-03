using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class WeaponPlacementUI : MonoBehaviour
{
    public event Action OnBackButtonPressed;

    [SerializeField] private WeaponsInfoStorage _infos;
    [SerializeField] private WeaponInventory _placer;
    [SerializeField] private Transform _spawnStart;
    [SerializeField] private GameObject _prefabWeaponUI;
    [SerializeField] private float _deltaPosY;
    [SerializeField] private Button _backToBuildMenuButton;

    private Dictionary<WeaponInfo, WeaponUI> _weaponsUI = new();



    void Awake()
    {
        _placer.AvailableWeapons.CollectionChanged += UpdateUI;
        _backToBuildMenuButton.onClick.AddListener(CallButtonClick);
    }



    private void UpdateUI(in ObservableCollections.NotifyCollectionChangedEventArgs<KeyValuePair<WeaponInfo, int>> e)
    {
        if (!e.IsSingleItem)
            throw new Exception("can't handle multiple items being changed yet");

        switch (e.Action)
        {
            case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                var spawned = Instantiate(_prefabWeaponUI, _spawnStart).GetComponent<WeaponUI>();
                spawned.Initialize(_infos.GetPreview(e.NewItem.Key), e.NewItem.Value, e.NewItem.Key);
                var pos = spawned.transform.localPosition;
                pos.y = (_placer.AvailableWeapons.Count - e.NewStartingIndex) * _deltaPosY;
                spawned.transform.localPosition = pos;
                _weaponsUI.Add(e.NewItem.Key, spawned);
                break;
            case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                Destroy(_weaponsUI[e.OldItem.Key].gameObject);
                _weaponsUI.Remove(e.OldItem.Key);
                break;
            case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                _weaponsUI[e.NewItem.Key].UpdateCount(e.NewItem.Value);
                break;
            default:
                throw new Exception($"can't handle {e.Action} action yet");
        }
    }
    private void CallButtonClick()
    {
        OnBackButtonPressed?.Invoke();
    }
}