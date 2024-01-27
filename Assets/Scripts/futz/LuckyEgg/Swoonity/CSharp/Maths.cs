using System;
using UnityEngine;

namespace Swoonity.CSharp
{
public static class Maths
{
	/// 2 * pi
	public const float TAU = 6.283185f;

	public static float MinMax(this (float min, float max) range, float value)
	{
		return Mathf.Clamp(value, range.min, range.max);
	}

	public static float MinMax(float min, float val, float max)
	{
		return Mathf.Clamp(val, min, max);
	}


	public static int Smallest(int a, int b) => a < b ? a : b;

	public static int Smallest(int a, int b, int c)
		=> a < b
			? a < c
				? a
				: c
			: b < c
				? b
				: c;


	public static int Greatest(int a, int b) => a > b ? a : b;

	public static int Greatest(int a, int b, int c)
		=> a > b
			? Greatest(a, c)
			: Greatest(b, c);

	public static int Greatest(int a, int b, int c, int d)
		=> a > b
			? Greatest(a, c, d)
			: Greatest(b, c, d);

	public static int Greatest(int a, int b, int c, int d, int e)
		=> a > b
			? Greatest(a, c, d, e)
			: Greatest(b, c, d, e);

	public static Vector3 Greatest(Vector3 a, Vector3 b) => a.sqrMagnitude > b.sqrMagnitude ? a : b;

	public static Vector3 Greatest(Vector3 a, Vector3 b, Vector3 c)
		=> a.sqrMagnitude > b.sqrMagnitude
			? Greatest(a, c)
			: Greatest(b, c);

	public static float Avg(this (float, float, float) floats)
	{
		var (a, b, c) = floats;
		return (a + b + c) / 3;
	}

	public static Vector3 Avg(this (Vector3, Vector3, Vector3) vectors)
	{
		var (a, b, c) = vectors;
		return (a + b + c) / 3;
	}


	/// Fast distance check, returns true if within Distance
	public static bool DistanceWithin(Vector3 source, Vector3 target, float distance)
	{
		return (target - source).sqrMagnitude < distance * distance;
	}

	/// Return the spheres and the radius of the given capsule (useful for physic tests with capsules)
	public static void GetCapsuleBounds(
		CapsuleCollider capsuleCollider,
		Vector3 position,
		out Vector3 bottom,
		out Vector3 top,
		out float radius
	)
	{
		Vector3 ls = capsuleCollider.transform.lossyScale;
		Vector3 direction = capsuleCollider.transform.up;
		float rScale = Mathf.Max(Mathf.Abs(ls.x), Mathf.Abs(ls.z));

		if (capsuleCollider.direction == 0) {
			direction = capsuleCollider.transform.right;
			rScale = Mathf.Max(Mathf.Abs(ls.y), Mathf.Abs(ls.z));
		}
		else if (capsuleCollider.direction == 2) {
			direction = capsuleCollider.transform.forward;
			rScale = Mathf.Max(Mathf.Abs(ls.x), Mathf.Abs(ls.y));
		}

		Vector3 toCenter = capsuleCollider.transform.TransformDirection(
			new Vector3(
				capsuleCollider.center.x * ls.x,
				capsuleCollider.center.y * ls.y,
				capsuleCollider.center.z * ls.z
			)
		);
		Vector3 center = position + toCenter;
		radius = capsuleCollider.radius * rScale;
		float halfHeight = capsuleCollider.height
		                 * Mathf.Abs(ls[capsuleCollider.direction])
		                 * 0.5f;

		if (radius < halfHeight) {
			bottom = center - direction * (halfHeight - radius);
			top = center + direction * (halfHeight - radius);
		}
		else {
			bottom = top = center;
		}
	}
}

public enum ValueComparisonType
{
	MIN_OR_ABOVE, // val >= min
	ABOVE_MIN, // val > min
	MAX_OR_BELOW, // val <= max
	BELOW_MAX, // val < max
	MIN_OR_ABOVE__MAX_OR_BELOW, // min <= val <= max
	ABOVE_MIN__BELOW_MAX, // min < val < max
	MIN_OR_ABOVE__BELOW_MAX, // min <= val < max 
	ABOVE_MIN__MAX_OR_BELOW, // min < val <= max 
	EQUAL_MIN, // val == min
	EQUAL_MAX, // val == max

	// RANGE_INCL, // min <= val <= max
	// RANGE_EXCL, // min < val < max
	// RANGE_INCL_MIN_EXCL_MAX, // min <= val < max 
	// RANGE_EXCL_MIN_INCL_MAX, // min < val <= max 
}

public static class ComparisonUtils
{
	public static bool Check(
		this ValueComparisonType type,
		float val,
		float min,
		float max,
		float equalityTolerance = 0.00001f
	)
		=> type switch {
			ValueComparisonType.MIN_OR_ABOVE => min <= val,
			ValueComparisonType.ABOVE_MIN => min < val,
			ValueComparisonType.MAX_OR_BELOW => val <= max,
			ValueComparisonType.BELOW_MAX => val < max,
			ValueComparisonType.MIN_OR_ABOVE__MAX_OR_BELOW => min <= val && val <= max,
			ValueComparisonType.ABOVE_MIN__BELOW_MAX => min < val && val < max,
			ValueComparisonType.MIN_OR_ABOVE__BELOW_MAX => min <= val && val < max,
			ValueComparisonType.ABOVE_MIN__MAX_OR_BELOW => min < val && val <= max,
			ValueComparisonType.EQUAL_MIN => Math.Abs(val - min) < equalityTolerance,
			ValueComparisonType.EQUAL_MAX => Math.Abs(val - max) < equalityTolerance,
			_ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
		};
}
}