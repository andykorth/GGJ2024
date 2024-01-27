using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Regent.Catalog;
using Regent.CogFacts;
using Regent.Core;
using Regent.Entities;
using Regent.Logging;
using Regent.SyncerFacts;
using Swoonity.Collections;
using Swoonity.CSharp;
using UnityEngine;
using static UnityEngine.Debug;

namespace Regent.Workers
{
/// REGISTRY<T1>
[Serializable]
public class RegistryWorker<T> : BaseWorker where T : Component, IHasEntity
{
	public Registry<T> Reg;

	public override void Initialize()
	{
		var field = Baron.GetType().GetAnyField(Fact.Name);

		Reg = field.GetValue(Baron) as Registry<T>;

		if (Reg == null) {
			Reg = new Registry<T>();
			field.SetValue(Baron, Reg);
		}

		Reg.Fact = Fact;
	}

	public override void Execute() { }

	public override void AddIfRelevant(Entity entity)
	{
		if (IsInterestedIn(entity)) {
			Reg._Register(entity.Get<T>());
		}
	}

	public override void HandleDestroyedIfRelevant(Entity entity)
	{
		if (IsInterestedIn(entity)) {
			Reg._Deregister(entity.Get<T>());
		}
	}


	public static RegistryWorker<T> Make() => new();
}

/// RUN<T1..T5>
[Serializable]
public class RunnWorker<T> : BaseWorker
{
	public Func<Entity, T> Register;
	public Action<T> Act;
	public Lup<int, T> Lup = new();

	public override void Initialize() { }

	public override void AddIfRelevant(Entity entity)
	{
		if (IsInterestedIn(entity)) {
			Lup.Set(entity.EntityId, Register(entity));
		}
	}

	public override void CutIfRelevant(int entityId) => Lup.Cut(entityId);

	public override void Execute()
	{
		foreach (var kvp in Lup) {
			try {
				Act(kvp.Value);
			}
			catch (Exception err) {
				LogWarning($"The following exception happened on Worker: {this}, entityId {kvp.Key}", Baron);
				LogException(err);
			}
		}
	}

	public static RunnWorker<T> Make(
		Func<Entity, T> register,
		Action<T> act
	)
		=> new() { Register = register, Act = act };
}

/// RUN<T1..T5>
[Serializable]
public class RunWorker : BaseWorker
{
	public Action<Entity> Act;
	public EntityLup Entities = new();

	public override void Initialize() { }

	public override void AddIfRelevant(Entity entity)
	{
		if (IsInterestedIn(entity)) {
			Entities.Set(entity.EntityId, entity);
		}
	}

	public override void CutIfRelevant(int entityId) => Entities.Cut(entityId);

	public override void Execute()
	{
		foreach (var kvp in Entities) {
			try {
				Act(kvp.Value);
			}
			catch (Exception err) {
				LogWarning($"The following exception happened on Worker: {this}", Baron);
				LogException(err, kvp.Value);
			}
		}
	}

	public static RunWorker Make(Action<Entity> act) => new() { Act = act };
}

/// ADDED<T1..T5>
[Serializable]
public class AddedWorker : BaseWorker
{
	public Action<Entity> Act;
	public Queue<Entity> Queue = new(16);

	public override void Initialize() { }

	public override void AddIfRelevant(Entity entity)
	{
		if (IsInterestedIn(entity)) {
			Queue.Enqueue(entity);
		}
	}

	public override void Execute() => Queue.Drain(Act);

	public static AddedWorker Make(Action<Entity> act) => new() { Act = act };
}

/// Removed
[Serializable]
public class RemovedWorker : BaseWorker
{
	public Action<int> Act;
	public HashSet<int> EntityIds = new();
	public Queue<int> Queue = new(16);

	public override void Initialize() { }

	public override void AddIfRelevant(Entity entity)
	{
		if (IsInterestedIn(entity)) {
			EntityIds.Add(entity.EntityId);
		}
	}

	public override void CutIfRelevant(int entityId)
	{
		if (EntityIds.Remove(entityId)) { // true if originally relevant
			Queue.Enqueue(entityId);
		}
	}

	public override void Execute() => Queue.Drain(Act);

	public static RemovedWorker Make(Action<int> act) => new() { Act = act };
}


/// ENABLED<T1..T5>
[Serializable]
public class EnabledWorker : BaseWorker
{
	public Action<Entity> Act;

	public override void Initialize() { }

