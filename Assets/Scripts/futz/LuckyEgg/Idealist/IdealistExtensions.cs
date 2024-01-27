using System;
using System.Collections.Generic;
using System.Text;

// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace Idealist
{
public static class Iteration
{
	/// <summary>Call action on each</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void Each<TEl>(
		this List<TEl> list,
		Action<TEl> fn
	)
	{
		foreach (var el in list) fn(el);
	}

	/// <summary>Call action on each</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void Each<TEl>(
		this List<TEl> list,
		Action<TEl, int> fn
	)
	{
		for (var i = 0; i < list.Count; i++) fn(list[i], i);
	}

	/// <summary>Call action on each</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	///	<inheritdoc cref="Each"/>
	public static void Each<TData, TEl>(
		this List<TEl> list,
		TData data,
		Action<TData, TEl> fn
	)
	{
		foreach (var el in list) fn(data, el);
	}

	/// <summary>Call action on each</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void Each<TData, TEl>(
		this List<TEl> list,
		TData data,
		Action<TData, TEl, int> fn
	)
	{
		for (var i = 0; i < list.Count; i++) fn(data, list[i], i);
	}

	/// <summary>Call action on each, in reverse 
	/// (Safe to remove elements as you go)</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void ReverseEach<TEl>(
		this List<TEl> list,
		Action<TEl> fn
	)
	{
		for (var i = list.Count - 1; i >= 0; i--) fn(list[i]);
	}

	/// <summary>Call action on each, in reverse 
	/// (Safe to remove elements as you go)</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void ReverseEach<TEl>(
		this List<TEl> list,
		Action<TEl, int> fn
	)
	{
		for (var i = list.Count - 1; i >= 0; i--) fn(list[i], i);
	}

	/// <summary>Call action on each, in reverse 
	/// (Safe to remove elements as you go)</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void ReverseEach<TData, TEl>(
		this List<TEl> list,
		TData data,
		Action<TData, TEl> fn
	)
	{
		for (var i = list.Count - 1; i >= 0; i--) fn(data, list[i]);
	}

	/// <summary>Call action on each, in reverse 
	/// (Safe to remove elements as you go)</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void ReverseEach<TData, TEl>(
		this List<TEl> list,
		TData data,
		Action<TData, TEl, int> fn
	)
	{
		for (var i = list.Count - 1; i >= 0; i--) fn(data, list[i], i);
	}

	/// <summary>fn( list1[i], list2[i] ) until either ends</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void Zip<TData, TEl1, TEl2>(
		this List<TEl1> list1,
		List<TEl2> list2,
		TData data,
		Action<TData, TEl1, TEl2> fn
	)
	{
		var count = list1.Count;
		if (list2.Count < count) count = list2.Count;
		for (var i = 0; i < count; i++) fn(data, list1[i], list2[i]);
	}

	/// <summary>fn( list1[i], list2[i] ) until either ends</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void Zip<TEl1, TEl2>(
		this List<TEl1> list1,
		List<TEl2> list2,
		Action<TEl1, TEl2> fn
	)
	{
		var count = list1.Count;
		if (list2.Count < count) count = list2.Count;
		for (var i = 0; i < count; i++) fn(list1[i], list2[i]);
	}

	/// <summary>fn( list1[i], list2[i] ) until either ends</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void Zip<TData, TEl1, TEl2>(
		this (List<TEl1> list1, List<TEl2> list2) lists,
		TData data,
		Action<TData, TEl1, TEl2> fn
	)
	{
		var (list1, list2) = lists;
		var count = list1.Count;
		if (list2.Count < count) count = list2.Count;
		for (var i = 0; i < count; i++) fn(data, list1[i], list2[i]);
	}

	/// <summary>fn( list1[i], list2[i] ) until either ends</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void Zip<TEl1, TEl2>(
		this (List<TEl1> list1, List<TEl2> list2) lists,
		Action<TEl1, TEl2> fn
	)
	{
		var (list1, list2) = lists;
		var count = list1.Count;
		if (list2.Count < count) count = list2.Count;
		for (var i = 0; i < count; i++) fn(list1[i], list2[i]);
	}


	/// <summary>call fn() * numberOfTimes</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void Times(this int numberOfTimes, Action fn)
	{
		for (var i = 0; i < numberOfTimes; i++) fn();
	}

	/// <summary>call fn(index) * numberOfTimes</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void Times(this int numberOfTimes, Action<int> fn)
	{
		for (var i = 0; i < numberOfTimes; i++) fn(i);
	}

	/// <summary>call fn(data) * numberOfTimes</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void Times<TData>(this int numberOfTimes, TData data, Action<TData> fn)
	{
		for (var i = 0; i < numberOfTimes; i++) fn(data);
	}

	/// <summary>call fn(data, index) * numberOfTimes</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void Times<TData>(this int numberOfTimes, TData data, Action<TData, int> fn)
	{
		for (var i = 0; i < numberOfTimes; i++) fn(data, i);
	}
}

public static class Alteration
{
	public static void AddIf<TEl>(this List<TEl> list, TEl el, bool addIf)
	{
		if (addIf) list.Add(el);
	}

	/// <inheritdoc cref="Alteration"/>
	public static void AddIf<TEl>(this List<TEl> list, TEl el, Func<TEl, bool> fn)
	{
		if (fn(el)) list.Add(el);
	}

	public static void AddIf<TArg1, TEl>(
		this List<TEl> list,
		TEl el,
		TArg1 arg1,
		Func<TArg1, TEl, bool> fn
	)
	{
		if (fn(arg1, el)) list.Add(el);
	}

	/// <summary>add if el != null</summary>
	public static bool TryAdd<TEl>(this List<TEl> list, TEl el)
	{
		if (el == null) return false;
		list.Add(el);
		return true;
	}

	public static void Add<TEl>(this List<TEl> list, TEl a, TEl b)
	{
		list.Add(a);
		list.Add(b);
	}

	public static void Add<TEl>(this List<TEl> list, TEl a, TEl b, TEl c)
	{
		list.Add(a);
		list.Add(b);
		list.Add(c);
	}

	public static void Add<TEl>(this List<TEl> list, TEl a, TEl b, TEl c, TEl d)
	{
		list.Add(a);
		list.Add(b);
		list.Add(c);
		list.Add(d);
	}

