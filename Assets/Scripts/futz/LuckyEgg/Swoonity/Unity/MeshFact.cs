using System;
using System.Collections.Generic;
using UnityEngine;
using V2 = UnityEngine.Vector2;
using V3 = UnityEngine.Vector3;
using V4 = UnityEngine.Vector4;

namespace Swoonity.Unity
{
[Serializable]
public class MeshFact
{
	public List<V3> Verts;
	public List<V3> Normals;
	public List<TriFact> Tris;
	public List<FaceFact> Faces;
	public Bounds Bounds;
}

[Serializable]
public class FaceFact
{
	public string Name;
	public V3 Normal;
	public V3 TanRight;
	public V3 TanUp;
	public Bounds Bounds;
	public V3 Size;
	public float Width;
	public float Height;
	public float Area;
	/// local to mesh transform
	public V3 Center;
	public List<TriFact> Tris = new();
	public List<V2> FacePoints = new();

	public V3 AccumulatorCenter;

	public override string ToString() => Name;
}

[Serializable]
public struct TriFact
{
	public int TriangleIndex;
	public (int iA, int iB, int iC) Indexes;
	public (V3 pA, V3 pB, V3 pC) Points;
	public V3 Normal;
	public V3 Center;
	public V3 TanRight;
	public V3 TanUp;

	public static TriFact FromMeshTris(
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

		var normal = (normals[iA] + normals[iB] + normals[iC]) / 3;

		var (tanRight, tanUp) = normal.GetNormalTangents();

		return new TriFact {
			TriangleIndex = triDex,
			Indexes = (iA, iB, iC),
			Points = (pA, pB, pC),
			Center = (pA + pB + pC) / 3,
			Normal = normal,
			TanRight = tanRight,
			TanUp = tanUp,
		};
	}
}

[Serializable]
public struct EdgeFact
{
	public int Origin; // vert index
	public int NextEdge; // edge index
}

// edges & edge connections, or just use raycasts instead?
// public Dictionary<(V3, V3), TempFace> Edges = new Dictionary<(V3, V3), TempFace>();
}