using System;
using UnityEngine;
using V3 = UnityEngine.Vector3;

namespace Swoonity.Unity
{
public enum Locality
{
	WORLD,
	LOCAL,
}

[Serializable]
public struct PhysRay
{
	public V3 Origin;
	public V3 Direction;
	public float Length;
	public LayerMask Mask;
	public Locality Locality;
	public float SphereSize; // radius

	public bool IsSphereRay => SphereSize > 0;

	public PhysRay(
		V3 origin,
		V3 direction,
		float length = Phys.DEFAULT_LENGTH,
		int mask = Phys.DEFAULT_MASK,
		Locality locality = Locality.WORLD,
		float sphereSize = 0
	)
	{
		Origin = origin;
		Direction = direction.normalized;
		Length = length;
		Mask = mask;
		Locality = locality;
		SphereSize = sphereSize;
	}

	public override string ToString()
		=> Locality switch {
			Locality.WORLD => $"PhysRay({Origin}->{Direction}*{Direction})",
			Locality.LOCAL => $"PhysRay({Origin}->{Direction}*{Direction}_local)",
			_ => throw new ArgumentOutOfRangeException()
		};


	/// calculates where origin should be given destination, direction, and length
	/// <remarks>origin = destination + (-direction * length)</remarks>
	public static PhysRay To(
		V3 destination,
		V3 direction,
		float length,
		int mask = Phys.DEFAULT_MASK,
		Locality locality = Locality.WORLD
	)
		=> new(destination + (-direction * length), direction, length, mask, locality);
}

public static class Util_PhysRay
{
	public static PhysRay ToPhysRay(
		this Ray uRay,
		float length = Phys.DEFAULT_LENGTH,
		int mask = Phys.DEFAULT_MASK,
		Locality locality = Locality.WORLD
	)
		=> new(uRay.origin, uRay.direction, length, mask, locality);

	/// ray.Origin + (ray.Direction * distance)
	public static V3 GetPoint(this PhysRay ray, float distance)
		=> ray.Origin + (ray.Direction * distance);
}
}