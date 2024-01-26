using System;
using UnityEngine;

/// <summary>
/// Contains a library of static functions that provide math functions similar to those in Unity's Mathf class.
/// </summary>
public static class Mathfx
{
	/// <summary>
	/// Interpolate from the given start value to the given end value using a Hermite spline.
	/// See more for Hermite polynomials:  https://en.wikipedia.org/wiki/Hermite_polynomials
	/// </summary>
	/// <param name="start">The value at the beginning of the interpolation.</param>
	/// <param name="end">The value at then end of the interpolation.</param>
	/// <param name="value">A percentile of completion (from 0.0 to 1.0) between start and finish.</param>
	/// <returns>The interpolated value between the start and end values.</returns>
	public static float Hermite(float start, float end, float value)
	{
		return Mathf.Lerp (start, end, value * value * (3.0f - 2.0f * value));
	}
	
	public static float Map(float from1, float to1, float from2, float to2, float t)
	{
		float min = (from2 > to2 ? to2 : from2);
		float max = (from2 > to2 ? from2 : to2);
		return Mathf.Clamp (((t - from1) / (to1 - from1) * (to2 - from2)) + from2, min, max);
	}

	/// <summary>
	/// Interpolate from the given start value to the given end value using a epsilon curve.
	/// (This will exponentially approach, but never reach the end value.)
	/// </summary>
	/// <param name="a">The value at the beginning of the interpolation.</param>
	/// <param name="b">The value at the end of the interpolation.</param>
	/// <param name="lambda">The speed of interpolation.</param>
	/// <param name="dt">The amount of time, in seconds, since the start of the interpolation.</param>
	/// <returns>The interpolated value between the start and end values.</returns>
	public static float Damp(float a, float b, float lambda, float dt)
	{
		return Mathf.Lerp(a, b, 1 - Mathf.Exp(-lambda * dt));
	}

	/// <summary>
	/// Interpolate from the given start Vector3 to the given end Vector3 using an epsilon curve.
	/// (This will exponentially approach, but never reach the end value.)
	/// </summary>
	/// <param name="a">The value at the beginning of the interpolation.</param>
	/// <param name="b">The value at the end of the interpolation.</param>
	/// <param name="lambda">The speed of interpolation.</param>
	/// <param name="dt">The amount of time, in seconds, since the start of the interpolation.</param>
	/// <returns>The interpolated value between the start and end values.</returns>
	public static Vector3 Damp(Vector3 a, Vector3 b, float lambda, float dt)
	{
		a.x = Damp (a.x, b.x, lambda, dt);
		a.y = Damp (a.y, b.y, lambda, dt);
		a.z = Damp (a.z, b.z, lambda, dt);

		return a;
	}

	/// <summary>
	/// Test if the difference between two floats is within a given range.
	/// </summary>
	/// <param name="a">The first value to test.</param>
	/// <param name="b">The second value to test.</param>
	/// <param name="range">The acceptable range of error between the test values.</param>
	/// <returns>True if the difference of the two values are within the given range.</returns>
	public static bool FloatsAreNear(float a, float b, float range = 0.001f)
	{
		return Mathf.Abs(a - b) < range;
	}

	/// <summary>
	/// Constant-rate interpolation between two values.
	/// See Scott's handy doccumentation here: https://gamasutra.com/blogs/ScottLembcke/20180418/316665/Logarithmic_Interpolation.php
	/// </summary>
	/// <param name="a">The start value.</param>
	/// <param name="b">The end value.</param>
	/// <param name="t">An interpolation distance from 0.0 to 1.0.</param>
	/// <returns>The interpolated value between the start and end values.</returns>
	public static float Logerp(float a, float b, float t) => a*Mathf.Pow(b/a, t);
	
	//https://www.iquilezles.org/www/articles/smin/smin.htm
	public static float smooth_min_exp(float a, float b, float k) => -Mathf.Log(Mathf.Exp(-k*a) + Mathf.Exp(-k*b))/k;

    public static Vector2 Rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }


    public static float smooth_max_exp(float a, float b, float k) =>  Mathf.Log(Mathf.Exp( k*a) + Mathf.Exp( k*b))/k;
	public static float smooth_clamp_exp(float value, float min, float max, float k) => smooth_max_exp(min, smooth_min_exp(value, max, k), k);
	
	// Golden ratio.
	public const float PHI = 1.6180339887498948482f;
	
	public static Vector2 R2Noise(int i) => new Vector2(
		(0.7548776662466927f*i)%1,
		(0.5698402909980532f*i)%1
	);
}
