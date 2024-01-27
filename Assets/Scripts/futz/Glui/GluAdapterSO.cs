// using System;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using UnityEngine.UIElements;
// using Vel = UnityEngine.UIElements.VisualElement;
//
// namespace Glui
// {
// // TODO: using this or not?
// /// Adapter: pool & linking between C# obj and visual element
// [Serializable]
// public abstract class GluAdapterSO<T> : ScriptableObject where T : IGluPoolableSO<T>, new()
// {
// 	[Header("Config")]
// 	public VisualTreeAsset ElementTemplate;
//
// 	Stack<T> _pool = new();
//
// 	public void Premake(int count)
// 	{
// 		for (var i = 0; i < count; i++) {
// 			_pool.Push(MakeNew());
// 		}
// 	}
//
// 	T MakeNew()
// 	{
// 		var obj = new T();
// 		var root = ElementTemplate.Instantiate()
// 		   .Children()
// 		   .FirstOrDefault(); // to get around Unity's stupid TemplateContainer 
//
// 		obj.InitRefs(this, root);
// 		return obj;
// 	}
//
// 	public T Take() => _pool.Count > 0 ? _pool.Pop() : MakeNew();
//
// 	public T TakeAndAddTo(Vel parent)
// 	{
// 		var obj = Take();
// 		parent.Add(obj.GetRoot());
// 		return obj;
// 	}
//
// 	public void Release(T obj)
// 	{
// 		obj.GetRoot().RemoveFromHierarchy();
// 		_pool.Push(obj);
// 		obj.AfterRelease();
// 	}
//
// 	public void Clear() => _pool.Clear();
//
// 	/// releases each obj in list and clears list
// 	public void ReleaseAll(List<T> list)
// 	{
// 		foreach (var obj in list) {
// 			Release(obj);
// 		}
//
// 		list.Clear();
// 	}
//
// 	public void TryRelease(T obj) => obj?.Release();
// }
//
// public interface IGluPoolableSO<T> where T : IGluPoolableSO<T>, new()
// {
// 	public void InitRefs(GluAdapterSO<T> adapter, VisualElement root);
// 	public VisualElement GetRoot();
// 	public void Release();
// 	public void AfterRelease();
// }
//
// public static class AdapterSOUtils
// {
// 	/// releases each obj in list and clears list
// 	public static void ReleaseAll<T>(this List<T> list)
// 		where T : IGluPoolableSO<T>, new()
// 	{
// 		foreach (var obj in list) {
// 			obj.Release();
// 		}
//
// 		list.Clear();
// 	}
//
// 	public static void Add<T>(this Vel container, IGluPoolableSO<T> el)
// 		where T : IGluPoolableSO<T>, new()
// 	{
// 		container.Add(el.GetRoot());
// 	}
//
// 	public static void AddTo<T>(this IGluPoolableSO<T> el, Vel container)
// 		where T : IGluPoolableSO<T>, new()
// 	{
// 		container.Add(el.GetRoot());
// 	}
//
// 	public static void TryAdd<T>(this Vel container, IGluPoolableSO<T> el)
// 		where T : IGluPoolableSO<T>, new()
// 	{
// 		if (el != null) container.Add(el.GetRoot());
// 	}
// }
// }