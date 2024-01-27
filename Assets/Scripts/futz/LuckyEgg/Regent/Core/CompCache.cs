using System;
using System.Collections.Generic;
using Swoonity.MHasher;
using Swoonity.Unity;
using UnityEngine;
using static UnityEngine.Debug;

namespace Regent.Core
{
[Serializable]
public class CompCache
{
	GameObject _gobj;
	public List<MHash> ListOfHashes = new(); // serialized at devtime
	public HashSet<MHash> SetOfHashes = new(); // loaded at runtime start, determines "Has" 
	public Dictionary<MHash, Component> Cache = new(); // refs loaded on demand then cached

	public void DevtimeInitialize(GameObject gobj)
	{
		_gobj = gobj;
		ListOfHashes.Clear();

		foreach (var comp in gobj.GetComponents<Component>()) {
			if (comp == null) {
				LogError($"CompCache: invalid component {comp} on {_gobj}", _gobj);
				continue;
			}

			var hash = MHash.Hash(comp.GetType());
			ListOfHashes.Add(hash);
		}
	}

	public void RuntimeInitialize(GameObject gobj)
	{
		_gobj = gobj;
		SetOfHashes.Clear();

		foreach (var hash in ListOfHashes) {
			SetOfHashes.Add(hash);
		}
	}

	public TComp TestAdd<TComp>() where TComp : Component
	{
		var comp = _gobj.AddComponent<TComp>();
		var hash = MHash.Hash(typeof(TComp));
		SetOfHashes.Add(hash);
		Cache.Add(hash, comp);
		return comp;
	}

	public bool Has(MHash hash) => SetOfHashes.Contains(hash);

	public bool Has<T>() => SetOfHashes.Contains(MHash.Get<T>());

	public bool HasAll(HashSet<MHash> hashSet) => hashSet.IsSubsetOf(SetOfHashes);

	public bool HasAll(MHash[] hashes)
	{
		foreach (var hash in hashes) {
			if (!Has(hash)) return false;
		}

		return true;
	}


	public T Load<T>(MHash hash) where T : Component
	{
		// hi👋
		// [Added.xxx] has issues when the object is immediately destroyed, try [Enabled.xxx]
		var comp = _gobj.GetComponent<T>();
		if (!comp) {
			throw new ErrAbsurd($"CompCache can't load component {typeof(T)}", _gobj);
		}

		Cache.Add(hash, comp);
		return comp;
	}

	public T Get<T>() where T : Component
	{
		var hash = MHash.Get<T>();

		// TODO: should this be checked after Cache to determine loading?
		if (!SetOfHashes.Contains(hash)) return null;

		var isLoaded = Cache.TryGetValue(hash, out var comp);
		if (!isLoaded) comp = Load<T>(hash);

		return (T)comp;
	}

	public (bool has, T comp) HasGet<T>() where T : Component
	{
		var hash = MHash.Get<T>();

		if (!SetOfHashes.Contains(hash)) return (false, null);

		var isLoaded = Cache.TryGetValue(hash, out var comp);
		if (!isLoaded) comp = Load<T>(hash);

		return (true, (T)comp);
	}

	public T GetOrThrow<T>() where T : Component
	{
		var hash = MHash.Get<T>();

		if (!SetOfHashes.Contains(hash)) {
			// hi👋👋
			// something went very wrong with copying components (I think)
			// try remaking the GameObjects from scratch
			throw new ErrAbsurd($"CompCache missing hash {typeof(T)} {hash}", _gobj);
		}

		var isLoaded = Cache.TryGetValue(hash, out var comp);
		if (!isLoaded) comp = Load<T>(hash);

		return (T)comp;
	}
}
}