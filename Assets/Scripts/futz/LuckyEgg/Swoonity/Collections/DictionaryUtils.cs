using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swoonity.Collections
{
public static class DictionaryUtils
{
	public static List<TV> ValueList<TK, TV>(this Dictionary<TK, TV> dict) => dict.Values.ToList();


	/// takes dictionary, converts each key using func, returns array
	/// GARBAGE: array returned 
	/// GARBAGE: cache the Func or use static method (not instance) to avoid GC
	public static TResult[] MapKeys<TK, TV, TResult>(
		this Dictionary<TK, TV> dict,
		Func<TK, TResult> func
	)
	{
		var array = new TResult[dict.Count];
		var dex = 0;

		foreach (var kvp in dict) {
			array[dex] = func(kvp.Key);
			++dex;
		}

		return array;
	}

	/// takes dictionary, converts each kvp using func, returns array
	/// GARBAGE: array returned 
	/// GARBAGE: cache the Func or use static method (not instance) to avoid GC
	public static TResult[] Map<TK, TV, TResult>(
		this Dictionary<TK, TV> dict,
		Func<(TK, TV), TResult> func
	)
	{
		var array = new TResult[dict.Count];
		var dex = 0;

		foreach (var kvp in dict) {
			array[dex] = func((kvp.Key, kvp.Value));
			++dex;
		}

		return array;
	}

	/// foreach (var (keyName, valueName) in dictionary)
	public static void Deconstruct<TK, TV>(this KeyValuePair<TK, TV> kvp, out TK key, out TV val)
	{
		key = kvp.Key;
		val = kvp.Value;
	}

	/// !dictionary.ContainsKey(key);
	public static bool MissingKey<TK, TV>(this Dictionary<TK, TV> dictionary, TK key)
		=> !dictionary.ContainsKey(key);

	/// alias for ContainsKey
	public static bool Has<TK, TV>(this Dictionary<TK, TV> dictionary, TK key)
		=> dictionary.ContainsKey(key);

	public static (bool has, TV val) HasGet<TK, TV>(this Dictionary<TK, TV> dictionary, TK key)
		=> dictionary.TryGetValue(key, out var val) ? (true, val) : (false, val);

	/// Adds kvp but throws if key already exists
	public static void AddOrThrow<TK, TV>(
		this Dictionary<TK, TV> dict,
		TK key,
		TV val,
		Func<TK, TV, TV, Exception> createException
	)
	{
		if (dict.TryGetValue(key, out var existing))
			throw createException(key, val, existing);

		dict[key] = val;
	}

	public static string Join<TK, TV>(
		this Dictionary<TK, TV> dict,
		string delimiter = ", ",
		string or = ""
	)
	{
		if (dict.Count == 0) return or;

		var builder = new StringBuilder();
		var countLeft = dict.Count;

		foreach (var (key, val) in dict) {
			--countLeft;
			builder.Append("{");
			builder.Append(key.ToString());
			builder.Append(": ");
			builder.Append(val.ToString());
			builder.Append("}");
			if (countLeft > 0) builder.Append(delimiter);
		}

		return builder.ToString();
	}


	/// Get value with key (or null if missing)
	public static T2 Get<T1, T2>(this Dictionary<T1, T2> dict, T1 key) where T2 : class
	{
		return dict.TryGetValue(key, out var result)
			? result
			: null;
	}

	/// Get value with key (or null if missing), then cast
	// public static T3 GetAs<T1, T2, T3>(this Dictionary<T1, T2> dict, T1 key)
	// 	where T2 : class 
	// 	where T3 : class, T2 {
	// 	return dict.TryGetValue(key, out var result)
	// 		? result as T3
	// 		: null;
	// }

	/// Get value with key or given value if missing (or default)
	public static T2 GetOr<T1, T2>(this Dictionary<T1, T2> dict, T1 key, T2 or = default)
		=> dict.TryGetValue(key, out var value)
			? value
			: or;


	// TODO: cleanup


	/// Chainable
	public static T2 AddThen<T1, T2>(this Dictionary<T1, T2> dictionary, T1 key, T2 val)
	{
		dictionary.Add(key, val);
		return val;
	}

	public static void Each<T, U>(
		this Dictionary<T, U>.ValueCollection valueCollection,
		Action<U> callback
	)
	{
		foreach (var val in valueCollection) {
			callback(val);
		}
	}

	/// For 
	public static void EachValue<T, U>(this Dictionary<T, U> dictionary, Action<U> callback)
	{
		foreach (var val in dictionary.Values) {
			callback(val);
		}
	}

	/// 
	public static U FirstOrNull<T, U>(
		this Dictionary<T, U> dictionary,
		Func<U, bool> predicate = null
	)
		where U : class
	{
		if (dictionary != null && dictionary.Count > 0) {
			if (predicate != null) {
				foreach (var val in dictionary.Values) {
					if (predicate(val)) {
						return val;
					}
				}
			}
			else {
				return dictionary.Values.First();
			}
		}

		return null;
	}

	// public static void Each<T, U>(this SyncDictionary<T, U> dictionary, Action<T, U> callback) {
	// 	foreach (KeyValuePair<T, U> item in dictionary) {
	// 		callback(item.Key, item.Value);
	// 	}
	// }

	public static void EachKey<T1, T2>(this Dictionary<T1, T2> dictionary, Action<T1> callback)
	{
		foreach (var key in dictionary.Keys) callback(key);
	}

	public static T2 Grab<T1, T2>(this Dictionary<T1, T2> dic, T1 key) where T2 : class
	{
		if (!dic.TryGetValue(key, out var val)) return null;
		dic.Remove(key);
		return val;
	}

	//public static T FirstKeyOrNull<T, U>(this SyncDictionary<T, U> dictionary, Func<T, U, bool> predicate = null) {
	//    if (dictionary != null && dictionary.Count > 0) {
	//        if (predicate != null) {
	//            foreach (KeyValuePair<T,U> item in dictionary) {
	//                if (predicate(item.Key, item.Value)) {
	//                    return item.Key;
	//                }
	//            }
	//        }
	//        else {
	//            return dictionary.Keys.First();
	//        }
	//    }
	//    return null;
	//}
}
}