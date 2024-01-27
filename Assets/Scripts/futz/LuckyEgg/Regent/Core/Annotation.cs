using Swoonity.CSharp;
using Swoonity.Unity;
using UnityEngine;
using V3 = UnityEngine.Vector3;
using static UnityEngine.Debug;

// ReSharper disable InconsistentNaming

#if UNITY_EDITOR

#endif

namespace Regent.Annotation
{
public interface IAnnotation
{
	// public Action<Entity> FnAnnotate { get; }
	/// TODO: docs
	public void Annotate();
}

public static class Anno
{
	// public static Matrix4x4 LastMatrix;
	public static Transform LastMatrixTf;

	const EventType REPAINT = EventType.Repaint;
	static readonly Quaternion ROT_LEFT = Quaternion.LookRotation(V3.left);
	static readonly Quaternion ROT_UP = Quaternion.LookRotation(V3.up);
	static readonly Quaternion ROT_FORWARD = Quaternion.LookRotation(V3.forward);


	// 	public static void Matrix(Matrix4x4 matrix) {
	// #if UNITY_EDITOR
	// 		LastMatrix = UnityEditor.Handles.matrix;
	// 		UnityEditor.Handles.matrix = matrix;
	// #endif
	// 	}

	public static void Matrix(Transform tform)
	{
#if UNITY_EDITOR
		UnityEditor.Handles.matrix = tform.localToWorldMatrix;
#endif
	}

	public static void MatrixWorld()
	{
#if UNITY_EDITOR
		UnityEditor.Handles.matrix = Matrix4x4.identity;
#endif
	}

	public static void Color(Color color)
	{
#if UNITY_EDITOR
		UnityEditor.Handles.color = color;
#endif
	}

	public static Camera SceneCam() => Camera.current;

	public static void Label(
		string text,
		V3 worldPos,
		int fontSize = 16,
		TextAnchor anchor = TextAnchor.MiddleCenter
	)
	{
#if UNITY_EDITOR
		if (text.Nil()) return;

		var style = new GUIStyle {
			fontSize = fontSize,
			alignment = anchor, // shit don't work
			wordWrap = true,
			normal = new GUIStyleState {
				textColor = UnityEditor.Handles.color,
			}
		};

		UnityEditor.Handles.Label(worldPos, text, style);
#endif
	}

	public static void Line(
		V3 start,
		V3 end,
		float thickness = 0f
	)
	{
#if UNITY_EDITOR
		UnityEditor.Handles.DrawLine(start, end, thickness);
#endif
	}

	public static void DottedLine(
		V3 start,
		V3 end,
		float dashPx = 4
	)
	{
#if UNITY_EDITOR
		UnityEditor.Handles.DrawDottedLine(start, end, dashPx);
#endif
	}

	public static void WireCube(
		V3 pos,
		float size = 0.05f
	)
	{
#if UNITY_EDITOR
		UnityEditor.Handles.DrawWireCube(pos, Vector3.one * size);
#endif
	}

	public static void Cube(
		V3 pos,
		float size = 0.05f
	)
	{
#if UNITY_EDITOR
		if (Event.current.type == REPAINT) {
			UnityEditor.Handles.CubeHandleCap(
				0,
				pos,
				Quaternion.identity,
				size,
				REPAINT
			);
		}
#endif
	}

	public static void Sphere(
		Vector3 worldPos,
		float size = .02f
	)
	{
#if UNITY_EDITOR
		if (Event.current.type == REPAINT) {
			UnityEditor.Handles.SphereHandleCap(
				0,
				worldPos,
				Quaternion.identity,
				size,
				REPAINT
			);
		}
#endif
	}

	public static void Ray(
		PhysRay ray,
		int thickness = 1,
		float lengthOverride = 0,
		Vector3 offset = default
	)
	{
#if UNITY_EDITOR
		var start = ray.Origin + offset;
		var end = start + (ray.Direction * lengthOverride.Or(ray.Length));

		UnityEditor.Handles.DrawLine(start, end, thickness);
#endif
	}

	public static void Arrow(
		V3 origin,
		V3 direction,
		float size = .1f // size and length :(
	)
	{
#if UNITY_EDITOR
		if (Event.current.type == REPAINT) {
			UnityEditor.Handles.ArrowHandleCap(
				0,
				origin,
				Quaternion.LookRotation(direction),
				size,
				REPAINT
			);
		}
#endif
	}

	public static void Arrow(
		V3 origin,
		Quaternion rotation,
		float size = .1f // size and length :(
	)
	{
#if UNITY_EDITOR
		if (Event.current.type == REPAINT) {
			UnityEditor.Handles.ArrowHandleCap(0, origin, rotation, size, REPAINT);
		}
#endif
	}

	/// use Anno.Matrix(transform) first
	public static void LocalDirectionArrows(
		float size = .1f, // size and length :(
		float alpha = 1f
	)
	{
#if UNITY_EDITOR
		var originalColor = UnityEditor.Handles.color;

		Color(UnityEngine.Color.red.WithAlpha(alpha));
		Arrow(V3.zero, ROT_LEFT, size);

		Color(UnityEngine.Color.green.WithAlpha(alpha));
		Arrow(V3.zero, ROT_UP, size);

		Color(UnityEngine.Color.blue.WithAlpha(alpha));
		Arrow(V3.zero, ROT_FORWARD, size);

		Color(originalColor);
#endif
	}

	// var labelPos = points.Avg();
	public static void Polygon(params V3[] points)
	{
#if UNITY_EDITOR
		UnityEditor.Handles.DrawAAConvexPolygon(points);
#endif
	}

	/// find exactly 1 def of type (Editor only)
	public static class ConfigFinder<T> where T : UnityEngine.Object
	{
		static T _instance;

		public static T Get(bool logErrors = true)
		{
#if UNITY_EDITOR
			if (_instance) return _instance;

			var nameOfType = typeof(T).Name;
			var guids = UnityEditor.AssetDatabase.FindAssets($"t:{nameOfType}");

			if (guids.Length == 0) {
				if (logErrors) LogError($"Missing {nameOfType} asset");
				return default;
			}

			if (guids.Length > 1) {
				if (logErrors) LogError($"Found more than one {nameOfType} asset");
			}

			var guid = guids[0];
			var assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
			_instance = UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)) as T;
			return _instance;
#else
			return null;
#endif
		}
	}
}
}