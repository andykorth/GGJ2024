using System;
using System.Collections.Generic;
using Swoonity.Collections;
using Swoonity.CSharp;
using UnityEngine;

namespace Swoonity.Unity
{
public static class GizCollider
{
	[Serializable]
	public class Dat
	{
		public static Dat DEFAULT = new Dat();


		public bool DrawVerts = true;
		public Color VertColor = HUES.violet;
		public float VertRadius = 0.01f;

		public bool DrawNormals = true;
		public Color NormalColor = HUES.norse_blue;
		public float NormalLength = 0.1f;

		public bool DrawFaces = true;
		public Color FaceColor = HUES.green_glow;
		public float FaceLength = 0.1f;


		public int VertCount;
		public List<Vector3> Verts = new List<Vector3>();
		public List<Vector3> Normals = new List<Vector3>();
		public List<Vector3> VertsWorld = new List<Vector3>();
		public List<Vector3> NormalsWorld = new List<Vector3>();
		public List<(int a, int b, int c)> TriIndices = new List<(int, int, int)>();
		public List<(Vector3 a, Vector3 b, Vector3 c, Vector3 normal)> Tris =
			new List<(Vector3, Vector3, Vector3, Vector3)>();
		public List<(Vector3 a, Vector3 b, Vector3 c, Vector3 normal)> TrisWorld =
			new List<(Vector3, Vector3, Vector3, Vector3)>();
		public List<Ray> Faces = new List<Ray>();
		public List<Ray> FacesWorld = new List<Ray>();
	}


	public static void GizMe(this MeshCollider meshCollider, Dat dat = null)
	{
		dat ??= Dat.DEFAULT;

		dat.LoadMeshDat(meshCollider.sharedMesh, meshCollider.transform);

		dat.DrawAsdf();
	}

	static void LoadMeshDat(this Dat dat, Mesh mesh, Transform meshTf)
	{
		dat.VertCount = mesh.vertexCount;
		var verts = dat.Verts.ClearThen();
		var normals = dat.Normals.ClearThen();
		var vertsWorld = dat.VertsWorld.ClearThen();
		var normalsWorld = dat.NormalsWorld.ClearThen();
		var trisRaw = mesh.triangles;
		var triIndices = dat.TriIndices.ClearThen();
		var tris = dat.Tris.ClearThen();
		var trisWorld = dat.TrisWorld.ClearThen();
		var faces = dat.Faces.ClearThen();
		var facesWorld = dat.FacesWorld.ClearThen();

		mesh.GetVertices(verts);
		mesh.GetNormals(normals);

		for (var i = 0; i < dat.Verts.Count; i++) {
			vertsWorld.Add(meshTf.TransformPoint(verts[i]));
			normalsWorld.Add(meshTf.TransformDirection(normals[i]));
		}

		for (var i = 0; i < trisRaw.Length; i += 3) {
			var aDex = trisRaw[i];
			var bDex = trisRaw[i + 1];
			var cDex = trisRaw[i + 2];
			triIndices.Add((aDex, bDex, cDex));

			var a = verts[aDex];
			var b = verts[bDex];
			var c = verts[cDex];
			var facePos = (a, b, c).Avg();
			var faceNormal = (normals[aDex] + normals[bDex] + normals[cDex]) / 3;
			tris.Add((a, b, c, faceNormal));
			faces.Add(new Ray(facePos, faceNormal));

			var aWorld = vertsWorld[aDex];
			var bWorld = vertsWorld[bDex];
			var cWorld = vertsWorld[cDex];
			var facePosWorld = meshTf.TransformPoint(facePos);
			var faceNormalWorld = meshTf.TransformDirection(faceNormal);
			trisWorld.Add((aWorld, bWorld, cWorld, faceNormalWorld));
			facesWorld.Add(new Ray(facePosWorld, faceNormalWorld));
		}
	}

	static void DrawAsdf(this Dat dat)
	{
		Gizmos.color = dat.VertColor;
		for (var i = 0; i < dat.VertCount; i++) {
			var vertPos = dat.VertsWorld[i];
			Gizmos.DrawSphere(vertPos, dat.VertRadius);
		}
	}
}
}