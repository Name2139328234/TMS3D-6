#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;



[CustomPropertyDrawer(typeof(Stat))]
public class StatDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty valueProp = property.FindPropertyRelative("_value");
        SerializedProperty positiveProp = property.FindPropertyRelative("_isPositive");

        float valueWidth = position.width * 0.8f;
        float positiveWidth = position.width * 0.2f;

        Rect valueRect = new Rect(position.x, position.y, valueWidth, position.height);
        Rect positiveRect = new Rect(position.x + valueWidth, position.y, positiveWidth, position.height);

        EditorGUI.PropertyField(valueRect, valueProp, label);
        EditorGUI.PropertyField(positiveRect, positiveProp, GUIContent.none);
    }
}
#endif
