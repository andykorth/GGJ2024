using Swoonity.Unity;
using UnityEditor;
using UnityEngine;

namespace Swoonity.Editor
{
[CustomPropertyDrawer(typeof(Material))]
public class MaterialDrawer : PropertyDrawer
{
	const int ROW_HEIGHT = 30;
	const int ICON_WIDTH = 30;
	const int ICON_MARGIN_LEFT = 4;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var current = property.objectReferenceValue;

		var fieldRect = position
		   .AddWidth(-ICON_WIDTH - ICON_MARGIN_LEFT);

		var iconRect = position
		   .WithX(fieldRect.xMax + ICON_MARGIN_LEFT)
		   .WithWidth(ICON_WIDTH);

		var next = EditorGUI.ObjectField(
			fieldRect,
			label,
			current,
			typeof(Material),
			false
		);

		if (next != property.objectReferenceValue) {
			property.objectReferenceValue = next;
		}

		DrawIcon(iconRect, current as Material, Color.grey);
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		=> ROW_HEIGHT;

	static void DrawIcon(Rect rect, Material material, Color orColor)
	{
		if (!material) {
			EditorGUI.DrawRect(rect, orColor);
			return;
		}

		var preview = AssetPreview.GetAssetPreview(material);
		if (!preview) {
			EditorGUI.DrawRect(rect, orColor);
			return;
		}

		GUI.DrawTexture(rect, preview);
	}
}
}