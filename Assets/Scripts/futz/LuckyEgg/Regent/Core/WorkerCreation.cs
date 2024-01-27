using System;
using System.Reflection;
using Regent.Barons;
using Regent.Cogs;
using Regent.Entities;
using Regent.Invokers;
using Regent.Logging;
using Regent.WorkerFacts;
using Swoonity.Collections;
using Swoonity.CSharp;
using uC = UnityEngine.Component;
using static UnityEngine.Debug;

namespace Regent.Workers
{
/// Makers
public static class WorkerCreation
{
	#region Registry workers

	public static BaseWorker MakeRegistryWorker<T1>() where T1 : uC, IHasEntity
		=> RegistryWorker<T1>.Make();

	#endregion

	#region Added workers

	public static BaseWorker MakeAddedWorker() => throw new Exception($"Don't use this");

	public static BaseWorker MakeAddedWorker1<T1>(Action<T1> handler)
		where T1 : uC
		=> AddedWorker.Make(entity => entity.Call(handler));

	public static BaseWorker MakeAddedWorker2<T1, T2>(Action<T1, T2> handler)
		where T2 : uC
		where T1 : uC
		=> AddedWorker.Make(entity => entity.Call(handler));

	public static BaseWorker MakeAddedWorker3<T1, T2, T3>(Action<T1, T2, T3> handler)
		where T3 : uC
		where T2 : uC
		where T1 : uC
		=> AddedWorker.Make(entity => entity.Call(handler));

	public static BaseWorker MakeAddedWorker4<T1, T2, T3, T4>(Action<T1, T2, T3, T4> handler)
		where T4 : uC
		where T3 : uC
		where T2 : uC
		where T1 : uC
		=> AddedWorker.Make(entity => entity.Call(handler));

	public static BaseWorker MakeAddedWorker5<T1, T2, T3, T4, T5>(
		Action<T1, T2, T3, T4, T5> handler
	)
		where T5 : uC
		where T4 : uC
		where T3 : uC
		where T2 : uC
		where T1 : uC
		=> AddedWorker.Make(entity => entity.Call(handler));

	#endregion

	#region Run workers

	public static BaseWorker MakeRunWorker() => throw new Exception($"Don't use this");

	public static BaseWorker MakeRunWorker1<T1>(Action<T1> handler)
		where T1 : uC
		=> RunWorker.Make(entity => entity.Call(handler));

	public static BaseWorker MakeRunWorker2<T1, T2>(Action<T1, T2> handler)
		where T2 : uC
		where T1 : uC
		=> RunWorker.Make(entity => entity.Call(handler));

	public static BaseWorker MakeRunWorker3<T1, T2, T3>(Action<T1, T2, T3> handler)
		where T3 : uC
		where T2 : uC
		where T1 : uC
		=> RunWorker.Make(entity => entity.Call(handler));

	public static BaseWorker MakeRunWorker4<T1, T2, T3, T4>(Action<T1, T2, T3, T4> handler)
		where T4 : uC
		where T3 : uC
		where T2 : uC
		where T1 : uC
		=> RunWorker.Make(entity => entity.Call(handler));

	public static BaseWorker MakeRunWorker5<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> handler)
		where T5 : uC
		where T4 : uC
		where T3 : uC
		where T2 : uC
		where T1 : uC
		=> RunWorker.Make(entity => entity.Call(handler));

	public static BaseWorker MakeRunnWorker() => throw new Exception($"Don't use this");

	public static BaseWorker MakeRunnWorker1<T1>(Action<T1> handler)
		where T1 : uC
		=> RunnWorker<T1>.Make(
			static entity => entity.GetOrThrow<T1>(),
			handler
		);

	public static BaseWorker MakeRunnWorker2<T1, T2>(Action<T1, T2> handler)
		where T2 : uC
		where T1 : uC
		=> RunnWorker<(T1, T2)>.Make(
			static entity => (
				entity.GetOrThrow<T1>(),
				entity.GetOrThrow<T2>()
			),
			comps => {
				var (v1, v2) = comps;
				handler(v1, v2);
			}
		);

	public static BaseWorker MakeRunnWorker3<T1, T2, T3>(Action<T1, T2, T3> handler)
		where T3 : uC
		where T2 : uC
		where T1 : uC
		=> RunnWorker<(T1, T2, T3)>.Make(
			static entity => (
				entity.GetOrThrow<T1>(),
				entity.GetOrThrow<T2>(),
				entity.GetOrThrow<T3>()
			),
			comps => {
				var (v1, v2, v3) = comps;
				handler(v1, v2, v3);
			}
		);

	public static BaseWorker MakeRunnWorker4<T1, T2, T3, T4>(Action<T1, T2, T3, T4> handler)
		where T4 : uC
		where T3 : uC
		where T2 : uC
		where T1 : uC
		=> RunnWorker<(T1, T2, T3, T4)>.Make(
			static entity => (
				entity.GetOrThrow<T1>(),
				entity.GetOrThrow<T2>(),
				entity.GetOrThrow<T3>(),
				entity.GetOrThrow<T4>()
			),
			comps => {
				var (v1, v2, v3, v4) = comps;
				handler(v1, v2, v3, v4);
			}
		);

