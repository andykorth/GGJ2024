using System.Linq;
using Bobsled.Defs;
using UnityEditor;
using static UnityEngine.Debug;

namespace Bobsled.Editors
{
public static class BobsledUtils
{
	public static T[] FindAssets<T>(string folder) where T : class
		=> AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { folder })
		   .Select(LoadFromGuid<T>)
		   .ToArray();

	public static T[] FindAssets<T>() where T : class
		=> AssetDatabase.FindAssets($"t:{typeof(T).Name}")
		   .Select(LoadFromGuid<T>)
		   .ToArray();

	public static string[] FindAssetPaths<T>()
		=> AssetDatabase.FindAssets($"t:{typeof(T).Name}")
		   .Select(AssetDatabase.GUIDToAssetPath)
		   .ToArray();


	public static string[] FindAssetPaths<T>(string folder)
		=> AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { folder })
		   .Select(AssetDatabase.GUIDToAssetPath)
		   .ToArray();

	public static T LoadFromGuid<T>(string guid) where T : class
		=> LoadFromPath<T>(AssetDatabase.GUIDToAssetPath(guid));

	public static T LoadFromPath<T>(string path) where T : class
		=> AssetDatabase.LoadAssetAtPath(path, typeof(T)) as T;


	/// find exactly 1 def of type (or return this def if def isn't null)
	public static T FindSingle<T>(this T def) where T : Definition
	{
		if (def) return def;

		var nameOfType = typeof(T).Name;
		var guids = AssetDatabase.FindAssets($"t:{nameOfType}");

		if (guids.Length == 0) {
			LogError($"Missing {nameOfType} asset");
			return default;
		}

		if (guids.Length > 1) {
			LogError($"Found more than one {nameOfType} asset");
		}

		var guid = guids[0];
		return LoadFromGuid<T>(guid);
	}
}

/// find exactly 1 def of type (Editor only)
public static class SingletonEditorConfig<T> where T : Definition
{
	static T _instance;

	public static T Get(bool logErrors = true)
	{
		if (_instance) return _instance;

		var nameOfType = typeof(T).Name;
		var guids = AssetDatabase.FindAssets($"t:{nameOfType}");

		if (guids.Length == 0) {
			if (logErrors) LogError($"Missing {nameOfType} asset");
			return default;
		}

		if (guids.Length > 1) {
			if (logErrors) LogError($"Found more than one {nameOfType} asset");
		}

		var guid = guids[0];
		var assetPath = AssetDatabase.GUIDToAssetPath(guid);
		_instance = AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)) as T;
		return _instance;
	}
}
}