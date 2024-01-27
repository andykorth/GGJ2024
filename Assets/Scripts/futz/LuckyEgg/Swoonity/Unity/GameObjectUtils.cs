using System;
using System.Collections.Generic;
using Swoonity.Collections;
using UnityEngine;

namespace Swoonity.Unity
{
public static class GameObjectUtils
{
	public static GameObject NewChild(this GameObject gobj, string name)
	{
		var newTf = new GameObject(name).transform;
		newTf.SetParentAndReset(gobj.transform);
		return newTf.gameObject;
	}

	public static T GetOrAdd<T>(this GameObject gobj) where T : Component
	{
		var c = gobj.GetComponent<T>();
		return c == null ? gobj.AddComponent<T>() : c;
	}

	public static List<T> GetInChildren<T>(this GameObject comp)
		where T : Component
	{
		var list = new List<T>();
		comp.GetComponentsInChildren(list);
		return list;
	}

	/// Adds component (type), then casts to T
	/// ?? for abstraction I guess ??
	public static T AddComponentAs<T>(this GameObject gobj, Type componentType)
		where T : MonoBehaviour
		=> (T)gobj.AddComponent(componentType);

	/// Gets component (type), then casts to T
	public static T GetComponentAs<T>(this GameObject gobj, Type componentType)
		where T : MonoBehaviour
		=> (T)gobj.GetComponent(componentType);

	/// Gets component (type), then casts to T
	public static T GetOrAddComponentAs<T>(this GameObject gobj, Type componentType)
		where T : MonoBehaviour
		=> (T)gobj.GetComponent(componentType)
		?? (T)gobj.AddComponent(componentType);


	/// GetComponent on each gameObject, returning list of valid (non-null) components
	/// GC: list returned 
	public static List<TComp> MapToValidComponents<TComp>(this GameObject[] gos)
	{
		var list = new List<TComp>();
		foreach (var go in gos) {
			var comp = go.GetComponent<TComp>();
			if (comp != null) list.Add(comp);
		}

		return list;
	}


	// TODO: cleanup


	/// <summary>
	/// Do callback on gameobject if it has the component
	/// </summary>
	public static void DoIfHas<T>(this GameObject go, Action<T> callback) where T : Component
	{
		var component = go.GetComponent<T>();
		if (component != null) {
			callback(component);
		}
	}

	/// <summary>
	/// Do callback on gameobject if it or a parent has the component
	/// </summary>
	public static void DoIfParentHas<T>(this GameObject go, Action<T> callback)
		where T : Component
	{
		var component = go.GetComponentInParent<T>();
		if (component != null) {
			callback(component);
		}
	}

	public static T GetComponentSafe<T>(this GameObject go) where T : Component
	{
		return go != null
			? go.GetComponent<T>()
			: null;
	}
}
}