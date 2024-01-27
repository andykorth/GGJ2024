using System;
using System.Collections.Generic;
using Swoonity.Collections;
using Swoonity.CSharp;
using UnityEngine;

namespace Swoonity.Unity
{
public static class GizMesh
{
	[Serializable]
	public class Dat
	{
		public static Dat DEFAULT = new Dat();


		public bool DoVerts = true;
		public Color VertColor = HUES.violet;
		public float VertRadius = 0.01f;

		public bool DoNormals = true;
		public Color NormalColor = HUES.norse_blue;
		public float NormalLength = 0.1f;

		public bool DoFaces = true;
		public Color FaceColor = HUES.green_glow;
		public float FaceLength = 0.1f;


		public int VertCount;
		public List<Vector3> Verts = new List<Vector3>();
		public List<Vector3> Normals = new List<Vector3>();
		public List<(int a, int b, int c)> TriIndices = new List<(int, int, int)>();
		public List<Ray> Faces = new List<Ray>();
		public Bounds Bounds;
	}

	static void LoadMeshDat(this Dat dat, Mesh mesh, Transform meshTf)
	{
		dat.VertCount = mesh.vertexCount;
		var verts = dat.Verts.ClearThen();
		var normals = dat.Normals.ClearThen();
		var triIndices = dat.TriIndices.ClearThen();
		var faces = dat.Faces.ClearThen();
		dat.Bounds = mesh.bounds;

		mesh.GetVertices(verts);
		mesh.GetNormals(normals);

		var trisRaw = mesh.triangles;
		for (var i = 0; i < trisRaw.Length; i += 3) {
			var aDex = trisRaw[i];
			var bDex = trisRaw[i + 1];
			var cDex = trisRaw[i + 2];
			triIndices.Add((aDex, bDex, cDex));

			var facePos = (verts[aDex] + verts[bDex] + verts[cDex]) / 3;
			var faceNormal = (normals[aDex] + normals[bDex] + normals[cDex]) / 3;
			faces.Add(new Ray(facePos, faceNormal));
		}
	}


	public static void GizMe(this Mesh mesh, Transform meshTf, Dat dat = null)
	{
		dat ??= Dat.DEFAULT;

		dat.LoadMeshDat(mesh, meshTf);

		Giz.Matrix(meshTf);

		if (dat.DoVerts) dat.DrawVerts();
		if (dat.DoNormals) dat.DrawNormals();
		if (dat.DoFaces) dat.DrawFaces();

		Giz.Reset();
	}

	static void DrawVerts(this Dat dat)
	{
		Giz.Color(dat.VertColor);
		for (var i = 0; i < dat.VertCount; i++) {
			var vertPos = dat.Verts[i];
			Giz.Sphere(vertPos, dat.VertRadius);
		}
	}

	static void DrawNormals(this Dat dat)
	{
		Giz.Color(dat.NormalColor);
		for (var i = 0; i < dat.VertCount; i++) {
			var vertPos = dat.Verts[i];
			var normal = dat.Normals[i];
			Giz.Ray(vertPos, normal, dat.NormalLength);
		}
	}

	static void DrawFaces(this Dat dat)
	{
		Giz.Color(dat.FaceColor);
		foreach (var face in dat.Faces) {
			Giz.Ray(face, dat.FaceLength);
		}
	}
}
}