	public static void Add<TEl>(this List<TEl> list, TEl a, TEl b, TEl c, TEl d, TEl e)
	{
		list.Add(a);
		list.Add(b);
		list.Add(c);
		list.Add(d);
		list.Add(e);
	}

	public static void Add<TEl>(this List<TEl> list, (TEl, TEl) tuple)
	{
		list.Add(tuple.Item1);
		list.Add(tuple.Item2);
	}

	public static void Add<TEl>(this List<TEl> list, (TEl, TEl, TEl) tuple)
	{
		list.Add(tuple.Item1);
		list.Add(tuple.Item2);
		list.Add(tuple.Item3);
	}

	public static void Add<TEl>(this List<TEl> list, (TEl, TEl, TEl, TEl) tuple)
	{
		list.Add(tuple.Item1);
		list.Add(tuple.Item2);
		list.Add(tuple.Item3);
		list.Add(tuple.Item4);
	}

	public static void Add<TEl>(this List<TEl> list, (TEl, TEl, TEl, TEl, TEl) tuple)
	{
		list.Add(tuple.Item1);
		list.Add(tuple.Item2);
		list.Add(tuple.Item3);
		list.Add(tuple.Item4);
		list.Add(tuple.Item5);
	}

	public static void AddEachIf<TEl>(
		this List<TEl> into,
		List<TEl> from,
		Func<TEl, bool> fn
	)
	{
		foreach (var el in from) {
			if (fn(el)) into.Add(el);
		}
	}

	public static void AddEachIf<TEl, TR>(
		this List<TR> into,
		List<TEl> from,
		Func<TEl, bool> fnIf,
		Func<TEl, TR> fnMap
	)
	{
		foreach (var el in from) {
			if (fnIf(el)) {
				into.Add(fnMap(el));
			}
		}
	}

	public static void AddAll<TEl>(this List<TEl> list, List<TEl> other)
	{
		foreach (var el in other) {
			list.Add(el);
		}
	}

	/// <summary>add and return its index</summary>
	public static int AddGetIndex<TEl>(this List<TEl> list, TEl el)
	{
		var index = list.Count;
		list.Add(el);
		return index;
	}

	/// <summary>add element if list doesn't contain it (return true if added)</summary>
	public static bool AddDistinct<TEl>(this List<TEl> list, TEl el)
	{
		if (list.Contains(el)) return false;

		list.Add(el);
		return true;
	}

	/// <summary>add if no element passes: fn(toAdd, existingEl)</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static bool AddDistinct<TEl>(this List<TEl> list, TEl toAdd, Func<TEl, TEl, bool> fn)
	{
		foreach (var el in list) {
			if (fn(toAdd, el)) return false;
		}

		list.Add(toAdd);
		return true;
	}

