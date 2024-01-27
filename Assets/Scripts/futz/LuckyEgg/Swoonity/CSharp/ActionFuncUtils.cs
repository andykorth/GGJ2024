using System;
using System.Collections.Generic;

namespace Swoonity.CSharp
{
public static class ActionFuncUtils
{
	public static TR Try<TR>(this Func<TR> fn) => fn != null ? fn() : default;
	public static TR Try<T1, TR>(this Func<T1, TR> fn, T1 t1) => fn != null ? fn(t1) : default;


	/// Invokes an action if it's not null
	public static void Try(this Action action)
	{
		if (action != null) {
			action();
		}
	}

	/// Invokes an action if it's not null
	public static void Try<T>(this Action<T> action, T arg1)
	{
		if (action != null) {
			action(arg1);
		}
	}

	/// Invokes an action if it's not null
	public static void Try<T, U>(this Action<T, U> action, T arg1, U arg2)
	{
		if (action != null) {
			action(arg1, arg2);
		}
	}

	/// Invokes an action if it's not null
	public static void Try<T, U, V>(this Action<T, U, V> action, T arg1, U arg2, V arg3)
	{
		if (action != null) {
			action(arg1, arg2, arg3);
		}
	}

	public static void InvokeAll(this List<Action> list)
	{
		foreach (var action in list) {
			action();
		}
	}

	public static void InvokeAll<T1>(this List<Action<T1>> list, T1 value1)
	{
		foreach (var action in list) {
			action(value1);
		}
	}

	public static void InvokeAll<T1, T2>(this List<Action<T1, T2>> list, T1 value1, T2 value2)
	{
		foreach (var action in list) {
			action(value1, value2);
		}
	}


	/// Checks conditional, or if conditional is null, returns ifNull (default: false)
	public static bool Try(this Func<bool> conditional, bool ifNull = false)
		=> conditional?.Invoke() ?? ifNull;

	/// Checks conditional, or if conditional is null, returns ifNull (default: false)
	public static bool Try<T1>(this Func<T1, bool> conditional, T1 arg1, bool ifNull = false)
		=> conditional?.Invoke(arg1) ?? ifNull;

	/// Checks conditional, or if conditional is null, returns ifNull (default: false)
	public static bool Try<T1, T2>(
		this Func<T1, T2, bool> conditional,
		T1 arg1,
		T2 arg2,
		bool ifNull = false
	)
		=> conditional?.Invoke(arg1, arg2) ?? ifNull;

	/// Checks conditional, or if conditional is null, returns ifNull (default: false)
	public static bool Try<T1, T2, T3>(
		this Func<T1, T2, T3, bool> conditional,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		bool ifNull = false
	)
		=> conditional?.Invoke(arg1, arg2, arg3) ?? ifNull;
}
}