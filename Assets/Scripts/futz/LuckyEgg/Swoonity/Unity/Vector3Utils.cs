using System;
using System.Collections.Generic;
using Swoonity.Collections;
using Swoonity.CSharp;
using UnityEngine;
using V2 = UnityEngine.Vector2;
using V3 = UnityEngine.Vector3;
using Random = UnityEngine.Random;

namespace Swoonity.Unity
{
public static class Vector3Utils
{
	public static void Deconstruct(
		this V3 vector,
		out float x,
		out float y,
		out float z
	)

	{
		x = vector.x;
		y = vector.y;
		z = vector.z;
	}

	/// UNCLAMPED lerp
	public static V3 Lerp(this (V3 start, V3 end) vecs, float t)
		=> V3.LerpUnclamped(vecs.start, vecs.end, t);

	// public static string ToFullString(this V3 v) => $"({v.x}, {v.y}, {v.z})";
	public static string ToFullString(this V3 v) => v.ToString("F8");

	public static bool InRange(this V3 from, V3 to, float distance)
		=> (to - from).sqrMagnitude <= distance * distance;

	public static bool FartherThan(this V3 from, V3 to, float distance)
		=> (to - from).sqrMagnitude > distance * distance;

	/// less/equal to distance (uses sqrMagnitude)
	public static bool Within(this (V3 from, V3 to) vecs, float distance)
		=> (vecs.to - vecs.from).sqrMagnitude <= distance * distance;

	/// absolute distance between a and b
	public static float Distance(this (V3 a, V3 b) vecs) => V3.Distance(vecs.a, vecs.b);

	/// distance square magnitude (cheaper than Distance)
	public static float DistanceSqr(this (V3 a, V3 b) vecs) => (vecs.b - vecs.a).sqrMagnitude;

	/// halfway point between a and b
	public static V3 Mid(this V3 a, V3 b) => (a + b) / 2;

	/// halfway point between a and b
	public static V3 Mid(this (V3 a, V3 b) vecs) => (vecs.a + vecs.b) / 2;

	/// halfway point between a, b, c
	public static V3 Mid(this (V3 a, V3 b, V3 c) vecs) => (vecs.a + vecs.b + vecs.c) / 3;

	/// halfway point between a, b, c, d
	public static V3 Mid(this (V3 a, V3 b, V3 c, V3 d) vecs)
		=> (vecs.a + vecs.b + vecs.c + vecs.d) / 4;

	/// move point AWAY from given normal/up direction
	public static V3 Sink(this V3 point, V3 normalOrUp, float sinkAmount)
		=> point + (-normalOrUp * sinkAmount);

	public static (V3 start, V3 end) Lengthen(this (V3 start, V3 end) vecs, float amount)
	{
		var (start, end) = vecs;
		var dir = (end - start).normalized;
		var add = dir * amount;
		return (start - add, end + add);
	}

	public static bool Approx(this V2 a, V2 b)
		=> a.x.Approx(b.x)
		&& a.y.Approx(b.y);

	public static bool Approx(this V3 a, V3 b)
		=> a.x.Approx(b.x)
		&& a.y.Approx(b.y)
		&& a.z.Approx(b.z);

	public static float DistanceSqr(this V3 origin, V3 target) => (target - origin).sqrMagnitude;

	public static V3 FlipX(this V3 vec) => vec.WithX(-vec.x);
	public static V3 FlipY(this V3 vec) => vec.WithY(-vec.y);
	public static V3 FlipZ(this V3 vec) => vec.WithZ(-vec.z);

	public static V3 Avg(this List<V3> vectors)
	{
		if (vectors.Count == 0) return V3.zero;

		var accumulator = V3.zero;
		foreach (var vector in vectors) {
			accumulator += vector;
		}

		return accumulator / vectors.Count;
	}

	public static V3 Avg(this V3[] vectors)
	{
		if (vectors.Nil()) return V3.zero;

		var accumulator = V3.zero;
		foreach (var vector in vectors) {
			accumulator += vector;
		}

		return accumulator / vectors.Length;
	}

	public static Bounds Bounds(this List<V3> vals)
	{
		if (vals.Count == 0) return new Bounds();

		var bounds = new Bounds(vals[0], V3.zero);
		for (var i = 1; i < vals.Count; i++) {
			bounds.Encapsulate(vals[i]);
		}

		return bounds;
	}