	/// <summary>Each element is converted and added to another list (destination list is CLEARED first)</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void MapInto<TEl, TR>(
		this List<TEl> from,
		List<TR> into,
		Func<TEl, TR> fn
	)
	{
		into.Clear();
		foreach (var element in from) {
			into.Add(fn(element));
		}
	}

	/// <summary>Each element is converted and added to another list (destination list is CLEARED first)</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void MapInto<TEl, TR, TData>(
		this List<TEl> from,
		List<TR> into,
		TData data,
		Func<TData, TEl, TR> fn
	)
	{
		into.Clear();
		foreach (var element in from) {
			into.Add(fn(data, element));
		}
	}

	/// <summary>Each element is converted and added to another list (destination list is CLEARED first)</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void MapInto<TEl, TR, TData>(
		this List<TEl> from,
		List<TR> into,
		TData data,
		Func<TData, TEl, int, TR> fn
	)
	{
		into.Clear();
		for (var i = 0; i < from.Count; i++) {
			var element = from[i];
			into.Add(fn(data, element, i));
		}
	}

	/// <summary>Each element is converted and added FROM another list (destination list is CLEARED first)</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void MapFrom<TEl, TR>(
		this List<TR> into,
		List<TEl> from,
		Func<TEl, TR> fn
	)
	{
		into.Clear();
		foreach (var element in from) {
			into.Add(fn(element));
		}
	}

	/// <summary>Each element is converted and added FROM another list (destination list is CLEARED first)</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void MapFrom<TEl, TR, TData>(
		this List<TR> into,
		List<TEl> from,
		TData data,
		Func<TData, TEl, TR> fn
	)
	{
		into.Clear();
		foreach (var element in from) {
			into.Add(fn(data, element));
		}
	}

	/// <summary>Each element is converted and added FROM another list (destination list is CLEARED first)</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void MapFrom<TEl, TR, TData>(
		this List<TR> into,
		List<TEl> from,
		TData data,
		Func<TData, TEl, int, TR> fn
	)
	{
		into.Clear();
		for (var i = 0; i < from.Count; i++) {
			var element = from[i];
			into.Add(fn(data, element, i));
		}
	}

	/// <summary>Each element is converted and added to a new list</summary>
	/// GARBAGE: new list
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static List<TR> MapNew<TEl, TR>(
		this List<TEl> from,
		Func<TEl, TR> fn
	)
	{
		var list = new List<TR>(from.Count);
		foreach (var element in from) {
			list.Add(fn(element));
		}

		return list;
	}

	/// <summary>Each element is converted and added to a new list</summary>
	/// GARBAGE: new list
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static List<TR> MapNew<TEl, TR>(
		this List<TEl> from,
		Func<TEl, int, TR> fn
	)
	{
		var list = new List<TR>(from.Count);
		for (var i = 0; i < from.Count; i++) list.Add(fn(from[i], i));
		return list;
	}

	/// <summary>Each element is converted and added to a new list</summary>
	/// GARBAGE: new list
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static List<TR> MapNew<TEl, TR, TData>(
		this List<TEl> from,
		TData data,
		Func<TData, TEl, TR> fn
	)
	{
		var list = new List<TR>(from.Count);
		foreach (var element in from) {
			list.Add(fn(data, element));
		}

		return list;
	}

	/// <summary>Each element is converted and added to another list</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void AddMapInto<TEl, TR>(
		this List<TEl> from,
		List<TR> into,
		Func<TEl, TR> fn
	)
	{
		foreach (var element in from) {
			into.Add(fn(element));
		}
	}

	/// <summary>Each element is converted and added to another list</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void AddMapInto<TEl, TR, TData>(
		this List<TEl> from,
		List<TR> into,
		TData data,
		Func<TData, TEl, TR> fn
	)
	{
		foreach (var element in from) {
			into.Add(fn(data, element));
		}
	}

	/// <summary>Each element is converted and added to another list</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void AddMapInto<TEl, TR, TData>(
		this List<TEl> from,
		List<TR> into,
		TData data,
		Func<TData, TEl, int, TR> fn
	)
	{
		for (var i = 0; i < from.Count; i++) {
			var element = from[i];
			into.Add(fn(data, element, i));
		}
	}

	/// <summary>Each element is converted and added FROM another list</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void AddMapFrom<TEl, TR>(
		this List<TR> into,
		List<TEl> from,
		Func<TEl, TR> fn
	)
	{
		foreach (var element in from) {
			into.Add(fn(element));
		}
	}

	/// <summary>Each element is converted and added FROM another list</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void AddMapFrom<TEl, TR, TData>(
		this List<TR> into,
		List<TEl> from,
		TData data,
		Func<TData, TEl, TR> fn
	)
	{
		foreach (var element in from) {
			into.Add(fn(data, element));
		}
	}

	/// <summary>Each element is converted and added FROM another list</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void AddMapFrom<TEl, TR, TData>(
		this List<TR> into,
		List<TEl> from,
		TData data,
		Func<TData, TEl, int, TR> fn
	)
	{
		for (var i = 0; i < from.Count; i++) {
			var element = from[i];
			into.Add(fn(data, element, i));
		}
	}

	/// <summary>does NOT keep list order</summary>
	public static void RemoveAtFast<TEl>(this List<TEl> list, int index)
	{
		var last = list.Count - 1;
		list[index] = list[last]; // overwrite target with last element
		list.RemoveAt(last); // remove last (faster to remove at end of list)
	}

	/// <summary>Removes and returns element at index</summary>
	public static TEl GrabAt<TEl>(this List<TEl> list, int index, bool keepOrder = false)
	{
		var element = list[index];
		if (keepOrder) list.RemoveAt(index);
		else list.RemoveAtFast(index);
		return element;
	}

	/// <summary>Removes and returns first element</summary>
	public static TEl GrabFirst<TEl>(this List<TEl> list, bool keepOrder = false) => list.GrabAt(0, keepOrder);

	/// <summary>Removes and returns last element</summary>
	public static TEl GrabLast<TEl>(this List<TEl> list)
	{
		var last = list.Count - 1;
		var element = list[last];
		list.RemoveAt(last);
		return element;
	}

	/// <summary>Iterates backward to remove elements that fail the predicate</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void Filter<TEl>(
		this List<TEl> list,
		Func<TEl, bool> fn
	)
	{
		for (var dex = list.Count - 1; dex >= 0; dex--) {
			if (!fn(list[dex])) list.RemoveAt(dex);
		}
	}

	/// <summary>Iterates backward to remove elements that fail the predicate</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void Filter<TEl, TData>(
		this List<TEl> list,
		TData data,
		Func<TData, TEl, bool> fn
	)
	{
		for (var dex = list.Count - 1; dex >= 0; dex--) {
			if (!fn(data, list[dex])) list.RemoveAt(dex);
		}
	}

	/// <summary>Iterates backward to remove elements that are null</summary>
	public static void RemoveNulls<TEl>(this List<TEl> list) where TEl : class
	{
		for (var dex = list.Count - 1; dex >= 0; dex--) {
			if (list[dex] == null) list.RemoveAt(dex);
		}
	}

	/// <summary>list.Clear</summary>
	/// <remarks>chainable</remarks>
	public static List<TEl> _Clear<TEl>(this List<TEl> list)
	{
		list.Clear();
		return list;
	}

	public static void AddTimes<TEl>(this List<TEl> list, TEl el, int count)
	{
		for (var i = 0; i < count; i++) {
			list.Add(el);
		}
	}

	public static void ForceSize<TEl>(this List<TEl> list, int size, TEl stub = default)
	{
		var currentCount = list.Count;
		if (currentCount == size) return;
		if (currentCount < size) {
			for (var i = 0; i < size - currentCount; i++) {
				list.Add(stub);
			}
		}
		else {
			for (var i = 0; i < currentCount - size; i++) {
				list.GrabLast();
			}
		}
	}

	/// <summary>replace any null elements with given element</summary>
	public static void FillNull<TEl>(this List<TEl> list, TEl el) where TEl : class
	{
		for (var i = 0; i < list.Count; i++) {
			if (list[i] == null) {
				list[i] = el;
			}
		}
	}

	/// <summary>for(i under max) Add(i) Add(i)</summary>
	public static void AddCount(this List<int> list, int max)
	{
		for (var i = 0; i < max; i++) {
			list.Add(i);
		}
	}

	public static void StealFrom<TEl>(
		this List<TEl> into,
		List<TEl> from,
		int count = 1,
		bool fromEnd = true
	)
	{
		if (count > from.Count) count = from.Count;

		if (fromEnd) {
			for (var i = 0; i < count; i++) {
				into.Add(from.GrabLast());
			}
		}
		else {
			for (var i = 0; i < count; i++) {
				into.Add(from.GrabFirst());
			}
		}
	}

	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void StealFrom<TEl, TR>(
		this List<TR> into,
		List<TEl> from,
		Func<TEl, TR> fnMap,
		int count = 1,
		bool fromEnd = true
	)
	{
		if (count > from.Count) count = from.Count;

		if (fromEnd) {
			for (var i = 0; i < count; i++) {
				into.Add(fnMap(from.GrabLast()));
			}
		}
		else {
			for (var i = 0; i < count; i++) {
				into.Add(fnMap(from.GrabFirst()));
			}
		}
	}

	/// take N elements from a "bag" list,
	/// add them to the "into" list,
	/// refill as necessary using the readonly "possibilities" list
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static void GrabBag<TEl>(
		this List<TEl> into,
		List<TEl> bag,
		int amount,
		List<TEl> possibilities,
		bool clearIntoList = false
	)
	{
		if (clearIntoList) into.Clear();
		if (amount > bag.Count) bag.ShuffleAdd(possibilities);

		for (var i = 0; i < amount; i++) {
			into.Add(bag.GrabLast());
		}
	}

	public static void MoveLastTo<TEl>(this List<TEl> from, List<TEl> into, int count = 1)
	{
		if (count > from.Count) count = from.Count;

		for (var i = 0; i < count; i++) {
			into.Add(from.GrabLast());
		}
	}
}

