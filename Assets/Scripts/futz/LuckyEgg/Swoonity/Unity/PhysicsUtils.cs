using System.Collections.Generic;
using UnityEngine;

namespace Swoonity.Unity
{
public static class PhysicsUtils
{
	public static bool Has(this LayerMask mask, int layer)
	{
		return mask == (mask | (1 << layer));
	}

	/// Sets a gameObject's layer and optionally its children's layers.
	public static void SetLayer(
		this GameObject go,
		int layerNumber,
		bool setChildren = false
	)
	{
		go.layer = layerNumber;
		if (!setChildren) return;

		foreach (Transform child in go.transform) {
			SetLayer(child.gameObject, layerNumber, true);
		}
	}

	/// Sets a component's gameObject layer and optionally its children's layers.
	public static void SetLayer(
		this Component comp,
		int layerNumber,
		bool setChildren = false
	)
		=> comp.gameObject.SetLayer(layerNumber, setChildren);


	public static void SetLayers(
		this List<GameObject> gos,
		int layerNumber
	)
	{
		foreach (var go in gos) {
			go.layer = layerNumber;
		}
	}

	public static void SetLayers<TComp>(
		this List<TComp> comps,
		int layerNumber
	) where TComp : Component
	{
		foreach (var comp in comps) {
			comp.gameObject.layer = layerNumber;
		}
	}
}
}