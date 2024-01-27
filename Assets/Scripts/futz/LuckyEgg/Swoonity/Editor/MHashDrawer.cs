using Swoonity.MHasher;
using UnityEditor;
using UnityEngine;

namespace Swoonity.Editor
{
[CustomPropertyDrawer(typeof(MHash))]
public class MHashDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty propMHash, GUIContent label)
	{
		var propHashVal = propMHash.FindPropertyRelative("Value");

		var guiWasEnabled = GUI.enabled;
		GUI.enabled = false;
		EditorGUI.PropertyField(position, propHashVal, label);
		GUI.enabled = guiWasEnabled;
	}
}
}