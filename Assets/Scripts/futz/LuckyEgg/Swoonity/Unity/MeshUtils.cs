using System;
using System.Collections.Generic;
using Swoonity.Collections;
using Swoonity.CSharp;
using UnityEngine;
using V3 = UnityEngine.Vector3;
using V4 = UnityEngine.Vector4;

namespace Swoonity.Unity
{
// TODO: cleanup
public static class MeshUtils
{
	/// gets mesh's list of (split) vertices
	public static List<V3> GetVertList(this Mesh mesh)
	{
		var splitVertices = new List<V3>(mesh.vertexCount);
		mesh.GetVertices(splitVertices);
		return splitVertices;
	}

	public static List<V3> GetNormalList(this Mesh mesh)
	{
		var normals = new List<V3>(mesh.vertexCount);
		mesh.GetNormals(normals);
		return normals;
	}

	/// list of triangle indexes (each 3 indexes is a triangle)
	public static List<int> GetTriangleList(this Mesh mesh)
	{
		return mesh.triangles.ToList();
	}


	public static void ComputeTris(
		this Mesh mesh,
		List<V3> meshVerts,
		List<V3> meshNormals,
		List<V4> meshTangents,
		List<TriInfo> fillList
	)
	{
		var triangles = mesh.triangles;
		for (var triDex = 0; triDex < triangles.Length; triDex += 3) {
			fillList.Add(
				TriInfo.FromMeshTris(
					triDex,
					triangles,
					meshVerts,
					meshNormals
				)
			);
		}
	}

	public static void ComputeFaces(this List<TriInfo> tris, List<FaceInfo> fillList) { }


	/// requires 2 points (verts) in the same location
	public static bool ConnectsTo(this TriInfo tri1, TriInfo tri2)
	{
		var (pA, pB, pC) = tri1.Points;
		return tri2.HasPoint(pA)
			? tri2.HasPoint(pB) || tri2.HasPoint(pC)
			: tri2.HasPoint(pB) && tri2.HasPoint(pC);
	}

	public static bool HasPoint(this TriInfo tri, V3 point)
	{
		var (pA, pB, pC) = tri.Points;
		return point == pA || point == pB || point == pC;
	}

	/// note: assumes facing same general direction (0..180)
	public static bool SameNormalish(this TriInfo tri1, TriInfo tri2, float creaseTolerance = 0)
	{
		return Vector3.Angle(tri1.Normal, tri2.Normal).Approx(creaseTolerance);
	}

	public static bool SameNormal(this TriInfo tri1, TriInfo tri2)
		=> tri1.Normal.Approx(tri2.Normal);

	public static bool Coplanarish(this TriInfo tri1, TriInfo tri2, float creaseTolerance = 0)
		=> tri1.ConnectsTo(tri2) && tri1.SameNormalish(tri2);


	/// compare sharedMesh.bounds.size.sqrMagnitude
	public static MeshFilter BiggerApprox(MeshFilter a, MeshFilter b)
		=> a.sharedMesh.bounds.size.sqrMagnitude > b.sharedMesh.bounds.size.sqrMagnitude
			? a
			: b;
}

[Serializable]
public struct TriInfo
{
	public int TriangleIndex;
	public (int iA, int iB, int iC) Indexes;
	public (V3 pA, V3 pB, V3 pC) Points;
	public (V3 nA, V3 nB, V3 nC) Normals;
	public V3 Middle;
	public V3 Normal;
	public V3 TanRight;
	public V3 TanUp;
	public Bounds Bounds;

	public static TriInfo FromMeshTris(
		int triDex,
		int[] triangles,
		List<Vector3> verts,
		List<Vector3> normals
	)
	{
		var iA = triangles[triDex];
		var iB = triangles[triDex + 1];
		var iC = triangles[triDex + 2];

		var pA = verts[iA];
		var pB = verts[iB];
		var pC = verts[iC];

		var nA = normals[iA];
		var nB = normals[iB];
		var nC = normals[iC];
		var normal = (nA + nB + nC) / 3;

		// (when you look at a face, the normal is pointed BACK at you)
		// var tanA = normal.Cross(V3.forward);
		// var tanB = normal.Cross(V3.up);
		// var tanRight = tanA.magnitude > tanB.magnitude ? tanA : tanB;
		// var tanUp = -normal.Cross(tanRight);
		var (tanRight, tanUp) = normal.GetNormalTangents();

		var bounds = new Bounds(pA, V3.zero);
		bounds.Encapsulate(pB);
		bounds.Encapsulate(pC);

		return new TriInfo {
			TriangleIndex = triDex,
			Indexes = (iA, iB, iC),
			Points = (pA, pB, pC),
			Normals = (nA, nB, nC),
			Middle = (pA + pB + pC) / 3,
			Normal = normal,
			TanRight = tanRight,
			TanUp = tanUp,
			Bounds = bounds,
		};
	}
}

[Serializable]
public struct FaceInfo
{
	public V3 Center;
	public V3 Min;
	public V3 Max;
	public V3 Normal;

	public FaceInfo(V3 center, V3 min, V3 max, V3 normal)
	{
		Center = center;
		Min = min;
		Max = max;
		Normal = normal;
	}
}
}