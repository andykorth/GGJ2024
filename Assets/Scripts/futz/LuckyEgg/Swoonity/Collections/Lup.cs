using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Debug;

// ReSharper disable InconsistentNaming

namespace Swoonity.Collections
{
/// Lup/Lookup (Dictionary wrapper)
/// TKey --> TVal
/// Set, Cut, Has, All, Get
[Serializable]
public class Lup<TKey, TVal> : Dictionary<TKey, TVal>, ISerializationCallbackReceiver
{
	public List<LupKvp<TKey, TVal>> KvpList;

	public Lup(int initialSize = 0)
	{
		KvpList = new List<LupKvp<TKey, TVal>>(initialSize);
	}

	public virtual void Set(TKey key, TVal val) => this[key] = val;
	public virtual bool Cut(TKey key) => Remove(key);
	public virtual bool Has(TKey key) => ContainsKey(key);
	public virtual bool Missing(TKey key) => !Has(key);
	public virtual bool TryGet(TKey key, out TVal val) => TryGetValue(key, out val);
	public virtual TVal Get(TKey key) => TryGet(key, out var val) ? val : default;

	public virtual (bool has, TVal val) HasGet(TKey key)
		=> TryGet(key, out var val) ? (true, val) : (false, default);

	/// Gets val and Cuts it
	public virtual TVal Nab(TKey key)
	{
		var (has, val) = HasGet(key);
		if (has) Cut(key);
		return val;
	}

	/// Gets val and Cuts it
	public virtual (bool has, TVal val) HasNab(TKey key)
	{
		var (has, val) = HasGet(key);
		if (has) Cut(key);
		return (has, val);
	}

	/// set val and return it
	public virtual TVal SetGet(TKey key, TVal val) => this[key] = val;

	/// if missing, use fnMake to set
	public virtual TVal GetOrSet(TKey key, Func<TVal> fnMake)
	{
		var has = TryGetValue(key, out var val);
		if (has) return val;
		return this[key] = fnMake();
	}

	public virtual Lup<TKey, TVal> ClearThen()
	{
		Clear();
		return this;
	}

	/// chainable Clear()
	public virtual Lup<TKey, TVal> _Clear()
	{
		Clear();
		return this;
	}

	public bool Any() => Count > 0;
	public bool Nil() => Count == 0;
	public bool IsEmpty => Count == 0;


	/// throws if already has key
	public virtual void AddOrThrow(TKey key, TVal val)
	{
		if (Has(key))
			throw new Exception($"{this} already contains key {key} for {val}");
		Set(key, val);
	}

	/// throws if missing key
	public virtual TVal GetOrThrow(TKey key, Func<TKey, Exception> getError = null)
	{
		if (TryGetValue(key, out var val))
			return val;

		if (getError != null) throw getError(key);
		throw new Exception($"{this} missing key: {key}");
	}

	public virtual bool HasAll(TKey[] keys)
	{
		foreach (var key in keys) {
			if (Missing(key)) return false;
		}

		return true;
	}


	#region ISerializationCallbackReceiver

	[HideInInspector] public bool HasDupe;
	[HideInInspector] public LupKvp<TKey, TVal> Dupe;

	public void OnBeforeSerialize()
	{
		// Log($"Lup<{typeof(TKey)},{typeof(TVal)}> OnBeforeSerialize, {Dict.Count} count, {GetType()}");

		KvpList.Clear();

		foreach (var (key, val) in this) {
			KvpList.Add(new LupKvp<TKey, TVal>(key, val));
		}

		if (HasDupe) {
			KvpList.Add(Dupe);
		}
	}

	public void OnAfterDeserialize()
	{
		HasDupe = false;
		Clear();

		foreach (var (key, val) in KvpList) {
			if (key == null) {
				// already has key
				HasDupe = true;
				Dupe = new LupKvp<TKey, TVal>(default, val ?? default, true);
				continue;
			}

			if (Has(key)) {
				// already has key
				HasDupe = true;
				Dupe = new LupKvp<TKey, TVal>(key, val, true);
				continue;
			}

			this[key] = val;
		}
	}

	#endregion

	public override string ToString() => this.Join();
}

[Serializable]
public struct LupKvp<TKey, TVal>
{
	public TKey Key;
	public TVal Value;
	public bool IsDupe;

	public LupKvp(TKey key, TVal val, bool isDupe = false)
	{
		Key = key;
		Value = val;
		IsDupe = isDupe;
	}

	public LupKvp((TKey, TVal) kvp)
		: this(kvp.Item1, kvp.Item2) { }

	public void Deconstruct(out TKey key, out TVal value)
	{
		key = Key;
		value = Value;
	}

	public override string ToString() => $"{Key}: {Value}";
}
}


// TODO: update these to be similar to Idealist
//
// /// GARBAGE: cache the Action to avoid GC
// public virtual void Each(Action<TKey, TVal> act) {
// 	foreach (var kvp in this) {
// 		act(kvp.Key, kvp.Value);
// 	}
// }
//
// /// GARBAGE: cache the Action to avoid GC
// public virtual void Each(Action<Lup<TKey, TVal>, TKey, TVal> act) {
// 	foreach (var kvp in this) {
// 		act(this, kvp.Key, kvp.Value);
// 	}
// }
//
// /// GARBAGE: cache the Action to avoid GC
// public virtual void EachKey(Action<TKey> act) {
// 	foreach (var kvp in this) {
// 		act(kvp.Key);
// 	}
// }
//
// /// GARBAGE: cache the Action to avoid GC
// public virtual void EachVal(Action<TVal> act) {
// 	foreach (var kvp in this) {
// 		act(kvp.Value);
// 	}
// }
//
// /// GARBAGE: cache the Func to avoid GC
// public virtual bool MakeIfMissing(TKey key, Func<TVal> maker) {
// 	if (Has(key)) return false;
// 	Set(key, maker());
// 	return true;
// }
//
//
// /// GARBAGE: cache the Func to avoid GC
// public virtual TVal GetOrMake(TKey key, Func<TVal> maker) {
// 	var (has, val) = HasGet(key);
// 	if (has) return val;
//
// 	val = maker();
// 	Set(key, val);
// 	return val;
// }