	public static Bounds Bounds(this V3[] vals)
	{
		if (vals.Nil()) return new Bounds();

		var bounds = new Bounds(vals[0], V3.zero);
		for (var i = 1; i < vals.Length; i++) {
			bounds.Encapsulate(vals[i]);
		}

		return bounds;
	}

	/// get abs distance and direction From -> To
	public static (float distance, V3 direction) DistanceDirection(this (V3 from, V3 to) vecs)
	{
		var result = vecs.to - vecs.from;
		var magnitude = V3.Magnitude(result);
		if (magnitude <= 0) return (0, V3.zero);
		return (magnitude, result / magnitude);
	}

	// a.y * b.z  -  a.z * b.y,
	// a.z * b.x  -  a.x * b.z,
	// a.x * b.y  -  a.y * b.x
	/// <summary>get perpendicular direction (if a=X, b=Y, then c=Z)</summary>
	/// <remarks>LEFT HAND RULE: with left hand, make L and point middle finger forward. a=thumb, b=pointer, c=middle</remarks>
	public static V3 Cross(this V3 a, V3 b) => V3.Cross(a, b);

	public static V3 CrossLeft(this V3 v) => V3.Cross(v, V3.left);
	public static V3 CrossRight(this V3 v) => V3.Cross(v, V3.right);
	public static V3 CrossDown(this V3 v) => V3.Cross(v, V3.down);
	public static V3 CrossUp(this V3 v) => V3.Cross(v, V3.up);
	public static V3 CrossBack(this V3 v) => V3.Cross(v, V3.back);
	public static V3 CrossForward(this V3 v) => V3.Cross(v, V3.forward);

	/// <summary>get perpendicular direction (if a=X, b=Y, then c=Z)</summary>
	/// <remarks>LEFT HAND RULE: with left hand, make L and point middle finger forward. a=thumb, b=pointer, c=middle</remarks>
	public static V3 Cross(this (V3 a, V3 b) vecs) => V3.Cross(vecs.a, vecs.b);

	/// How similar two *NORMALIZED* vectors are to each other.
	/// 1: same direction, 0: perpendicular, -1: opposite
	/// <remarks>a.x * b.x + a.y * b.y + a.z * b.z</remarks>
	public static float Dot(this V3 a, V3 b) => V3.Dot(a, b);

	/// How similar two *NORMALIZED* vectors are to each other.
	/// 1: same direction, 0: perpendicular, -1: opposite
	/// <remarks>a.x * b.x + a.y * b.y + a.z * b.z</remarks>
	public static float Dot(this (Vector3 a, Vector3 b) vecs) => V3.Dot(vecs.a, vecs.b);

	public static float DotLeft(this V3 v) => V3.Dot(v, V3.left);
	public static float DotRight(this V3 v) => V3.Dot(v, V3.right);
	public static float DotDown(this V3 v) => V3.Dot(v, V3.down);
	public static float DotUp(this V3 v) => V3.Dot(v, V3.up);
	public static float DotBack(this V3 v) => V3.Dot(v, V3.back);
	public static float DotForward(this V3 v) => V3.Dot(v, V3.forward);

	/// planeNormal * dot(vec, onto)
	public static V3 ProjectOn(this V3 vec, V3 onto) => V3.Project(vec, onto);

	/// 
	public static float DistanceFromPlane(this V3 point, V3 anyPointOnPlane, V3 planeNormal)
		=> V3.Project(point - anyPointOnPlane, planeNormal).magnitude;


	public static Vector3 GetNormalTangent(this V3 normal)
		=> Maths.Greatest(
			normal.Cross(V3.forward),
			normal.Cross(V3.up)
		);

	public static Vector3 GetNormalBitan(this V3 normal)
		=> -normal.Cross(normal.GetNormalTangent());

	/// When you look at a face, the normal is pointed BACK at you.
	/// TANGENT: greater of normal.Cross(0, 0, 1) or normal.Cross(0, 1, 0)
	/// BITAN: -normal.Cross(TANGENT)
	public static (V3 tangent, V3 bitan) GetNormalTangents(this V3 normal)
	{
		var tanA = normal.Cross(V3.forward);
		var tanB = normal.Cross(V3.up);
		var tangent = tanA.magnitude > tanB.magnitude ? tanA : tanB;
		var bitan = -normal.Cross(tangent);
		return (tangent, bitan);
	}

