using UnityEngine;



[CreateAssetMenu(fileName = "PartLevelUtility", menuName = "Custom/PartLevelUtility")]
public class PartLevelUtility : ScriptableObject
{
    [SerializeField] private int _maxLevel;
    [SerializeField] private int _levelStatIncrease;
    [SerializeField] private int _levelPriceIncrease;
    [SerializeField] private Color _defaultLevelColor;
    [SerializeField] private Color _secondLevelColor;
    [SerializeField] private Color _maxLevelColor;
}
