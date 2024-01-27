using System;
using System.Collections.Generic;
using UnityEngine;
using V3 = UnityEngine.Vector3;

namespace Swoonity.Unity
{
public static class ColliderUtils
{
	/// shoot a raycast into a collider (throws if fails to hit, which should never happen)
	public static RaycastHit RaycastIn(
		this Collider collider,
		V3 innerPos,
		V3 inDirection,
		float raySize = 100
	)
	{
		inDirection = inDirection.normalized;
		var origin = innerPos + (-inDirection * raySize);
		var ray = new Ray(origin, inDirection);

		var didHit = collider.Raycast(ray, out var hit, raySize);
		if (!didHit) {
			Debug.DrawRay(origin, inDirection * raySize, Color.red, 5f);
			throw new ErrAbsurd(
				$"RaycastIn missed. innerPos: {innerPos}, origin: {origin}, inDir: {inDirection}, size: {raySize}"
			);
			// Debug.LogWarning($"RaycastIn missed. innerPos: {innerPos}, origin: {origin}, inDir: {inDirection}, size: {raySize}");
		}

		return hit;
	}

	public static RaycastHit RaycastIn(this Collider collider, V3 inDirection, float raySize = 100)
		=> collider.RaycastIn(collider.bounds.center, inDirection, raySize);

	public static void SetPhysicsMat(this List<Collider> colliders, PhysicMaterial mat)
	{
		foreach (var collider in colliders) {
			collider.material = mat;
		}
	}

	public static void SetPhysicsMat(this Collider[] colliders, PhysicMaterial mat)
	{
		foreach (var collider in colliders) {
			collider.material = mat;
		}
	}


	public static V3 ClosestPoint(this Collider[] colliders, V3 targetPos)
	{
		if (colliders == null) return V3.zero;

		var numOfColliders = colliders.Length;
		if (colliders.Length == 1) return colliders[0].ClosestPoint(targetPos);

		var closest = colliders[0].ClosestPoint(targetPos);
		var distance = closest.DistanceSqr(targetPos);

		for (var i = 1; i < numOfColliders; i++) {
			var potentialClosest = colliders[i].ClosestPoint(targetPos);
			var potentialDistance = potentialClosest.DistanceSqr(targetPos);

			if (potentialDistance < distance) {
				closest = potentialClosest;
				distance = potentialDistance;
			}
		}

		return closest;
	}


	public static Bounds GetBounds(this Collider[] colliders)
	{
		if (colliders == null) return default;

		var numOfColliders = colliders.Length;
		if (numOfColliders == 1) return colliders[0].bounds;

		var startingBounds = colliders[0].bounds;
		var bounds = new Bounds(startingBounds.center, startingBounds.size);

		for (var dex = 1; dex < numOfColliders; dex++) {
			var coll = colliders[dex];
			bounds.Encapsulate(coll.bounds);
		}

		return bounds;
	}

	public static Bounds GetBounds<T>(this List<T> colliders) where T : Collider
	{
		if (colliders == null) return default;

		var numOfColliders = colliders.Count;
		if (numOfColliders == 1) return colliders[0].bounds;

		var startingBounds = colliders[0].bounds;
		var bounds = new Bounds(startingBounds.center, startingBounds.size);

		for (var dex = 1; dex < numOfColliders; dex++) {
			var coll = colliders[dex];
			bounds.Encapsulate(coll.bounds);
		}

		return bounds;
	}


	public static bool Contains(this Collider[] colliders, V3 point)
	{
		foreach (var collider in colliders) {
			if (collider.bounds.Contains(point)) return true;
		}

		return false;
	}


	public static V3 GetSize(this Collider collider)
		=> collider switch {
			BoxCollider box => box.GetSize(),
			CapsuleCollider capsule => capsule.GetSize(),
			MeshCollider mesh => mesh.GetSize(),
			SphereCollider sphere => sphere.GetSize(),
			_ => throw new ArgumentOutOfRangeException(nameof(collider))
		};

	public static V3 GetSize(this BoxCollider box) => box.size;

	public static V3 GetSize(this CapsuleCollider capsule)
	{
		var diameter = capsule.radius;
		var height = capsule.height;
		var direction = capsule.direction;
		Debug.LogWarning("CapsuleCollider.GetSize hasn't been tested! TODO");
		return direction switch {
			0 => new V3(height, diameter, diameter),
			1 => new V3(diameter, height, diameter),
			2 => new V3(diameter, diameter, height),
			_ => throw new ArgumentOutOfRangeException()
		};
	}

	public static V3 GetSize(this MeshCollider mesh) => mesh.sharedMesh.bounds.size;
	public static V3 GetSize(this SphereCollider sphere) => V3.one * sphere.radius * 2;

	public static float GetHeight(this Collider collider) => collider.bounds.size.y;

	public static int ComputeFaces(this Collider collider, List<FaceInfo> fill)
		=> collider switch {
			BoxCollider box => box.ComputeFaces(fill),
			// CapsuleCollider capsule => capsule.ComputeFaces(fill),
			// MeshCollider mesh => mesh.ComputeFaces(fill),
			// SphereCollider sphere => sphere.ComputeFaces(fill),
			_ => throw new Exception($"{nameof(collider)} TODO")
		};

	public static int ComputeFaces(this BoxCollider box, List<FaceInfo> fill)
	{
		var center = box.center;
		var size = box.size;
		var extents = size / 2;
		var min = center - extents;
		var max = center + extents;

		fill.Add(new FaceInfo(center.WithX(min), min, max.WithX(min), V3.left));
		fill.Add(new FaceInfo(center.WithX(max), min.WithX(max), max, V3.right));
		fill.Add(new FaceInfo(center.WithY(min), min, max.WithY(min), V3.down));
		fill.Add(new FaceInfo(center.WithY(max), min.WithY(max), max, V3.up));
		fill.Add(new FaceInfo(center.WithZ(min), min, max.WithZ(min), V3.back));
		fill.Add(new FaceInfo(center.WithZ(max), min.WithZ(max), max, V3.forward));

		return 6;
	}

	public static void SetBounds(this BoxCollider box, Bounds bounds)
	{
		box.size = bounds.size;
		box.center = bounds.center;
	}
}
}