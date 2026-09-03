using R3;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class WeaponUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public ReadOnlyReactiveProperty<bool> IsPointerDown { get => _isPointerDown; }
    public WeaponInfo Info { get => _info; }
    public int Count { get => _count; set => _count = value; }

    [SerializeField] private Image _preview;
    [SerializeField] private TextMeshProUGUI _countUI;

    private ReactiveProperty<bool> _isPointerDown = new();
    private WeaponInfo _info;
    private int _count;



    public void OnPointerDown(PointerEventData eventData)
    {
        _isPointerDown.Value = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPointerDown.Value = false;
    }
    public void Initialize(Sprite icon, int count, WeaponInfo info)
    {
        _preview.sprite = icon;
        _count = count;
        _countUI.text = count.ToString();
        _info = info;
    }
    public void UpdateCount(int count)
    {
        _count = count;
        _countUI.text = count.ToString();
    }
}
