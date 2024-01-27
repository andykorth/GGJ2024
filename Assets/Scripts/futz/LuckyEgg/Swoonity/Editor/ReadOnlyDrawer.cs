using Swoonity.Unity;
using UnityEditor;
using UnityEngine;

namespace Swoonity.Editor
{
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var wasGuiEnabled = GUI.enabled;
		GUI.enabled = false;
		EditorGUI.PropertyField(position, property, label);
		GUI.enabled = wasGuiEnabled;
	}
}
}