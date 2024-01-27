using System;
using System.Collections.Generic;
using Regent.Barons;
using Regent.Entities;
using Regent.WorkerFacts;
using Swoonity.Collections;
using Swoonity.MHasher;
using UnityEngine;
using uC = UnityEngine.Component;

namespace Regent.Workers
{
public enum WorkerMemberType
{
	UNSET,

	METHOD,
	FIELD,
}

public enum WorkerTrigger
{
	UNSET,

	ADDED,
	REMOVED,
	ENABLED,
	DISABLED,
	RUN,
	VERIFY,
	REACT,
	ALL, // TODO: implement
	TIMED, // TODO: implement

	REGISTRY,
}

// TODO: move
public static class WorkerLogic
{
	public static bool IsInterestedIn(this WorkerFact fact, Entity entity)
	{
		if (fact.RequiresAuthor && !entity.IsAuthor) return false;
		if (fact.RequiresRemote && !entity.IsRemote) return false;

		return entity.Has(fact.RequiredTypeHashes);
	}

	// Optimize: could make RequiredTypes a HashSet (or similar) and overlap with one on Entity
}

[Serializable]
public abstract class BaseWorker : IMHashable
{
	public WorkerFact Fact;

	public Baron Baron;
	public GameObject SceneGobj;

	public virtual bool IsInterestedIn(Entity entity) => Fact.IsInterestedIn(entity);

	public bool IsEnabled => SceneGobj && SceneGobj.activeInHierarchy;
	public void On() => SceneGobj.SetActive(true);
	public void Off() => SceneGobj.SetActive(false);


	public abstract void Initialize();

	public void __Initialize(Baron baron, WorkerFact fact)
	{
		Baron = baron;
		Fact = fact;
		Initialize();
	}

	public virtual bool CanExecute() => IsEnabled;
	public virtual void AddIfRelevant(Entity entity) { }
	public virtual void CutIfRelevant(int entityId) { }
	public virtual void HandleDestroyedIfRelevant(Entity entity) { }

	public abstract void Execute();

	public void TryExecute()
	{
		if (CanExecute()) Execute();
	}


	public MHash GetHash() => Fact.HashId;

	public override string ToString() => $"[{Baron}.{Fact.Name}({GetType().Name})]";
}


/// wraps Lup<MHash, BaseWorker>
[Serializable]
public class WorkerLup : Lup<MHash, BaseWorker>
{
	public WorkerLup(int initialSize = 0) : base(initialSize) { }

	List<BaseWorker> _workers = new();

	public void Set(BaseWorker worker)
	{
		Set(worker.GetHash(), worker);
		_workers.Add(worker);
	}

	public void Cut(BaseWorker worker)
	{
		Cut(worker.GetHash());
		_workers.Remove(worker); // workers are removed rarely
	}

	public void Workers_TryExecuteAll()
	{
		foreach (var worker in _workers) {
			worker.TryExecute();
		}
	}

	public void Workers_AddIfRelevant(Entity entity)
	{
		foreach (var worker in _workers) {
			worker.AddIfRelevant(entity);
		}
	}

	public void Workers_CutIfRelevant(int entityId)
	{
		foreach (var worker in _workers) {
			worker.CutIfRelevant(entityId);
		}
	}

	public void Workers_HandleDestroyedIfRelevant(Entity entity)
	{
		foreach (var worker in _workers) {
			worker.HandleDestroyedIfRelevant(entity);
		}
	}
}
}