using UnityEngine;

namespace Swoonity.Unity
{
public static class DebugUtils
{
	public static void DebugDraw(
		this Ray ray,
		float length = 1f,
		Color color = default,
		float duration = 5f
	)
		=> Debug.DrawRay(ray.origin, ray.direction * length, color.Or(Color.red), duration);

	public static void DebugDraw(
		this (Vector3 origin, Vector3 direction) ray,
		float length = 1f,
		Color color = default,
		float duration = 5f
	)
		=> Debug.DrawRay(ray.origin, ray.direction * length, color.Or(Color.red), duration);

	public static void DebugDraw(
		this PhysRay ray,
		Color color = default,
		float duration = 5f
	)
		=> Debug.DrawRay(ray.Origin, ray.Direction * ray.Length, color.Or(Color.red), duration);
}
}