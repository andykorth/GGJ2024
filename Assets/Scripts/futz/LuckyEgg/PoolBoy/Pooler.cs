using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static UnityEngine.Debug;

namespace PoolBoyPooling
{
/// pooled C# objects
public class Pooler : MonoBehaviour
{
	#region Unity serializable

	public int Id;
	public List<PoolStats> AllPools = new();

	#endregion

	#region Managing singleton & pools

	const string SCENE_NAME = "=== POOLER: ";
	const int DEFAULT_MAX = 16;

	static int _id;
	static Pooler _instance;

	/// optional max (only used if first initializing)
	static (Stack<TObj> stack, PoolStats stats) GetPool<TObj>(int max = DEFAULT_MAX)
		where TObj : new()
	{
		if (!_instance) {
			if (!Application.isPlaying) throw new Exception($"app not playing");
			_instance = new GameObject($"{SCENE_NAME} {typeof(TObj).Name}").AddComponent<Pooler>();
			_id = _instance.Id = Random.Range(1, int.MaxValue);
			// Log($"created Pooler: {_id} {typeof(TObj).Name}    {Application.isPlaying}".LgYellow());
		}

		var (id, stack, stats) = StorageForPooler<TObj>.Get();

		if (id == _id) return (stack, stats);

		// either uninitialized or used by previous pooler instance

		if (stack == null) { // create new pool
			stack = StorageForPooler<TObj>.Stack = new Stack<TObj>(max);

			stats = StorageForPooler<TObj>.Stats = new PoolStats();
			stats.TypeName = typeof(TObj).Name;
			stats.Max = max;
		}

		StorageForPooler<TObj>.PoolerId = _id;
		_instance.AllPools.Add(stats);

		return (stack, stats);
	}

	#endregion

	#region API

	public static TObj Take<TObj>() where TObj : new()
	{
		var (stack, stats) = GetPool<TObj>();

		var stackCount = stack.Count;
		if (stackCount <= 0) {
			stats.Created++;
			return new TObj();
		}

		stats.Available = stackCount - 1;
		return stack.Pop();
	}

	/// if obj comes from pool, use fnReset on it
	public static TObj Take<TObj>(Action<TObj> fnReset) where TObj : new()
	{
		var (stack, stats) = GetPool<TObj>();

		var stackCount = stack.Count;
		if (stackCount <= 0) {
			stats.Created++;
			return new TObj();
		}

		stats.Available = stackCount - 1;
		var obj = stack.Pop();
		fnReset(obj);
		return obj;
	}

	public static void Release<TObj>(TObj obj) where TObj : new()
	{
		var (stack, stats) = GetPool<TObj>();

		var stackCount = stack.Count;
		if (stackCount < stats.Max) {
			stack.Push(obj);
			stats.Available = stackCount + 1;
		}
		else {
			// at max
			stats.Trashed++;
		}
	}

	public static void ReleaseAll<TObj>(List<TObj> list) where TObj : new()
	{
		var (stack, stats) = GetPool<TObj>();

		var max = stats.Max;
		var stackCount = stack.Count;

		foreach (var obj in list) {
			if (stackCount < max) {
				stack.Push(obj);
				stackCount++;
			}
			else {
				stats.Trashed++;
			}
		}

		stats.Available = stackCount;

		list.Clear();
	}

	public static void SetMax<TObj>(int max) where TObj : new()
	{
		var (stack, stats) = GetPool<TObj>(max);
		stats.Max = max;
	}

	#endregion
}

[Serializable]
public class PoolStats
{
	public string TypeName;
	public int Max;
	public int Available;
	public int Created;
	public int Trashed;
}

public static class StorageForPooler<TObj> where TObj : new()
{
	public static int PoolerId;
	public static Stack<TObj> Stack;
	public static PoolStats Stats;

	public static (int poolerId, Stack<TObj> stack, PoolStats stats) Get()
		=> (PoolerId, Stack, Stats);
}
}