using System;
using UnityEngine;

namespace Swoonity.Unity
{
public static class RaycastHitUtils
{
	/// Returns true == hit's layer
	public static bool IsLayer(this RaycastHit hit, int layer)
	{
		return hit.collider.gameObject.layer == layer;
	}

	public static bool InLayerMask(this RaycastHit hit, LayerMask mask)
	{
		if (!hit.collider) return false;
		var layer = hit.collider.gameObject.layer;
		return mask == (mask | (1 << layer));
	}

	/// Returns true == hit's tag
	public static bool HasTag(this RaycastHit hit, string tag)
	{
		return hit.collider.gameObject.CompareTag(tag);
	}

	/// Iterates over hit array, checking for smallest distance. Can override array length (for nonAlloc calls).
	public static int ClosestDex(this RaycastHit[] hits, int lengthOverride = int.MaxValue)
	{
		var result = -1;
		var distance = float.MaxValue;
		var length = Mathf.Min(hits.Length, lengthOverride);

		for (int dex = 0; dex < length; dex++) {
			if (hits[dex].distance < distance) {
				distance = hits[dex].distance;
				result = dex;
			}
		}

		return result;
	}

	/// Iterates over hit array, checking for smallest distance. Can override array length (for nonAlloc calls).
	public static Transform Closest(this RaycastHit[] hits, int lengthOverride = int.MaxValue)
	{
		var result = hits.ClosestDex(lengthOverride);
		if (result < 0) return null;
		return hits[result].transform;
	}

	/// Iterates over hit array, checking for smallest distance. Can override array length (for nonAlloc calls).
	public static Transform ClosestWhere(
		this RaycastHit[] hits,
		Func<int, bool> predicate,
		int lengthOverride = int.MaxValue
	)
	{
		Transform result = null;
		var distance = float.MaxValue;
		var length = Mathf.Min(hits.Length, lengthOverride);

		for (int dex = 0; dex < length; dex++) {
			if (predicate(dex) && hits[dex].distance < distance) {
				distance = hits[dex].distance;
				result = hits[dex].transform;
			}
		}

		return result;
	}
}
}