using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SisterPersistence
{
/**
	 
	to skip a field, use [NonSerialized]
	  
	*/
[Serializable]
public class Sister
{
	[Header("Config")]
	public string RootPath = "default";
	public string Folder = "";
	public string Extension = ".le.json";
	public bool PrettyJson = false;

	[Header("State")]
	public List<string> FileNames = new();
	public bool IsBusy;
	public Result LastResult;

	public string GetRootPath()
	{
		if (RootPath == "") return Application.persistentDataPath;
		if (RootPath == "default") return Application.persistentDataPath;
		return RootPath;
	}

	public string MakePath(string name = "")
		=> Path.Combine(
			GetRootPath(),
			Folder,
			name == ""
				? ""
				: $"{name}{Extension}"
		);

	public List<string> DetectFileNames()
	{
		FileNames.Clear();

		var filePaths = Directory.GetFiles(
			MakePath(),
			$"*{Extension}"
		);

		foreach (var path in filePaths) {
			// FileNames.Add(Path.GetFileNameWithoutExtension(path));
			var name = Path.GetFileName(path).Replace(Extension, "");
			FileNames.Add(name);
		}

		return FileNames;
	}

	public UniTask<Result> Save(string name, SisterRecord record)
	{
		if (IsBusy) throw new Exception($"TODO: Sister already busy");

		record.Reset();
		record.Pack();
		return SaveObj(name, record);
	}

	/**
		TODO docs
		must await, do not edit obj
		maybe copy to immutable data obj
		*/
	public async UniTask<Result> SaveObj(string name, object obj)
	{
		if (IsBusy) throw new Exception($"TODO: Sister already busy");

		Result result;
		IsBusy = true;
		var path = MakePath(name);

		try {
			{ /* THREAD START */
				await UniTask.SwitchToThreadPool();

				var jsonString = JsonUtility.ToJson(obj, PrettyJson);
				File.WriteAllText(path, jsonString);

				await UniTask.SwitchToMainThread();
			} /* THREAD END */

			result = new Result {
				IsComplete = true,
				WasSuccessful = true,
				DebugString = $"Saved to {path}",
			};
		}
		catch (Exception err) {
			Debug.LogError(err);
			result = new Result {
				IsComplete = true,
				WasSuccessful = false,
				DebugString = err.Message,
			};
		}

		IsBusy = false;
		LastResult = result;
		return result;
	}

	public async UniTask<Result> Load(string name, SisterRecord record)
	{
		if (IsBusy) throw new Exception($"TODO: Sister already busy");

		record.Reset();

		var result = await LoadObj(name, record);
		if (result.WasSuccessful) {
			record.Unpack();
		}

		return result;
	}

	public async UniTask<Result> LoadObj(string name, object obj)
	{
		if (IsBusy) throw new Exception($"TODO: Sister already busy");

		Result result;
		IsBusy = true;
		var path = MakePath(name);

		try {
			{ /* THREAD START */
				await UniTask.SwitchToThreadPool();

				var jsonString = File.ReadAllText(path);
				JsonUtility.FromJsonOverwrite(jsonString, obj);

				await UniTask.SwitchToMainThread();
			} /* THREAD END */

			result = new Result {
				IsComplete = true,
				WasSuccessful = true,
				DebugString = $"Loaded from {path}",
			};
		}
		catch (Exception err) {
			// Debug.LogError(err);
			result = new Result {
				IsComplete = true,
				WasSuccessful = false,
				DebugString = err.Message,
			};
		}

		IsBusy = false;
		LastResult = result;
		return result;
	}

	[Serializable]
	public struct Result
	{
		public bool IsComplete;
		public bool WasSuccessful;
		public string DebugString;
	}

	public enum Operation
	{
		UNDEFINED,
		PACK,
		UNPACK,
	}
}
}


// public static T LoadAs<T>() {
// 	// JsonUtility.FromJson<T>(string)
// }

// public static uObject LoadObj() {
// 	// JsonUtility.FromJson(string, Type)
// }


// public async UniTask<Result> LoadInto<T>(T target) {
// JsonUtility.FromJsonOverwrite(string, uObject)
// await Threaded_FromJsonOverwrite(json, target);
// }


// static async UniTask<string> Threaded_ToJson(object obj, bool prettyPrint = false) {
// 	string jsonString;
//
// 	{ /* THREAD START */
// 		await UniTask.SwitchToThreadPool();
//
// 		jsonString = JsonUtility.ToJson(obj, prettyPrint);
//
// 		await UniTask.SwitchToMainThread();
// 	} /* THREAD END */
//
// 	return jsonString;
// }

// static async UniTask<object> Threaded_FromJsonOverwrite(string json, object obj) {
// 	{ /* THREAD START */
// 		await UniTask.SwitchToThreadPool();
//
// 		JsonUtility.FromJsonOverwrite(json, obj);
//
// 		await UniTask.SwitchToMainThread();
// 	} /* THREAD END */
//
// 	return obj;
// }