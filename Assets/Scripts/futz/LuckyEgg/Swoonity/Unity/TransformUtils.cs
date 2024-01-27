using System;
using UnityEngine;
using Tf = UnityEngine.Transform;
using V3 = UnityEngine.Vector3;
using Quat = UnityEngine.Quaternion;

namespace Swoonity.Unity
{
public static class TransformUtils
{
	/// Creates new empty Tf under target Tf
	public static Tf NewChild(this Tf parent, string name = "child")
	{
		var newTf = new GameObject(name).transform;
		newTf.SetParentAndReset(parent);
		return newTf;
	}

	public static Tf NewSibling(
		this Tf sibling,
		string name = "sibling",
		bool putBefore = false
	)
	{
		var newTf = sibling.parent.NewChild(name);
		var siblingIndex = sibling.GetSiblingIndex();
		newTf.SetSiblingIndex(putBefore ? siblingIndex : siblingIndex + 1);
		return newTf;
	}

	/// this function already multiplies by deltaTime
	public static void MoveTowards(this Tf tf, V3 to, float speedPerSec)
		=> tf.position = V3.MoveTowards(
			tf.position,
			to,
			speedPerSec * Time.deltaTime
		);

	/// set Tf pos/rot/sca to target's
	public static void SetTo(this Tf tf, Tf target)
	{
		tf.position = target.position;
		tf.rotation = target.rotation;
		tf.localScale = target.localScale;
	}

	/// set Tf pos/rot/sca to target's local
	public static void SetToLocal(this Tf tf, Tf target)
	{
		tf.localPosition = target.localPosition;
		tf.localRotation = target.localRotation;
		tf.localScale = target.localScale;
	}

	public static void SetPosRot(this Tf tf, Tf target)
	{
		tf.position = target.position;
		tf.rotation = target.rotation;
	}

	/// set position, rotation, localScale
	public static void Set(this Tf tf, V3 pos, Quat rot, V3 sca)
	{
		tf.position = pos;
		tf.rotation = rot;
		tf.localScale = sca;
	}

	/// set position, rotation, localScale
	public static void Set(this Tf tf, (V3 pos, Quat rot, V3 sca) vals)
		=> (tf.position, tf.rotation, tf.localScale) = vals;

	public static void SetPosition(this Tf tf, Tf target) => tf.position = target.position;
	public static void SetPosition(this Tf tf, V3 position) => tf.position = position;

	public static void SetPosition(this Tf tf, float x, float y) => tf.position = new Vector3(x, y, tf.position.z);

	public static void SetPosition(this Tf tf, float x, float y, float z) => tf.position = new Vector3(x, y, z);

	public static void SetRotation(this Tf tf, Tf target) => tf.rotation = target.rotation;
	public static void SetRotation(this Tf tf, Quat rotation) => tf.rotation = rotation;
	public static void SetScale(this Tf tf, Tf target) => tf.localScale = target.localScale;
	public static void SetScale(this Tf tf, V3 scale) => tf.localScale = scale;

	public static void SmoothDamp(this Tf tf, V3 toPos, ref V3 velocity, float smoothTime)
	{
		tf.position = V3.SmoothDamp(tf.position, toPos, ref velocity, smoothTime);
	}

	public static void LerpTo(this Tf tf, Tf target, float fraction)
	{
		tf.position = V3.Lerp(tf.position, target.position, fraction);
		tf.rotation = Quat.Lerp(tf.rotation, target.rotation, fraction);
		tf.localScale = V3.Lerp(tf.localScale, target.localScale, fraction);
	}

	public static void Lerp(
		this Tf tf,
		(V3 start, V3 end) pos,
		(Quat start, Quat end) rot,
		(V3 start, V3 end) sca,
		float fraction
	)
	{
		tf.position = V3.Lerp(pos.start, pos.end, fraction);
		tf.rotation = Quat.Lerp(rot.start, rot.end, fraction);
		tf.localScale = V3.Lerp(sca.start, sca.end, fraction);
	}

	public static void Lerp(
		this Tf tf,
		(V3 pos, Quat rot, V3 sca) start,
		(V3 pos, Quat rot, V3 sca) end,
		float fraction
	)
	{
		tf.position = V3.Lerp(start.pos, end.pos, fraction);
		tf.rotation = Quat.Lerp(start.rot, end.rot, fraction);
		tf.localScale = V3.Lerp(start.sca, end.sca, fraction);
	}

