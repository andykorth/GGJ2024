using System;
using System.Collections.Generic;
using UnityEngine;
using uObject = UnityEngine.Object;
using static UnityEngine.Debug;

namespace PoolBoyPooling
{
/// pooled prefabs (GameObject & Component) of varying types
/// TODO: SUPER_MAX should be per def type
/// TODO: maybe rename to Spawn / Despawn
public class SuperPool : MonoBehaviour
{
	#region Unity serializable

	public Transform Tf;
	public int TotalAvailable;
	public bool UseSuperMax;
	public int SuperMax;
	public float SuperMaxTrimPercent;
	public List<Pool> AllPools = new();
	public Dictionary<uint, Pool> PoolDictionary = new();

	#endregion

	#region Managing singleton & pools

	const string SCENE_SUPERPOOL_NAME = "==SUPER POOL==";
	const string SCENE_POOL_PREFIX = "POOL| ";
	const bool USE_SUPER_MAX_DEFAULT = true;
	const int SUPER_MAX_DEFAULT = 500;
	const float SUPER_MAX_TRIM_PERCENT = .5f;

	static SuperPool _instance;

	/// optional max (only used if first initializing)
	static Pool GetPool(IPoolDef poolDef)
	{
		if (!_instance) {
			_instance = new GameObject(SCENE_SUPERPOOL_NAME).AddComponent<SuperPool>();
			_instance.Tf = _instance.transform;
			_instance.UseSuperMax = USE_SUPER_MAX_DEFAULT;
			_instance.SuperMax = SUPER_MAX_DEFAULT;
			_instance.SuperMaxTrimPercent = SUPER_MAX_TRIM_PERCENT;
		}

		var assetHash = poolDef.GetAssetHash();

		var has = _instance.PoolDictionary.TryGetValue(assetHash, out var pool);
		if (has) return pool; //>> pool exists

		var poolName = poolDef.ToString();
		var max = poolDef.GetPoolMax();
		var storageTf = new GameObject($"{SCENE_POOL_PREFIX}{poolName}").transform;

		pool = new Pool {
			PoolName = poolName,
			Max = max,
			StorageTf = storageTf,
			Stack = new Stack<GameObject>(max),
		};

		_instance.PoolDictionary.Add(assetHash, pool);
		_instance.AllPools.Add(pool);
		storageTf.SetParent(_instance.Tf);

		return pool; //>> pool created
	}

	/// returns stack.Count
	public static int TryTrimPool(Pool pool, int numToTrim)
	{
		var stack = pool.Stack;
		var poolMax = pool.Max;

		for (var i = 0; i < numToTrim; i++) {
			if (stack.Count <= poolMax) return stack.Count; //>> cannot trim any more
			var poolable = stack.Pop();
			Log($"SuperPool: destroy gameObject {poolable.gameObject} (trim)");
			Destroy(poolable);
		}

		return stack.Count; //>> trimmed
	}

	public static void CheckSuperMax()
	{
		if (_instance.TotalAvailable <= _instance.SuperMax) return; //>> under super max

		var totalAvailable = 0;

		foreach (var pool in _instance.AllPools) {
			var numToTrim = Mathf.RoundToInt(pool.Available * _instance.SuperMaxTrimPercent);
			totalAvailable += TryTrimPool(pool, numToTrim);
		}

		Log($"SuperPool applied SuperMax trim: {_instance.TotalAvailable} --> {totalAvailable}");
		_instance.TotalAvailable = totalAvailable;
	}

	public static bool CheckPoolCanSleep(Pool pool)
	{
		if (_instance.UseSuperMax) {
			CheckSuperMax();
			return true;
		}

		return pool.Available < pool.Max;
	}

	static TComp InstantiateDef<TComp>(IPoolDef poolDef) where TComp : MonoBehaviour, IPoolable
	{
		var prefab = poolDef.GetPrefab();
		if (!prefab)
			throw new Exception($"SuperPool: poolDef missing prefab {poolDef} {typeof(TComp)}");

		var newPoolable = Instantiate(prefab).GetComponent<TComp>();
		newPoolable.SetPoolDef(poolDef);
		return newPoolable;
	}

	#endregion

	#region API

	/// if obj comes from pool, gameObject.SetActive(true)
	public static TComp Take<TComp>(IPoolDef poolDef, bool unparent = true)
		where TComp : MonoBehaviour, IPoolable
	{
		if (poolDef == null) throw new Exception($"SuperPool: null poolDef {typeof(TComp)}");

		var pool = GetPool(poolDef);
		var stack = pool.Stack;

		var stackCount = stack.Count;
		if (stackCount <= 0) {
			pool.Created++;
			return InstantiateDef<TComp>(poolDef);
		}

		var gobj = stack.Pop();
		var poolable = gobj.GetComponent<TComp>(); // OPTIMIZE: GetComponent

		if (unparent) gobj.transform.parent = null;
		gobj.SetActive(true);

		pool.Available = stackCount - 1;
		return poolable;
	}


