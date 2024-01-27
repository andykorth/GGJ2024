using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bobsled.Editors
{
public class BobsledEditor : EditorWindow
{
	[MenuItem("Lucky Egg/Bobsled")]
	public static void ShowWindow()
	{
		var window = GetWindow<BobsledEditor>();

		window.titleContent = new GUIContent("Bobsled");

		window.minSize = new Vector2(250, 100);
	}

	void OnEnable()
	{
		Bobsled.DrawEditor(rootVisualElement);
	}
}
}