	public static void LerpPosRot(this Tf tf, Tf target, float fraction)
	{
		tf.position = V3.Lerp(tf.position, target.position, fraction);
		tf.rotation = Quat.Lerp(tf.rotation, target.rotation, fraction);
	}

	public static void LerpPosition(this Tf tf, Tf target, float fraction)
		=> tf.position = V3.Lerp(tf.position, target.position, fraction);

	public static void LerpPosition(this Tf tf, V3 position, float fraction)
		=> tf.position = V3.Lerp(tf.position, position, fraction);

	public static void LerpRotation(this Tf tf, Tf target, float fraction)
		=> tf.rotation = Quat.Lerp(tf.rotation, target.rotation, fraction);

	public static void LerpRotation(this Tf tf, Quat rotation, float fraction)
		=> tf.rotation = Quat.Lerp(tf.rotation, rotation, fraction);

	public static void LerpScale(this Tf tf, Tf target, float fraction)
		=> tf.localScale = V3.Lerp(tf.localScale, target.localScale, fraction);

	public static void LerpScale(this Tf tf, V3 scale, float fraction)
		=> tf.localScale = V3.Lerp(tf.localScale, scale, fraction);

	public static void MirrorRotation(this Tf tf)
	{
		var rot = tf.rotation;
		var normal = tf.parent.right; // normal of plane
		tf.rotation = Quat.LookRotation(
			V3.Reflect(rot * V3.forward, normal),
			V3.Reflect(rot * V3.up, normal)
		);
	}

	public static void MirrorLocal(this Tf tf)
	{
		tf.MirrorRotation();
		tf.localPosition = tf.localPosition.FlipX();
		tf.localScale = tf.localScale.FlipX();
	}

	// public static V3 ToLocal(this V3 worldPoint, Tf tf)
	// 	=> tf.InverseTfPoint(worldPoint);
	//
	// public static V3 ToWorld(this V3 localPoint, Tf tf)
	// 	=> tf.TfPoint(localPoint);
	//
	// public static V3 ToLocalDir(this V3 worldDir, Tf tf)
	// 	=> tf.InverseTfDirection(worldDir);
	//
	// public static V3 ToWorldDir(this V3 localDir, Tf tf)
	// 	=> tf.TfDirection(localDir);


	public static V3 LocalPointOf(this Tf tf, V3 worldPoint) => tf.InverseTransformPoint(worldPoint);

	public static V3 WorldPointOf(this Tf tf, V3 localPoint) => tf.TransformPoint(localPoint);

	public static V3 LocalDirectionOf(this Tf tf, V3 worldDir) => tf.InverseTransformDirection(worldDir);

	public static V3 WorldDirectionOf(this Tf tf, V3 localDir) => tf.TransformDirection(localDir);

	/// (DTF) direction = to - from
	/// var displacement = targetPos - myPos; // Vector3
	/// var distance = displacement.magnitude;
	/// var direction = displacement.normalized;
	public static V3 DirectionTo(this Tf tf, V3 toWorldPosition) => (toWorldPosition - tf.position).normalized;

	public static V3 LocalDirectionTo(this Tf tf, V3 toWorldPosition)
		=> tf.InverseTransformDirection((toWorldPosition - tf.position).normalized);


	// TODO: cleanup
	// TODO: cleanup
	// TODO: cleanup
	// TODO: cleanup


	public static T AddComponent<T>(this Tf t) where T : Component => t.gameObject.AddComponent<T>();

	public static bool CutComponent<T>(this Tf t) where T : Component
	{
		var comp = t.GetComponent<T>();
		if (!comp) return false;
		UnityEngine.Object.Destroy(comp);
		return true;
	}

	public static void ResetWorldRotation(this Tf t) => t.rotation = Quat.identity;
	public static void ResetLocalRotation(this Tf t) => t.localRotation = Quat.identity;

	public static void ResetWorldPosition(this Tf t) => t.position = Vector3.zero;
	public static void ResetLocalPosition(this Tf t) => t.localPosition = Vector3.zero;

