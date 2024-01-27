using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Swoonity.Collections;
using Swoonity.CSharp;
using Random = UnityEngine.Random;

namespace Swoonity.Unity
{
public static class MoveToSwoonity
{
	/// <summary>
	/// Adds a value but ensures result is above 0.
	/// </summary>
	public static float AddAboveZero(this float originalValue, float valueToAdd)
	{
		return Mathf.Max(0, originalValue + valueToAdd);
	}

	/// <summary>
	/// Subtracts a value but ensures result is above 0.
	/// </summary>
	public static float SubtractAboveZero(this float originalValue, float valueToSubtract)
	{
		return Mathf.Max(0, originalValue - valueToSubtract);
	}

	/// <summary>
	/// Converts string to float (or returns 0f if invalid string).
	/// </summary>
	public static float ToFloat(this string stringValue)
	{
		return string.IsNullOrEmpty(stringValue) ? 0f : float.Parse(stringValue);
	}

	/// <summary>
	/// Returns TRUE if string is null or empty.
	/// </summary>
	public static bool IsNullOrEmpty(this string stringValue)
	{
		return string.IsNullOrEmpty(stringValue);
	}

	/// <summary>
	/// Returns a random index in an array.
	/// </summary>
	public static int RandomIndex<T>(this T[] array)
	{
		if (array.Length == 0) {
			throw new IndexOutOfRangeException(
				"Cannot retrieve a random index from an empty array"
			);
		}

		return
			Random.Range(
				0,
				array.Length
			); // TODO: Convert to ArrayExtensions.randomNumberGenerator similar to RandomElement<T>
	}

	/// <summary>
	/// Sets a gameObject's tag and optionally its children's tags.
	/// </summary>
	public static void SetTag(this GameObject go, string tag, bool setChildrenTags = false)
	{
		go.tag = tag;
		if (setChildrenTags) {
			foreach (Transform child in go.transform) {
				SetTag(child.gameObject, tag, setChildrenTags);
			}
		}
	}

	/// <summary>
	/// Shifts transform's sibling index by given amount. Can optionally wrap. By default, shifting an item past the beginning or end will do nothing.
	/// </summary>
	public static void ShiftSiblingIndexBy(
		this Transform transform,
		int adjustment,
		bool wrap = false
	)
	{
		var index = transform.GetSiblingIndex();
		var count = transform.parent.childCount;
		if (index == 0 && adjustment < 0) {
			if (wrap) {
				transform.SetAsLastSibling();
			}
		}
		else if (index == count - 1 && adjustment > 0) {
			if (wrap) {
				transform.SetAsFirstSibling();
			}
		}
		else {
			transform.SetSiblingIndex(index + adjustment);
		}
	}

	/// <summary>
	/// Rotates a transform to the desired rotation, but only within maxDegreesPerSecond.
	/// </summary>
	public static void RotateUpTo(
		this Transform transform,
		Quaternion desiredRotation,
		float maxDegreesPerSecond,
		bool useLocal = false
	)
	{
		var maxRotation = maxDegreesPerSecond * Time.deltaTime;
		if (useLocal) {
			var fromRotation = transform.localRotation;
			transform.localRotation =
				Quaternion.Angle(fromRotation, desiredRotation) < maxRotation
					? desiredRotation
					: Quaternion.RotateTowards(fromRotation, desiredRotation, maxRotation);
		}
		else {
			var fromRotation = transform.rotation;
			transform.rotation = Quaternion.Angle(fromRotation, desiredRotation) < maxRotation
				? desiredRotation
				: Quaternion.RotateTowards(fromRotation, desiredRotation, maxRotation);
		}
	}

	/// <summary>
	/// Removes and returns an element at "index" from the list.
	/// </summary>
	public static T GrabAt<T>(this List<T> list, int index)
	{
		var element = list[index];
		list.RemoveAt(index);
		return element;
	}

	/// <summary>
	/// Removes and returns a random element from the list.
	/// </summary>
	// public static T GrabRandom<T>(this List<T> list) {
	// 	var element = list.RandomElement();
	// 	list.Remove(element);
	// 	return element;
	// }

	// /// <summary>
	// /// Only adds the element to the list if the list does not already contain the element.
	// /// </summary>
	// public static void AddDistinct<T>(this List<T> list, T element) {
	// 	if (!list.Contains(element)) {
	// 		list.Add(element);
	// 	}
	// }

	/// <summary>
	/// Returns the center of a group of Vector3s.
	/// </summary>
	public static Vector3 CenterPoint(this List<Vector3> vectors)
	{
		var result = Vector3.zero;
		var length = 0;
		vectors.Each_DEPRECATED(
			vector => {
				result += vector;
				++length;
			}
		);
		return result / length;
	}

	/// <summary>
	/// Returns the first substring between two strings.
	/// Optional: Include startString and endString (default: FALSE)
	/// </summary>
	public static string SubstringBetween(
		this string targetString,
		string startString,
		string endString,
		bool includeOuterStrings = false
	)
	{
		var startIndex = targetString.IndexOf(startString) + startString.Length;
		var endIndex = targetString.IndexOf(endString, startIndex);
		if (includeOuterStrings) {
			return "{0}{1}{2}".Fmt(
				startString,
				targetString.Substring(startIndex, endIndex - startIndex),
				endString
			);
		}

		return targetString.Substring(startIndex, endIndex - startIndex);
	}

	// /// <summary>
	// /// Similar to string.Split but removes empty entries and trims spaces
	// /// </summary>
	// public static string[] SplitRemoveEmpty(this string targetString, params char[] separator) {
	// 	return targetString
	// 		.Split(separator, StringSplitOptions.RemoveEmptyEntries)
	// 		.Select(stringEntry => stringEntry.Trim())
	// 		.Where(stringEntry => stringEntry != "")
	// 		.ToArray();
	// }

	/// <summary>
	/// Returns true if Camera can see world position
	/// </summary>
	public static bool CanSee(this Camera camera, Vector3 worldPosition)
	{
		var viewportPosition = camera.WorldToViewportPoint(worldPosition);
		return viewportPosition.x.IsWithin(0, 1)
		    && viewportPosition.y.IsWithin(0, 1);
	}
}
}