	public override void AddIfRelevant(Entity entity)
	{
		if (IsInterestedIn(entity)) {
			Act(entity);
		}
	}

	public override void Execute() => throw new Exception($"cannot execute EnabledWorker: {Fact}");

	public static EnabledWorker Make(Action<Entity> act) => new() { Act = act };
}

/// DISABLED<T1..T5>
[Serializable]
public class DisabledWorker : BaseWorker
{
	public Action<Entity> Act;

	public override void Initialize() { }

	public override void HandleDestroyedIfRelevant(Entity entity)
	{
		if (IsInterestedIn(entity)) {
			Act(entity);
		}
	}

	public override void Execute() => throw new Exception($"cannot execute DisabledWorker: {Fact}");

	public static DisabledWorker Make(Action<Entity> act) => new() { Act = act };
}

/// All
/// TODO: finish implementing
[Serializable]
public class AllWorker : BaseWorker
{
	public Action<EntityLup> Act;
	public EntityLup Entities = new();

	public override void Initialize() { }

	public override void AddIfRelevant(Entity entity)
	{
		if (IsInterestedIn(entity)) {
			Entities.Set(entity.EntityId, entity);
		}
	}

	public override void CutIfRelevant(int entityId) => Entities.Cut(entityId);

	public override void Execute() => Act(Entities);

	public static AllWorker Make(Action<EntityLup> act) => new() { Act = act };
}

/// TIMED<T1..T5>
/// TODO: finish implementing
[Serializable]
public class TimedWorker : BaseWorker
{
	public float TimedSeconds;
	public float NextActivation;

	public Action<Entity> Act;
	public EntityLup Entities = new();

	public override void Initialize()
	{
		TimedSeconds = Fact.TimedSeconds;
		NextActivation = TimedSeconds;
	}

	public override void AddIfRelevant(Entity entity)
	{
		if (IsInterestedIn(entity)) {
			Entities.Set(entity.EntityId, entity);
		}
	}

	public override void CutIfRelevant(int entityId) => Entities.Cut(entityId);

	public override void Execute()
	{
		if (NextActivation > Time.time) return; //>> still waiting

		NextActivation = TimedSeconds;

		foreach (var kvp in Entities) {
			try {
				Act(kvp.Value);
			}
			catch (Exception err) {
				LogException(err, kvp.Value);
			}
		}
	}

	public static TimedWorker Make(Action<Entity> act) => new() { Act = act };
}

[Serializable]
public abstract class SyncerWorker : BaseWorker
{
	public CogInfo CogInfo;
	public SyncerInfo SyncerInfo;
}

/// Verify<TVal>
[Serializable]
public class VerifyWorker<TVal> : SyncerWorker
{
	public Func<Entity, TVal, Entity, sbyte> Act;
	public VerifyForwarder<TVal> VerifyFwd;
	public Queue<(
		Entity tar,
		TVal args,
		Entity src,
		bool needResult,
		UniTaskCompletionSource<sbyte> promise
		)> CuedQueue = new(4);

	public override void Initialize()
	{
		CogInfo = RegentCatalogRuntime.GetCogInfo(Fact.RequiredTypeHashes[0]);
		SyncerInfo = CogInfo.GetSyncerInfoByName(Fact.SyncerName);
		SyncerFactMakers.CheckSyncerInfoInit<TVal>(SyncerInfo);
		VerifyFwd = (VerifyForwarder<TVal>)SyncerInfo.VerifyFwd;
		VerifyFwd.SetWorker(this);
	}

	public UniTask<sbyte> Cue(Entity tar, TVal val, Entity src)
	{
		var promise = new UniTaskCompletionSource<sbyte>(); // TODO: perf, garbage?
		CuedQueue.Enqueue((tar, val, src, true, promise));
		return promise.Task;
	}

	public override void Execute()
	{
		for (var i = 0; i < CuedQueue.Count; i++) {
			var (tar, val, src, needResult, promise)
				= CuedQueue.Dequeue();
			var result = Act(tar, val, src);
			if (needResult) promise.TrySetResult(result);
		}
	}

