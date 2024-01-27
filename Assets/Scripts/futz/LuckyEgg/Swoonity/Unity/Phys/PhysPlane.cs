using System;
using JetBrains.Annotations;
using UnityEngine;
using V3 = UnityEngine.Vector3;

namespace Swoonity.Unity
{
/// an infinite plane (serializable)
/// <remarks>Represented by a Normal and an Offset (distance from 0,0,0)</remarks>
[Serializable]
public struct PhysPlane
{
	[Tooltip("Normal of plane")]
	public V3 Normal;

	[Tooltip("Distance from origin (0,0,0) in the direction of the Normal")] // or -distance?
	public float Offset;

	public V3 Center => Normal * Offset;

	public PhysPlane(V3 normal, V3 anyPointOnPlane)
	{
		Normal = normal.normalized;
		Offset = -Vector3.Dot(Normal, anyPointOnPlane);
	}


	[Pure] public float Dot(V3 point) => V3.Dot(Normal, point);

	/// signed distance from plane
	/// <remarks>negative if on plane or NOT on normal side</remarks>
	[Pure] public float DistanceTo(V3 point) => V3.Dot(Normal, point) + Offset;

	/// point is on the side of the Normal direction
	/// <remarks>A point on the plane (0 distance) returns FALSE</remarks>
	[Pure] public bool IsNormalSide(V3 point) => DistanceTo(point) > 0f;

	/// point is NOT on the side of the Normal direction
	/// <remarks>A point on the plane (0 distance) returns TRUE</remarks>
	[Pure] public bool NotNormalSide(V3 point) => DistanceTo(point) <= 0f;

	/// get closest point on a plane to given point
	[Pure] public V3 ClosestPointTo(V3 point) => point - Normal * DistanceTo(point);

	/// get closest point on plane and signed distance from original point
	[Pure] public (V3 planePoint, float distance) Place(V3 point)
	{
		var distance = DistanceTo(point);
		var planePoint = point - Normal * distance;
		return (planePoint, distance);
	}

	/// checks if ray hits plane, hit point, and the distance traveled
	/// <remarks>if parallel: (false, V3.zero, 0), if wrong direction: (false, point, -distance)</remarks>
	[Pure] public (bool didHit, Vector3 point, float distance) CheckRay(V3 origin, V3 direction)
	{
		var dotDirNormal = V3.Dot(direction, Normal);
		if (Mathf.Approximately(dotDirNormal, 0f)) return (false, V3.zero, 0f); //>> parallel

		var distance = (-V3.Dot(origin, Normal) - Offset) / dotDirNormal;
		return (distance > 0, origin + direction * distance, distance);
	}

	/// move a planePoint as if the plane was no longer offset
	/// <remarks>planePoint - (Normal * Offset)</remarks>
	[Pure] public V3 SinkToOrigin(V3 planePoint) => planePoint + (Normal * Offset);

	/// move a planePoint along normal
	[Pure] public V3 SinkTo(V3 planePoint, V3 sinkTo)
	{
		var distance = V3.Dot(Normal, sinkTo - planePoint);
		return planePoint + (Normal * distance);
	}

	public override string ToString() => $"plane[{Normal} + {Offset}]";


	#region sugar

	/// "above" pretending like Normal is UP
	/// <remarks>A point on the plane (0 distance) returns FALSE</remarks>
	[Pure] public bool IsAbove(V3 point) => DistanceTo(point) > 0f;

	/// "below" pretending like Normal is UP
	/// <remarks>A point on the plane (0 distance) returns TRUE</remarks>
	[Pure] public bool IsBelow(V3 point) => DistanceTo(point) <= 0f;

	/// "facing" pretending like Normal is FORWARD
	/// <remarks>A point on the plane (0 distance) returns FALSE</remarks>
	[Pure] public bool IsFacing(V3 point) => DistanceTo(point) > 0f;

	/// "facing" pretending like Normal is FORWARD
	/// <remarks>A point on the plane (0 distance) returns TRUE</remarks>
	[Pure] public bool NotFacing(V3 point) => DistanceTo(point) <= 0f;

	#endregion

	#region other planes

	public bool Faces(PhysPlane other)
	{
		var dotNormal = V3.Dot(other.Normal, Normal);
		if (dotNormal >= 0) return false; //>> perpendicular or same direction

		return IsNormalSide(other.Center);
	}

	#endregion
}
}