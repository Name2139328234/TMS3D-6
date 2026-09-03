using ObservableCollections;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.Cinemachine;
using UnityEngine;



public class ShipCamera : MonoBehaviour
{
    [SerializeField] private Ship _ship;
    [SerializeField] private CinemachineTargetGroup _group;
    [SerializeField] private GameObject _cinemachineCamerasParent;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _desiredFOV;

    private bool _isGroupUpdatedLastFrame;



    void Awake()
    {
        _ship.Parts.CollectionChanged += UpdateGroup;
    }
    void Update()
    {
        if (_isGroupUpdatedLastFrame)
        {
            _cinemachineCamerasParent.SetActive(false);
            _camera.fieldOfView = _desiredFOV;
            _isGroupUpdatedLastFrame = false;
        }
    }



    private void UpdateGroup(in NotifyCollectionChangedEventArgs<KeyValuePair<Vector3Int, GameObject>> partArgs)
    {
        if (!partArgs.IsSingleItem)
            throw new Exception("Can't handle more than one ship part change at a time yet");

        if (partArgs.Action == NotifyCollectionChangedAction.Add)
            _group.AddMember(partArgs.NewItem.Value.transform, 1f, 1f);
        else if (partArgs.Action != NotifyCollectionChangedAction.Remove)
            _group.RemoveMember(partArgs.OldItem.Value.transform);
        else
            throw new Exception($"Unexpected action {partArgs.Action} can't be handled yet");

        _isGroupUpdatedLastFrame = true;
        _cinemachineCamerasParent.SetActive(true);
    }
}
