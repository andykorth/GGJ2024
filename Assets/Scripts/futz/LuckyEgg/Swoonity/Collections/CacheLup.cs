using System;
using System.Collections.Generic;

namespace Swoonity.Collections
{
[Serializable]
public class CacheLup<TKey, TVal> : Lup<TKey, TVal>
{
	Func<TKey, (bool, TVal)> _loader;
	public void SetLoader(Func<TKey, (bool, TVal)> loader) => _loader = loader;
	public virtual Func<TKey, (bool, TVal)> GetLoader() => _loader;

	HashSet<TKey> _misses = new();

	public void ResetMisses() => _misses.Clear();

	public (bool, TVal) TryLoad(TKey key)
	{
		var alreadyMissing = _misses.Contains(key);
		if (alreadyMissing) return (false, default);

		var (wasLoaded, loadedVal) = _loader(key);
		if (!wasLoaded) {
			_misses.Add(key);
			return (false, default);
		}

		Set(key, loadedVal);
		return (true, loadedVal);
	}


	public override bool TryGet(TKey key, out TVal val)
	{
		var isLoaded = TryGetValue(key, out val);
		if (isLoaded) return true;

		(isLoaded, val) = TryLoad(key);
		return isLoaded;
	}

	public override bool Has(TKey key)
	{
		var isLoaded = ContainsKey(key);
		if (isLoaded) return true;

		(isLoaded, _) = TryLoad(key);
		return isLoaded;
	}
}
}