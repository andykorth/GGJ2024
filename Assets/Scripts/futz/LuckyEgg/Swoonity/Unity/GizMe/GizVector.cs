using System.Collections.Generic;
using UnityEngine;
using V3 = UnityEngine.Vector3;

namespace Swoonity.Unity
{
public static class GizVector
{
	public static void Ray(this V3 center, V3 direction, float length = .1f)
	{
		Gizmos.DrawRay(center, direction * length);
	}

	public static void Ray(this V3 center, V3 direction, Color color, float length = .1f)
	{
		Gizmos.color = color;
		Gizmos.DrawRay(center, direction * length);
	}

	public static void SphereRay(
		this V3 center,
		V3 direction,
		Color color,
		float radius = .1f,
		float length = .1f
	)
	{
		Gizmos.color = color;
		Gizmos.DrawSphere(center, radius);
		Gizmos.DrawRay(center, direction * length);
	}

	public static void Arrow(
		this V3 center,
		V3 direction,
		float length = .1f,
		float headLength = .25f,
		float headAngle = 20f
	)
	{
		Giz.Arrow(center, direction, length, headLength, headAngle);
	}

	public static void Arrow(
		this V3 center,
		V3 direction,
		Color color,
		float length = .1f,
		float headLength = .25f,
		float headAngle = 20f
	)
	{
		Gizmos.color = color;
		Giz.Arrow(center, direction, length, headLength, headAngle);
	}

	public static void Sphere(this V3 center, float radius = .1f)
	{
		Gizmos.DrawSphere(center, radius);
	}

	public static void Sphere(this V3 center, Color color, float radius = .1f)
	{
		Gizmos.color = color;
		Gizmos.DrawSphere(center, radius);
	}

	public static void Spheres(
		this List<V3> centers,
		Color color,
		float radius = .1f,
		V3 offset = default
	)
	{
		Gizmos.color = color;
		foreach (var center in centers) {
			Gizmos.DrawSphere(center + offset, radius);
		}
	}

	public static void Cube(this V3 center, float size = .1f)
	{
		Gizmos.DrawCube(center, V3.one * size);
	}

	public static void Cube(this V3 center, Color color, float size = .1f)
	{
		Gizmos.color = color;
		Gizmos.DrawCube(center, V3.one * size);
	}

	public static void Cube(this V3 center, V3 size)
	{
		Gizmos.DrawCube(center, size);
	}

	public static void Cube(this V3 center, Color color, V3 size)
	{
		Gizmos.color = color;
		Gizmos.DrawCube(center, size);
	}

	public static void WireCube(this V3 center, float size = .1f)
	{
		Gizmos.DrawWireCube(center, V3.one * size);
	}

	public static void WireCube(this V3 center, Color color, float size = .1f)
	{
		Gizmos.color = color;
		Gizmos.DrawWireCube(center, V3.one * size);
	}

	public static void WireCube(this V3 center, V3 size)
	{
		Gizmos.DrawWireCube(center, size);
	}

	public static void WireCube(this V3 center, Color color, V3 size)
	{
		Gizmos.color = color;
		Gizmos.DrawWireCube(center, size);
	}

	public static void Cubes(
		this List<V3> centers,
		Color color,
		float size = .1f,
		V3 offset = default
	)
	{
		Gizmos.color = color;
		foreach (var center in centers) {
			Gizmos.DrawCube(center + offset, V3.one * size);
		}
	}
}
}