using System;
using System.Collections.Generic;
using Swoonity.Collections;
using UnityEngine;

namespace Swoonity.Unity
{
public static class ComponentUtils
{
	public static T AddComponent<T>(this Component comp) where T : Component
		=> comp.gameObject.AddComponent<T>();

	public static T GetOrAdd<T>(this Component comp) where T : Component
	{
		var go = comp.gameObject;
		var c = go.GetComponent<T>();
		return c == null ? go.AddComponent<T>() : c;
	}

	public static List<T> GetInChildren<T>(this Component comp)
		where T : Component
	{
		var list = new List<T>();
		comp.GetComponentsInChildren(list);
		return list;
	}

	/// GetComponentsInChildren<T> into list
	public static List<T> FillChildList<T>(this Component comp, List<T> list)
		where T : Component
	{
		comp.GetComponentsInChildren(list);
		return list;
	}


	/// Cancels any pending method invoke, then calls method invoke with delay
	public static void Reinvoke(this MonoBehaviour mono, string methodName, float delay = 0)
	{
		mono.CancelInvoke(methodName);
		mono.Invoke(methodName, delay);
	}

	/// Calls method invoke with delay, optionally cancelling any pending invokes (default: true)
	public static void Invoke(
		this string methodName,
		MonoBehaviour on,
		float delay = 0,
		bool cancelExisting = true
	)
	{
		if (cancelExisting) on.CancelInvoke(methodName);
		on.Invoke(methodName, delay);
	}

	/// <summary>
	/// If component is not null, return callback results
	/// </summary>
	public static bool CallIfValid<T>(this T component, Func<T, bool> callback)
		where T : MonoBehaviour
	{
		return component != null
		    && callback(component);
	}


	/// <summary>
	/// Faster than null check when you want to check if a local component variable has been assigned.
	/// (Unity overwrites `val == null` to check lifecycle of the underlying engine object)
	/// Do NOT use if component is external and could have been destroyed.
	/// </summary>
	/// <param name="comp"></param>
	/// <returns></returns>
	public static bool WasAssigned(this Component comp)
	{
		return !object.ReferenceEquals(comp, null);
	}

	/// <summary>
	/// Faster than null check when you want to check if a local component variable has been assigned.
	/// (Unity overwrites `val == null` to check lifecycle of the underlying engine object)
	/// Do NOT use if component is external and could have been destroyed.
	/// </summary>
	/// <param name="comp"></param>
	/// <returns></returns>
	public static bool IsUnassigned(this Component comp)
	{
		return object.ReferenceEquals(comp, null);
	}


	/// Calculates physics frame count (how many physics/FixedUpdate frames)
	public static int FixedFrameCount(this MonoBehaviour _)
		=> Mathf.RoundToInt(Time.fixedTime / Time.fixedDeltaTime);
}
}