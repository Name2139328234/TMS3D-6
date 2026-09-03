using R3;
using System;
using UnityEngine;
using UnityEngine.InputSystem;



public class DragWeaponUI : MonoBehaviour
{
    public event Action<WeaponInfo, WeaponsPlatform> OnDragSuccess;

    [SerializeField] private WeaponUI _ui;

    private Vector3 _initialPos;
    private bool _isDragged;



    void Start()
    {
        _initialPos = transform.position;
        
        _ui.IsPointerDown
            .Skip(1)//avoid calling method on first value emission (if not done, will emit "false" at initialization, setting position to Vector3.zero)
            .Subscribe(ChangeState)
            .AddTo(this);
    }
    void Update()
    {
        if (_isDragged)
        {
            transform.position = Mouse.current.position.ReadValue();
        }
    }



    private void ChangeState(bool isDown)
    {
        if (isDown)
        {
            _initialPos = transform.position;

            if (_ui.Count <= 0)
                return;

            _isDragged = true;
        }
        else
        {
            transform.position = _initialPos;
            _isDragged = false;

            if (_ui.Count <= 0)
                return;

            var hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero, 1f);

            foreach (var hit in hits)
            {
                if (!hit)
                    continue;

                var platform = hit.collider.GetComponent<WeaponsPlatform>();

                if (!platform)
                    continue;

                OnDragSuccess?.Invoke(_ui.Info, platform);
            }
        }
    }
}
