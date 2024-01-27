using System;
using System.Collections.Generic;
using System.Linq;
using Swoonity.CSharp;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Debug;

namespace Swoonity.Editor
{
/// WARNING: don't use FindAssets on domain reloads (it gets slower the bigger the project gets)
public static class AssetUtils
{
	public static bool CheckAssetPathExists(this string fullAssetPath)
	{
		var guid = AssetDatabase.AssetPathToGUID(
			fullAssetPath,
			AssetPathToGUIDOptions.OnlyExistingAssets
		);
		return !string.IsNullOrEmpty(guid);
	}

	public static List<T> ListAssets<T>(this Type assetType) where T : class
		=> AssetDatabase.FindAssets($"t:{assetType.Name}")
		   .Select(LoadFromGuid<T>)
		   .ToList();

	public static List<T> ListAssets<T>() where T : class
		=> AssetDatabase.FindAssets($"t:{typeof(T).Name}")
		   .Select(LoadFromGuid<T>)
		   .ToList();

	public static T[] FindAssets<T>(this Type assetType) where T : class
		=> AssetDatabase.FindAssets($"t:{assetType.Name}")
		   .Select(LoadFromGuid<T>)
		   .ToArray();

	public static T[] FindAssets<T>() where T : class
		=> AssetDatabase.FindAssets($"t:{typeof(T).Name}")
		   .Select(LoadFromGuid<T>)
		   .ToArray();

	public static T[] FindAssets<T>(string folder) where T : class
		=> AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { folder })
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


	public static T FindOrMakeAssetSingle<T>(string pathNameOfNew = "") where T : ScriptableObject
	{
		var nameOfType = typeof(T).Name;
		var guids = AssetDatabase.FindAssets($"t:{nameOfType}");

		if (guids.Length > 0) {
			var guid = guids[0];
			if (guids.Length > 1) {
				LogError($"Found more than one {nameOfType} asset");
			}

			return LoadFromGuid<T>(guid);
		}

		var asset = ScriptableObject.CreateInstance<T>();

		AssetDatabase.CreateAsset(
			asset,
			pathNameOfNew.Nil()
				? $"Assets/{nameOfType}.asset"
				: pathNameOfNew
		);
		AssetDatabase.SaveAssets();
		return asset;
	}


	public static T FindSingle<T>() where T : class
	{
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

	public static T FindAssetSingle<T>(this EditorWindow _) where T : class => FindSingle<T>();


	public static void SelectAsset(this ScriptableObject so) => Selection.activeObject = so;
}
}