/// Iterates backward, will remove element if fn returns true (done)
/// <inheritdoc cref="NotesOnGarbageCollection"/>
public static class Processing
{
	/// <inheritdoc cref="Processing"/>
	public static void Process<TEl>(
		this List<TEl> list,
		Func<TEl, bool> fn
	)
	{
		for (var dex = list.Count - 1; dex >= 0; dex--) {
			if (fn(list[dex])) {
				list.RemoveAt(dex);
			}
		}
	}

	/// <inheritdoc cref="Processing"/>
	public static void Process<TEl>(
		this List<TEl> list,
		Func<TEl, int, bool> fn
	)
	{
		for (var dex = list.Count - 1; dex >= 0; dex--) {
			if (fn(list[dex], dex)) {
				list.RemoveAt(dex);
			}
		}
	}

	/// <inheritdoc cref="Processing"/>
	public static void Process<TEl, TData>(
		this List<TEl> list,
		TData data,
		Func<TData, TEl, bool> fn
	)
	{
		for (var dex = list.Count - 1; dex >= 0; dex--) {
			if (fn(data, list[dex])) {
				list.RemoveAt(dex);
			}
		}
	}

	/// <inheritdoc cref="Processing"/>
	public static void Process<TEl, TData>(
		this List<TEl> list,
		TData data,
		Func<TData, TEl, int, bool> fn
	)
	{
		for (var dex = list.Count - 1; dex >= 0; dex--) {
			if (fn(data, list[dex], dex)) {
				list.RemoveAt(dex);
			}
		}
	}

	/// <summary>(uses RemoveAtFast, ORDER IS NOT KEPT) </summary>
	/// <inheritdoc cref="Processing"/>
	public static void ProcessFast<TEl>(
		this List<TEl> list,
		Func<TEl, bool> fn
	)
	{
		var lastDex = list.Count - 1;
		for (var dex = lastDex; dex >= 0; dex--) {
			if (fn(list[dex])) {
				list[dex] = list[lastDex]; // overwrite target with last element
				list.RemoveAt(lastDex); // remove last (faster to remove at end of list)
				lastDex--;
			}
		}
	}

	/// <summary>(uses RemoveAtFast, ORDER IS NOT KEPT) </summary>
	/// <inheritdoc cref="Processing"/>
	public static void ProcessFast<TEl>(
		this List<TEl> list,
		Func<TEl, int, bool> fn
	)
	{
		var lastDex = list.Count - 1;
		for (var dex = lastDex; dex >= 0; dex--) {
			if (fn(list[dex], dex)) {
				list[dex] = list[lastDex]; // overwrite target with last element
				list.RemoveAt(lastDex); // remove last (faster to remove at end of list)
				lastDex--;
			}
		}
	}

	/// <summary>(uses RemoveAtFast, ORDER IS NOT KEPT) </summary>
	/// <inheritdoc cref="Processing"/>
	public static void ProcessFast<TEl, TData>(
		this List<TEl> list,
		TData data,
		Func<TData, TEl, bool> fn
	)
	{
		var lastDex = list.Count - 1;
		for (var dex = lastDex; dex >= 0; dex--) {
			if (fn(data, list[dex])) {
				list[dex] = list[lastDex]; // overwrite target with last element
				list.RemoveAt(lastDex); // remove last (faster to remove at end of list)
				lastDex--;
			}
		}
	}

	/// <summary>(uses RemoveAtFast, ORDER IS NOT KEPT) </summary>
	/// <inheritdoc cref="Processing"/>
	public static void ProcessFast<TEl, TData>(
		this List<TEl> list,
		TData data,
		Func<TData, TEl, int, bool> fn
	)
	{
		var lastDex = list.Count - 1;
		for (var dex = lastDex; dex >= 0; dex--) {
			if (fn(data, list[dex], dex)) {
				list[dex] = list[lastDex]; // overwrite target with last element
				list.RemoveAt(lastDex); // remove last (faster to remove at end of list)
				lastDex--;
			}
		}
	}

	/// <summary>(uses RemoveAtFast, ORDER IS NOT KEPT) </summary>
	/// <inheritdoc cref="Processing"/>
	public static void ProcessFast<TEl>(
		this List<TEl> list,
		Func<TEl, bool> fnProcess,
		Action<TEl> fnWhenRemoved
	)
	{
		var lastDex = list.Count - 1;
		for (var dex = lastDex; dex >= 0; dex--) {
			if (fnProcess(list[dex])) {
				list[dex] = list[lastDex]; // overwrite target with last element
				list.RemoveAt(lastDex); // remove last (faster to remove at end of list)
				lastDex--;
				fnWhenRemoved(list[dex]);
			}
		}
	}

	/// <summary>(uses RemoveAtFast, ORDER IS NOT KEPT) </summary>
	/// <inheritdoc cref="Processing"/>
	public static void ProcessFast<TEl>(
		this List<TEl> list,
		Func<TEl, int, bool> fnProcess,
		Action<TEl, int> fnWhenRemoved
	)
	{
		var lastDex = list.Count - 1;
		for (var dex = lastDex; dex >= 0; dex--) {
			if (fnProcess(list[dex], dex)) {
				list[dex] = list[lastDex]; // overwrite target with last element
				list.RemoveAt(lastDex); // remove last (faster to remove at end of list)
				lastDex--;
				fnWhenRemoved(list[dex], dex);
			}
		}
	}

	/// <summary>(uses RemoveAtFast, ORDER IS NOT KEPT) </summary>
	/// <inheritdoc cref="Processing"/>
	public static void ProcessFast<TEl, TData>(
		this List<TEl> list,
		TData data,
		Func<TData, TEl, bool> fnProcess,
		Action<TData, TEl> fnWhenRemoved
	)
	{
		var lastDex = list.Count - 1;
		for (var dex = lastDex; dex >= 0; dex--) {
			if (fnProcess(data, list[dex])) {
				list[dex] = list[lastDex]; // overwrite target with last element
				list.RemoveAt(lastDex); // remove last (faster to remove at end of list)
				lastDex--;
				fnWhenRemoved(data, list[dex]);
			}
		}
	}

