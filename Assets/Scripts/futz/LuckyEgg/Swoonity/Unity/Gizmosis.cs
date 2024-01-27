using UnityEngine;

namespace Swoonity.Unity
{
/* TODO: see GizMe */

public static class Gizmosis
{
	public static void DrawRay(
		this Color color,
		Vector3 origin,
		Vector3 direction,
		float magnitude = 1
	)
	{
		Gizmos.color = color;
		Gizmos.DrawRay(origin, direction * magnitude);
	}

	public static void DrawRay(this Color color, Ray ray, float magnitude = 1)
	{
		color.DrawRay(ray.origin, ray.direction, magnitude);
	}

	public static void DrawRayWithCap(
		this Color color,
		Vector3 origin,
		Vector3 direction,
		float magnitude = 1,
		float capRadius = .1f
	)
	{
		Gizmos.color = color;
		var end = origin + (direction * magnitude);
		Gizmos.DrawLine(origin, end);
		Gizmos.DrawSphere(end, capRadius);
	}

	public static void DrawSphere(this Color color, Vector3 position, float radius = 0.1f)
	{
		Gizmos.color = color;
		Gizmos.DrawSphere(position, radius);
	}

	public static void DrawCube(this Color color, Vector3 position, Vector3 size)
	{
		Gizmos.color = color;
		Gizmos.DrawCube(position, size);
	}

	public static void DrawWireCube(this Color color, Vector3 position, Vector3 size)
	{
		Gizmos.color = color;
		Gizmos.DrawWireCube(position, size);
	}
}
}