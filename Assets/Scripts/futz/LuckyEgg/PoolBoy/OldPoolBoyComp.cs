// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using Object = UnityEngine.Object;
//
// namespace PoolBoyPooling
// {
// [System.Serializable]
// public class OldPoolBoyComp<T> where T : Component, IOldPoolable
// {
// 	public List<T> Available;
// 	public Dictionary<int, T> CurrentElements;
//
// 	PoolConfig _config;
//
//
// 	public static OldPoolBoyComp<T> Create(PoolConfig config)
// 		=> new OldPoolBoyComp<T>().Initialize(config);
//
//
// 	public OldPoolBoyComp<T> Initialize(PoolConfig config)
// 	{
// 		if (!config.Prefab) throw new Exception($"{config.Name} missing Prefab");
//
// 		_config = config;
//
// 		Available = new List<T>(_config.Prespawn);
// 		CurrentElements = new Dictionary<int, T>(_config.InitialCapacity);
//
// 		_config.Prefab.SetActive(false);
//
// 		for (var dex = 0; dex < _config.Prespawn; dex++) {
// 			Available.Add(Instantiate());
// 		}
//
// 		return this;
// 	}
//
// 	public T Spawn(int id, Vector3 position = default, Quaternion rotation = default)
// 	{
// 		var element = Available.Count == 0
// 			? Instantiate(position, rotation)
// 			: Available.GrabLast();
//
// 		CurrentElements[id] = element;
// 		element.gameObject.SetActive(true);
// 		element.TriggerSpawn();
//
// 		return element;
// 	}
//
// 	/// Prefab is turned off, so new object will be too
// 	T Instantiate(Vector3 position = default, Quaternion rotation = default)
// 	{
// 		var gobj = Object.Instantiate(_config.Prefab, position, rotation);
//
// 		if (_config.Root) {
// 			var tform = gobj.transform;
// 			tform.SetParent(_config.Root);
// 			tform.localPosition = position;
// 			tform.localRotation = rotation;
// 			tform.localScale = Vector3.one;
// 		}
//
// 		return gobj.GetComponent<T>();
// 	}
//
// 	public void Despawn(int id)
// 	{
// 		if (CurrentElements.TryGetValue(id, out var element)) {
// 			element.gameObject.SetActive(false);
// 			element.TriggerDespawn();
// 			Available.Add(element);
// 		}
// 	}
//
// 	public T Get(int id)
// 	{
// 		return CurrentElements.TryGetValue(id, out var result)
// 			? result
// 			: null;
// 	}
// }
//
// [System.Serializable]
// public class PoolConfig
// {
// 	public string Name = "Unnamed";
// 	public int Prespawn = 8;
// 	public int InitialCapacity = 8;
// 	public GameObject Prefab;
// 	public Transform Root;
// }
//
// public interface IOldPoolable
// {
// 	void TriggerSpawn();
// 	void TriggerDespawn();
// }
// }
//
//
// /*
//
// 	if we add pooling back to Baron entities:
//
//
// 		[HideInInspector] public bool IsSpawned;
//
// 		/// Called from a pool manager
// 		public void TriggerSpawn() {
// 			IsSpawned = true;
// 			WhenSpawned();
// 		}
//
// 		/// Called from a pool manager
// 		public void TriggerDespawn() {
// 			IsSpawned = false;
// 			WhenDespawned();
// 		}
//
// 		/// When I'm spawned by a pool manager
// 		protected virtual void WhenSpawned() { }
//
// 		/// When I'm despawned by a pool manager
// 		protected virtual void WhenDespawned() { }
//
// */