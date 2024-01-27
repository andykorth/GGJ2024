using Swoonity.Collections;
using UnityEditor.SceneManagement;

namespace Swoonity.Editor
{
public static class SceneUtils
{
	public static void Load(string main, params string[] adds)
	{
		EditorSceneManager.OpenScene(main);

		if (adds == null) return;

		foreach (var add in adds) {
			EditorSceneManager.OpenScene(add, OpenSceneMode.Additive);
		}
	}

	/// [mainScene, ...additiveScenes]
	public static void LoadScenes(this string[] scenes)
	{
		if (scenes.Nil()) return;

		EditorSceneManager.OpenScene(scenes[0]);

		for (var dex = 1; dex < scenes.Length; dex++) {
			EditorSceneManager.OpenScene(scenes[dex], OpenSceneMode.Additive);
		}
	}
}
}