using System;
using System.Collections.Generic;
using Swoonity.MHasher;

namespace Swoonity.Collections
{
[Serializable]
public class MHashLup<TVal> : Lup<MHash, TVal> where TVal : IMHashable
{
	public MHashLup(int initialSize = 0) : base(initialSize) { }

	public void Set(TVal val) => Set(val.GetHash(), val);
	public void Cut(TVal val) => Cut(val.GetHash());

	public void AddRange(List<TVal> list)
	{
		foreach (var val in list) {
			Set(val);
		}
	}

	public void AddRange(TVal[] array)
	{
		foreach (var val in array) {
			Set(val);
		}
	}
}
}