	/// <summary>(uses RemoveAtFast, ORDER IS NOT KEPT) </summary>
	/// <inheritdoc cref="Processing"/>
	public static void ProcessFast<TEl, TData>(
		this List<TEl> list,
		TData data,
		Func<TData, TEl, int, bool> fnProcess,
		Action<TData, TEl, int> fnWhenRemoved
	)
	{
		var lastDex = list.Count - 1;
		for (var dex = lastDex; dex >= 0; dex--) {
			if (fnProcess(data, list[dex], dex)) {
				list[dex] = list[lastDex]; // overwrite target with last element
				list.RemoveAt(lastDex); // remove last (faster to remove at end of list)
				lastDex--;
				fnWhenRemoved(data, list[dex], dex);
			}
		}
	}
}

public static class Introspection
{
	/// not null and not empty
	public static bool Any<TEl>(this List<TEl> list) => list != null && list.Count > 0;

	/// null OR empty
	public static bool Nil<TEl>(this List<TEl> list) => list == null || list.Count <= 0;

	/// alias of !list.Contains
	public static bool Missing<TEl>(this List<TEl> list, TEl el) => !list.Contains(el);

	public static bool IsValidIndex<TEl>(this List<TEl> list, int index) => index >= 0 && index < list.Count;

	public static int LastIndex<TEl>(this List<TEl> list) => list.Count - 1;

	/// alias of list.Contains
	public static bool Has<TEl>(this List<TEl> list, TEl el) => list != null && list.Contains(el);

	/// true if list contains any of the elements in "anyOf" list
	/// <remarks>both lists must have 1+ elements otherwise returns false</remarks>
	public static bool HasAny<TEl>(this List<TEl> list, List<TEl> anyOf)
	{
		if (list.Nil() || anyOf.Nil()) return false;
		foreach (var el in anyOf) {
			if (list.Contains(el)) return true;
		}

		return false;
	}

	/// <summary>True if list has <b>1+</b> elements that pass predicate function (checks until true)</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static bool Has<TEl>(this List<TEl> list, Func<TEl, bool> fn)
	{
		foreach (var el in list) {
			if (fn(el)) return true;
		}

		return false;
	}

	/// <summary>True if list has <b>1+</b> elements that pass predicate function (checks until true)</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static bool Has<TEl, TData>(this List<TEl> list, TData data, Func<TData, TEl, bool> fn)
	{
		foreach (var el in list) {
			if (fn(data, el)) return true;
		}

		return false;
	}

	/// <summary>True if list contains target, uses getter function</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static bool Has<TEl, TTarget>(
		this List<TEl> list,
		TTarget target,
		Func<TEl, TTarget> fnGet
	)
	{
		foreach (var el in list) {
			if (Equals(fnGet(el), target)) return true;
		}

		return false;
	}

	/// true if contains 1+ null elements
	public static bool HasNull<TEl>(this List<TEl> list)
	{
		foreach (var el in list) {
			if (el == null) {
				return true;
			}
		}

		return false;
	}

	/// null OR empty OR if contains 1+ null elements
	public static bool NilOrHasNull<TEl>(this List<TEl> list) => list.Nil() || list.HasNull();

	/// <summary>True if list has <b>0</b> elements that pass predicate function (checks all)</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static bool None<TEl>(
		this List<TEl> list,
		Func<TEl, bool> fn,
		bool ifEmptyReturnBool = true
	)
	{
		if (list.Count == 0) return ifEmptyReturnBool;
		foreach (var el in list) {
			if (fn(el)) return false;
		}

		return true;
	}

	/// <summary>True if list <b>all</b> elements pass predicate (returns false on first fail)</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static bool All<TEl>(
		this List<TEl> list,
		Func<TEl, bool> fn,
		bool ifEmptyReturnBool = false
	)
	{
		if (list.Count == 0) return ifEmptyReturnBool;
		foreach (var el in list) {
			if (!fn(el)) return false;
		}

		return true;
	}

	/// <summary>True if list <b>all</b> elements pass predicate (returns false on first fail)</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static bool All<TEl, TData>(
		this List<TEl> list,
		TData data,
		Func<TData, TEl, bool> fn,
		bool ifEmptyReturnBool = false
	)
	{
		if (list.Count == 0) return ifEmptyReturnBool;
		foreach (var el in list) {
			if (!fn(data, el)) return false;
		}

		return true;
	}

	/// <summary>Counts all elements that pass a predicate function</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static int CountIf<TEl, TData>(
		this List<TEl> list,
		TData data,
		Func<TData, TEl, bool> fn
	)
	{
		var count = 0;
		foreach (var el in list) {
			if (fn(data, el)) count++;
		}

		return count;
	}

	/// <summary>Counts all elements that pass a predicate function</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static int CountIf<TEl>(this List<TEl> list, Func<TEl, bool> fn)
	{
		var count = 0;
		foreach (var el in list) {
			if (fn(el)) count++;
		}

		return count;
	}

	/// TODO: untested
	/// <summary>Compares each element with its potential twin</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static bool CheckEach<TElA, TElB>(
		this List<TElA> listA,
		List<TElB> listB,
		Func<TElA, TElB, bool> fn
	)
	{
		var count = listA.Count;
		if (count != listB.Count) return false;

		for (var i = 0; i < count; i++) {
			if (!fn(listA[i], listB[i])) return false;
		}

		return true;
	}
}

public static class Getters
{
	/// <summary>returns element at, or fallback if invalid index</summary>
	public static TEl GetOr<TEl>(this List<TEl> list, int index, TEl fallback = default)
		=> list != null && index >= 0 && index < list.Count
			? list[index]
			: fallback;

	/// <summary>returns element at, or fallback if invalid index</summary>
	public static TVal GetOr<TEl, TVal>(
		this List<TEl> list,
		int index,
		Func<TEl, TVal> fnInnerGet,
		TVal fallback = default
	)
		=> list != null && index >= 0 && index < list.Count
			? fnInnerGet(list[index])
			: fallback;

