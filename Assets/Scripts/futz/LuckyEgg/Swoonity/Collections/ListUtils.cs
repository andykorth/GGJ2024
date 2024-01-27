using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swoonity.Collections
{
public static class ListUtils
{
	public static void Add<T>(this List<T> list, (T, T) tuple)
	{
		list.Add(tuple.Item1);
		list.Add(tuple.Item2);
	}

	public static void Add<T>(this List<T> list, (T, T, T) tuple)
	{
		list.Add(tuple.Item1);
		list.Add(tuple.Item2);
		list.Add(tuple.Item3);
	}

	public static void Add<T>(this List<T> list, (T, T, T, T) tuple)
	{
		list.Add(tuple.Item1);
		list.Add(tuple.Item2);
		list.Add(tuple.Item3);
		list.Add(tuple.Item4);
	}


	/// returns element if index is >= 0 < count, else default
	public static T GetIfValidIndex<T>(this List<T> list, int dex)
	{
		if (dex < 0 || dex >= list.Count) return default;
		return list[dex];
	}


	/// 
	/// GARBAGE: cache the Func or use static method (not instance) to avoid GC
	public static (T, string) GetOrMsg<T>(
		this List<T> list,
		Func<T, bool> predicate,
		string msg
	)
	{
		foreach (var elem in list) {
			if (predicate(elem)) return (elem, "");
		}

		return (default, msg);
	}

	/// 
	public static (T, string) GetOrMsg<T>(this List<T> list, int dex, string msg)
	{
		if (dex < 0 || dex >= list.Count) return (default, msg);
		return (list[dex], "");
	}


	/// list forEach that returns itself
	/// GARBAGE: cache the Action or use static method (not instance) to avoid GC
	public static List<T> Each_DEPRECATED<T>(this List<T> list, Action<T> action)
	{
		foreach (var elem in list) action(elem);
		return list;
	}

	/// list forEach (with index) that returns itself
	/// GARBAGE: cache the Action or use static method (not instance) to avoid GC
	// public static List<T> Each<T>(this List<T> list, Action<T, int> action) {
	// 	for (var dex = 0; dex < list.Count; dex++) {
	// 		action(list[dex], dex);
	// 	}
	//
	// 	return list;
	// }

	/// list forEach that returns itself + data object
	/// GARBAGE: cache the Action or use static method (not instance) to avoid GC
	// public static List<TElem> Each<TElem, TData>(this List<TElem> list, TData data, Action<TElem,TData> action) {
	// 	foreach (var elem in list) action(elem, data);
	// 	return list;
	// }

	// /// takes list, converts each element using func, returns array
	// /// GARBAGE: array returned 
	// /// GARBAGE: cache the Func or use static method (not instance) to avoid GC
	// public static TR[] MapToArray<T1, TR>(this List<T1> list, Func<T1, TR> func) {
	// 	var array = new TR[list.Count];
	// 	for (var dex = 0; dex < list.Count; dex++) {
	// 		array[dex] = func(list[dex]);
	// 	}
	//
	// 	return array;
	// }

	// /// maps list and adds each new element into another list
	// /// GARBAGE: cache the Func or use static method (not instance) to avoid GC
	// public static void MapInto<T1, TR>(this List<T1> list, List<TR> into, Func<T1, TR> func) {
	// 	foreach (var element in list) {
	// 		into.Add(func(element));
	// 	}
	// }

	// /// takes list, converts each element using func, checks predicate on new element
	// /// returns new list
	// /// GARBAGE: list returned 
	// /// GARBAGE: cache the Func or use static method (not instance) to avoid GC
	// public static List<TR> MapIf<T1, TR>(
	// 	this List<T1> list,
	// 	Func<T1, TR> func,
	// 	Func<TR, bool> predicate
	// ) {
	// 	var result = new List<TR>();
	//
	// 	foreach (var original in list) {
	// 		var converted = func(original);
	// 		if (predicate(converted)) result.Add(converted);
	// 	}
	//
	// 	return result;
	// }
	//
	// /// each element is converted, then mutated
	// /// GARBAGE: new array returned & cache Func or use static method (not instance)
	// public static List<TR> Map<T1, TR>(this List<T1> list, Func<T1, TR> func) {
	// 	var result = new List<TR>();
	// 	foreach (var original in list) {
	// 		result.Add(func(original));
	// 	}
	//
	// 	return result;
	// }
	//
	// /// each element is converted, then mutated
	// /// GARBAGE: new list returned 
	// /// GARBAGE: cache Func or use static method (not instance) to avoid GC
	// public static List<TR> Map<T1, TR>(
	// 	this List<T1> list,
	// 	Func<T1, TR> converter,
	// 	Func<TR, TR> mutator
	// ) {
	// 	var result = new List<TR>();
	// 	foreach (var original in list) {
	// 		result.Add(mutator(converter(original)));
	// 	}
	//
	// 	return result;
	// }

	/// AddRange, but mapped using converter func
	/// GARBAGE: cache Func or use static method (not instance) to avoid GC
	public static void AddFrom<T1, TR>(this List<TR> list, List<T1> from, Func<T1, TR> func)
	{
		foreach (var element in from) {
			list.Add(func(element));
		}
	}

	/// GARBAGE: cache the Action or use static method (not instance) to avoid GC
	public static void Each_DEPRECATED<T>(this IEnumerable<T> list, Action<T> action)
	{
		foreach (var elem in list) action(elem);
	}

	// /// Returns true if list is NOT null and NOT empty
	// public static bool Any<T>(this List<T> list) => list != null && list.Count > 0;

	// /// Returns true if list is NULL or EMPTY
	// public static bool Nil<T>(this List<T> list) => list == null || list.Count == 0;


	public static int Add<T>(this List<T> list, List<T> range, int limit)
	{
		if (limit == 0 || range.Count == 0) return 0;

		if (limit < 0 || limit >= range.Count) {
			list.AddRange(range);
			return range.Count;
		}

		for (var dex = 0; dex < limit; dex++) {
			list.Add(range[dex]);
		}

		return limit;
	}

	/// Returns first element (or null) 
	public static T FirstOrNull<T>(this List<T> list) where T : class
		=> list != null && list.Count > 0 ? list[0] : null;

	/// Returns last element (or null) 
	public static T LastOrNull<T>(this List<T> list) where T : class
	{
		if (list == null) return null;
		var dex = list.Count - 1;
		if (dex < 0) return null;
		return list[dex];
	}

	/// Iterates backward to remove elements that fail the predicate.
	/// GARBAGE: cache the Func or use static method (not instance) to avoid GC
	public static void Filter<T>(this List<T> list, Func<T, bool> predicate)
	{
		for (var dex = list.Count - 1; dex >= 0; dex--) {
			if (predicate(list[dex])) continue;
			list.RemoveAt(dex);
		}
	}

	/// Iterates backward to remove elements that fail the predicate (with data object).
	/// GARBAGE: cache the Func or use static method (not instance) to avoid GC
	public static void Filter<TElement, TData>(
		this List<TElement> list,
		TData data,
		Func<TElement, TData, bool> predicate
	)
	{
		for (var dex = list.Count - 1; dex >= 0; dex--) {
			if (predicate(list[dex], data)) continue;
			list.RemoveAt(dex);
		}
	}

	/// Iterates backward to remove elements that are null
	public static void RemoveNulls<T>(this List<T> list) where T : class
	{
		for (var dex = list.Count - 1; dex >= 0; dex--) {
			if (list[dex] == null) {
				list.RemoveAt(dex);
			}
		}
	}


	// TODO: cleanup
	// TODO: cleanup
	// TODO: cleanup


	// /// <summary>
	// /// Add any number of elements
	// /// </summary>
	// public static void Add<T>(this List<T> list, params T[] elements)
	// {
	// 	foreach (var element in elements) {
	// 		list.Add(element);
	// 	}
	// }

	/// <summary>
	/// Clears list (and returns itself so you can chain)
	/// </summary>
	public static List<T> ClearThen<T>(this List<T> list)
	{
		list.Clear();
		return list;
	}

	/// <summary>
	/// Removes nulls in list (and returns itself so you can chain)
	/// </summary>
	public static List<T> ClearNullsThen<T>(this List<T> list) where T : UnityEngine.Object
	{
		list.RemoveAll(e => e == null);
		return list;
	}

	/// <summary>
	/// Add element to a list if it passes a condition
	/// </summary>
	public static bool AddIf<T>(this List<T> list, T element, Func<T, bool> condition)
	{
		if (condition(element)) {
			list.Add(element);
			return true;
		}

		return false;
	}

	/// <summary>
	/// Adds element at beginning
	/// </summary>
	public static void AddFront<T>(this List<T> list, T element)
	{
		list.Insert(0, element);
	}

	/// <summary>
	/// Only adds the element to the list if the element is not null.
	/// </summary>
	public static void AddIfValid<T>(this List<T> list, T element)
	{
		if (element != null) {
			list.Add(element);
		}
	}

	/// Only adds the element to the list if the element is not null and list doesn't already contain it.
	public static void AddDistinctValid<T>(this List<T> list, T element)
	{
		if (element != null && !list.Contains(element)) {
			list.Add(element);
		}
	}

	/// Adds element (if !null) then returns element (for chaining)
	public static T AddThen<T>(this List<T> list, T element) where T : class
	{
		if (element != null) {
			list.Add(element);
			return element;
		}

		return null;
	}

	/// Removes elements that pass predicate, to limit (default none)
	/// NOTE: iterates backwards
	public static int RemoveIf<T>(this List<T> list, Func<T, bool> predicate, int limit = 0)
	{
		var removed = 0;
		if (limit == 0) limit = list.Count;

		for (var dex = list.Count - 1; dex >= 0; dex--) {
			if (!predicate(list[dex])) continue;

			list.RemoveAt(dex);
			removed++;
			if (removed >= limit) return removed;
		}

		return removed;
	}

	/// Removes first element that pass predicate
	public static bool RemoveFirst<T>(this List<T> list, Func<T, bool> predicate)
	{
		for (var dex = 0; dex < list.Count; dex++) {
			if (!predicate(list[dex])) continue;
			list.RemoveAt(dex);
			return true;
		}

		return false;
	}

	/// <summary>
	/// Only removes the element from the list if the element is not null.
	/// </summary>
	public static void RemoveIfValid<T>(this List<T> list, T element)
	{
		if (element != null) {
			list.Remove(element);
		}
	}

	/// <summary>
	/// Removes element at index, using callback first (assumes no modification of list)
	/// </summary>
	public static void DoAndRemove<T>(this List<T> list, int dex, Action<T> callback)
	{
		callback(list[dex]);
		list.RemoveAt(dex);
	}

	/// <summary>
	/// Removes all elements that pass a predicate and uses a callback on each
	/// </summary>
	public static void RemoveAllAndDo<T>(
		this List<T> list,
		Func<T, bool> condition,
		Action<T> callback
	)
	{
		for (int i = list.Count - 1; i >= 0; i--) {
			if (condition(list[i])) {
				callback(list[i]);
				list.RemoveAt(i);
			}
		}
	}

	public static void ShiftForward<T>(this List<T> list)
	{
		if (list.Count <= 1) return;

		list.AddFront(list[list.Count - 1]);
		list.RemoveAt(list.Count - 1);
	}

	public static void ShiftBack<T>(this List<T> list)
	{
		if (list.Count <= 1) return;

		list.Add(list[0]);
		list.RemoveAt(0);
	}

	/// <summary>
	/// Adds elements to a list from a source list if the element passes a condition
	/// </summary>
	public static List<T> AddWhere<T>(
		this List<T> list,
		List<T> otherList,
		Func<T, bool> condition
	)
	{
		otherList.Each_DEPRECATED(
			element => {
				if (condition(element)) {
					list.Add(element);
				}
			}
		);
		return list;
	}

	/// <summary>
	/// Moves elements from one list to another
	/// </summary>
	public static void Transfer<T>(this List<T> fromList, List<T> toList)
	{
		fromList.Each_DEPRECATED(toList.Add);
		fromList.Clear();
	}

	/// Moves an element from one list to another
	public static void TransferElement<T>(this List<T> fromList, T element, List<T> toList)
	{
		fromList.Remove(element);
		toList.Add(element);
	}

	/// <summary>
	/// Moves elements from one list to another if it passes a condition
	/// </summary>
	public static void Transfer<T>(
		this List<T> fromList,
		List<T> toList,
		Func<T, bool> condition
	)
	{
		var startingIndex = toList.Count;
		fromList.Each_DEPRECATED(
			element => {
				if (condition(element)) {
					toList.Add(element);
				}
			}
		);

		toList.EachStartingAt(startingIndex, e => fromList.Remove(e));
	}

	/// <summary>
	/// Adds elements, selecting from another list using the callback
	/// </summary>
	public static List<T> AddSelect<T, U>(
		this List<T> list,
		List<U> otherList,
		Func<U, T> callback
	)
	{
		otherList.Each_DEPRECATED(element => { list.Add(callback(element)); });
		return list;
	}

	/// <summary>
	/// Adds elements to a list from a source list if the element fails a condition
	/// </summary>
	public static List<T> AddExceptWhere<T>(
		this List<T> list,
		List<T> otherList,
		Func<T, bool> condition
	)
	{
		otherList.Each_DEPRECATED(
			element => {
				if (!condition(element)) {
					list.Add(element);
				}
			}
		);
		return list;
	}

	/// <summary>
	/// Iterates through a list using a callback on each element (with index supplied)
	/// </summary>
	public static void EachIndex<T>(this List<T> list, Action<T, int> callback)
	{
		for (int index = 0; index < list.Count; index++) {
			callback(list[index], index);
		}
	}

	/// <summary>
	/// Iterates through a list using a callback on each element if it passes predicate
	/// </summary>
	public static void EachIf<T>(
		this List<T> list,
		Func<T, bool> predicate,
		Action<T> callback
	)
	{
		for (int index = 0; index < list.Count; index++) {
			if (predicate(list[index])) {
				callback(list[index]);
			}
		}
	}

	/// <summary>
	/// Iterates through a list using different callbacks on each element depending on if it passes/fails a predicate
	/// </summary>
	public static void EachIfElse<T>(
		this List<T> list,
		Func<T, bool> predicate,
		Action<T> passCallback,
		Action<T> failCallback
	)
	{
		for (int index = 0; index < list.Count; index++) {
			if (predicate(list[index])) {
				passCallback(list[index]);
			}
			else {
				failCallback(list[index]);
			}
		}
	}

	/// <summary>
	/// Iterates through a list starting at an index
	/// </summary>
	public static void EachStartingAt<T>(
		this List<T> list,
		int startingIndex,
		Action<T> callback
	)
	{
		for (int index = startingIndex; index < list.Count; index++) {
			callback(list[index]);
		}
	}

	/// <summary>
	/// Iterates through a list using a callback on each element, then clears the list
	/// </summary>
	public static void EachThenClear<T>(this List<T> list, Action<T> callback)
	{
		for (int index = 0; index < list.Count; index++) {
			callback(list[index]);
		}

		list.Clear();
	}

	/// <summary>
	/// Iterates through a list using a callback on each element (starts from end)
	/// </summary>
	public static void EachBackward<T>(this List<T> list, Action<T> callback)
	{
		for (int index = list.Count - 1; index >= 0; index--) {
			callback(list[index]);
		}
	}

	// /// <summary>
	// /// Returns true if any elements in a list pass predicate
	// /// </summary>
	// public static bool Any<T>(this List<T> list, Func<T, bool> predicate) {
	// 	for (int index = 0; index < list.Count; index++) {
	// 		if (predicate(list[index])) {
	// 			return true;
	// 		}
	// 	}
	//
	// 	return false;
	// }

	/// <summary>
	/// Returns true if no elements in a list pass predicate
	/// </summary>
	public static bool None<T>(this List<T> list, Func<T, bool> predicate)
	{
		for (int index = 0; index < list.Count; index++) {
			if (predicate(list[index])) {
				return false;
			}
		}

		return true;
	}

	/// <summary>
	/// Returns true if all elements in a list pass predicate
	/// </summary>
	public static bool All<T>(this List<T> list, Func<T, bool> predicate)
	{
		for (int index = 0; index < list.Count; index++) {
			if (!predicate(list[index])) {
				return false;
			}
		}

		return true;
	}

	/// <summary>
	/// Returns true if all elements in a list pass predicate (provides index)
	/// </summary>
	public static bool AllIndex<T>(this List<T> list, Func<T, int, bool> predicate)
	{
		for (int index = 0; index < list.Count; index++) {
			if (!predicate(list[index], index)) {
				return false;
			}
		}

		return true;
	}

	/// <summary>
	/// Returns first element (or null) that meets predicate
	/// </summary>
	public static T FirstOrNull<T>(this List<T> list, Func<T, bool> predicate = null)
		where T : class
	{
		if (list != null && list.IsNotEmpty()) {
			if (predicate != null) {
				for (int index = 0; index < list.Count; index++) {
					if (predicate(list[index])) {
						return list[index];
					}
				}
			}
			else {
				return list[0];
			}
		}

		return null;
	}

	/// <summary>
	/// Removes and returns the first element from the list.
	/// </summary>
	public static T GrabFirstOrNull<T>(this List<T> list) where T : class
	{
		return list.IsNotEmpty()
			? list.GrabFirst()
			: null;
	}

	/// <summary>
	/// Removes and returns the first element from the list.
	/// </summary>
	public static T GrabFirst<T>(this List<T> list)
	{
		if (list.IsEmpty()) {
			throw new NullReferenceException("Cannot GrabFirst from an empty list.");
		}

		var element = list[0];
		list.RemoveAt(0);
		return element;
	}

	/// <summary>
	/// Removes and returns the last element from the list.
	/// </summary>
	public static T GrabLast<T>(this List<T> list)
	{
		if (list.IsEmpty()) {
			throw new NullReferenceException("Cannot GrabLast from an empty list.");
		}

		var element = list[list.Count - 1];
		list.RemoveAt(list.Count - 1);
		return element;
	}

	/// <summary>
	/// Finds first element that passes predicate and uses callback on it (returns true if found)
	/// </summary>
	public static bool FindAndCall<T>(
		this List<T> list,
		Func<T, bool> predicate,
		Action<T> callback
	) where T : class
	{
		if (list != null && list.IsNotEmpty()) {
			for (int index = 0; index < list.Count; index++) {
				if (predicate(list[index])) {
					callback(list[index]);
					return true;
				}
			}
		}

		return false;
	}


	/// <summary>
	/// Returns a random element from a list (optional: list of weights)
	/// </summary>
	public static T RandomElement<T>(this List<T> list, List<float> weights = null)
	{
		if (weights == null) {
			return list[UnityEngine.Random.Range(0, list.Count)];
		}

		var randomWeight = UnityEngine.Random.value * weights.Sum();
		var totalWeight = 0f;
		var index = weights.FindIndex(
			weight => {
				totalWeight += weight;
				return randomWeight <= totalWeight;
			}
		);

		return list[index];
	}

	static List<int> _randomWhere_Indexes = new List<int>(100);

	/// Returns a random element from elements within a list that pass a predicate (or null)
	public static T RandomWhere<T>(this List<T> list, Func<T, bool> predicate) where T : class
	{
		_randomWhere_Indexes.Clear();

		list.EachIndex(
			(element, dex) => {
				if (predicate(element)) {
					_randomWhere_Indexes.Add(dex);
				}
			}
		);

		return _randomWhere_Indexes.IsNotEmpty()
			? list[_randomWhere_Indexes.RandomElement()]
			: null;
	}

	/// Returns a random element from elements within a list that pass a predicate (or null)
	public static int RandomDexWhere<T>(this List<T> list, Func<T, bool> predicate)
		where T : class
	{
		_randomWhere_Indexes.Clear();

		list.EachIndex(
			(element, dex) => {
				if (predicate(element)) {
					_randomWhere_Indexes.Add(dex);
				}
			}
		);

		return _randomWhere_Indexes.IsNotEmpty()
			? _randomWhere_Indexes.RandomElement()
			: -1;
	}

	/// <summary>
	/// Searches through a list, returning the closest distance (using a provided distance function)
	/// </summary>
	public static T Closest<T>(this List<T> list, Func<T, float> distanceFunction)
		where T : class
	{
		var finalDistance = -1f;
		T result = null;

		for (int index = 0; index < list.Count; index++) {
			var newDistance = distanceFunction(list[index]);
			if (index == 0 || newDistance < finalDistance) {
				result = list[index];
				finalDistance = newDistance;
			}
		}

		return result;
	}

	/// <summary>
	/// Searches through a list, returning the closest distance (using a provided distance function) and outs final distance
	/// </summary>
	public static T Closest<T>(
		this List<T> list,
		Func<T, float> distanceFunction,
		out float finalDistance
	) where T : class
	{
		finalDistance = -1;
		T result = null;

		for (int index = 0; index < list.Count; index++) {
			var newDistance = distanceFunction(list[index]);
			if (index == 0 || newDistance < finalDistance) {
				result = list[index];
				finalDistance = newDistance;
			}
		}

		return result;
	}

	/// <summary>
	/// Returns a string of all elements.ToString() joined with seperators
	/// </summary>
	public static string Stringify<T>(this List<T> list, string separator = ", ")
	{
		var builder = new StringBuilder();
		var lastIndex = list.Count - 1;

		for (var index = 0; index <= lastIndex; index++) {
			builder.Append(list[index]);
			if (index < lastIndex) {
				builder.Append(separator);
			}
		}

		return builder.ToString();
	}

	public static string Join<T>(this List<T> list, string delimiter = ", ", string or = "")
	{
		if (list.Count == 0) return or;

		var builder = new StringBuilder();
		var lastIndex = list.Count - 1;

		for (var index = 0; index <= lastIndex; index++) {
			builder.Append(list[index]);
			if (index < lastIndex) {
				builder.Append(delimiter);
			}
		}

		return builder.ToString();
	}

	public static string JoinLine<T>(this List<T> list, string delimiter = "\n", string or = "")
		=> list.Join(delimiter, or);

	public static string Join<T>(
		this List<T> list,
		Func<T, string> select,
		string delimiter = ", "
	)
	{
		var builder = new StringBuilder();
		var lastIndex = list.Count - 1;

		for (var index = 0; index <= lastIndex; index++) {
			builder.Append(select(list[index]));
			if (index < lastIndex) {
				builder.Append(delimiter);
			}
		}

		return builder.ToString();
	}

	/// <summary>
	/// Returns true if list is empty
	/// </summary>
	public static bool IsEmpty<T>(this List<T> list)
	{
		return list.Count == 0;
	}

	/// <summary>
	/// Returns true if list is NOT empty
	/// </summary>
	public static bool IsNotEmpty<T>(this List<T> list)
	{
		return list.Count > 0;
	}

	/// <summary>
	/// Shuffles around the list's elements randomly
	/// </summary>
	public static void Shuffle<T>(this List<T> list)
	{
		var count = list.Count;
		for (var i = 0; i < count; ++i) {
			var temp = list[i];
			var swapIndex = UnityEngine.Random.Range(0, count);
			list[i] = list[swapIndex];
			list[swapIndex] = temp;
		}
	}

	/// <summary>
	/// Returns random valid index in a list
	/// </summary>
	public static int RandomIndex<T>(this List<T> list)
	{
		return UnityEngine.Random.Range(0, list.Count);
	}

	/// <summary>
	/// Searches a list for the first element that passes a predicate, returning true if found (and outs its index)
	/// </summary>
	public static bool SearchIndex<T>(
		this List<T> list,
		Func<T, bool> predicate,
		out int result
	)
	{
		for (int index = 0; index < list.Count; index++) {
			if (predicate(list[index])) {
				result = index;
				return true;
			}
		}

		result = -1;
		return false;
	}

	/// Returns index of first element that passes predicate (or returns -1 if none, or takes defaultOverride)
	public static int IndexOfFirst<T>(
		this List<T> list,
		Func<T, bool> predicate,
		int defaultOverride = -1
	)
	{
		for (int index = 0; index < list.Count; index++) {
			if (predicate(list[index])) {
				return index;
			}
		}

		return defaultOverride;
	}

	/// Counts all elements that pass predicate
	public static int Count<T>(this List<T> list, Func<T, bool> predicate)
	{
		var result = 0;
		for (var dex = 0; dex < list.Count; dex++) {
			if (predicate(list[dex])) result++;
		}

		return result;
	}

	/// Returns element that passes predicate (reverse order from end)
	public static T Last<T>(this List<T> list, Func<T, bool> predicate) where T : class
	{
		for (var dex = list.Count - 1; dex >= 0; dex--) {
			if (predicate(list[dex])) return list[dex];
		}

		return null;
	}
}
}