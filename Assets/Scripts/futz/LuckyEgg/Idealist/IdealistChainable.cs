namespace Idealist
{
using System;
using System.Collections.Generic;

// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

// TODO: this isn't done yet

namespace Idealist
{
// TODO: note there is no null checking (intentional)

public static class Iteration
{
	/// <summary>Call action on each</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static List<TEl> _Each<TEl>(
		this List<TEl> list,
		Action<TEl> fn
	)
	{
		foreach (var el in list) fn(el);
		return list;
	}

	/// <summary>Call action on each</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static List<TEl> _Each<TEl>(
		this List<TEl> list,
		Action<TEl, int> fn
	)
	{
		for (var i = 0; i < list.Count; i++) fn(list[i], i);
		return list;
	}

	/// <summary>Call action on each</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	///	<inheritdoc cref="Each"/>
	public static List<TEl> _Each<TData, TEl>(
		this List<TEl> list,
		TData data,
		Action<TData, TEl> fn
	)
	{
		foreach (var el in list) fn(data, el);
		return list;
	}

	/// <summary>Call action on each</summary>
	///	<inheritdoc cref="NotesOnGarbageCollection"/>
	public static List<TEl> _Each<TData, TEl>(
		this List<TEl> list,
		TData data,
		Action<TData, TEl, int> fn
	)
	{
		for (var i = 0; i < list.Count; i++) fn(data, list[i], i);
		return list;
	}
}

/// <summary>Asdf</summary>
///	<inheritdoc cref="NotesOnGarbageCollection"/>
public static class Alteration
{
	public static List<TEl> _Add<TEl>(this List<TEl> list, TEl el)
	{
		list.Add(el);
		return list;
	}

	public static List<TEl> _AddFront<TEl>(this List<TEl> list, TEl el)
	{
		list.Insert(0, el);
		return list;
	}

	public static List<TEl> _AddIf<TEl>(this List<TEl> list, TEl el, Func<TEl, bool> fn)
	{
		if (fn(el)) list.Add(el);
		return list;
	}

	public static List<TEl> _AddIf<TArg1, TEl>(
		this List<TEl> list,
		TEl el,
		TArg1 arg1,
		Func<TArg1, TEl, bool> fn
	)
	{
		if (fn(arg1, el)) list.Add(el);
		return list;
	}

	public static List<TEl> _Remove<TEl>(this List<TEl> list, TEl el)
	{
		list.Remove(el);
		return list;
	}

	public static List<TEl> _RemoveAt<TEl>(this List<TEl> list, int index)
	{
		list.RemoveAt(index);
		return list;
	}


	public static List<TEl> _Asdf<TEl>(this List<TEl> list)
	{
		return list;
	}

	/// all kinds of garbage, only use at edit time!
	public static List<TEl> _Sort<TEl>(this List<TEl> list, Func<TEl, TEl, int> fnCompare)
	{
		list.Sort((a, b) => fnCompare(a, b));
		return list;
	}
}

/// <summary>Asdf</summary>
///	<inheritdoc cref="NotesOnGarbageCollection"/>
public static class Introspection
{
	public static List<TEl> _NewIfNull<TEl>(this List<TEl> list) => list ?? new List<TEl>();
}

// public static class NullChecking {
// 	/// not null and not empty
// 	public static List<TEl> _NullCheck<TEl>(this List<TEl> list) => list ?? EmptyListStorage<TEl>.EMPTY;
// }
//
// // I don't like this because there's nothing ensuring EMPTY stays empty
// public static class EmptyListStorage<TEl> {
// 	public static readonly List<TEl> EMPTY = new List<TEl>();
// }

// /// <inheritdoc cref="Iteration"/>
// public static List<TEl> _Each<TArg1, TArg2, TEl>(
// 	this List<TEl> list,
// 	TArg1 arg1,
// 	TArg2 arg2,
// 	Action<TArg1, TArg2, TEl> fn
// ) {
// 	foreach (var el in list) fn(arg1, arg2, el);
// 	return list;
// }
//
// /// <inheritdoc cref="Iteration"/>
// public static List<TEl> _Each<TArg1, TArg2, TArg3, TEl>(
// 	this List<TEl> list,
// 	TArg1 arg1,
// 	TArg2 arg2,
// 	TArg3 arg3,
// 	Action<TArg1, TArg2, TArg3, TEl> fn
// ) {
// 	foreach (var el in list) fn(arg1, arg2, arg3, el);
// 	return list;
// }
}
}