	/// <summary>returns element at, or fallback if invalid index</summary>
	public static TEl GetOr<TEl>(
		this List<TEl> list,
		int index,
		int orIndex,
		TEl fallback = default
	)
	{
		if (list == null) return fallback;
		if (index >= 0 && index < list.Count) return list[index];
		if (orIndex >= 0 && orIndex < list.Count) return list[orIndex];
		return fallback;
	}

	/// <summary>returns element at, or fallback if invalid index</summary>
	public static TEl GetOrThrow<TEl>(this List<TEl> list, int index)
		=> list != null && index >= 0 && index < list.Count
			? list[index]
			: throw new Exception($"list missing index {index}");

	/// <summary>returns first element, or fallback if list null/empty</summary>
	public static TEl FirstOr<TEl>(this List<TEl> list, TEl fallback = default)
		=> list != null && list.Count > 0
			? list[0]
			: fallback;

	/// <summary>returns last element, or fallback if list null/empty</summary>
	public static TEl LastOr<TEl>(this List<TEl> list, TEl fallback = default)
		=> list != null && list.Count > 0
			? list[list.Count - 1]
			: fallback;


	/// <summary>returns first element that passes predicate, or fallback</summary>
	public static TEl FirstOr<TEl>(
		this List<TEl> list,
		Func<TEl, bool> fn,
		TEl fallback = default
	)
	{
		if (list == null) return fallback;
		if (list.Count == 0) return fallback;
		foreach (var el in list) {
			if (fn(el)) return el;
		}

		return fallback;
	}

	/// <summary>returns first element that passes predicate, or fallback</summary>
	public static TEl FirstOr<TEl, TData>(
		this List<TEl> list,
		TData data,
		Func<TData, TEl, bool> fn,
		TEl fallback = default
	)
	{
		if (list == null) return fallback;
		if (list.Count == 0) return fallback;
		foreach (var el in list) {
			if (fn(data, el)) return el;
		}

		return fallback;
	}

	/// <summary>indexOf but searches from end</summary>
	public static int IndexOfReverse<TEl>(this List<TEl> list, TEl el)
	{
		for (var i = list.Count - 1; i >= 0; i--) {
			if (Equals(el, list[i])) return i;
		}

		return -1;
	}

	/// <summary>indexOf but start at an index (skips anything before)</summary>
	public static int IndexOf<TEl>(this List<TEl> list, TEl el, int startAt)
	{
		for (var i = startAt; i < list.Count; i++) {
			if (Equals(el, list[i])) return i;
		}

		return -1;
	}

	/// <summary>indexOf OR if fail: add it</summary>
	public static int IndexOrAdd<TEl>(this List<TEl> list, TEl el)
	{
		var index = list.IndexOf(el);
		if (index >= 0) return index;
		list.Add(el);
		return list.Count - 1;
	}

	/// <summary>returns first (element, index) that passes predicate, or (fallback, -1) if none</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static (TEl el, int index) Find<TEl, TData>(
		this List<TEl> list,
		TData data,
		Func<TData, TEl, bool> fn,
		TEl fallback = default
	)
	{
		if (list != null) {
			for (var index = 0; index < list.Count; index++) {
				var el = list[index];
				if (fn(data, el)) return (el, index);
			}
		}

		return (fallback, -1);
	}

	/// <summary>You know the law: LIST`FOO` ENTERS, ONE FOO LEAVES!</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static TEl ThunderDome<TEl>(
		this List<TEl> list,
		Func<TEl, TEl, TEl> fnFight
	)
	{
		if (list == null || list.Count == 0) throw new Exception("list needs 1+ elements");
		if (list.Count == 1) return list[0];

		var winner = list[0];
		for (var i = 1; i < list.Count; i++) {
			winner = fnFight(winner, list[i]);
		}

		return winner;
	}

	/// <summary>You know the law: LIST`FOO` ENTERS, ONE FOO LEAVES!</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static TEl ThunderDome<TEl>(
		this List<TEl> list,
		Func<TEl, TEl, TEl> fnFight,
		TEl fallback
	)
	{
		if (list == null) return fallback;
		if (list.Count == 0) return fallback;
		if (list.Count == 1) return list[0];

		var winner = list[0];
		for (var i = 1; i < list.Count; i++) {
			winner = fnFight(winner, list[i]);
		}

		return winner;
	}

	/// TODO: UNTESTED
	/// <summary>You know the law: LIST`FOO` ENTERS, ONE FOO LEAVES!</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static (TEl winner, TVal val) ThunderDome<TEl, TVal>(
		this List<TEl> list,
		Func<TEl, TVal> fnVal,
		Func<(TEl, TVal), (TEl, TVal), (TEl, TVal)> fnFight,
		(TEl, TVal) fallback = default
	)
		where TEl : class
	{
		if (list == null) return fallback;
		if (list.Count == 0) return fallback;

		var first = list[0];
		var result = (first, fnVal(first));

		for (var i = 1; i < list.Count; i++) {
			var next = list[i];
			result = fnFight(result, (next, fnVal(next)));
		}

		return result;
	}

	/// TODO: UNTESTED
	/// <summary>You know the law: LIST`FOO` ENTERS, ONE FOO LEAVES!</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static (TEl winner, TVal val) ThunderDome<TData, TEl, TVal>(
		this List<TEl> list,
		TData data,
		Func<TData, TEl, TVal> fnVal,
		Func<(TEl, TVal), (TEl, TVal), (TEl, TVal)> fnFight,
		(TEl, TVal) fallback = default
	)
		where TEl : class
	{
		if (list == null) return fallback;
		if (list.Count == 0) return fallback;

		var first = list[0];
		var result = (first, fnVal(data, first));

		for (var i = 1; i < list.Count; i++) {
			var next = list[i];
			result = fnFight(result, (next, fnVal(data, next)));
		}

		return result;
	}

	/// <summary>Add up val from each element (fnGetVal)</summary>
	/// <inheritdoc cref="NotesOnGarbageCollection"/>
	public static float Sum<TEl>(
		this List<TEl> list,
		Func<TEl, float> fnGetVal
	)
	{
		var result = 0f;
		foreach (var el in list) result += fnGetVal(el);
		return result;
	}

	/// <summary>Add up val from each element (fnGetVal)</summary>
	/// <inheritdoc cref="NotesOnGarbageCollection"/>
	public static float Sum<TData, TEl>(
		this List<TEl> list,
		TData data,
		Func<TData, TEl, float> fnGetVal
	)
	{
		var result = 0f;
		foreach (var el in list) result += fnGetVal(data, el);
		return result;
	}

	/// <summary>Add up val from each element (fnGetVal)</summary>
	/// <inheritdoc cref="NotesOnGarbageCollection"/>
	public static int Sum<TEl>(
		this List<TEl> list,
		Func<TEl, int> fnGetVal
	)
	{
		var result = 0;
		foreach (var el in list) result += fnGetVal(el);
		return result;
	}

	/// <summary>Add up val from each element (fnGetVal)</summary>
	/// <inheritdoc cref="NotesOnGarbageCollection"/>
	public static int Sum<TData, TEl>(
		this List<TEl> list,
		TData data,
		Func<TData, TEl, int> fnGetVal
	)
	{
		var result = 0;
		foreach (var el in list) result += fnGetVal(data, el);
		return result;
	}

	/// <summary>result = foreach: fn(result, el)</summary>
	/// <inheritdoc cref="NotesOnGarbageCollection"/>
	public static TVal Aggregate<TEl, TVal>(
		this List<TEl> list,
		Func<TVal, TEl, TVal> fn,
		TVal start
	)
	{
		var result = start;
		foreach (var el in list) result = fn(result, el);
		return result;
	}

	/// <summary>result = foreach: fn(result, el)</summary>
	/// <inheritdoc cref="NotesOnGarbageCollection"/>
	public static TVal Aggregate<TData, TEl, TVal>(
		this List<TEl> list,
		TData data,
		Func<TData, TVal, TEl, TVal> fn,
		TVal start
	)
	{
		var result = start;
		foreach (var el in list) result = fn(data, result, el);
		return result;
	}

	/// map val01 to list elements
	/// TODO: untested
	public static TEl Map01<TEl>(this List<TEl> list, float val01)
	{
		if (list == null) return default;

		var count = list.Count;
		if (count == 0) return default;

		var index = (int)Math.Floor(val01 * count);
		if (index < 0) index = 0;
		if (index >= 1) index = count - 1;

		return list[index];
	}
}

