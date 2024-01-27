using UnityEditor;
using UnityEngine;

namespace _Code.Swoonity.Editor
{
public static class BetterEditorLabel
{
	/// Draws Handles.Label with better style
	public static void Label(Vector3 position, string text, int fontSize = 16, Color? color = null)
	{
		Handles.Label(position, text, GetStyle(fontSize, color));
	}

	public static GUIStyle GetStyle(int fontSize, Color? color)
	{
		return new GUIStyle {
			fontSize = fontSize,
			alignment = TextAnchor.MiddleCenter,
			normal = new GUIStyleState {
				textColor = color ?? Color.white,
			},
			//				fontStyle = FontStyle.Bold,
		};
	}
}
}