	public static V2 GetPlanarPoint(this V3 point, V3 xDirection, V3 yDirection)
		=> new(point.Dot(xDirection), point.Dot(yDirection));

	/// <summary>
	/// Returns the vector3 with its magnitude clamped to ClampValue (Default: 1)
	/// magnitude is unsigned (absolute)
	/// </summary>
	public static V3 Clamp(this V3 vector, float clampValue = 1)
	{
		return V3.ClampMagnitude(vector, clampValue);
	}


	public static V3 WithX(this V3 vec, float val) => new(val, vec.y, vec.z);
	public static V3 WithY(this V3 vec, float val) => new(vec.x, val, vec.z);
	public static V3 WithZ(this V3 vec, float val) => new(vec.x, vec.y, val);

	public static V3 WithX(this V3 a, V3 b) => a.WithX(b.x);
	public static V3 WithY(this V3 a, V3 b) => a.WithY(b.y);
	public static V3 WithZ(this V3 a, V3 b) => a.WithZ(b.z);

	public static V3 XY(this V3 vec) => new(vec.x, vec.y, 0);
	public static V3 XZ(this V3 vec) => new(vec.x, 0, vec.z);
	public static V3 YZ(this V3 vec) => new(0, vec.y, vec.z);

	public static V3 AddX(this V3 vec, float val) => new(vec.x + val, vec.y, vec.z);
	public static V3 AddY(this V3 vec, float val) => new(vec.x, vec.y + val, vec.z);
	public static V3 AddZ(this V3 vec, float val) => new(vec.x, vec.y, vec.z + val);

	public static V3 Add(this V3 vec, V3 add) => vec + add;

	/// <summary>
	/// Returns V3 with xValue added to vector.x
	/// </summary>
	public static V3 WithAddedX(this V3 vector, float xValue)
	{
		vector.x += xValue;
		return vector;
	}

	/// <summary>
	/// Returns V3 with yValue added to vector.y
	/// </summary>
	public static V3 WithAddedY(this V3 vector, float yValue)
	{
		vector.y += yValue;
		return vector;
	}

	/// <summary>
	/// Returns V3 with zValue added to vector.z
	/// </summary>
	public static V3 WithAddedZ(this V3 vector, float zValue)
	{
		vector.z += zValue;
		return vector;
	}

	/// <summary>
	/// Returns V3 with added values
	/// </summary>
	public static V3 WithAdded(this V3 vector, float x = 0, float y = 0, float z = 0)
	{
		vector.x += x;
		vector.y += y;
		vector.z += z;
		return vector;
	}

	/// <summary>
	/// Round x, y, z, to nearest whole number
	/// </summary>
	public static V3 Round(this V3 vector)
	{
		vector.x = Mathf.Round(vector.x);
		vector.y = Mathf.Round(vector.y);
		vector.z = Mathf.Round(vector.z);
		return vector;
	}

	public static V3 RoundTo(this V3 vector, float amount)
		=> new V3(
			vector.x.RoundTo(amount),
			vector.y.RoundTo(amount),
			vector.z.RoundTo(amount)
		);

	/// Reduce vector by a percentage (with optional deltaTime multiplier)
	public static V3 Decay(this V3 vector, float percentDecay, float deltaTime = 1)
		=> vector * (1 - (percentDecay * deltaTime));

	/// Returns absolute distance to other vector
	public static float DistanceTo(this V3 startVector, V3 targetVector)
		=> V3.Distance(startVector, targetVector);

	public static bool IsZero(this V3 vector) => vector == Vector3.zero;
	public static bool NotZero(this V3 vector) => vector != Vector3.zero;

	public static V3 EnsureNotZero(this V3 vector)
		=> vector != Vector3.zero ? vector : new Vector3(0.0001f, 0.0001f, 0.0001f);

	public static V3 EnsureNotZero(this V3 vector, V3 or) => vector != Vector3.zero ? vector : or;
	public static V3 OrIfZero(this V3 vector, V3 or) => vector != Vector3.zero ? vector : or;

	/// if zero, returns Vector3.one (useful for resetting transform Scale, etc.)
	public static V3 OrOne(this V3 vector) => vector != Vector3.zero ? vector : V3.one;

	public static float AngleTo(this V3 from, V3 to) => V3.Angle(from, to);
	public static float AngleFrom(this V3 to, V3 from) => V3.Angle(from, to);