/// <remarks><b>Idealist.Randomization.Range</b> must be set to use this extension!</remarks>
public static class Randomization
{
	/// MUST BE SET! assumes: Range(minInclusive, maxExclusive)
	public static Func<int, int, int> Range = UnityEngine.Random.Range;
	// TODO: why hidden dep? maybe use ifdef
	// => throw new Exception("Idealist.Randomization.Range must be set to use this extension");

	/// <summary>gets random element</summary>
	/// <inheritdoc cref="Randomization"/>
	public static TEl GetRandom<TEl>(this List<TEl> list) => list[Range(0, list.Count)];

	/// <summary>-1: random any, 0: first, 5: random of first 5</summary>
	/// <inheritdoc cref="Randomization"/>
	public static TEl GetRandomUpTo<TEl>(this List<TEl> list, int upTo)
	{
		if (upTo <= -1) return list[Range(0, list.Count)];
		if (list.Count > upTo) upTo = list.Count;
		return list[Range(0, upTo)];
	}

	/// <summary>gets random element index</summary>
	/// <inheritdoc cref="Randomization"/>
	public static int GetRandomIndex<TEl>(this List<TEl> list) => Range(0, list.Count);

	/// <summary>gets random element and its index</summary>
	/// <inheritdoc cref="Randomization"/>
	public static (TEl element, int index) GetRandomAndIndex<TEl>(this List<TEl> list)
	{
		var index = Range(0, list.Count);
		return (list[index], index);
	}

	/// <summary>gets random index and its element</summary>
	/// <inheritdoc cref="Randomization"/>
	public static (int index, TEl element) GetRandomIndexVal<TEl>(this List<TEl> list)
	{
		var index = Range(0, list.Count);
		return (index, list[index]);
	}

	/// <summary>-1: random any, 0: first, 5: random of first 5</summary>
	/// <inheritdoc cref="Randomization"/>
	public static int GetRandomIndexUpTo<TEl>(this List<TEl> list, int upTo)
	{
		if (upTo <= -1) return Range(0, list.Count);
		if (list.Count > upTo) upTo = list.Count;
		return Range(0, upTo);
	}

	/// <summary>removes and returns random element</summary>
	/// <inheritdoc cref="Randomization"/>
	public static TEl GrabRandom<TEl>(this List<TEl> list) => list.GrabAt(Range(0, list.Count));

	/// TODO: needs testing!
	public static void Shuffle<TEl>(this List<TEl> list)
	{
		var count = list.Count;
		for (var i = 0; i < count; ++i) {
			var swapIndex = Range(0, count);
			(list[i], list[swapIndex]) = (list[swapIndex], list[i]); // I think this works?
		}
	}

	/// adds another list then shuffles full list
	public static void ShuffleAdd<TEl>(this List<TEl> list, List<TEl> other)
	{
		foreach (var el in other) {
			list.Add(el);
		}

		list.Shuffle();
	}

	/// <summary>for(i under max) Add(i), then shuffle list</summary>
	public static void ShuffleAddCount(this List<int> list, int max)
	{
		for (var i = 0; i < max; i++) {
			list.Add(i);
		}

		list.Shuffle();
	}
}

public static class Stringification
{
	const string COMMA_SPACE_DELIMITER = ", ";
	const string EMPTY_STRING = "";
	const string STRING_SAYS_EMPTY = "empty";
	const string LINE_DELIMITER = "\n";

	/// <summary>Builds string of elements.ToString() joined with delimiter</summary>
	/// <remarks>GARBAGE: returned string (uses System.Text.StringBuilder)</remarks>
	public static string Join<T>(
		this List<T> list,
		string delimiter = COMMA_SPACE_DELIMITER,
		string emptyReturns = EMPTY_STRING
	)
	{
		if (list.Count == 0) return emptyReturns;

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

	/// <summary>Builds string of fn(element) joined with delimiter</summary>
	/// <remarks>GARBAGE: returned string (uses System.Text.StringBuilder)</remarks>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static string Join<T>(
		this List<T> list,
		Func<T, string> fn,
		string delimiter = COMMA_SPACE_DELIMITER,
		string emptyReturns = EMPTY_STRING
	)
	{
		if (list.Count == 0) return emptyReturns;

		var builder = new StringBuilder();
		var lastIndex = list.Count - 1;

		for (var index = 0; index <= lastIndex; index++) {
			builder.Append(fn(list[index]));
			if (index < lastIndex) {
				builder.Append(delimiter);
			}
		}

		return builder.ToString();
	}

