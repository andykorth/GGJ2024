using System;
using System.Collections.Generic;
using Swoonity.CSharp;

namespace Swoonity.Collections
{
public static class QueueUtils
{
	public static int Drain<TVal>(this Queue<TVal> queue, Action<TVal> fn, int max = -1)
	{
		var iters = max >= 0 ? queue.Count.OrMax(max) : queue.Count;

		for (var i = 0; i < iters; i++) {
			fn(queue.Dequeue());
		}

		return iters;
	}
}
}