	/// <summary>
	/// Returns distance to target Tf
	/// </summary>
	public static float Distance(this Tf t, Tf target)
	{
		return V3.Distance(t.position, target.position);
	}

	/// <summary>
	/// Returns if distance to target is less than to float
	/// </summary>
	public static bool DistanceWithin(this Tf t, Tf target, float targetDistance)
	{
		return (t.position - target.position).sqrMagnitude < targetDistance * targetDistance;
		//			return V3.Distance(t.position, target.position) <= targetDistance;
	}

	/// <summary>
	/// Set a Tf's X.
	/// </summary>
	public static void SetX(this Tf t, float x)
	{
		t.position = t.position.WithX(x);
	}

	/// <summary>
	/// Set a Tf's Y.
	/// </summary>
	public static void SetY(this Tf t, float y)
	{
		t.position = t.position.WithY(y);
	}

	/// <summary>
	/// Set a Tf's Z.
	/// </summary>
	public static void SetZ(this Tf t, float z)
	{
		t.position = t.position.WithZ(z);
	}

	/// <summary>
	/// Set a Tf's local X.
	/// </summary>
	public static void SetLocalX(this Tf t, float x)
	{
		t.localPosition = t.localPosition.WithX(x);
	}

	/// <summary>
	/// Set a Tf's local Y.
	/// </summary>
	public static void SetLocalY(this Tf t, float y)
	{
		t.localPosition = t.localPosition.WithY(y);
	}

	/// <summary>
	/// Set a Tf's local Z.
	/// </summary>
	public static void SetLocalZ(this Tf t, float z)
	{
		t.localPosition = t.localPosition.WithZ(z);
	}

	/// <summary>
	/// Set a Tf's euler rotation X.
	/// </summary>
	public static void SetRotationX(this Tf t, float x)
	{
		t.eulerAngles = t.eulerAngles.WithX(x);
	}

	/// <summary>
	/// Set a Tf's euler rotation Y.
	/// </summary>
	public static void SetRotationY(this Tf t, float y)
	{
		t.eulerAngles = t.eulerAngles.WithY(y);
	}

	/// <summary>
	/// Set a Tf's euler rotation Z.
	/// </summary>
	public static void SetRotationZ(this Tf t, float z)
	{
		t.eulerAngles = t.eulerAngles.WithZ(z);
	}

	/// <summary>
	/// Set a Tf's local euler rotation X.
	/// </summary>
	public static void SetLocalRotationX(this Tf t, float x)
	{
		t.localEulerAngles = t.localEulerAngles.WithX(x);
	}

	/// <summary>
	/// Set a Tf's local euler rotation Y.
	/// </summary>
	public static void SetLocalRotationY(this Tf t, float y)
	{
		t.localEulerAngles = t.localEulerAngles.WithY(y);
	}

	/// <summary>
	/// Set a Tf's local euler rotation Z.
	/// </summary>
	public static void SetLocalRotationZ(this Tf t, float z)
	{
		t.localEulerAngles = t.localEulerAngles.WithZ(z);
	}

	/// <summary>
	/// Add to a Tf's local X euler rotation.
	/// </summary>
	public static void AddLocalEulerX(this Tf t, float amountToAdd)
	{
		t.localEulerAngles = t.localEulerAngles.WithAddedX(amountToAdd);
	}

	/// <summary>
	/// Add to a Tf's local Y euler rotation.
	/// </summary>
	public static void AddLocalEulerY(this Tf t, float amountToAdd)
	{
		t.localEulerAngles = t.localEulerAngles.WithAddedY(amountToAdd);
	}

	/// <summary>
	/// Add to a Tf's local Z euler rotation.
	/// </summary>
	public static void AddLocalEulerZ(this Tf t, float amountToAdd)
	{
		t.localEulerAngles = t.localEulerAngles.WithAddedZ(amountToAdd);
	}

	/// <summary>
	/// Set a Tf's scale x y z
	/// </summary>
	public static void SetScale(this Tf t, float xScale, float yScale, float zScale)
	{
		t.localScale = new V3(xScale, yScale, zScale);
	}

	/// <summary>
	/// Set a Tf's scale uniformly
	/// </summary>
	public static void SetScale(this Tf t, float uniformScale)
	{
		t.localScale = new V3(uniformScale, uniformScale, uniformScale);
	}

