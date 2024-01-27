using System;
using System.Collections.Generic;
using UnityEngine;
using static Swoonity.Unity.Phys;
using V3 = UnityEngine.Vector3;

namespace Swoonity.Unity
{
/// serializable version of RaycastHit, but checks for component
/// <remarks>if hit collider is missing component, counts as a miss</remarks>
[Serializable]
public struct PhysHit<T> where T : Component
{
	public PhysRay Ray;

	/// hit something and it had target component
	[Header("Result")]
	public bool HitAny;
	public bool HasTarget => Target;
	public bool Blocked => HitAny && !Target;
	public bool Missed => !HitAny;

	public V3 Point;
	public V3 Normal;
	public float Distance;

	/// component on hit collider or parents (null if miss)
	public T Target;

	/// Target.transform (null if miss)
	public Transform Tf;

	/// collider hit, regardless of whether it had target component (null if miss)
	public Collider Collider;

	/// hit collider Rigidbody (null if miss or collider is not attached to a Rigidbody)
	public Rigidbody Rigid;

	[Tooltip("Did this come from Phys.Cast? Or was it just created from Unity serialization?")]
	public bool WasCast;

	public static PhysHit<T> Create(
		(bool hitAny, RaycastHit uHit) results,
		PhysRay ray = default,
		Debugging debugging = Debugging.OFF
	)
	{
		var (hitAny, uHit) = results;
		if (!hitAny) { //>> miss
			return new PhysHit<T> { Ray = ray, WasCast = true, }
			   .DebugIf(debugging is Debugging.ALWAYS or Debugging.ON_MISS);
		}

		var collider = uHit.collider;

		var target = collider.GetComponentInParent<T>();
		if (!target) { //>> blocked
			return new PhysHit<T> {
					HitAny = true,
					Point = uHit.point,
					Normal = uHit.normal,
					Distance = uHit.distance,
					Collider = collider,
					Rigid = collider.attachedRigidbody,
					Ray = ray,
					WasCast = true,
				}
			   .DebugIf(debugging is Debugging.ALWAYS or Debugging.ON_BLOCKED);
		}

		return new PhysHit<T> { //>> has target
				HitAny = true,
				Point = uHit.point,
				Normal = uHit.normal,
				Distance = uHit.distance,
				Target = target,
				Tf = target.transform,
				Collider = collider,
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

	public static PhysHit<T> Cast<T>(
		this PhysRay ray,
		Debugging debugging = Debugging.OFF
	) where T : Component
		=> PhysHit<T>.Create(_Cast(ray), ray, debugging);

	public static PhysHit<T> Cast<T>(
		V3 origin,
		V3 direction,
		float length = DEFAULT_LENGTH,
		int mask = DEFAULT_MASK,
		Debugging debugging = Debugging.OFF
	) where T : Component
		=> Cast<T>(
			new PhysRay(origin, direction, length, mask),
			debugging
		);

	public static PhysHit<T> Cast<T>(
		Ray uRay,
		float length = DEFAULT_LENGTH,
		int layerMask = DEFAULT_MASK,
		Debugging debugging = Debugging.OFF
	) where T : Component
		=> Cast<T>(
			new PhysRay(uRay.origin, uRay.direction, length, layerMask),
			debugging
		);

	public static PhysHit<T> Cast<T>(
		Ray uRay,
		float sphereSize,
		float length = DEFAULT_LENGTH,
		int layerMask = DEFAULT_MASK,
		Debugging debugging = Debugging.OFF
	) where T : Component
		=> Cast<T>(
			new PhysRay(uRay.origin, uRay.direction, length, layerMask, sphereSize: sphereSize),
			debugging
		);

	#endregion

	#region CastAll

	public static int CastAll<T>(
		this PhysRay ray,
		List<PhysHit<T>> hits,
		bool collectOnlyTargets = true,
		bool clearList = true,
		Debugging debugging = Debugging.OFF
	) where T : Component
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
			var hit = PhysHit<T>.Create((true, uHit), ray, debugging);
			if (hit.HasTarget || !collectOnlyTargets) {
				hits.Add(hit);
			}
		}

		return hitCount;
	}

	public static int CastAll<T>(
		List<PhysHit<T>> hits,
		Ray uRay,
		float length = DEFAULT_LENGTH,
		int layerMask = DEFAULT_MASK,
		bool collectOnlyTargets = true,
		bool clearList = true,
		Debugging debugging = Debugging.OFF
	) where T : Component
		=> CastAll<T>(
			new PhysRay(uRay.origin, uRay.direction, length, layerMask),
			hits,
			collectOnlyTargets,
			clearList,
			debugging
		);

	public static int CastAll<T>(
		List<PhysHit<T>> hits,
		Ray uRay,
		float sphereSize,
		float length = DEFAULT_LENGTH,
		int layerMask = DEFAULT_MASK,
		bool collectOnlyTargets = true,
		bool clearList = true,
		Debugging debugging = Debugging.OFF
	) where T : Component
		=> CastAll<T>(
			new PhysRay(uRay.origin, uRay.direction, length, layerMask, sphereSize: sphereSize),
			hits,
			collectOnlyTargets,
			clearList,
			debugging
		);

	#endregion

	#region Sugar

	/// COLLIDER is in layer
	public static bool InLayerMask<T>(this PhysHit<T> hit, LayerMask mask) where T : Component
		=> hit.Collider && mask == (mask | (1 << hit.Collider.gameObject.layer));

	// public static bool ColliderHasTag<T>(this PhysHit<T> hit, string tag) where T : Component
	// 	=> hit.Collider && hit.Collider.CompareTag(tag);
	//
	// public static bool TargetHasTag<T>(this PhysHit<T> hit, string tag) where T : Component
	// 	=> hit.Target && hit.Target.CompareTag(tag);

	/// hit.Point or if miss: orPoint
	public static V3 GetPointOr<T>(this PhysHit<T> hit, V3 orPoint) where T : Component
		=> hit.HitAny ? hit.Point : orPoint;

	/// hit.Point or if miss: projects point along ray
	public static V3 GetPointOr<T>(this PhysHit<T> hit, float orDistanceIfMiss) where T : Component
		=> hit.HitAny ? hit.Point : hit.Ray.GetPoint(orDistanceIfMiss);


	public static PhysHit<TR> Convert<T, TR>(this PhysHit<T> original, TR target)
		where T : Component
		where TR : Component
	{
		return new PhysHit<TR> {
			HitAny = original.HitAny,
			Point = original.Point,
			Normal = original.Normal,
			Distance = original.Distance,
			Target = target,
			Tf = target.transform,
			Collider = original.Collider,
			Rigid = original.Rigid,
			Ray = original.Ray,
			WasCast = original.WasCast,
		};
	}

	public static PhysHit<T> WithDistance<T>(this PhysHit<T> hit, float distance)
		where T : Component
	{
		hit.Distance = distance;
		return hit;
	}

	#endregion

	#region Debugging

	public static PhysHit<T> DoDebug<T>(this PhysHit<T> hit) where T : Component
	{
		var ray = hit.Ray;
		Debug.DrawRay(ray.Origin, ray.Direction * ray.Length, Color.green, DEBUG_DURATION);
		return hit;
	}

	public static PhysHit<T> DebugIf<T>(this PhysHit<T> hit, bool doDebug) where T : Component
		=> doDebug ? hit.DoDebug() : hit;

	#endregion
}
}