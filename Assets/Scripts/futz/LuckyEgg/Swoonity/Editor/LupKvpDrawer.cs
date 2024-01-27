using Swoonity.Collections;
using UnityEditor;
using UnityEngine;

namespace Swoonity.Editor
{
[CustomPropertyDrawer(typeof(LupKvp<,>))]
public class LupKvpDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty propKvp, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, propKvp);

		var propKey = propKvp.FindPropertyRelative("Key");
		var propVal = propKvp.FindPropertyRelative("Value");


		// position = EditorGUI.PrefixLabel(
		// 	position,
		// 	GUIUtility.GetControlID(FocusType.Passive),
		// 	new GUIContent(name)
		// );

		var half = position.width / 2;

		var keyRect = new Rect(position.x, position.y, half, position.height);
		var valRect = new Rect(position.x + half, position.y, half, position.height);

		var prevColor = GUI.color;

		var isDupe = propKvp.FindPropertyRelative("IsDupe").boolValue;
		if (isDupe) GUI.color = Color.red;

		EditorGUI.PropertyField(keyRect, propKey, GUIContent.none);
		EditorGUI.PropertyField(valRect, propVal, GUIContent.none);

		GUI.color = prevColor;

		EditorGUI.EndProperty();
	}
}
}