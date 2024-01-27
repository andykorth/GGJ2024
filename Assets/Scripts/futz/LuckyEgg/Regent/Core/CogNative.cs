using Regent.Catalog;
using Regent.Entities;
using Regent.SyncerFacts;
using Swoonity.MHasher;
using UnityEngine;

namespace Regent.Cogs
{
[RequireComponent(typeof(Entity))]
public abstract class CogNative : MonoBehaviour, IHasEntity, ICog, IMHashable
{
	[HideInInspector] public Transform Tform;
	[HideInInspector] public Entity Entity;
	public int EntityId => Entity.EntityId;
	public Entity GetEntity() => Entity;
	public int GetEntityId() => Entity.EntityId;

	[HideInInspector] public MHash HashId;
	public void SetHashId(MHash hashId) => HashId = hashId;
	public MHash GetHash() => HashId;


	protected virtual void WhenEditorValidates() { }
	protected virtual void WhenAwake() { }
	protected virtual void WhenDestroyed() { }

	protected virtual void InternalWhenEditorValidates() { }
	protected virtual void InternalWhenAwake() { }
	protected virtual void InternalWhenDestroyed() { }

	void OnValidate()
	{
		Tform = transform;
		if (!Entity) Entity = GetComponent<Entity>();
		RegentValidators.ValidateCog(this);
		SyncerFactMakers.DevtimeInitializeCogSyncers(this);
		InternalWhenEditorValidates();
		WhenEditorValidates();
	}


	protected void Awake()
	{
		Tform = transform;
		InternalWhenAwake();
		WhenAwake();
	}

	protected void OnDestroy()
	{
		InternalWhenDestroyed();
		WhenDestroyed();
	}


	public override string ToString() => $"{name}({GetType().Name} #{EntityId})";


	#region Sugar

	/// alias for .Entity.HasGet
	public (bool has, T comp) HasGet<T>() where T : Component => Entity.HasGet<T>();

	/// alias for .Entity.Get
	public T1 Get<T1>() where T1 : Component => Entity.Get<T1>();

	/// alias for .Entity.Get
	public (T1, T2)
		Get<T1, T2>()
		where T1 : Component
		where T2 : Component
		=> (Entity.Get<T1>(), Entity.Get<T2>());

	/// alias for .Entity.Get
	public (T1, T2, T3)
		Get<T1, T2, T3>()
		where T1 : Component
		where T2 : Component
		where T3 : Component
		=> (Entity.Get<T1>(), Entity.Get<T2>(), Entity.Get<T3>());


	public void TrySet<T1>(ref T1 c1, bool checkChildren = false)
		where T1 : Component
	{
		if (!c1) c1 = checkChildren ? GetComponentInChildren<T1>() : GetComponent<T1>();
	}

	public void TrySet<T1, T2>(ref T1 c1, ref T2 c2, bool checkChildren = false)
		where T1 : Component
		where T2 : Component
	{
		if (!c1) c1 = checkChildren ? GetComponentInChildren<T1>() : GetComponent<T1>();
		if (!c2) c2 = checkChildren ? GetComponentInChildren<T2>() : GetComponent<T2>();
	}

	public void TrySet<T1, T2, T3>(ref T1 c1, ref T2 c2, ref T3 c3, bool checkChildren = false)
		where T1 : Component
		where T2 : Component
		where T3 : Component
	{
		if (!c1) c1 = checkChildren ? GetComponentInChildren<T1>() : GetComponent<T1>();
		if (!c2) c2 = checkChildren ? GetComponentInChildren<T2>() : GetComponent<T2>();
		if (!c3) c3 = checkChildren ? GetComponentInChildren<T3>() : GetComponent<T3>();
	}


	public void TrySet<T1>(Transform onTf, ref T1 c1, bool checkChildren = false)
		where T1 : Component
	{
		if (!onTf) return; //>> no target transform
		if (!c1) c1 = checkChildren ? onTf.GetComponentInChildren<T1>() : onTf.GetComponent<T1>();
	}

	public void TrySet<T1, T2>(Transform onTf, ref T1 c1, ref T2 c2, bool checkChildren = false)
		where T1 : Component
		where T2 : Component
	{
		if (!onTf) return; //>> no target transform
		if (!c1) c1 = checkChildren ? onTf.GetComponentInChildren<T1>() : onTf.GetComponent<T1>();
		if (!c2) c2 = checkChildren ? onTf.GetComponentInChildren<T2>() : onTf.GetComponent<T2>();
	}

	public void TrySet<T1, T2, T3>(
		Transform onTf,
		ref T1 c1,
		ref T2 c2,
		ref T3 c3,
		bool checkChildren = false
	)
		where T1 : Component
		where T2 : Component
		where T3 : Component
	{
		if (!onTf) return; //>> no target transform
		if (!c1) c1 = checkChildren ? onTf.GetComponentInChildren<T1>() : onTf.GetComponent<T1>();
		if (!c2) c2 = checkChildren ? onTf.GetComponentInChildren<T2>() : onTf.GetComponent<T2>();
		if (!c3) c3 = checkChildren ? onTf.GetComponentInChildren<T3>() : onTf.GetComponent<T3>();
	}

	/// if (!c1) c1 = GetComponentInParent
	/// TODO: confusing name
	public void TrySetParent<T1>(ref T1 c1)
		where T1 : Component
	{
		if (!c1) c1 = GetComponentInParent<T1>();
	}

	/// similar to GetComponentInParent, but using Entity, Entity.ParentEntity, CCache
	public T1 FindComp<T1>() where T1 : Component => Entity.FindComp<T1>();

	public static bool InPlayMode => Application.isPlaying;
	public static bool InEditMode => !Application.isPlaying;

	#endregion
}
}