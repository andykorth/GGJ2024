using System;
using Regent.Entities;
using uC = UnityEngine.Component;
using Result = Cysharp.Threading.Tasks.UniTask<sbyte>;

namespace Regent.Invokers
{
public static class WorkerInvokers
{
	public static void Call<T1>(this Entity entity, Action<T1> action)
		where T1 : uC
		=> action.Invoke(entity.GetOrThrow<T1>());

	public static void Call<T1, T2>(this Entity entity, Action<T1, T2> action)
		where T1 : uC
		where T2 : uC
		=> action.Invoke(
			entity.GetOrThrow<T1>(),
			entity.GetOrThrow<T2>()
		);

	public static void Call<T1, T2, T3>(this Entity entity, Action<T1, T2, T3> action)
		where T1 : uC
		where T2 : uC
		where T3 : uC
		=> action.Invoke(
			entity.GetOrThrow<T1>(),
			entity.GetOrThrow<T2>(),
			entity.GetOrThrow<T3>()
		);

	public static void Call<T1, T2, T3, T4>(this Entity entity, Action<T1, T2, T3, T4> action)
		where T1 : uC
		where T2 : uC
		where T3 : uC
		where T4 : uC
		=> action.Invoke(
			entity.GetOrThrow<T1>(),
			entity.GetOrThrow<T2>(),
			entity.GetOrThrow<T3>(),
			entity.GetOrThrow<T4>()
		);

	public static void Call<T1, T2, T3, T4, T5>(
		this Entity entity,
		Action<T1, T2, T3, T4, T5> action
	)
		where T1 : uC
		where T2 : uC
		where T3 : uC
		where T4 : uC
		where T5 : uC
		=> action.Invoke(
			entity.GetOrThrow<T1>(),
			entity.GetOrThrow<T2>(),
			entity.GetOrThrow<T3>(),
			entity.GetOrThrow<T4>(),
			entity.GetOrThrow<T5>()
		);

	public static Result Call<T1>(this Entity entity, Func<T1, Result> func)
		where T1 : uC
		=> func.Invoke(entity.GetOrThrow<T1>());

	public static Result Call<T1, T2>(this Entity entity, Func<T1, T2, Result> func)
		where T1 : uC
		where T2 : uC
		=> func.Invoke(
			entity.GetOrThrow<T1>(),
			entity.GetOrThrow<T2>()
		);

	public static Result Call<T1, T2, T3>(this Entity entity, Func<T1, T2, T3, Result> func)
		where T1 : uC
		where T2 : uC
		where T3 : uC
		=> func.Invoke(
			entity.GetOrThrow<T1>(),
			entity.GetOrThrow<T2>(),
			entity.GetOrThrow<T3>()
		);

	public static Result Call<T1, T2, T3, T4>(this Entity entity, Func<T1, T2, T3, T4, Result> func)
		where T1 : uC
		where T2 : uC
		where T3 : uC
		where T4 : uC
		=> func.Invoke(
			entity.GetOrThrow<T1>(),
			entity.GetOrThrow<T2>(),
			entity.GetOrThrow<T3>(),
			entity.GetOrThrow<T4>()
		);

	public static Result Call<T1, T2, T3, T4, T5>(
		this Entity entity,
		Func<T1, T2, T3, T4, T5, Result> func
	)
		where T1 : uC
		where T2 : uC
		where T3 : uC
		where T4 : uC
		where T5 : uC
		=> func.Invoke(
			entity.GetOrThrow<T1>(),
			entity.GetOrThrow<T2>(),
			entity.GetOrThrow<T3>(),
			entity.GetOrThrow<T4>(),
			entity.GetOrThrow<T5>()
		);
}
}