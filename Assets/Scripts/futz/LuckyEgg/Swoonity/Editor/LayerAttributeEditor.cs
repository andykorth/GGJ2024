using Swoonity.Unity;
using UnityEditor;
using UnityEngine;

namespace _Code.Swoonity.Editor
{
[CustomPropertyDrawer(typeof(LayerAttribute))]
class LayerAttributeEditor : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		property.intValue = EditorGUI.LayerField(position, label, property.intValue);
	}
}
}