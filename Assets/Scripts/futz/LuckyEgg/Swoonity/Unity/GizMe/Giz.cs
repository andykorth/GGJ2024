using System.Collections.Generic;
using UnityEngine;

namespace Swoonity.Unity
{
public static class Giz
{
	public static void Color(Color color) => Gizmos.color = color;

	public static Matrix4x4 LastMatrix;

	public static void Matrix(Matrix4x4 matrix)
	{
		LastMatrix = Gizmos.matrix;
		Gizmos.matrix = matrix;
	}

	public static void Matrix(Transform tform) => Matrix(tform.localToWorldMatrix);

	public static void Reset()
	{
		Gizmos.matrix = LastMatrix;
	}

	public static void Sphere(Vector3 center, float radius = .1f)
		=> Gizmos.DrawSphere(center, radius);

	public static void WireSphere(Vector3 center, float radius = .1f)
		=> Gizmos.DrawWireSphere(center, radius);

	public static void Cube(Vector3 center, Vector3 size) => Gizmos.DrawCube(center, size);

	public static void Cube(Vector3 center, float size = .2f)
		=> Gizmos.DrawCube(center, Vector3.one * size);

	public static void WireCube(Vector3 center, Vector3 size) => Gizmos.DrawWireCube(center, size);

	public static void WireCube(Vector3 center, float size = .2f)
		=> Gizmos.DrawWireCube(center, Vector3.one * size);

	public static void Line(Vector3 start, Vector3 end) => Gizmos.DrawLine(start, end);

	public static void Ray(Vector3 origin, Vector3 direction, float length = 1f)
		=> Gizmos.DrawRay(origin, direction * length);

	public static void Ray(Ray ray, float length = 1f)
		=> Gizmos.DrawRay(ray.origin, ray.direction * length);


	public static void RayCap(
		Vector3 origin,
		Vector3 direction,
		float length = 1f,
		float radius = .1f
	)
	{
		Ray(origin, direction, length);
		Sphere(origin + (direction * length), radius);
	}


	public static void Arrow(
		Vector3 origin,
		Vector3 direction,
		float length = 1f,
		float headLength = .25f,
		float headAngle = 20f
	)
	{
		Ray(origin, direction, length);
		var dir = direction * length;
		if (dir.IsZero()) {
			dir = Vector3.down;
			length = 10f;
		}

		var right = Quaternion.LookRotation(dir)
		          * Quaternion.Euler(0, 180 + headAngle, 0)
		          * Vector3.forward
		          * length;
		var left = Quaternion.LookRotation(dir)
		         * Quaternion.Euler(0, 180 - headAngle, 0)
		         * Vector3.forward
		         * length;
		Ray(origin + dir, right * headLength);
		Ray(origin + dir, left * headLength);
	}


	public static void Barbell(Vector3 start, Vector3 end, float radius = .1f)
	{
		Sphere(start, radius);
		Line(start, end);
		Sphere(end, radius);
	}

	public static void Bounds(Bounds bounds) => Gizmos.DrawWireCube(bounds.center, bounds.size);


	public static void Spheres(List<Vector3> centers, float radius = .1f)
	{
		foreach (var center in centers) {
			Gizmos.DrawSphere(center, radius);
		}
	}
}
}