	public static VerifyWorker<TVal> Make(Func<Entity, TVal, Entity, sbyte> act)
		=> new() { Act = act };
}

public class BaseVerifyForwarder { }

/// 1 per syncer declaration (forwards cues to VerifyWorker)
public class VerifyForwarder<TVal> : BaseVerifyForwarder
{
	public VerifyWorker<TVal> VerifyWorker;
	public void SetWorker(VerifyWorker<TVal> worker) => VerifyWorker = worker;
	public UniTask<sbyte> Cue(Entity tar, TVal val, Entity src) => VerifyWorker.Cue(tar, val, src);
}

/// React<TVal>
[Serializable]
public class ReactWorker<TVal> : SyncerWorker
{
	public Action<Entity, TVal> Act;
	public ReactForwarder<TVal> ReactFwd;
	public int RequiredTypesCount;
	public bool IsInstant;

	/// int key is InstanceId, *NOT* EntityId (entity might not be registered yet)
	public Lup<int, (Entity entity, TVal val)> EntityValsToUpdate = new();

	public override void Initialize()
	{
		RequiredTypesCount = Fact.RequiredTypeHashes.Length;
		CogInfo = RegentCatalogRuntime.GetCogInfo(Fact.RequiredTypeHashes[0]);
		SyncerInfo = CogInfo.GetSyncerInfoByName(Fact.SyncerName);
		SyncerFactMakers.CheckSyncerInfoInit<TVal>(SyncerInfo);
		IsInstant = Fact.IsInstant;

		// hi👋  InvalidCastException? Worker React value is wrong type
		ReactFwd = (ReactForwarder<TVal>)SyncerInfo.ReactFwd;
		ReactFwd.AddWorker(this);
	}

	public void CueReaction(Entity entity, TVal val)
	{
		if (!CheckRequiredTypes(entity)) return; //>> missing components

		// Log($"{this} cued: {entity} <<<< {val}"._RLg(RLog.Worker), entity);

		if (IsInstant) {
			// Log($"Instant {this}, {entity} {val}"._RLg(RLog.Worker), entity);
			if (!HasRequiredAuthority(entity)) return; //>> wrong authority
			Act(entity, val);
			return;
		}

		EntityValsToUpdate.Set(entity.InstanceId, (entity, val));
	}

	public override void Execute()
	{
		if (EntityValsToUpdate.IsEmpty) return; //>> nothing to execute

		// Log($"{this} EXECUTING on {EntityValsToUpdate.Count} entities"._RLg(RLog.Worker));

		foreach (var kvp in EntityValsToUpdate) {
			var (entity, val) = kvp.Value;

			if (!entity) continue; //>> was destroyed
			if (!entity.IsRegistered) return; //>> was disabled/destroyed

			// wait to check authority here to allow for net entities to initialize/register
			if (!HasRequiredAuthority(entity)) continue; //>> wrong authority

			try {
				Act(entity, val);
			}
			catch (Exception err) {
				LogException(err, entity);
			}
		}

		EntityValsToUpdate.Clear();
	}

	bool CheckRequiredTypes(Entity entity)
	{
		// OPTIMIZE
		var reqTypes = Fact.RequiredTypeHashes;
		// 0: cog (will always have), 1: val, 2-4: optional type
		return RequiredTypesCount switch {
			1 => true,
			2 => true,
			3 => entity.Has(reqTypes[2]),
			4 => entity.Has(reqTypes[2]) && entity.Has(reqTypes[3]),
			5 => entity.Has(reqTypes[2]) && entity.Has(reqTypes[3]) && entity.Has(reqTypes[4]),
			_ => throw new Exception($"Bad ReactWorker {this} {SyncerInfo}")
		};
	}

	bool HasRequiredAuthority(Entity entity)
	{
		if (entity.IsAuthor) {
			if (Fact.RequiresRemote) return false; //>> not remote
		}
		else {
			if (Fact.RequiresAuthor) return false; //>> not author
		}

		return true;
	}

	public override string ToString() => $"[{Baron}.{Fact.Name}({typeof(TVal)})]";

	public static ReactWorker<TVal> Make(Action<Entity, TVal> act) => new() { Act = act };
}

public class BaseReactForwarder { }

/// 1 per syncer declaration (forwards cues to ReactWorker list)
public class ReactForwarder<TVal> : BaseReactForwarder
{
	public List<ReactWorker<TVal>> ReactWorkers = new();

	public void AddWorker(ReactWorker<TVal> worker) => ReactWorkers.Add(worker);
	public void CutWorker(ReactWorker<TVal> worker) => ReactWorkers.Remove(worker);

	public void CueReaction(Entity entity, TVal val)
	{
		foreach (var worker in ReactWorkers) {
			worker.CueReaction(entity, val);
		}
	}
}
}