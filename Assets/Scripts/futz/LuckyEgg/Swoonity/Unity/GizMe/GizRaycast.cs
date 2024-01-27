using System;
using UnityEngine;

namespace Swoonity.Unity
{
public static class GizRaycast
{
	[Serializable]
	public class Dat
	{
		public static Dat DEFAULT = new Dat();

		public Color RayColor = HUES.blazing_yellow;

		public Color HitPointColor = HUES.fiery_red;
		public float HitRadius = 0.1f;

		public Color NormalColor = HUES.norse_blue;
		public float NormalLength = 1.5f;
		public float NormalCapRadius = 0.05f;
	}

	public static void GizMe(
		this Ray ray,
		RaycastHit hit = default,
		float maxDistance = 999,
		Dat dat = null
	)
	{
		dat ??= Dat.DEFAULT;

		Giz.Color(dat.RayColor);
		Giz.Ray(ray.origin, ray.direction, maxDistance);

		if (hit.transform == null) return; // no hit

		Giz.Color(dat.HitPointColor);
		Giz.Sphere(hit.point, dat.HitRadius);

		Giz.Color(dat.NormalColor);
		Giz.RayCap(hit.point, hit.normal, dat.NormalLength, dat.NormalCapRadius);
		// Giz.Ray(hit.point, hit.normal, dat.NormalLength);
		// Giz.Sphere(hit.point + hit.normal * dat.NormalLength, dat.NormalCapRadius);

		// hit.collider
		// hit.triangleIndex
	}

	public static void GizMe(
		this PhysHit hit,
		Dat dat = null
	)
	{
		dat ??= Dat.DEFAULT;

		Giz.Color(dat.RayColor);
		Giz.Ray(hit.Ray.Origin, hit.Ray.Direction, hit.Ray.Length);

		if (hit.HitAny) return; // no hit

		Giz.Color(dat.HitPointColor);
		Giz.Sphere(hit.Point, dat.HitRadius);

		Giz.Color(dat.NormalColor);
		Giz.RayCap(hit.Point, hit.Normal, dat.NormalLength, dat.NormalCapRadius);
		// Giz.Ray(hit.point, hit.normal, dat.NormalLength);
		// Giz.Sphere(hit.point + hit.normal * dat.NormalLength, dat.NormalCapRadius);

		// hit.collider
		// hit.triangleIndex
	}

	public static void GizMe<T>(
		this PhysHit<T> hit,
		Dat dat = null
	) where T : Component
	{
		dat ??= Dat.DEFAULT;

		Giz.Color(dat.RayColor);
		Giz.Ray(hit.Ray.Origin, hit.Ray.Direction, hit.Ray.Length);

		if (hit.HitAny) return; // no hit

		Giz.Color(dat.HitPointColor);
		Giz.Sphere(hit.Point, dat.HitRadius);

		Giz.Color(dat.NormalColor);
		Giz.RayCap(hit.Point, hit.Normal, dat.NormalLength, dat.NormalCapRadius);
	}
}
}