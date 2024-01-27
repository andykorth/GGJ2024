using System.Collections.Generic;
using Swoonity.CSharp;
using UnityEngine;
using V2 = UnityEngine.Vector2;

namespace Swoonity.Unity
{
public static class Vector2Utils
{
	public static void Deconstruct(this Vector2 vector, out float x, out float y)
	{
		x = vector.x;
		y = vector.y;
	}

	public static Vector2 Direction2d(this float angle)
	{
		var radian = angle * Mathf.Deg2Rad;
		return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
	}

	/// <summary>
	/// Returns the V2 with its magnitude clamped to ClampValue (Default: 1)
	/// </summary>
	public static V2 Clamp(this V2 vector, float clampValue = 1)
	{
		return V2.ClampMagnitude(vector, clampValue);
	}

	/// <summary>
	/// Converts a V2 to a V3, but places the Y in the Z (and makes resulting Y = 0 or optional override)
	/// </summary>
	public static Vector3 ToV3XZ(this V2 vector, float yOverride = 0)
	{
		return new Vector3(vector.x, yOverride, vector.y);
	}

	/// <summary>
	/// Returns true if both X and Y are within Min and Max.
	/// </summary>
	public static bool IsWithin(this V2 vector, float min, float max)
	{
		return vector.x.IsWithin(min, max)
		    && vector.y.IsWithin(min, max);
	}

	/// <summary>
	/// Returns V2 with new X Value
	/// </summary>
	public static V2 WithX(this V2 vector, float xValue)
	{
		vector.x = xValue;
		return vector;
	}

	/// <summary>
	/// Returns V2 with new Y Value
	/// </summary>
	public static V2 WithY(this V2 vector, float yValue)
	{
		vector.y = yValue;
		return vector;
	}

	/// <summary>
	/// Returns V2 with xValue added to vector.x
	/// </summary>
	public static V2 WithAddedX(this V2 vector, float xValue)
	{
		vector.x += xValue;
		return vector;
	}

	/// <summary>
	/// Returns V2 with yValue added to vector.y
	/// </summary>
	public static V2 WithAddedY(this V2 vector, float yValue)
	{
		vector.y += yValue;
		return vector;
	}

	public static V2 RoundTo(this V2 vector, float amount)
		=> new(vector.x.RoundTo(amount), vector.y.RoundTo(amount));

	public static bool IsZero(this V2 vector) => vector.x.IsZero() && vector.y.IsZero();

	public static bool NotZero(this V2 vector) => vector.x.NotZero() || vector.y.NotZero();

	///
	public static V2 NormalizeToScreen(this V2 vector)
	{
		return new V2(vector.x / Screen.width, vector.y / Screen.height);
	}


	public static Rect RectBounds(this List<V2> vals)
	{
		if (vals == null || vals.Count == 0) return new Rect();
		// TODO: use Vector min max

		var leastX = float.MaxValue;
		var leastY = float.MaxValue;
		var mostX = float.MinValue;
		var mostY = float.MinValue;

		foreach (var val in vals) {
			if (val.x < leastX) leastX = val.x;
			if (val.x > mostX) mostX = val.x;

			if (val.y < leastY) leastY = val.y;
			if (val.y > mostY) mostY = val.y;
		}

		var least = new V2(leastX, leastY);
		var most = new V2(mostX, mostY);
		var size = most - least;
		var center = least + size / 2;
		return new Rect(center, size);
	}

	/// How similar two *NORMALIZED* vectors are to each other.
	/// 1: same direction, 0: perpendicular, -1: opposite
	/// <remarks>a.x * b.x + a.y * b.y</remarks>
	public static float Dot(this V2 a, V2 b) => V2.Dot(a, b);

	/// How similar two *NORMALIZED* vectors are to each other.
	/// 1: same direction, 0: perpendicular, -1: opposite
	/// <remarks>a.x * b.x + a.y * b.y</remarks>
	public static float Dot(this (V2 a, V2 b) vecs) => V2.Dot(vecs.a, vecs.b);

	public static float DotLeft(this V2 v) => V2.Dot(v, V2.left);
	public static float DotRight(this V2 v) => V2.Dot(v, V2.right);
	public static float DotDown(this V2 v) => V2.Dot(v, V2.down);
	public static float DotUp(this V2 v) => V2.Dot(v, V2.up);
}
}