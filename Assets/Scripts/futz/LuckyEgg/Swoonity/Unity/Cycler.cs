using System;
using System.Collections.Generic;
using Swoonity.CSharp;

namespace Swoonity.Unity
{
[Serializable]
public class Cycler
{
	public int Current;

	public int Min;
	public int Max;

	public Cycler(int min = 0, int max = int.MaxValue, int current = 0)
	{
		_Set(current, min, max);
	}


	int _Set(int val)
	{
		Current = val.Wrap(Min, Max);
		return Current;
	}

	int _Set(int val, int max)
	{
		Max = max;
		return Set(val);
	}

	int _Set(int val, int min, int max)
	{
		Min = min;
		Max = max;
		return Set(val);
	}


	#region API

	public int Set(int val) => _Set(val);
	public int Set(int val, int max) => _Set(val, Min, max);
	public int Set(int val, int min, int max) => _Set(val, min, max);

	public int SetMinMax(int min, int max) => _Set(Current, min, max);
	public int SetMin(int min) => _Set(Current, min, Max);
	public int SetMax(int max) => _Set(Current, Min, max);

	public int Cycle(int by = 1) => _Set(Current + by);
	public int Forward(int add = 1) => _Set(Current + add);
	public int Back(int subtract = 1) => _Set(Current - subtract);

	/// cycles through a list
	public T Cycle<T>(List<T> list, int by = 1) => list[_Set(Current + by, list.Count)];

	/// cycles a list, until predicate passes (or it checked all)
	public T Cycle<T>(List<T> list, Func<T, bool> predicate, int by = 1)
	{
		_Set(Current, list.Count);
		for (var i = 0; i < Max; i++) {
			var next = list[_Set(Current + by)];
			if (predicate(next)) return next;
		}

		return default;
	}

	/// cycles through indexes of a list
	public int CycleIndex<T>(List<T> list, int by = 1) => _Set(Current + by, list.Count);

	#endregion
}
}