using System.Collections.Generic;
using UnityEngine;

namespace Swoonity.Unity
{
public static class ScriptableObjectUtils
{
	public static void LoadScriptableObjectRefs<T>(this List<T> into) where T : ScriptableObject
	{
#if UNITY_EDITOR
		into.Clear();

		foreach (var guid in UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T).Name}")) {
			into.Add(
				UnityEditor.AssetDatabase.LoadAssetAtPath<T>(
					UnityEditor.AssetDatabase.GUIDToAssetPath(guid)
				)
			);
		}
#else
		throw new System.Exception($"this only works in the editor");
#endif
	}
}
}