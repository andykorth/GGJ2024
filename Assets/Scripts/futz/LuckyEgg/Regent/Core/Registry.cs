using System;
using System.Collections.Generic;
using Regent.Entities;
using Regent.WorkerFacts;

namespace Regent.Core
{
/// TODO: choose dictionary or List
/// register/deregister happens immediately (not staged, but after Enabled/Disabled workers)
[Serializable]
public class Registry<T> where T : IHasEntity
{
	// TODO: delete
	public static int TempInc;
	int _id = ++TempInc;
	public int Id => _id;

	Dictionary<int, T> _dict = new();

	public WorkerFact Fact;
	public int Count;
	// public int Count => _dict.Count;

	public List<T> Value = new();
	public event Action EventValueChanged;

	public void _Register(T el)
	{
		_dict[el.GetEntityId()] = el;
		Count = _dict.Count;

		Value.Add(el);
		EventValueChanged?.Invoke();
	}

	public void _Deregister(T el)
	{
		_dict.Remove(el.GetEntityId());
		Count = _dict.Count;

		Value.Remove(el);
		EventValueChanged?.Invoke();
	}

	// public void _Deregister(int id)
	// {
	// 	_dict.Remove(id);
	// 	Count = _dict.Count;
	//
	//
	// 	for (var dex = Values.Count - 1; dex >= 0; dex--) {
	// 		if (Values[dex] == null) Values.RemoveAt(dex);
	// 	}
	// 	EventValueChanged?.Invoke();
	// }

	public void Each(Action<T> fn)
	{
		foreach (var (_, el) in _dict) fn(el);
	}

	public void Each<TData>(TData data, Action<TData, T> fn)
	{
		foreach (var (_, el) in _dict) fn(data, el);
	}

	public Dictionary<int, T> GetDict() => _dict;

	public override string ToString()
	{
		return $"Registry{_id}";
	}

	// public static void RemoveAtFast(List<T> list, int index)
	// {
	// 	var last = list.Count - 1;
	// 	list[index] = list[last];
	// 	list.RemoveAt(last);
	// }
}
}