	public static BaseWorker MakeRunnWorker5<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> handler)
		where T5 : uC
		where T4 : uC
		where T3 : uC
		where T2 : uC
		where T1 : uC
		=> RunnWorker<(T1, T2, T3, T4, T5)>.Make(
			static entity => (
				entity.GetOrThrow<T1>(),
				entity.GetOrThrow<T2>(),
				entity.GetOrThrow<T3>(),
				entity.GetOrThrow<T4>(),
				entity.GetOrThrow<T5>()
			),
			comps => {
				var (v1, v2, v3, v4, v5) = comps;
				handler(v1, v2, v3, v4, v5);
			}
		);

	#endregion

	#region Removed workers

	public static BaseWorker MakeRemovedWorker(Action<int> handler) => RemovedWorker.Make(handler);

	#endregion

	#region Enabled workers

	public static BaseWorker MakeEnabledWorker() => throw new Exception($"Don't use this");

	public static BaseWorker MakeEnabledWorker1<T1>(Action<T1> handler)
		where T1 : uC
		=> EnabledWorker.Make(entity => entity.Call(handler));

	public static BaseWorker MakeEnabledWorker2<T1, T2>(Action<T1, T2> handler)
		where T2 : uC
		where T1 : uC
		=> EnabledWorker.Make(entity => entity.Call(handler));

	public static BaseWorker MakeEnabledWorker3<T1, T2, T3>(Action<T1, T2, T3> handler)
		where T3 : uC
		where T2 : uC
		where T1 : uC
		=> EnabledWorker.Make(entity => entity.Call(handler));

	public static BaseWorker MakeEnabledWorker4<T1, T2, T3, T4>(Action<T1, T2, T3, T4> handler)
		where T4 : uC
		where T3 : uC
		where T2 : uC
		where T1 : uC
		=> EnabledWorker.Make(entity => entity.Call(handler));

	public static BaseWorker MakeEnabledWorker5<T1, T2, T3, T4, T5>(
		Action<T1, T2, T3, T4, T5> handler
	)
		where T5 : uC
		where T4 : uC
		where T3 : uC
		where T2 : uC
		where T1 : uC
		=> EnabledWorker.Make(entity => entity.Call(handler));

	#endregion

	#region Disabled workers

	public static BaseWorker MakeDisabledWorker() => throw new Exception($"Don't use this");

	public static BaseWorker MakeDisabledWorker1<T1>(Action<T1> handler)
		where T1 : uC
		=> DisabledWorker.Make(entity => entity.Call(handler));

	public static BaseWorker MakeDisabledWorker2<T1, T2>(Action<T1, T2> handler)
		where T2 : uC
		where T1 : uC
		=> DisabledWorker.Make(entity => entity.Call(handler));

	public static BaseWorker MakeDisabledWorker3<T1, T2, T3>(Action<T1, T2, T3> handler)
		where T3 : uC
		where T2 : uC
		where T1 : uC
		=> DisabledWorker.Make(entity => entity.Call(handler));

	public static BaseWorker MakeDisabledWorker4<T1, T2, T3, T4>(Action<T1, T2, T3, T4> handler)
		where T4 : uC
		where T3 : uC
		where T2 : uC
		where T1 : uC
		=> DisabledWorker.Make(entity => entity.Call(handler));

	public static BaseWorker MakeDisabledWorker5<T1, T2, T3, T4, T5>(
		Action<T1, T2, T3, T4, T5> handler
	)
		where T5 : uC
		where T4 : uC
		where T3 : uC
		where T2 : uC
		where T1 : uC
		=> DisabledWorker.Make(entity => entity.Call(handler));

	#endregion

	#region All workers

	public static BaseWorker MakeAllWorker(Action<EntityLup> handler) => AllWorker.Make(handler);

	#endregion

	#region Verify workers

	public static BaseWorker MakeVerifyWorker() => throw new Exception($"Don't use this");

	public static BaseWorker MakeVerifyWorker2<TCog, TVal>(Func<TCog, TVal, sbyte> handler)
		where TCog : uC, ICog
		=> VerifyWorker<TVal>.Make((tar, val, _) => handler(tar.GetOrThrow<TCog>(), val));

	public static BaseWorker MakeVerifyWorker3<TCog, TVal, TSrc>(
		Func<TCog, TVal, TSrc, sbyte> handler
	)
		where TCog : uC, ICog
		where TSrc : uC, ICog
		=> VerifyWorker<TVal>.Make(
			(tar, val, src) => handler(tar.GetOrThrow<TCog>(), val, src.Get<TSrc>())
		);

	#endregion

	#region React workers

	public static BaseWorker MakeReactWorker() => throw new Exception($"Don't use this");

	public static BaseWorker MakeReactWorker1<TCog>(Action<TCog> handler)
		where TCog : uC, ICog
		=> ReactWorker<int>.Make((entity, __) => handler(entity.GetOrThrow<TCog>()));

	public static BaseWorker MakeReactWorker2<TCog, TVal>(Action<TCog, TVal> handler)
		where TCog : uC, ICog
		=> ReactWorker<TVal>.Make((entity, val) => handler(entity.GetOrThrow<TCog>(), val));

	public static BaseWorker MakeReactWorker3<TCog, TVal, T1>(Action<TCog, TVal, T1> handler)
		where TCog : uC, ICog
		where T1 : uC
		=> ReactWorker<TVal>.Make(
			(entity, val) => handler(
				entity.GetOrThrow<TCog>(),
				val,
				entity.GetOrThrow<T1>()
			)
		);

	public static BaseWorker MakeReactWorker4<TCog, TVal, T1, T2>(
		Action<TCog, TVal, T1, T2> handler
	)
		where TCog : uC, ICog
		where T1 : uC
		where T2 : uC
		=> ReactWorker<TVal>.Make(
			(entity, val) => handler(
				entity.GetOrThrow<TCog>(),
				val,
				entity.GetOrThrow<T1>(),
				entity.GetOrThrow<T2>()
			)
		);

	public static BaseWorker MakeReactWorker5<TCog, TVal, T1, T2, T3>(
		Action<TCog, TVal, T1, T2, T3> handler
	)
		where TCog : uC, ICog
		where T1 : uC
		where T2 : uC
		where T3 : uC
		=> ReactWorker<TVal>.Make(
			(entity, val) => handler(
				entity.GetOrThrow<TCog>(),
				val,
				entity.GetOrThrow<T1>(),
				entity.GetOrThrow<T2>(),
				entity.GetOrThrow<T3>()
			)
		);

	#endregion


	public static string GetMakerString(this WorkerTrigger trigger, int paramCount = 0)
	{
		return trigger switch {
			WorkerTrigger.ADDED => nameof(MakeAddedWorker) + paramCount,
			WorkerTrigger.REMOVED => nameof(MakeRemovedWorker),
			WorkerTrigger.ENABLED => nameof(MakeEnabledWorker) + paramCount,
			WorkerTrigger.DISABLED => nameof(MakeDisabledWorker) + paramCount,
			// WorkerTrigger.RUN => nameof(MakeRunWorker) + paramCount,
			WorkerTrigger.RUN => nameof(MakeRunnWorker) + paramCount,
			WorkerTrigger.VERIFY => nameof(MakeVerifyWorker) + paramCount,
			WorkerTrigger.REACT => nameof(MakeReactWorker) + paramCount,
			WorkerTrigger.ALL => nameof(MakeAllWorker),
			WorkerTrigger.REGISTRY => nameof(MakeRegistryWorker),

			_ => throw new Exception($"Worker maker missing: {trigger}"),
		};
	}

	public static BaseWorker MakeWorker(Baron baron, WorkerFact workerFact)
	{
		var makerMethod = typeof(WorkerCreation)
		   .GetAnyMethod(workerFact.MakerString);

		if (makerMethod == null)
			throw new MissingMethodException($"Missing {workerFact.MakerString}");

		var reqTypes = workerFact.RequiredTypeNames
		   .Map(Type.GetType);

		if (RLog.Worker.On) {
			Log(
				$"MakeWorker: {workerFact.Name} with {workerFact.MakerString}, types: {reqTypes.Join()}"
				   ._RLog(RLog.Worker)
			);
		}

		var maker = makerMethod.IsGenericMethod
			? makerMethod.MakeGenericMethod(reqTypes)
			: makerMethod;

		try {
			var worker = workerFact.WorkerMemberType switch {
				WorkerMemberType.METHOD => InvokeMaker_Method(baron, workerFact, maker),
				WorkerMemberType.FIELD => InvokeMaker_Field(baron, workerFact, maker),
				WorkerMemberType.UNSET => throw new ArgumentOutOfRangeException(),
				_ => throw new ArgumentOutOfRangeException()
			};
			worker.__Initialize(baron, workerFact);

			return worker;
		}
		catch (Exception err) {
			throw new Exception($"MakeWorker failed on {workerFact}: {err.Message}");
		}
	}

	static BaseWorker InvokeMaker_Method(Baron baron, WorkerFact workerFact, MethodInfo maker)
	{
		var action = baron.GetType()
		   .GetAnyMethod(workerFact.Name)
		   .CreateDelegate(baron);
		return (BaseWorker)maker.Invoke(null, new object[] { action });
	}

	static BaseWorker InvokeMaker_Field(Baron baron, WorkerFact workerFact, MethodInfo maker)
		=> (BaseWorker)maker.Invoke(null, new object[] { });
}
}