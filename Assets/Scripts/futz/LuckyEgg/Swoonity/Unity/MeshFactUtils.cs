using System;
using System.Collections.Generic;
using Swoonity.CSharp;
using UnityEngine;
using V3 = UnityEngine.Vector3;
using V4 = UnityEngine.Vector4;

namespace Swoonity.Unity
{
public static class MeshFactUtils
{
	/// requires convex (relies on unique normals)
	/// TODO: non-convex, maybe check normal AND any shared verts
	public static MeshFact MakeFact(this Mesh mesh, float normalSameThreshold = 0.001f)
	{
		var vertCount = mesh.vertexCount;

		var verts = new List<Vector3>(vertCount);
		mesh.GetVertices(verts);

		var normals = new List<Vector3>(vertCount);
		mesh.GetNormals(normals);

		var triangles = mesh.triangles;
		var triCount = triangles.Length / 3;
		var tris = new List<TriFact>(triCount);
		var faces = new List<FaceFact>();

		for (var triDex = 0; triDex < triangles.Length; triDex += 3) {
			var iA = triangles[triDex];
			var iB = triangles[triDex + 1];
			var iC = triangles[triDex + 2];

			var pA = verts[iA];
			var pB = verts[iB];
			var pC = verts[iC];

			var normal = (normals[iA] + normals[iB] + normals[iC]) / 3;

			var (tanRight, tanUp) = normal.GetNormalTangents();

			var tri = new TriFact {
				TriangleIndex = triDex,
				Indexes = (iA, iB, iC),
				Points = (pA, pB, pC),
				Center = (pA + pB + pC) / 3,
				Normal = normal,
				TanRight = tanRight,
				TanUp = tanUp,
			};

			tris.Add(tri);

			FaceFact face = null;

			foreach (var existingFace in faces) {
				var diff = normal.Dot(existingFace.Normal);
				if (diff + normalSameThreshold > 1f) {
					face = existingFace;
					break;
				}
			}

			if (face != null) {
				// combine
				face.Normal = (normal + face.Normal) / 2f;
				face.Tris.Add(tri);
				face.Bounds.Encapsulate(tri.Points);
				face.AccumulatorCenter += tri.Center;
				continue;
			}

			// new face
			face = new FaceFact {
				Normal = tri.Normal,
				Bounds = tri.Points.GetBounds(),
				AccumulatorCenter = tri.Center,
			};
			face.Tris.Add(tri);
			faces.Add(face);
		}


		foreach (var face in faces) {
			var faceCenter = face.AccumulatorCenter / face.Tris.Count;

			var (tanRight, tanUp) = face.Normal.GetNormalTangents();

			foreach (var tri in face.Tris) {
				var (pA, pB, pC) = tri.Points;
				var fpA = (pA - faceCenter).GetPlanarPoint(tanRight, tanUp);
				var fpB = (pB - faceCenter).GetPlanarPoint(tanRight, tanUp);
				var fpC = (pC - faceCenter).GetPlanarPoint(tanRight, tanUp);
				face.FacePoints.Add(fpA);
				face.FacePoints.Add(fpB);
				face.FacePoints.Add(fpC);
			}

			var rectBounds = face.FacePoints.RectBounds();
			face.Width = rectBounds.width;
			face.Height = rectBounds.height;
			face.Area = face.Width * face.Height;
			face.Center = faceCenter;
			face.TanRight = tanRight;
			face.TanUp = tanUp;
		}

		faces.Sort(static (a, b) => (b.Area - a.Area).ConvertToSign());

		for (var index = 0; index < faces.Count; index++) {
			var face = faces[index];
			face.ApplyFaceName(index);
		}

		return new MeshFact {
			Verts = verts,
			Normals = normals,
			Tris = tris,
			Faces = faces,
			Bounds = mesh.bounds,
		};
	}

	public static void ApplyFaceName(this FaceFact face, int index)
	{
		var width = face.Width.R1();
		var height = face.Height.R1();
		var dir = face.Normal.ToDirectionString();
		face.Name = $"{index}{dir} ({width} x {height})";
	}


	/// TODO: check rotation?
	public static MeshFact MakeFact(this BoxCollider box, bool fillAll = false)
	{
		if (fillAll) {
			throw new NotImplementedException($"TODO: full MeshFact for BoxCollider");
		}

		var center = box.center;
		var size = box.size;
		var extents = size / 2;
		var min = center - extents;
		var max = center + extents;

		var faces = new List<FaceFact>(6);

		var ldb = new V3(min.x, min.y, min.z);
		var ldf = new V3(min.x, min.y, max.z);
		var lub = new V3(min.x, max.y, min.z);
		var luf = new V3(min.x, max.y, max.z);
		var rdb = new V3(max.x, min.y, min.z);
		var rdf = new V3(max.x, min.y, max.z);
		var rub = new V3(max.x, max.y, min.z);
		var ruf = new V3(max.x, max.y, max.z);

		faces.Add(MakeBoxFace(V3.left, (ldb, ldf, lub, luf), 0));
		faces.Add(MakeBoxFace(V3.right, (rdb, rdf, rub, ruf), 1));
		faces.Add(MakeBoxFace(V3.down, (ldb, ldf, rdb, rdf), 2));
		faces.Add(MakeBoxFace(V3.up, (lub, luf, rub, ruf), 3));
		faces.Add(MakeBoxFace(V3.back, (ldb, lub, rdb, rub), 4));
		faces.Add(MakeBoxFace(V3.forward, (ldf, luf, rdf, ruf), 5));

		return new MeshFact {
			Faces = faces,
			Bounds = new Bounds(center, size),
		};
	}

