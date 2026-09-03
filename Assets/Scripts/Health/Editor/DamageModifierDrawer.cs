#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;



[CustomPropertyDrawer(typeof(DamageModifiers.DamageModifier))]
public class DamageModifierDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty kindProp = property.FindPropertyRelative("Kind");
        SerializedProperty modProp = property.FindPropertyRelative("Modification");

        float kindWidth = position.width * 0.5f;
        float modWidth = position.width * 0.5f;

        Rect kindRect = new Rect(position.x, position.y, kindWidth, position.height);
        Rect modRect = new Rect(position.x + kindWidth, position.y, modWidth, position.height);

        EditorGUI.PropertyField(kindRect, kindProp, GUIContent.none);
        EditorGUI.PropertyField(modRect, modProp, GUIContent.none);
    }
}
#endif