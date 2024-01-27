/*     Trenton Greyoak @gmail.com     Lucky Egg 2023     MIT License (this file only)     */
using System;
using UnityEngine;

namespace Swoonity.Unity
{
[CreateAssetMenu(menuName = nameof(Versioner))]
public class Versioner : ScriptableObject
{
	public string VersionName = "Alpha";

	public bool AutoSet = true;

	[Header("Values")]
	public string Year;
	public string Month;
	public string Day;
	public string Hour;
	public string Minute;
	public string Version;
	public string NameAndVersion;


	public void Set(string source)
	{
		var now = DateTime.Now;

		Year = now.Year.ToString("D4");
		Month = now.Month.ToString("D2");
		Day = now.Day.ToString("D2");
		Hour = now.Hour.ToString("D2");
		Minute = now.Minute.ToString("D2");

		Version = $"{Year}.{Month}{Day}.{Hour}{Minute}";
		NameAndVersion = $"{VersionName} {Version}";

		Debug.Log($"<b>Version: <color=#E566D3>{this}</color></b>   <i>({source})</i>", this);
	}

	public override string ToString() => NameAndVersion;


#if UNITY_EDITOR

	[UnityEditor.MenuItem("CONTEXT/Versioner/Set Version")]
	static void SetVersion(UnityEditor.MenuCommand command)
		=> ((Versioner)command.context).Set("set by menu");

	[UnityEditor.Callbacks.DidReloadScripts]
	static void OnReloadScripts() => UpdateVersion();

	[UnityEditor.Callbacks.PostProcessBuild]
	static void OnPostProcessBuild(UnityEditor.BuildTarget _, string __) => UpdateVersion();

	[UnityEditor.Callbacks.PostProcessScene]
	static void OnPostProcessScene() => UpdateVersion();

	public static void UpdateVersion(
		[System.Runtime.CompilerServices.CallerMemberName] string caller = ""
	)
	{
		var assetGuids = UnityEditor.AssetDatabase.FindAssets($"t:{nameof(Versioner)}");

		foreach (var guid in assetGuids) {
			var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
			var asset = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(Versioner))
				as Versioner;

			if (asset == null || !asset.AutoSet) continue;

			asset.Set($"auto set by {caller}");
			UnityEditor.EditorUtility.SetDirty(asset);
		}
	}

#endif
}
}