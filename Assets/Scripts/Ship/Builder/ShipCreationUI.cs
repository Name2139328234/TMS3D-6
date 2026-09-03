using System;
using UnityEngine;
using UnityEngine.UI;



public class ShipCreationUI : MonoBehaviour
{
    public event Action<PartInfo> OnSelected;
    public event Action OnFinishButtonPressed;
    public event Action OnWeaponsButtonPressed;

    [SerializeField] private ShipBlockPricer _pricer;
    [SerializeField] private Transform _canvas;
    [SerializeField] private Vector2 _buttonDistance;
    [SerializeField] private GameObject _previewPrefab;
    [SerializeField] private BuilderButtonTip _tooltip;
    [SerializeField] private Button _finishButton;
    [SerializeField] private Button _weaponsButton;



    public void Initialize(ShipPartsStorage parts, ShipBlockPricer pricer)
    {
        _finishButton.onClick.AddListener(Finish);
        _weaponsButton.onClick.AddListener(CallWeaponsPress);

        _pricer = pricer;

        for (int y = 0; y < parts.AvailableParts.Length; y++)
        {
            var partLevelsInfo = parts.AvailableParts[y];

            /*
            for (int x = 0; x < partLevelsInfo.Levels.Length; x++)
            {
                var level = partLevelsInfo.Levels[x];
                var partSprite = level.GetComponent<SpriteRenderer>();

                var preview = Instantiate(_previewPrefab, _canvas);
                preview.transform.localPosition = new (x * _buttonDistance.x, y * _buttonDistance.y, 0);

                var button = preview.GetComponent<BuilderButton>();
                button.Initialize(partLevelsInfo.Kind, x, partSprite.sprite, partSprite.color);

                button.OnPress += SetSelected;
            }
            */
        }

        SetSelected(default);
    }



    private void SetSelected(PartInfo info)
    {
        OnSelected?.Invoke(info);

        _tooltip.SetData(_pricer.GetLevelledCost(info), info.Kind.ToString());
    }
    private void Finish()
    {
        OnFinishButtonPressed?.Invoke();
    }
    private void CallWeaponsPress()
    {
        OnWeaponsButtonPressed?.Invoke();
    }
}
