// using Swoonity.Unity;
// using UnityEditor;
// using UnityEngine;
//
// namespace Swoonity.Editor {
//
// [CustomPropertyDrawer(typeof(Sprite))]
// public class SpriteDrawer : PropertyDrawer {
// 	const int ROW_HEIGHT = 30;
// 	const int ICON_WIDTH = 30;
// 	const int ICON_MARGIN_LEFT = 4;
// 	const int FIELD_HEIGHT = 16;
// 	const int FIELD_MARGIN_TOP = 7;
//
// 	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
// 		var current = property.objectReferenceValue;
//
// 		var fieldRect = new Rect(
// 			position.x,
// 			position.y + FIELD_MARGIN_TOP,
// 			position.width - ICON_WIDTH - ICON_MARGIN_LEFT,
// 			FIELD_HEIGHT
// 		);
// 		
// 		var iconRect = position
// 		   .WithX(fieldRect.xMax + ICON_MARGIN_LEFT)
// 		   .WithWidth(ICON_WIDTH);
//
// 		var next = EditorGUI.ObjectField(
// 			fieldRect,
// 			label,
// 			current,
// 			typeof(Sprite),
// 			false
// 		);
//
// 		if (next != property.objectReferenceValue) {
// 			property.objectReferenceValue = next;
// 		}
//
// 		DrawIcon(iconRect, current as Sprite, Color.grey);
// 	}
//
// 	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
// 		=> ROW_HEIGHT;
//
// 	static void DrawIcon(Rect rect, Sprite sprite, Color orColor) {
// 		if (!sprite) {
// 			EditorGUI.DrawRect(rect, orColor);
// 			return;
// 		}
//
// 		GUI.DrawTexture(rect, sprite.texture);
// 	}
//
// }
//
// }
