using System;
using System.Collections.Generic;
using UnityEngine;

namespace PoolBoyPooling
{
//
//
//
// TODO: use Pooler
//
//
//


public class SimplePoolBoy<T>
{
	readonly Stack<T> _pool;
	readonly Func<T> _fnMakeNew;
	readonly int _limit;

	public SimplePoolBoy(Func<T> fnMakeNew, int limit = int.MaxValue, int premake = 0)
	{
		_fnMakeNew = fnMakeNew;
		_limit = limit;
		_pool = new Stack<T>(premake);

		for (var i = 0; i < premake; i++) {
			_pool.Push(_fnMakeNew());
		}
	}

	public T Take() => _pool.Count > 0 ? _pool.Pop() : _fnMakeNew();

	public void Release(T obj)
	{
		_pool.Push(obj);
	}
}

[Serializable]
public class StatPoolBoy<T> where T : new()
{
	[Header("Config")]
	public int PremakeCount = 0;

	[Header("State")]
	public int Created;
	public int Used;
	public int Available;

	Stack<T> _pool = new();

	public Func<T> FnInstantiate = () => new T();

	public void Initialize(Func<T> fnInstantiate = null)
	{
		if (fnInstantiate != null) FnInstantiate = fnInstantiate;

		Premake(PremakeCount);
	}

	public T Take()
	{
		++Used;

		if (Available > 0) {
			--Available;
			return _pool.Pop();
		}

		++Created;
		return FnInstantiate();
	}

	public void Release(T obj)
	{
		_pool.Push(obj);
		--Used;
		++Available;
	}

	public void Premake(int count)
	{
		Created += count;
		Available += count;
		for (var i = 0; i < count; i++) {
			_pool.Push(FnInstantiate());
		}
	}
}


[Serializable]
public class AdvancedPoolBoy<T> where T : new()
{
	[Header("Config")]
	public int Prealloc = 0;

	[Header("State")]
	public int Created;
	public int Used;
	public int Available;

	Stack<T> _stack = new();

	public void Initialize(
		Func<T> fnInstantiate = null,
		Action<T> fnWhenTaken = null,
		Action<T> fnWhenReleased = null
	)
	{
		if (fnInstantiate != null) FnInstantiate = fnInstantiate;
		if (fnWhenTaken != null) FnWhenTaken = fnWhenTaken;
		if (fnWhenReleased != null) FnWhenReleased = fnWhenReleased;

		Created = Prealloc;
		Available = Prealloc;
		for (var i = 0; i < Prealloc; i++) {
			_stack.Push(FnInstantiate());
		}
	}


	public Func<T> FnInstantiate = () => new T();
	public Action<T> FnWhenTaken = _ => { };
	public Action<T> FnWhenReleased = _ => { };

	public T Take()
	{
		T obj;

		if (Available > 0) {
			--Available;
			obj = _stack.Pop();
		}
		else {
			++Created;
			obj = FnInstantiate();
		}

		++Used;
		FnWhenTaken(obj);
		return obj;
	}

	public void Release(T obj)
	{
		FnWhenReleased(obj);
		_stack.Push(obj);
		--Used;
		++Available;
	}
}
}