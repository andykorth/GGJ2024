using System;
using System.Collections.Generic;
using UnityEngine;

namespace Swoonity.Unity
{
public static class TreeUtils
{
	public static void Traverse<TData>(
		this Transform tf,
		TData data,
		Action<TData, Transform, int, int> fn,
		int currentDepth = 0
	)
	{
		var index = 0;

		foreach (Transform child in tf) {
			fn(data, child, currentDepth, index);
			child.Traverse(data, fn, currentDepth + 1);
			++index;
		}
	}

	public static void Traverse<TData>(
		this Transform tf,
		TData data,
		Action<TData, Transform> fn
	)
	{
		foreach (Transform child in tf) {
			fn(data, child);
			child.Traverse(data, fn);
		}
	}

	public static void Traverse(
		this Transform tf,
		Action<Transform, int, int> fn,
		int currentDepth = 0
	)
	{
		var index = 0;

		foreach (Transform child in tf) {
			fn(child, currentDepth, index);
			child.Traverse(fn, currentDepth + 1);
			++index;
		}
	}

	public static void Traverse(
		this Transform tf,
		Action<Transform> fn
	)
	{
		foreach (Transform child in tf) {
			fn(child);
			child.Traverse(fn);
		}
	}

	public static void Traverse<T>(
		T el,
		Func<T, List<T>> fnGetChildren,
		Action<(T parent, T el, int elIndex)> fnAction,
		T parent = default,
		int elIndex = -1
	)
	{
		fnAction((parent, el, elIndex));

		var children = fnGetChildren(el);
		if (children == null) return;

		for (var index = 0; index < children.Count; index++) {
			Traverse(
				children[index],
				fnGetChildren,
				fnAction,
				el,
				index
			);
		}
	}

	public static void Traverse<TEl, TData>(
		TEl el,
		Func<TEl, List<TEl>> fnGetChildren,
		TData data,
		Action<(TData data, TEl parent, TEl el, int elIndex)> fnAction,
		TEl parent = default,
		int elIndex = -1
	)
	{
		fnAction((data, parent, el, elIndex));

		var children = fnGetChildren(el);
		if (children == null) return;

		for (var index = 0; index < children.Count; index++) {
			Traverse(
				children[index],
				fnGetChildren,
				data,
				fnAction,
				el,
				index
			);
		}
	}
}
}