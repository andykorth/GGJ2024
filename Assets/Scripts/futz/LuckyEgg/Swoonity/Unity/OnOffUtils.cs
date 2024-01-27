using System.Collections.Generic;
using Swoonity.Collections;
using UnityEngine;

namespace Swoonity.Unity
{
public static class OnOffGameObject
{
	/// Turn GameObject on
	public static void On(this GameObject go)
	{
		go.SetActive(true);
	}

	/// Turn GameObject off
	public static void Off(this GameObject go)
	{
		go.SetActive(false);
	}


	/// (null check) SetActive(true)
	public static void OnNC(this GameObject go)
	{
		if (go != null) go.SetActive(true);
	}

	/// (null check) SetActive(false)
	public static void OffNC(this GameObject go)
	{
		if (go != null) go.SetActive(false);
	}

	/// if (go) go.SetActive(isOn)
	public static void OnIf(this GameObject go, bool isOn)
	{
		if (go) go.SetActive(isOn);
	}

	/// if (go) go.SetActive(!isOff)
	public static void OffIf(this GameObject go, bool isOff)
	{
		if (go) go.SetActive(!isOff);
	}

	/// (null check) SetActive(value)
	public static void SetActiveNC(this GameObject go, bool isOn)
	{
		if (go != null) go.SetActive(isOn);
	}

	/// Turns GameObject OFF and returns it (for chaining)
	public static GameObject OffAnd(this GameObject go)
	{
		go.SetActive(false);
		return go;
	}

	/// Make a GameObject inactive then active
	public static void OffThenOn(this GameObject go)
	{
		go.SetActive(false);
		go.SetActive(true);
	}

	#region GameObject collections

	/// Turns all GameObjects ON
	public static void On(this List<GameObject> list)
	{
		foreach (var go in list) {
			go.SetActive(true);
		}
	}

	/// Turns all GameObjects OFF
	public static void Off(this List<GameObject> list)
	{
		foreach (var go in list) {
			go.SetActive(false);
		}
	}

	/// Turns all GameObjects ON
	public static void SetActive(this List<GameObject> list, bool isOn)
	{
		foreach (var go in list) {
			go.SetActive(isOn);
		}
	}

	/// Turns all GameObjects ON
	public static void On(this GameObject[] array)
	{
		foreach (var go in array) {
			go.SetActive(true);
		}
	}

	/// Turns all GameObjects OFF
	public static void Off(this GameObject[] array)
	{
		foreach (var go in array) {
			go.SetActive(false);
		}
	}

	/// Turns one GameObject ON at index, the rest OFF
	public static void OnSolo(this List<GameObject> list, int index)
	{
		for (var dex = 0; dex < list.Count; dex++) {
			list[dex].SetActive(dex == index);
		}
	}

	/// Turns one GameObject OFF at index, the rest ON
	public static void OffSolo(this List<GameObject> list, int index)
	{
		for (var dex = 0; dex < list.Count; dex++) {
			list[dex].SetActive(dex != index);
		}
	}

	/// Turns one GameObject ON at index, the rest OFF
	public static void OnSolo(this GameObject[] array, int index)
	{
		for (var dex = 0; dex < array.Length; dex++) {
			array[dex].SetActive(dex == index);
		}
	}

	/// Turns one GameObject OFF at index, the rest ON
	public static void OffSolo(this GameObject[] array, int index)
	{
		for (var dex = 0; dex < array.Length; dex++) {
			array[dex].SetActive(dex != index);
		}
	}

	/// Turns one GameObject ON at index, the rest OFF
	public static void OnSoloNC(this GameObject[] array, int index)
	{
		for (var dex = 0; dex < array.Length; dex++) {
			array[dex].SetActiveNC(dex == index);
		}
	}

	/// Turns one GameObject OFF at index, the rest ON
	public static void OffSoloNC(this GameObject[] array, int index)
	{
		for (var dex = 0; dex < array.Length; dex++) {
			array[dex].SetActiveNC(dex != index);
		}
	}

	#endregion
}

public static class OnOffGenericComponent
{
	/// Turns GameObject ON
	public static void On<T>(this T comp) where T : Component
	{
		comp.gameObject.SetActive(true);
	}

	/// Turns GameObject OFF
	public static void Off<T>(this T comp) where T : Component
	{
		comp.gameObject.SetActive(false);
	}

	/// Turns GameObject ON and returns it (for chaining)
	public static T OnAnd<T>(this T comp) where T : Component
	{
		comp.gameObject.SetActive(true);
		return comp;
	}

	/// Turns GameObject OFF and returns it (for chaining)
	public static T OffAnd<T>(this T comp) where T : Component
	{
		comp.gameObject.SetActive(false);
		return comp;
	}
}

public static class OnOffTransform
{
	/// Turn GameObject on/off
	public static void SetActive(this Transform tf, bool isActive = true)
	{
		tf.gameObject.SetActive(isActive);
	}

	/// Turn GameObject on
	public static void On(this Transform tf) => tf.gameObject.SetActive(true);

	/// Turn GameObject off
	public static void Off(this Transform tf) => tf.gameObject.SetActive(false);


	/// Turns all Transforms ON
	public static void On(this List<Transform> list)
	{
		foreach (var tf in list) {
			tf.gameObject.SetActive(true);
		}
	}

	/// Turns all Transforms OFF
	public static void Off(this List<Transform> list)
	{
		foreach (var tf in list) {
			tf.gameObject.SetActive(false);
		}
	}

	/// Turns all Transforms ON
	public static void On(this Transform[] array)
	{
		foreach (var tf in array) {
			tf.gameObject.SetActive(true);
		}
	}

	/// Turns all Transforms OFF
	public static void Off(this Transform[] array)
	{
		foreach (var tf in array) {
			tf.gameObject.SetActive(false);
		}
	}
}

public static class OnOffMisc
{
	/// (null check) enabled = true
	public static void EnableNC(this Collider coll)
	{
		if (coll) coll.enabled = true;
	}

	/// (null check) enabled = false
	public static void DisableNC(this Collider coll)
	{
		if (coll) coll.enabled = false;
	}

	public static void SetEnableNC(this Collider coll, bool enabled)
	{
		if (coll) coll.enabled = enabled;
	}

	public static void EnableColliders<T>(this List<T> list) where T : Collider
	{
		foreach (var comp in list) comp.enabled = true;
	}

	public static void DisableColliders<T>(this List<T> list) where T : Collider
	{
		foreach (var comp in list) comp.enabled = false;
	}

	public static void EnableColliders<T>(this T[] list) where T : Collider
	{
		foreach (var comp in list) comp.enabled = true;
	}

	public static void DisableColliders<T>(this T[] list) where T : Collider
	{
		foreach (var comp in list) comp.enabled = false;
	}

	/// (null check) enabled = true
	public static void EnableNC(this MonoBehaviour comp)
	{
		if (comp) comp.enabled = true;
	}

	/// (null check) enabled = false
	public static void DisableNC(this MonoBehaviour comp)
	{
		if (comp) comp.enabled = false;
	}

	public static void SetEnableNC(this MonoBehaviour comp, bool enabled)
	{
		if (comp) comp.enabled = enabled;
	}

	public static void Enable<T>(this List<T> list) where T : MonoBehaviour
	{
		foreach (var comp in list) comp.enabled = true;
	}

	public static void Disable<T>(this List<T> list) where T : MonoBehaviour
	{
		foreach (var comp in list) comp.enabled = false;
	}
}
}