	public static FaceFact MakeBoxFace(V3 normal, (V3 pA, V3 pB, V3 pC, V3 pD) points, int index)
	{
		var face = new FaceFact();

		var (pA, pB, pC, pD) = points;
		var (tanRight, tanUp) = normal.GetNormalTangents();

		face.AccumulatorCenter = pA + pB + pC + pD;
		var faceCenter = face.AccumulatorCenter / 4;

		var fpA = (pA - faceCenter).GetPlanarPoint(tanRight, tanUp);
		var fpB = (pB - faceCenter).GetPlanarPoint(tanRight, tanUp);
		var fpC = (pC - faceCenter).GetPlanarPoint(tanRight, tanUp);
		var fpD = (pD - faceCenter).GetPlanarPoint(tanRight, tanUp);
		face.FacePoints.Add(fpA);
		face.FacePoints.Add(fpB);
		face.FacePoints.Add(fpC);
		face.FacePoints.Add(fpD);

		var rectBounds = face.FacePoints.RectBounds();
		face.Width = rectBounds.width;
		face.Height = rectBounds.height;
		face.Area = face.Width * face.Height;
		face.Center = faceCenter;
		face.Normal = normal;
		face.TanRight = tanRight;
		face.TanUp = tanUp;
		face.Bounds = points.GetBounds();

		face.ApplyFaceName(index);
		return face;
	}


	public static MeshFact MakeFact(this Collider collider)
		=> collider switch {
			BoxCollider box => box.MakeFact(),
			MeshCollider mesh => mesh.sharedMesh.MakeFact(),
			// CapsuleCollider capsule => ,
			// SphereCollider sphere => ,
			_ => throw new ArgumentOutOfRangeException(nameof(collider))
		};

	/// requires local normal
	public static FaceFact GetFaceFromNormal(
		this MeshFact meshFact,
		V3 localNormal,
		float normalSameThreshold = 0.001f
	)
	{
		foreach (var face in meshFact.Faces) {
			var diff = localNormal.Dot(face.Normal);
			if (diff + normalSameThreshold > 1f) {
				return face;
			}
		}

		return null;
	}

	// /// assumes local normal unless "relativeTo" is included
	// public static FaceFact GetFaceFromNormal(
	// 	this MeshFact meshFact,
	// 	V3 normal,
	// 	Transform relativeTo = null,
	// 	float normalSameThreshold = 0.001f
	// ) {
	// 	if (relativeTo) normal = relativeTo.LocalDirectionOf(normal);
	//
	// 	foreach (var face in meshFact.Faces) {
	// 		var diff = normal.Dot(face.Normal);
	// 		if (diff + normalSameThreshold > 1f) {
	// 			return face;
	// 		}
	// 	}
	//
	// 	return null;
	// }
	//
	// public static FaceFact GetHitFace(
	// 	this MeshFact meshFact,
	// 	RaycastHit hit,
	// 	Transform relativeTo,
	// 	float normalSameThreshold = 0.001f
	// )
	// 	=> meshFact.GetFaceFromNormal(hit.normal, relativeTo, normalSameThreshold);
	//
	// /// relativeTo == null ? local : world
	// public static V3 GetCenter(this FaceFact face, Transform relativeTo = null)
	// 	=> relativeTo ? relativeTo.WorldPointOf(face.Center) : face.Center;
	//
	// /// relativeTo == null ? local : world
	// public static V3 GetTanRight(this FaceFact face, Transform relativeTo = null)
	// 	=> relativeTo ? relativeTo.WorldDirectionOf(face.TanRight) : face.TanRight;
	//
	// /// relativeTo == null ? local : world
	// public static V3 GetTanUp(this FaceFact face, Transform relativeTo = null)
	// 	=> relativeTo ? relativeTo.WorldDirectionOf(face.TanUp) : face.TanUp;
	//
	// /// relativeTo == null ? local : world
	// public static V3 GetNormal(this FaceFact face, Transform relativeTo = null)
	// 	=> relativeTo ? relativeTo.WorldDirectionOf(face.Normal) : face.Normal;

	public static bool IsSameSize(
		this FaceFact faceA,
		FaceFact faceB,
		bool ignoreOrientation = false
	)
	{
		var isExactSame = faceA.Width.Approx(faceB.Width) && faceA.Height.Approx(faceB.Height);
		if (isExactSame) return true;

		return ignoreOrientation
		    && faceA.Width.Approx(faceB.Height)
		    && faceA.Height.Approx(faceB.Width);
	}

	public static bool IsSameSize(
		this FaceFact faceA,
		bool aRotated,
		FaceFact faceB,
		bool bRotated
	)
	{
		return aRotated == bRotated
			? faceA.Width.Approx(faceB.Width) && faceA.Height.Approx(faceB.Height)
			: faceA.Height.Approx(faceB.Width) && faceA.Width.Approx(faceB.Height);
	}

	public static bool IsSameSize(this FaceFact faceA, float yRotA, FaceFact faceB, float yRotB)
	{
		return IsSizeRotated(yRotA) == IsSizeRotated(yRotB)
			? faceA.Width.Approx(faceB.Width) && faceA.Height.Approx(faceB.Height)
			: faceA.Height.Approx(faceB.Width) && faceA.Width.Approx(faceB.Height);
	}

	public static bool IsSizeRotated(float yRot)
	{
		return (yRot.RoundTo(90).Abs().RoundToInt() / 90).IsOdd();
	}

	// TODO: is this just for faces on top/bottom?
	public static bool IsSizeRotated(this FaceFact face, float yRot) => IsSizeRotated(yRot);
}
}