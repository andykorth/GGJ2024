using UnityEngine;

namespace Swoonity.Unity
{
public static class DirtyUtils
{
	public static void SetDirtyIfEditor(this GameObject gobj)
	{
#if UNITY_EDITOR
		UnityEditor.EditorUtility.SetDirty(gobj);
#endif
	}

	public static void SetDirtyIfEditor(this MonoBehaviour mb)
	{
#if UNITY_EDITOR
		UnityEditor.EditorUtility.SetDirty(mb);
#endif
	}


	public static void SetDirtyIfEditor(this ScriptableObject sobj)
	{
#if UNITY_EDITOR
		UnityEditor.EditorUtility.SetDirty(sobj);
#endif
	}

	public static void SaveAssetsIfEditor(this ScriptableObject sobj)
	{
#if UNITY_EDITOR
		UnityEditor.AssetDatabase.SaveAssets();
#endif
	}

	public static void RenameIfEditor(this ScriptableObject sobj, string name)
	{
#if UNITY_EDITOR
		var path = UnityEditor.AssetDatabase.GetAssetPath(sobj);
		UnityEditor.AssetDatabase.RenameAsset(path, name);
		UnityEditor.EditorUtility.SetDirty(sobj);
		// UnityEditor.AssetDatabase.Refresh();
		Debug.Log($"Renaming: {path}  -->  {name}");
#endif
	}
}
}