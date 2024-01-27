using System.IO;
using UnityEngine;
using uObj = UnityEngine.Object;
using static UnityEngine.Debug;

namespace Swoonity.Unity
{
public static class RunAssetUtils
{
	public static Texture2D GetPreviewIfEditor(uObj obj)
	{
#if UNITY_EDITOR
		UnityEditor.AssetPreview.GetAssetPreview(obj);
#endif
		return null;
	}

	// static Texture2D _tempTexture;

	public static Sprite SaveIconIfEditor(uObj obj, string pathNameRelToAssetsDir)
	{
#if UNITY_EDITOR

		// if (!_tempTexture) {
		// 	_tempTexture = UnityEditor.AssetPreview.GetAssetPreview(obj);
		// 	LogWarning($"START LOAD   - press it again :(");
		// 	return null;
		// }
		//
		// if (UnityEditor.AssetPreview.IsLoadingAssetPreviews()) {
		// 	LogWarning($"LOADING   - press it again :(");
		// 	return null;
		// }
		//
		// var preview = _tempTexture;
		// _tempTexture = null;

		var preview = UnityEditor.AssetPreview.GetAssetPreview(obj);
		// var preview = UnityEditor.AssetPreview.GetMiniThumbnail(obj);
		if (!preview) {
			LogWarning($"GetAssetPreview failed (cuz Unity), try again :(");
			return null;
		}

		var texture = ReadableCopy(preview);

		var pngBytes = texture.EncodeToPNG();

		var fullAssetsFolderPath = Application.dataPath; // C:/ProjectName/Assets

		var fullFilePath = $"{fullAssetsFolderPath}/{pathNameRelToAssetsDir}.png";
		var assetPath = $"Assets/{pathNameRelToAssetsDir}.png";

		Log($"saving to: {fullFilePath}");

		File.WriteAllBytes(fullFilePath, pngBytes);

		UnityEditor.AssetDatabase.Refresh();


		var importer = (UnityEditor.TextureImporter)UnityEditor.AssetImporter.GetAtPath(assetPath);
		// importer.alphaIsTransparency = true;
		importer.textureType = UnityEditor.TextureImporterType.Sprite;
		UnityEditor.EditorUtility.SetDirty(importer);
		importer.SaveAndReimport();

		var icon = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);

		// Log($"w: {icon.texture.width}, h: {icon.texture.height}");
		return icon;
#endif
		return null;
	}

	public static Texture2D ReadableCopy(Texture2D texture)
	{
		var prevRt = RenderTexture.active;

		var rt = new RenderTexture(texture.width, texture.height, 24);
		RenderTexture.active = rt;
		Graphics.Blit(texture, rt);

		var copy = new Texture2D(texture.width, texture.height);
		copy.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
		copy.Apply();

		RenderTexture.active = prevRt;

		return copy;
	}
}
}