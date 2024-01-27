using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swoonity.Collections
{
public static class ArrayUtils
{
	/// returns element if index is >= 0 < length, else default
	public static T GetIfValidIndex<T>(this T[] array, int dex)
	{
		if (dex < 0 || dex >= array.Length) return default;
		return array[dex];
	}

	/// takes array, converts each element using func, returns array
	/// GC: func (unless it's cached or static method), array returned 
	public static TR[] Map<T1, TR>(this T1[] original, Func<T1, TR> func)
	{
		var array = new TR[original.Length];
		for (var dex = 0; dex < original.Length; dex++) array[dex] = func(original[dex]);
		return array;
	}

	/// each element is converted, with extra param
	/// GARBAGE: new array returned, cache Func or use static method (not instance)
	public static TR[] Map<T1, TE1, TR>(this T1[] original, Func<T1, TE1, TR> func, TE1 extra1)
	{
		var array = new TR[original.Length];
		for (var dex = 0; dex < original.Length; dex++) array[dex] = func(original[dex], extra1);
		return array;
	}

	/// each element is converted, with extra params
	/// GARBAGE: new array returned, cache Func or use static method (not instance)
	public static TR[] Map<T1, TE1, TE2, TR>(
		this T1[] original,
		Func<T1, TE1, TE2, TR> func,
		TE1 extra1,
		TE2 extra2
	)
	{
		var array = new TR[original.Length];
		for (var dex = 0; dex < original.Length; dex++)
			array[dex] = func(original[dex], extra1, extra2);
		return array;
	}

	/// each element is converted, then mutated
	/// GARBAGE: new array returned, cache Func or use static method (not instance)
	public static TR[] Map<T1, TR>(
		this T1[] original,
		Func<T1, TR> converter,
		Func<TR, TR> mutator
	)
	{
		var array = new TR[original.Length];
		for (var dex = 0; dex < original.Length; dex++)
			array[dex] = mutator(converter(original[dex]));
		return array;
	}

	/// each pair is converted + reduced
	/// GARBAGE: new array returned, cache Func or use static method (not instance)
	public static TR[] MapDown<T1, T2, TR>(this (T1, T2)[] original, Func<T1, T2, TR> converter)
	{
		var array = new TR[original.Length];
		for (var dex = 0; dex < original.Length; dex++) {
			var (item1, item2) = original[dex];
			array[dex] = converter(item1, item2);
		}

		return array;
	}

	public static void MapInto<T1, T2, TR>(
		this (T1, T2)[] original,
		List<TR> into,
		Func<T1, T2, TR> converter
	)
	{
		foreach (var (el1, el2) in original) {
			into.Add(converter(el1, el2));
		}
	}

	public static T GetSafe<T>(this T[] array, int dex) where T : class
	{
		if (array == null || array.Length <= dex) return null;
		return array[dex];
	}

	/// Returns true if array is NOT null and NOT empty
	public static bool Any<T>(this T[] array) => array != null && array.Length > 0;

	/// Returns true if array is NULL or EMPTY
	public static bool Nil<T>(this T[] array) => array == null || array.Length == 0;

	/// Returns first element (or null) 
	public static T FirstOrNull<T>(this T[] array) where T : class
		=> array != null && array.Length > 0 ? array[0] : null;


	/// Returns last element (or null) 
	public static T LastOrNull<T>(this T[] array) where T : class
	{
		if (array == null) return null;
		var dex = array.Length - 1;
		if (dex < 0) return null;
		return array[dex];
	}

	/// GARBAGE: cache the Action or use static method (not instance) to avoid GC
	public static void Each<T>(this T[] array, Action<T> action)
	{
		foreach (var elem in array) action(elem);
	}

	/// GARBAGE: cache the Action or use static method (not instance) to avoid GC
	public static void Each<T>(this T[] array, Action<T, int> action)
	{
		for (var index = 0; index < array.Length; index++) {
			action(array[index], index);
		}
	}

	/// Gets first element, or throws if null/empty array
	public static T FirstOrThrow<T>(this T[] array)
	{
		if (array.Any()) return array[0];
		throw new Exception($"{nameof(FirstOrThrow)} bad array");
	}


	// TODO: cleanup
	// TODO: cleanup
	// TODO: cleanup

	/// <summary>
	/// Iterates through an array using a callback on each element
	/// </summary>
	public static void EachBackward<T>(this T[] array, Action<T> callback)
	{
		for (int index = array.Length - 1; index >= 0; index--) {
			callback(array[index]);
		}
	}

	/// <summary>
	/// Iterates through an array using a callback on each element (with index supplied)
	/// </summary>
	public static void EachIndex<T>(this T[] array, Action<T, int> callback)
	{
		for (int index = 0; index < array.Length; index++) {
			callback(array[index], index);
		}
	}

	/// <summary>
	/// Iterates through an array using a callback on each element if it passes predicate
	/// </summary>
	public static void EachIf<T>(this T[] array, Func<T, bool> predicate, Action<T> callback)
	{
		for (int index = 0; index < array.Length; index++) {
			if (predicate(array[index])) {
				callback(array[index]);
			}
		}
	}

	/// <summary>
	/// Returns true if any elements in an array pass predicate
	/// </summary>
	public static bool Any<T>(this T[] array, Func<T, bool> predicate)
	{
		for (int index = 0; index < array.Length; index++) {
			if (predicate(array[index])) {
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// Returns true if all elements in an array pass predicate
	/// </summary>
	public static bool All<T>(this T[] array, Func<T, bool> predicate)
	{
		for (int index = 0; index < array.Length; index++) {
			if (!predicate(array[index])) {
				return false;
			}
		}

		return true;
	}

	/// <summary>
	/// Shortcut for System.Array.FindIndex 
	/// </summary>
	public static int FindIndex<T>(this T[] array, Predicate<T> predicate)
	{
		return Array.FindIndex(array, predicate);
	}

	/// <summary>
	/// Returns the first index in the array where the element exists (returns -1 if can't find)
	/// </summary>
	public static int IndexOf<T>(this T[] array, T target)
	{
		for (var index = 0; index < array.Length; index++) {
			if (array[index].Equals(target)) {
				return index;
			}

			;
		}

		return -1;
	}

	/// <summary>
	/// Returns true if array contains target element
	/// </summary>
	public static bool Contains<T>(this T[] array, T target)
	{
		for (var index = 0; index < array.Length; index++) {
			if (array[index].Equals(target)) {
				return true;
			}

			;
		}

		return false;
	}

	/// <summary>
	/// Returns true if array contains target element
	/// </summary>
	public static bool DoesNotContain<T>(this T[] array, T target)
	{
		return !array.Contains(target);
	}

	/// <summary>
	/// Returns a random element from an array (optional: array of weights)
	/// </summary>
	public static T RandomElement<T>(this T[] array, float[] weights = null)
	{
		if (weights == null) {
			return array[UnityEngine.Random.Range(0, array.Length)];
		}

		var randomWeight = UnityEngine.Random.value * weights.Sum();
		var totalWeight = 0f;
		var index = weights.FindIndex(
			weight => {
				totalWeight += weight;
				return randomWeight <= totalWeight;
			}
		);

		return array[index];
	}

	/// <summary>
	/// Returns a string of all elements.ToString() joined with separators
	/// </summary>
	public static string Stringify<T>(this T[] array, string separator = ", ")
	{
		var builder = new StringBuilder();
		var lastIndex = array.Length - 1;

		for (int index = 0; index <= lastIndex; index++) {
			builder.Append(array[index]);
			if (index < lastIndex) {
				builder.Append(separator);
			}
		}

		return builder.ToString();
	}

	public static string Join<T>(this T[] array, string delimiter = ", ", string or = "")
	{
		if (array.Length == 0) return or;

		var builder = new StringBuilder();
		var lastIndex = array.Length - 1;

		for (var index = 0; index <= lastIndex; index++) {
			builder.Append(array[index]);
			if (index < lastIndex) {
				builder.Append(delimiter);
			}
		}

		return builder.ToString();
	}

	public static string Join<T>(this T[] array, Func<T, string> select, string delimiter = ", ")
	{
		var builder = new StringBuilder();
		var lastIndex = array.Length - 1;

		for (var index = 0; index <= lastIndex; index++) {
			builder.Append(select(array[index]));
			if (index < lastIndex) {
				builder.Append(delimiter);
			}
		}

		return builder.ToString();
	}

	/// <summary>
	/// Returns true if array is empty
	/// </summary>
	public static bool IsEmpty<T>(this T[] array)
	{
		return array.Length == 0;
	}

	/// <summary>
	/// Returns true if array is NOT empty
	/// </summary>
	public static bool IsNotEmpty<T>(this T[] array)
	{
		return array.Length > 0;
	}

	/// <summary>
	/// Returns first element (or null) that meets predicate
	/// </summary>
	public static T FirstOrNull<T>(this T[] array, Func<T, bool> predicate) where T : class
	{
		for (int index = 0; index < array.Length; index++) {
			if (predicate(array[index])) {
				return array[index];
			}
		}

		return null;
	}

	/// <summary>
	/// Finds first element that passes predicate and uses callback on it (returns true if found)
	/// </summary>
	public static bool FindAndCall<T>(this T[] array, Func<T, bool> predicate, Action<T> callback)
		where T : class
	{
		if (array != null && array.IsNotEmpty()) {
			for (int index = 0; index < array.Length; index++) {
				if (predicate(array[index])) {
					callback(array[index]);
					return true;
				}
			}
		}

		return false;
	}

	/// <summary>
	/// Returns array as list
	/// </summary>
	public static List<T> ToList<T>(this T[] array)
	{
		return System.Linq.Enumerable.ToList(array);
	}

	public static bool IsValidIndexFor<T>(this int index, T[] array)
	{
		return array != null && index >= 0 && index < array.Length;
	}

	public static bool IsValidIndex<T>(this T[] array, int index)
	{
		return array != null && index >= 0 && index < array.Length;
	}
}
}