	public static void SetScaleX(this Tf t, float x) => t.localScale = t.localScale.WithX(x);
	public static void SetScaleY(this Tf t, float y) => t.localScale = t.localScale.WithY(y);
	public static void SetScaleZ(this Tf t, float z) => t.localScale = t.localScale.WithZ(z);


	/// <summary>
	/// Sets a transform's parent and then optionally resets the transform's local: position, rotation, and scale.
	/// </summary>
	public static void SetParentAndReset(
		this Transform transform,
		Transform parent,
		bool resetLocalPosition = true,
		bool resetLocalRotation = true,
		bool resetLocalScale = true
	)
	{
		transform.SetParent(parent);
		if (resetLocalPosition) {
			transform.localPosition = Vector3.zero;
		}

		if (resetLocalRotation) {
			transform.localRotation = Quaternion.identity;
		}

		if (resetLocalScale) {
			transform.localScale = Vector3.one;
		}
	}


	public static void SetParentAndReset(
		this Transform tf,
		Transform parent
	)
	{
		tf.SetParent(parent);
		tf.localPosition = Vector3.zero;
		tf.localRotation = Quaternion.identity;
		tf.localScale = Vector3.one;
	}

	/// <summary>
	/// Iterates through children using a callback on each element
	/// </summary>
	/// <param name="parent"></param>
	/// <param name="callback"></param>
	public static void EachChild(this Tf parent, Action<Tf> callback)
	{
		foreach (Tf child in parent) {
			callback(child);
		}
	}

	/// <summary>
	/// Sets Tf's parent and localPosition/localRotation (and resets localScale)
	/// </summary>
	public static void SetParentLocal(
		this Tf t,
		Tf parent,
		V3 localPosition = default(V3),
		Quat localRotation = default(Quat),
		bool resetScale = false
	)
	{
		t.SetParent(parent);
		t.localPosition = localPosition;
		t.localRotation = localRotation;
		if (resetScale) {
			t.localScale = V3.one;
		}
	}

	public static void SetParentLocal(
		this Tf t,
		Tf parent,
		V3 localPosition,
		Quat localRotation,
		V3 localScale
	)
	{
		t.SetParent(parent);
		t.localPosition = localPosition;
		t.localRotation = localRotation;
		t.localScale = localScale;
	}

	public static void SetParentWorld(this Tf tf, Tf parent, V3 pos, Quat rot, V3 sca)
	{
		tf.SetParent(parent);
		tf.position = pos;
		tf.rotation = rot;
		tf.localScale = sca;
	}

	// /// Sets Tf's parent and position/rotation (and resets localScale)
	// public static void SetParentWorld(
	// 	this Tf t,
	// 	Tf parent,
	// 	V3 position = default(V3),
	// 	Quat rotation = default(Quat),
	// 	bool resetScale = false
	// ) {
	// 	t.SetParent(parent);
	// 	t.position = position;
	// 	t.rotation = rotation;
	// 	if (resetScale) {
	// 		t.localScale = V3.one;
	// 	}
	// }

	/// Gets vector relative to Tf (e.g. an adjusted movement vector relative to a camera)
	public static V3 GetRelativeVector(this Tf tf, V3 move3d)
	{
		return move3d.x * tf.right
		     + move3d.y * tf.up
		     + move3d.z * tf.forward;
	}

	public static V3 GetRelativeFlatVector(this Tf tf, V3 move3d) => tf.GetRelativeFlatVector(move3d, Vector3.up);

	public static V3 GetRelativeFlatVector(this Tf tf, V3 move3d, V3 groundUp)
	{
		var flatRot = Quaternion.LookRotation(-groundUp, tf.forward)
		            * Quaternion.Euler(-90f, 0, 0);
		return flatRot * move3d;
	}

	/// Get rotation (AngleAxis) of Tf euler Y (around Vector.Up)
	public static Quat GetSpinRotation(this Tf tf) => Quat.AngleAxis(tf.eulerAngles.y, V3.up);

	public static V3 Right(this Tf tf, float magnitude = 1) => tf.right * magnitude;
	public static V3 Up(this Tf tf, float magnitude = 1) => tf.up * magnitude;
	public static V3 Forward(this Tf tf, float magnitude = 1) => tf.forward * magnitude;
}
}