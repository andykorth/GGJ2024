using Swoonity.CSharp;
using Swoonity.Unity;
using UnityEditor;
using UnityEngine;

namespace Swoonity.Editor
{
[CustomPropertyDrawer(typeof(MandatoryAttribute))]
public class MandatoryDrawer : PropertyDrawer
{
	public static Color Color = new(1f, 0.0f, 0.0f, 1f);

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var previousColor = GUI.color;

		if (IsMissing(property)) GUI.color = Color;

		EditorGUI.PropertyField(position, property, label);
		GUI.color = previousColor;
	}

	static bool IsMissing(SerializedProperty prop)
		=> prop.propertyType switch {
			SerializedPropertyType.ObjectReference => prop.objectReferenceValue == null,
			SerializedPropertyType.String => prop.stringValue.Nil(),
			_ => true,
		};
}
}