	/// this function already multiplies by deltaTime
	public static V3 MoveTowards(this V3 from, V3 to, float speedPerSec)
		=> V3.MoveTowards(from, to, speedPerSec * Time.deltaTime);

	/// Takes list of normalized vectors, returns vector with closest angle to given direction
	public static (V3 vector, float angle) ClosestVectorTo(
		this List<V3> vectors,
		V3 direction,
		(V3 vector, float angle) or = default
	)
	{
		var count = vectors.Count;
		if (count == 0) return or;

		var vectorToBeat = vectors[0];
		var angleToBeat = V3.Angle(direction, vectorToBeat);

		for (var i = 1; i < count; i++) {
			var vector = vectors[i];
			var angle = V3.Angle(direction, vector);

			if (angle < angleToBeat) {
				vectorToBeat = vector;
				angleToBeat = angle;
			}
		}

		return (vectorToBeat, angleToBeat);
	}

	/// Takes list of normalized vectors, returns vector with farthest angle to given direction
	public static (V3 vector, float angle) FarthestVectorTo(
		this List<V3> vectors,
		V3 direction,
		(V3 vector, float angle) or = default
	)
	{
		var count = vectors.Count;
		if (count == 0) return or;

		var vectorToBeat = vectors[0];
		var angleToBeat = V3.Angle(direction, vectorToBeat);

		for (var i = 1; i < count; i++) {
			var vector = vectors[i];
			var angle = V3.Angle(direction, vector);

			if (angle > angleToBeat) {
				vectorToBeat = vector;
				angleToBeat = angle;
			}
		}

		return (vectorToBeat, angleToBeat);
	}


	/// Converts direction (forward) to rotation (with V3.up upwards direction) 
	public static Quaternion ToLookRotation(this V3 direction) => direction.ToLookRotation(V3.up);

	/// Converts direction (forward) to rotation (with upwards direction) 
	public static Quaternion ToLookRotation(this V3 direction, V3 upwards)
		=> Quaternion.LookRotation(direction.normalized, V3.up);

	public static bool IsNormalLeftRight(this V3 normal) => normal.x.Abs().Approx(1);
	public static bool IsNormalDownUp(this V3 normal) => normal.y.Abs().Approx(1);
	public static bool IsNormalBackForward(this V3 normal) => normal.z.Abs().Approx(1);


	public static string ToDirectionString(this V3 dir)
	{
		var x = dir.x;
		var y = dir.y;
		var z = dir.z;
		var xAbs = Mathf.Abs(x);
		var yAbs = Mathf.Abs(y);
		var zAbs = Mathf.Abs(z);

		if (xAbs > yAbs) {
			if (xAbs > zAbs) {
				return x < 0
					? "left" + (x > -1 ? "-ish" : "")
					: "right" + (x < 1 ? "-ish" : "");
			}

			return z < 0
				? "back" + (z > -1 ? "-ish" : "")
				: "forward" + (z < 1 ? "-ish" : "");
		}

		if (yAbs > zAbs) {
			return y < 0
				? "down" + (y > -1 ? "-ish" : "")
				: "up" + (y < 1 ? "-ish" : "");
		}

		return z < 0
			? "back" + (z > -1 ? "-ish" : "")
			: "forward" + (z < 1 ? "-ish" : "");
	}
}


[Serializable]
public struct Index2
{
	public int x;
	public int y;

	public Index2(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public Index2(float x, float y) : this(Mathf.RoundToInt(x), Mathf.RoundToInt(y)) { }
	public Index2(V2 vector) : this(vector.x, vector.y) { }

	public static bool operator ==(Index2 a, Index2 b)
	{
		return a.Equals(b);
	}

	public static bool operator !=(Index2 a, Index2 b)
	{
		return !a.Equals(b);
	}

	public override bool Equals(object obj)
	{
		return base.Equals(obj); // TODO: Fix this?
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public int RandomWithin()
	{
		return UnityEngine.Random.Range(x, y + 1);
	}

	public static Index2 One = new Index2(1, 1);
}
}


// public enum OrientTo {
// 	NORMAL,
// 	TANGENT, // greater of normal.Cross(0, 0, 1) or normal.Cross(0, 1, 0)
// 	BITAN, // -normal.Cross(TAN_RIGHT)
// }