	/// if obj comes from pool, uses fnRevive on it
	public static TComp Take<TComp>(IPoolDef poolDef, Action<TComp> fnRevive, bool unparent = true)
		where TComp : MonoBehaviour, IPoolable
	{
		if (poolDef == null) throw new Exception($"SuperPool: null poolDef {typeof(TComp)}");

		var pool = GetPool(poolDef);
		var stack = pool.Stack;

		var stackCount = stack.Count;
		if (stackCount <= 0) {
			pool.Created++;
			return InstantiateDef<TComp>(poolDef);
		}

		var gobj = stack.Pop();
		var poolable = gobj.GetComponent<TComp>(); // OPTIMIZE: GetComponent

		if (unparent) gobj.transform.parent = null;
		fnRevive(poolable);

		pool.Available = stackCount - 1;
		return poolable;
	}

	/// if stored, gameObject.SetActive(false)
	public static void Release<TComp>(TComp poolable, bool parentToStorage = true)
		where TComp : MonoBehaviour, IPoolable
	{
		if (!poolable.gameObject.activeSelf) {
			Log($"SuperPool: tried to release inactive gameObject {poolable.gameObject}");
			return;
		}

		var poolDef = poolable.GetPoolDef();

		if (poolDef == null) {
			Log($"SuperPool: destroy gameObject {poolable.gameObject} (poolDef null)");
			Destroy(poolable.gameObject);
			return; //>> poolDef missing (could have been scene obj)
		}

		var pool = GetPool(poolDef);
		var stack = pool.Stack;

		if (!CheckPoolCanSleep(pool)) {
			pool.Trashed++;
			Log($"SuperPool: destroy gameObject {poolable.gameObject} (pool at max)");
			Destroy(poolable.gameObject);
			return; //>> destroy (pool at max)
		}

		var gobj = poolable.gameObject;
		gobj.SetActive(false);
		if (parentToStorage) gobj.transform.SetParent(pool.StorageTf);

		stack.Push(gobj);
		pool.Available = stack.Count;
		++_instance.TotalAvailable;
		//>> stored
	}

	/// if stored, uses fnSleep on it
	public static void Release<TComp>(
		TComp poolable,
		Action<TComp> fnSleep,
		bool parentToStorage = true
	)
		where TComp : MonoBehaviour, IPoolable
	{
		if (!poolable.gameObject.activeSelf) {
			Log($"SuperPool: tried to release inactive gameObject {poolable.gameObject}");
			return;
		}

		var poolDef = poolable.GetPoolDef();

		if (poolDef == null) {
			Log($"SuperPool: destroy gameObject {poolable.gameObject} (poolDef null)2");
			Destroy(poolable.gameObject);
			return; //>> poolDef missing (could have been scene obj)
		}

		var pool = GetPool(poolDef);
		var stack = pool.Stack;

		if (!CheckPoolCanSleep(pool)) {
			pool.Trashed++;
			Log($"SuperPool: destroy gameObject {poolable.gameObject} (pool at max)2");
			Destroy(poolable.gameObject);
			return; //>> destroy (pool at max)
		}

		var gobj = poolable.gameObject;
		fnSleep(poolable);
		if (parentToStorage) gobj.transform.SetParent(pool.StorageTf);

		stack.Push(gobj);
		pool.Available = stack.Count;
		++_instance.TotalAvailable;
		//>> stored
	}

	// public static void ReleaseAll<TComp>(List<TComp> list) where TComp : MonoBehaviour, IPoolable {
	// 	var (stack, stats) = GetPool<TComp>();
	//
	// 	var max = stats.Max;
	// 	var stackCount = stack.Count;
	//
	// 	foreach (var obj in list) {
	// 		if (stackCount < max) {
	// 			stack.Push(obj);
	// 			stackCount++;
	// 		}
	// 		else {
	// 			stats.Trashed++;
	// 		}
	// 	}
	//
	// 	stats.Available = stackCount;
	//
	// 	list.Clear();
	// }

	#endregion


	[Serializable]
	public class Pool
	{
		public string PoolName;
		public int Max;
		public int Available;
		public int Created;
		public int Trashed;
		public Transform StorageTf;
		public Stack<GameObject> Stack;
	}
}

public interface IPoolDef
{
	public uint GetAssetHash();
	public GameObject GetPrefab();
	public int GetPoolMax();
}

public interface IPoolable
{
	public IPoolDef GetPoolDef();
	public void SetPoolDef(IPoolDef poolDef);
}
}