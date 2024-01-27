using UnityEngine;
using V3 = UnityEngine.Vector3;

namespace Swoonity.Unity
{
public static class BoundsUtils
{
	public static float GetHeight(this Bounds bounds) => bounds.size.y;

	public static Bounds GetBounds(this (V3 a, V3 b, V3 c) points)
	{
		var bounds = new Bounds(points.a, V3.zero);
		bounds.Encapsulate(points.b);
		bounds.Encapsulate(points.c);
		return bounds;
	}

	public static Bounds GetBounds(this (V3 a, V3 b, V3 c, V3 d) points)
	{
		var bounds = new Bounds(points.a, V3.zero);
		bounds.Encapsulate(points.b);
		bounds.Encapsulate(points.c);
		bounds.Encapsulate(points.d);
		return bounds;
	}

	public static void Encapsulate(this Bounds bounds, (V3 a, V3 b, V3 c) points)
	{
		bounds.Encapsulate(points.a);
		bounds.Encapsulate(points.b);
		bounds.Encapsulate(points.c);
	}

	public static bool IsBigger(this Bounds boundsA, Bounds boundsB)
		=> boundsA.size.sqrMagnitude > boundsB.size.sqrMagnitude;
}
}