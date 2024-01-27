using System;
using System.Collections.Generic;
using UnityEngine;
using static Swoonity.Unity.Phys;
using V3 = UnityEngine.Vector3;

namespace Swoonity.Unity
{
/// serializable version of RaycastHit
[Serializable]
public struct PhysHit
{
	public PhysRay Ray;

	[Header("Result")]
	public bool HitAny;
	public bool Missed => !HitAny;

	public V3 Point;
	public V3 Normal;
	public float Distance;

	/// hit collider (null if miss)
	public Collider Collider;

	/// Collider.transform (null if miss)
	public Transform Tf;

	/// hit collider Rigidbody (null if miss or collider is not attached to a Rigidbody)
	public Rigidbody Rigid;

	[Tooltip("Did this come from Phys.Cast? Or was it just created from Unity serialization?")]
	public bool WasCast;

	public static PhysHit Create(
		(bool hitAny, RaycastHit uHit) results,
		PhysRay ray = default,
		Debugging debugging = Debugging.OFF
	)
	{
		var (hitAny, uHit) = results;
		if (!hitAny) { //>> miss
			return new PhysHit { Ray = ray, WasCast = true, }
			   .DebugIf(debugging is Debugging.ALWAYS or Debugging.ON_MISS);
		}

		var collider = uHit.collider;

		return new PhysHit { //>> hit
				HitAny = true,
				Point = uHit.point,
				Normal = uHit.normal,
				Distance = uHit.distance,
				Collider = collider,
				Tf = collider.transform,
				Rigid = collider.attachedRigidbody,
				Ray = ray,
				WasCast = true,
			}
		   .DebugIf(debugging is Debugging.ALWAYS or Debugging.ON_HIT);
	}
}


public static partial class Phys
{
	#region Raycast

	public static PhysHit Cast(
		this PhysRay ray,
		Debugging debugging = Debugging.OFF
	)
		=> PhysHit.Create(_Cast(ray), ray, debugging);

	public static PhysHit Cast(
		V3 origin,
		V3 direction,
		float length = DEFAULT_LENGTH,
		int mask = DEFAULT_MASK,
		Debugging debugging = Debugging.OFF
	)
		=> Cast(new PhysRay(origin, direction, length, mask), debugging);

	public static PhysHit Cast(
		Ray uRay,
		float length = DEFAULT_LENGTH,
		int layerMask = DEFAULT_MASK,
		Debugging debugging = Debugging.OFF
	)
		=> Cast(new PhysRay(uRay.origin, uRay.direction, length, layerMask), debugging);

	public static PhysHit Cast(
		Ray uRay,
		float sphereSize,
		float length = DEFAULT_LENGTH,
		int layerMask = DEFAULT_MASK,
		Debugging debugging = Debugging.OFF
	)
		=> Cast(
			new PhysRay(uRay.origin, uRay.direction, length, layerMask, sphereSize: sphereSize),
			debugging
		);

	#endregion

	#region CastAll

	public static int CastAll(
		this PhysRay ray,
		List<PhysHit> hits,
		bool clearList = true,
		Debugging debugging = Debugging.OFF
	)
	{
		if (clearList) hits.Clear();

		var hitCount = ray.IsSphereRay
			? Physics.SphereCastNonAlloc(
				ray.Origin,
				ray.SphereSize,
				ray.Direction,
				__RaycastHit_Alloc,
				ray.Length,
				ray.Mask
			)
			: Physics.RaycastNonAlloc(
				ray.Origin,
				ray.Direction,
				__RaycastHit_Alloc,
				ray.Length,
				ray.Mask
			);

		for (var i = 0; i < hitCount; i++) {
			var uHit = __RaycastHit_Alloc[i];
			var hit = PhysHit.Create((true, uHit), ray, debugging);
			hits.Add(hit);
		}

		return hitCount;
	}

	public static int CastAll(
		Ray uRay,
		List<PhysHit> hits,
		float length = DEFAULT_LENGTH,
		int layerMask = DEFAULT_MASK,
		bool clearList = true,
		Debugging debugging = Debugging.OFF
	)
		=> CastAll(
			new PhysRay(uRay.origin, uRay.direction, length, layerMask),
			hits,
			clearList,
			debugging
		);

	public static int CastAll(
		Ray uRay,
		List<PhysHit> hits,
		float sphereSize,
		float length = DEFAULT_LENGTH,
		int layerMask = DEFAULT_MASK,
		bool clearList = true,
		Debugging debugging = Debugging.OFF
	)
		=> CastAll(
			new PhysRay(uRay.origin, uRay.direction, length, layerMask, sphereSize: sphereSize),
			hits,
			clearList,
			debugging
		);

	#endregion

	#region Collider.Raycast

	public static PhysHit Cast(
		this PhysRay ray,
		Collider collider,
		Debugging debugging = Debugging.OFF
	)
	{
		var hitAny = collider.Raycast(new Ray(ray.Origin, ray.Direction), out var uHit, ray.Length);
		return PhysHit.Create((hitAny, uHit), ray, debugging);
	}

	public static PhysHit Cast(
		this Collider collider,
		PhysRay ray,
		Debugging debugging = Debugging.OFF
	)
		=> Cast(ray, collider, debugging);

	public static PhysHit Cast(
		this Collider collider,
		V3 origin,
		V3 direction,
		float length = DEFAULT_LENGTH,
		Debugging debugging = Debugging.OFF
	)
		=> Cast(new PhysRay(origin, direction, length), collider, debugging);


	public static (bool didHit, PhysHit hit) TryCast(
		this PhysRay ray,
		Collider collider,
		Debugging debugging = Debugging.OFF
	)
	{
		var hit = Cast(ray, collider, debugging);
		return (hit.HitAny, hit);
	}

	#endregion

	#region Sugar

	public static PhysHit ToPhysHit(this RaycastHit uHit, PhysRay ray = default)
		=> PhysHit.Create((uHit.collider, uHit), ray);

	public static bool InLayerMask(this PhysHit hit, LayerMask mask)
		=> hit.HitAny && mask == (mask | (1 << hit.Collider.gameObject.layer));

	// public static bool HasTag(this PhysHit hit, string tag)
	// 	=> hit.HitAny && hit.Collider.CompareTag(tag);

	/// hit.Collider?.GetComponentInParent (or if checkParents = false, GetComponent)
	public static T Get<T>(this PhysHit hit, bool checkParents = true) where T : Component
		=> hit.HitAny
			? checkParents
				? hit.Collider.GetComponentInParent<T>()
				: hit.Collider.GetComponent<T>()
			: null;

	#endregion

	#region Debugging

	public static PhysHit DoDebug(this PhysHit hit)
	{
		var ray = hit.Ray;
		Debug.DrawRay(ray.Origin, ray.Direction * ray.Length, Color.green, DEBUG_DURATION);
		return hit;
	}

	public static PhysHit DebugIf(this PhysHit hit, bool doDebug) => doDebug ? hit.DoDebug() : hit;

	public static PhysHit TryDebug(this PhysHit hit, Debugging debugging)
		=> debugging switch {
			Debugging.OFF => hit,
			Debugging.ALWAYS => hit.DoDebug(),
			Debugging.ON_HIT => hit.HitAny ? hit.DoDebug() : hit,
			Debugging.ON_MISS => hit.Missed ? hit.DoDebug() : hit,
			_ => hit
		};

	#endregion
}
}