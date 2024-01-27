using System.Collections.Generic;

namespace Idealist.Tests
{
public static class TestIteration
{
	public static void Test1(List<int> list, List<(string, int)> logs)
	{
		list.Each(logs, static (logs, el) => logs.Add(("iter", el)));

		list.Each(
			(5, 7),
			(tup, ele) => { var (five, seven) = tup; }
		);

		// var tup = (a: 5, b: 7);

		list.Each(
			(a: 5, b: 7),
			(data, ele) => {
				var a = data.a;
			}
		);

		list.Each(
			(five: 5, two: 2),
			(data, ele) => {
				var asdf = data.five;
			}
		);

		list.Each(
			(five: 5, two: 2),
			(data, ele) => { var (asdf, _) = data; }
		);
	}
}
}


// /// <inheritdoc cref="Iteration"/>
// public static List<TEl> Each<TEl, TD1>(
// 	this List<TEl> list,
// 	TD1 d1,
// 	Action<TD1, TEl> fn
// ) {
// 	foreach (var el in list) fn(d1, el);
// 	return list;
// }
//
// /// <inheritdoc cref="Iteration"/>
// public static List<TEl> Each<TEl, TD1, TD2>(
// 	this List<TEl> list,
// 	TD1 d1,
// 	TD2 d2,
// 	Action<TD1, TD2, TEl> fn
// ) {
// 	foreach (var el in list) fn(d1, d2, el);
// 	return list;
// }
//
// /// <inheritdoc cref="Iteration"/>
// public static List<TEl> Each<TEl, TD1, TD2, TD3>(
// 	this List<TEl> list,
// 	TD1 d1,
// 	TD2 d2,
// 	TD3 d3,
// 	Action<TD1, TD2, TD3, TEl> fn
// ) {
// 	foreach (var el in list) fn(d1, d2, d3, el);
// 	return list;
// }