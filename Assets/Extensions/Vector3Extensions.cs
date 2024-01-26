using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains methods that extend Unity's Vector3 struct.
/// </summary>
static public class Vector3Extensions
{
    /// <summary>
    /// Returns a copy of the Vector3 with the given value in the x component.
    /// </summary>
    /// <param name="v">The Vector3 to copy.</param>
    /// <param name="x">The new value for the x component.</param>
    /// <returns>A copy of the Vector3 with the given value in the x component.</returns>
    static public Vector3 WithX(this Vector3 v, float x)
    {
        return new Vector3(x, v.y, v.z);
    }

    /// <summary>
    /// Returns a copy of the Vector3 with the given value in the y component.
    /// </summary>
    /// <param name="v">The Vector3 to copy.</param>
    /// <param name="y">The new value for the y component.</param>
    /// <returns>A copy of the Vector3 with the given value in the y component.</returns>
    static public Vector3 WithY(this Vector3 v, float y)
    {
        return new Vector3(v.x, y, v.z);
    }

    /// <summary>
    /// Returns a copy of the Vector3 with the given value in the z component.
    /// </summary>
    /// <param name="v">The Vector3 to copy.</param>
    /// <param name="z">The new value for the z component.</param>
    /// <returns>A copy of the Vector3 with the given value in the z component.</returns>
    static public Vector3 WithZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }

    /// <summary>
    /// Set the components of this Vector3 to the values of the components in the given Vector3.
    /// </summary>
    /// <param name="v">This Vector3.</param>
    /// <param name="other">The Vector3 to copy component values.</param>
    /// <returns>This Vector3.</returns>
    static public Vector3 Set(this Vector3 v, Vector3 other)
    {
        v.x = other.x;
        v.y = other.y;
        v.z = other.z;

        return v;
    }

    /// <summary>
    /// Pick a GameObject from the given list that is closest (in world space) to the given target GameObject.
    /// </summary>
    /// <param name="objList">A list of GameObjects to search.</param>
    /// <param name="target">The GameObject to measure distance from.</param>
    /// <param name="closest">Output value that is the closest GameObject in the list.  (Or null if none were found.)</param>
    /// <returns>The distance to the closest GameObject.  (Or float.MaxValue if none were found.)</returns>
    static public float FindClosest(this IEnumerable<GameObject> objList, GameObject target, out GameObject closest)
    {
        closest = null;
        float closestDist = float.MaxValue;

        foreach (GameObject obj in objList)
        {
            float dist = Vector3.Distance(target.transform.position, obj.transform.position);
            if (closest == null || dist < closestDist)
            {
                closest = obj;
                closestDist = dist;
            }
        }

        return closestDist;
    }

    /// <summary>
    /// Pick a Component from the given list that is a member of the GameObject that is closest (in world space) to the given Component's GameObject.
    /// </summary>
    /// <param name="objList">A list of Components to search.</param>
    /// <param name="target">The Component to measure distance from.</param>
    /// <param name="closest">Output value that is the Component that is a member of the GameObject that is closest. (Or null if none were found.)</param>
    /// <returns>The distance to the closest GameObject.  (Or float.MaxValue if none were found.)</returns>
    static public float FindClosest(this IEnumerable<Component> objList, Component target, out Component closest)
    {
        closest = null;
        float closestDist = float.MaxValue;

        foreach (Component obj in objList)
        {
            float dist = Vector3.Distance(target.transform.position, obj.transform.position);
            if (closest == null || dist < closestDist)
            {
                closest = obj;
                closestDist = dist;
            }
        }

        return closestDist;
    }

    /// <summary>
    /// A lerp between two vectors using a hermite curve.
	/// See more for Hermite polynomials:  https://en.wikipedia.org/wiki/Hermite_polynomials
	/// </summary>
	/// <param name="start">The value at the beginning of the interpolation.</param>
	/// <param name="end">The value at then end of the interpolation.</param>
	/// <param name="value">A percentile of completion (from 0.0 to 1.0) between start and finish.</param>
	/// <returns>The interpolated value between the start and end values.</returns>
    static public Vector3 Hermite(Vector3 start, Vector3 end, float t)
    {
        return Vector3.Lerp(start, end, Mathfx.Hermite(0, 1, t));
    }
}
