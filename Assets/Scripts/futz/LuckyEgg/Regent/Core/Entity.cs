using System;
using System.Collections.Generic;
using Regent.Coordinator;
using Regent.Core;
using Swoonity.Collections;
using Swoonity.MHasher;
using UnityEngine;
using static UnityEngine.Debug;

namespace Regent.Entities
{
[DisallowMultipleComponent]
public class Entity : MonoBehaviour, IHasEntity
{
	public static IRegentCoordinator __Coordinator;

	[HideInInspector] public Transform Tform;
	public bool HasLiaison;
	public MonoBehaviour LiaisonComp;
	IEntityLiaison _liaison;

	public Entity GetEntity() => this;

	[Header("State")]
	[Tooltip("Unique ID specific to local session GameObject instance")]
	public int InstanceId;
	public bool IsRegistered;
	[Tooltip("0 unset, <0 native, >0 net")]
	public int EntityId;
	public int GetEntityId() => EntityId;

	public Entity ParentEntity;
	public Entity RootEntity => ParentEntity ? ParentEntity.RootEntity : this;

	// public bool IsPending => EntityId == 0 || !IsRegistered && gameObject.activeSelf;
	public bool IsNative => !HasLiaison;
	public bool IsServer => IsRegistered && HasLiaison && _liaison.IsServer();
	public bool IsClient => IsRegistered && HasLiaison && _liaison.IsClient();
	public bool IsAuthor => IsRegistered && HasLiaison && _liaison.IsAuthor();
	public bool IsRemote => IsRegistered && HasLiaison && _liaison.IsRemote();

	public CompCache CCache = new();

	void OnValidate()
	{
		Tform = transform;
		CheckForLiaison();
		CheckForParentEntity();
		CCache.DevtimeInitialize(gameObject);
	}

	void CheckForLiaison()
	{
		var liaison = GetComponent<IEntityLiaison>();
		if (liaison == null) {
			HasLiaison = false;
			return;
		}

		HasLiaison = true;
		LiaisonComp = liaison.GetLiaisonComponent(); // for Unity serialization
		liaison.LinkEntity(this);
	}

	void CheckForParentEntity()
	{
		var parent = Tform.parent;
		ParentEntity = parent
			? parent.GetComponentInParent<Entity>(true)
			: null;
	}

	void OnEnable()
	{
		InstanceId = GetInstanceID();
		Tform = transform;
		CCache.RuntimeInitialize(gameObject);

		if (HasLiaison) {
			_liaison = LiaisonComp as IEntityLiaison;
			return;
		}

		__Register();
	}


	void OnDisable()
	{
		if (HasLiaison) return;
		__Deregister();
	}

	void OnDestroy() => __Deregister();

	public void __Register()
	{
		// Log($"Entity.__Register {this}"._RLog(RLog.Entity));
		if (IsRegistered) return;
		IsRegistered = true;
		__Coordinator.HandleEntityCreated(this);
	}

	public void __Deregister()
	{
		if (!IsRegistered) return;
		IsRegistered = false;
		__Coordinator.HandleEntityDestroyed(this);
	}

	public bool Has<T1>() where T1 : Component => CCache.Has<T1>();
	public bool Has(MHash hash) => CCache.Has(hash);
	public bool Has(MHash[] hashes) => CCache.HasAll(hashes);
	public bool Has(HashSet<MHash> hashSet) => CCache.HasAll(hashSet);
	public T Get<T>() where T : Component => CCache.Get<T>();
	public (bool has, T comp) HasGet<T>() where T : Component => CCache.HasGet<T>();
	public T GetOrThrow<T>() where T : Component => CCache.GetOrThrow<T>();

	public (T1, T2)
		Get<T1, T2>()
		where T1 : Component
		where T2 : Component
		=> (CCache.Get<T1>(), CCache.Get<T2>());

	public (T1, T2, T3)
		Get<T1, T2, T3>()
		where T1 : Component
		where T2 : Component
		where T3 : Component
		=> (CCache.Get<T1>(), CCache.Get<T2>(), CCache.Get<T3>());

	/// similar to GetComponentInParent, but using Entity, Entity.ParentEntity, CCache
	public T1 FindComp<T1>() where T1 : Component
	{
		var (has, comp) = CCache.HasGet<T1>();
		if (has) return comp;
		if (ParentEntity) return ParentEntity.FindComp<T1>();
		return null;
	}

	public override string ToString()
		=> HasLiaison
			? IsRegistered
				? $"{EntityId}{(IsAuthor ? "a" : "r")}#{name}"
				: $"{EntityId}#{name} (iid {InstanceId})"
			: IsRegistered
				? $"{EntityId}#{name}"
				: $"{EntityId}#{name} (iid {InstanceId})";
}

public interface IHasEntity
{
	public Entity GetEntity();
	public int GetEntityId();
}

public interface IEntityLiaison
{
	MonoBehaviour GetLiaisonComponent();

	void LinkEntity(Entity entity);

	bool IsServer();
	bool IsClient();
	bool IsAuthor();
	bool IsRemote();
}


/// wraps Lup<int, Entity>
[Serializable]
public class EntityLup : Lup<int, Entity>
{
	public EntityLup(int initialSize = 0) : base(initialSize) { }

	/// sets entity.EntityId => entity
	public void Set(Entity ent) => Set(ent.EntityId, ent);

	/// removes entity.EntityId from lup
	public void Cut(Entity ent) => Cut(ent.EntityId);
}
}