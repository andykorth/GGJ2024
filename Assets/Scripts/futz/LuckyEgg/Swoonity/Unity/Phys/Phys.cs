using System.Collections.Generic;
using UnityEngine;
using V3 = UnityEngine.Vector3;

// ReSharper disable InconsistentNaming

namespace Swoonity.Unity
{
public static partial class Phys
{
	public const float DEFAULT_LENGTH = float.PositiveInfinity;
	public const int DEFAULT_MASK = Physics.DefaultRaycastLayers;

	public const int MAX_HIT_ALLOC = 99;
	public static void SetMaxHitAllocation(int max) => __RaycastHit_Alloc = new RaycastHit[max];
	static RaycastHit[] __RaycastHit_Alloc = new RaycastHit[MAX_HIT_ALLOC];

	public const int MAX_COLLIDER_ALLOC = 99;
	public static void SetMaxColliderAllocation(int max) => __Collider_Alloc = new Collider[max];
	static Collider[] __Collider_Alloc = new Collider[MAX_COLLIDER_ALLOC];

	public static float DEBUG_DURATION = 5f;

	public enum Debugging
	{
		OFF,
		ALWAYS,
		ON_HIT,
		ON_BLOCKED,
		ON_MISS,
	}


	#region Unity Wrappers

	static (bool hitAny, RaycastHit uHit) _Cast(this PhysRay ray)
		=> _Cast(ray.Origin, ray.Direction, ray.Length, ray.Mask, ray.SphereSize);

	static (bool hitAny, RaycastHit uHit) _Cast(
		V3 origin,
		V3 direction,
		float length,
		int mask,
		float radius = 0f
	)
	{
		RaycastHit uHit;
		var hitAny = radius > 0f
			? Physics.SphereCast(origin, radius, direction, out uHit, length, mask)
			: Physics.Raycast(origin, direction, out uHit, length, mask);
		return (hitAny, uHit);
	}

	#endregion


	#region Overlap

	public static int Sphere<T>(
		List<T> targets,
		V3 origin,
		float radius,
		int layerMask = DEFAULT_MASK,
		bool checkParents = true,
		bool clearList = true,
		Debugging debugging = Debugging.OFF
	) where T : Component
	{
		if (clearList) targets.Clear();

		var hitCount = Physics.OverlapSphereNonAlloc(origin, radius, __Collider_Alloc, layerMask);

		for (var i = 0; i < hitCount; i++) {
			var collider = __Collider_Alloc[i];
			var comp = checkParents
				? collider.GetComponentInParent<T>()
				: collider.GetComponent<T>();
			if (comp) targets.Add(comp);
		}

		return hitCount;
	}

	#endregion
}
}