	/// <summary>Builds string of elements.ToString() joined with delimiter</summary>
	/// <remarks>GARBAGE: returned string (uses System.Text.StringBuilder)</remarks>
	public static string JoinPrefix<T>(
		this List<T> list,
		string prefix,
		string delimiter = COMMA_SPACE_DELIMITER,
		string emptyReturns = STRING_SAYS_EMPTY
	)
	{
		if (list.Count == 0) return $"{prefix} {emptyReturns}";

		var builder = new StringBuilder();
		builder.Append(prefix);

		var lastIndex = list.Count - 1;

		for (var index = 0; index < lastIndex; index++) {
			builder.Append(list[index]);
			builder.Append(delimiter);
		}

		builder.Append(list[lastIndex]);

		return builder.ToString();
	}

	/// <summary>Builds string of elements.ToString() joined with delimiter</summary>
	/// <remarks>GARBAGE: returned string (uses System.Text.StringBuilder)</remarks>
	public static string JoinPrefix<T>(
		this List<T> list,
		string prefix,
		Func<T, string> fn,
		string delimiter = COMMA_SPACE_DELIMITER,
		string emptyReturns = STRING_SAYS_EMPTY
	)
	{
		if (list.Count == 0) return $"{prefix} {emptyReturns}";

		var builder = new StringBuilder();
		builder.Append(prefix).Append(" ");

		var lastIndex = list.Count - 1;

		for (var index = 0; index < lastIndex; index++) {
			builder.Append(fn(list[index]));
			builder.Append(delimiter);
		}

		builder.Append(fn(list[lastIndex]));

		return builder.ToString();
	}

	/// <summary>Builds string of elements.ToString() joined with "\n" delimiter</summary>
	/// <remarks>GARBAGE: returned string (uses System.Text.StringBuilder)</remarks>
	public static string LineJoin<T>(
		this List<T> list,
		string emptyReturns = EMPTY_STRING
	)
		=> Join(list, LINE_DELIMITER, emptyReturns);


	public static void SplitInto<T>(
		this string str,
		List<T> into,
		Func<string, T> fnParse,
		bool parseEmpties = true,
		char delimiter = ',',
		bool clearDestinationList = false
	)
	{
		if (clearDestinationList) into.Clear();
		if (string.IsNullOrEmpty(str)) return; //>> empty string
		var strLength = str.Length;
		var iStart = 0;

		for (var i = 0; i < strLength; i++) {
			var curr = str[i];
			if (curr != delimiter) continue; //>> not delimiter

			if (i == iStart) {
				if (parseEmpties) into.Add(fnParse(string.Empty));
				iStart = i + 1;
				continue; //>> two delimiters in a row
			}

			into.Add(fnParse(str[iStart..i]));
			iStart = i + 1;
		}

		if (iStart == strLength) return; //>> ends with delimiter 

		into.Add(fnParse(str[iStart..]));
	}

	static Func<string, int> fnParseIndex = static s => int.TryParse(s, out var i) ? i : -1;

	/// "1,3,5,bad" => list.Add(1, 3, 5, -1)
	public static void SplitIndexesInto(
		this string str,
		List<int> into,
		bool parseEmpties = true,
		char delimiter = ',',
		bool clearDestinationList = false
	)
		=> str.SplitInto(into, fnParseIndex, parseEmpties, delimiter, clearDestinationList);

	public static void SplitEach<TData>(
		this string str,
		TData data,
		Action<TData, string> fn,
		bool callEachEmpty = true,
		char delimiter = ','
	)
	{
		if (string.IsNullOrEmpty(str)) return; //>> empty string
		var strLength = str.Length;
		var iStart = 0;

		for (var i = 0; i < strLength; i++) {
			var curr = str[i];
			if (curr != delimiter) continue; //>> not delimiter

			if (i == iStart) {
				if (callEachEmpty) fn(data, "");
				iStart = i + 1;
				continue; //>> two delimiters in a row
			}

			fn(data, str[iStart..i]);
			iStart = i + 1;
		}

		if (iStart == strLength) return; //>> ends with delimiter 

		fn(data, str[iStart..]);
	}
}

// public static class Misc {
//
//
// 	/// <remarks>GARBAGE: allocates new list </remarks>
// 	public static List<TEl> ToList<TEl>(this TEl[] array) {
// 		
// 	}
// }

/// <list type="bullet">
/// 	<listheader><b>TLDR</b></listheader>
/// 	<item>
///			<c>When using delegate (Action or Func), use static lambda to prevent a closure.</c>
///		</item>
/// 	<item>
///			<c>Use optional passthrough data instead of a closure.</c>
///		</item>
/// </list>
/// <list type="bullet">
/// 	<listheader><b>GOOD (allocates once)</b></listheader>
/// 	<item>
///			<c>.Each( cachedAction )</c>
///		</item>
/// 	<item>
///			<c>.Each( data, (data, el) => data.etc = el) )</c>
///		</item>
/// 	<item>
///			<c>.Each( (thing: foo, other: bar), (data, el) => data.etc = data.thing) )</c>
///		</item>
/// 	<item>
///			<b><c>.Each( data, static (data, el) => data.etc = el) )</c> <i>// ensure no closure</i></b>
///		</item>
/// 	<item>
///			<c>.Each( (el) => StaticFunction(el) )</c> <i>// yeah I know</i>
///		</item>
/// </list>
/// <list type="bullet">
/// 	<listheader><b>BAD (always allocates)</b></listheader>
/// 	<item>
///			<c>.Each( (el) => capturedVar.etc = el )</c> <i>// due to closure</i>
///		</item>
/// 	<item>
///			<c>.Each( InstanceFunction )</c> <i>// due to closure</i>
///		</item>
/// 	<item>
///			<c>.Each( (el) => InstanceFunction(el) )</c> <i>// due to closure</i>
///		</item>
/// 	<item>
///			<c>.Each( StaticFunction )</c> <i>// due to... uh, compiler bug?</i>
///		</item>
/// </list>
public static class NotesOnGarbageCollection { }
}