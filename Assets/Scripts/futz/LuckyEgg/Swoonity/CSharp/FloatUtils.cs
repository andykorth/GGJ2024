using System;
using System.Collections.Generic;
using UnityEngine;
using V3 = UnityEngine.Vector3;

// TODO: uses UnityEngine
namespace Swoonity.CSharp
{
public static class FloatUtils
{
	public static float Squared(this float val) => val * val;
	public static float Sqrt(this float val) => Mathf.Sqrt(val);

	/// if val is zero, return new val instead
	public static float Or(this float val, float or) => Mathf.Approximately(val, 0) ? or : val;

	/// if val is zero, return new val instead
	public static float EnsureNotZero(this float val, float newValIfZero = 0.0001f)
		=> Mathf.Approximately(val, 0) ? newValIfZero : val;

	public static bool IsZero(this float val) => Mathf.Approximately(val, 0f);
	public static bool NotZero(this float val) => !Mathf.Approximately(val, 0f);
	public static bool IsOne(this float val) => Mathf.Approximately(val, 1f);
	public static bool NotOne(this float val) => !Mathf.Approximately(val, 1f);

	// public static bool Is0(this float val) => Mathf.Approximately(val, 0f);
	// public static bool Not0(this float val) => !Mathf.Approximately(val, 0f);
	// public static bool Is1(this float val) => Mathf.Approximately(val, 1f);
	// public static bool Not1(this float val) => !Mathf.Approximately(val, 1f);

	/// val ? ifTrue : ifFalse
	public static float ToFloat(this bool val, float ifTrue = 1f, float ifFalse = 0f) => val ? ifTrue : ifFalse;

	public static float Abs(this float val) => Mathf.Abs(val);

	public static bool Approx(this float val, float other) => Mathf.Approximately(val, other);
	public static bool Approx(this (float a, float b) vals) => Mathf.Approximately(vals.a, vals.b);


	public static float Greatest(this (float a, float b) vals)
		=> vals.a >= vals.b ? vals.a : vals.b;

	/// return cap if val is over
	public static float Cap(this float val, float cap) => val > cap ? cap : val;

	/// return min if val is under
	public static float OrMin(this float val, float min) => val < min ? min : val;

	/// return max if val is over
	public static float OrMax(this float val, float max) => val > max ? max : val;

	public static float OrMinMax(this float val, float min, float max = float.MaxValue)
	{
		if (val < min) return min;
		if (val > max) return max;
		return val;
	}


	/// Clamps float between min..max
	public static float Clamp(this float val, float min, float max)
	{
		if (val < min) return min;
		if (val > max) return max;
		return val;
	}

	/// Clamps float between min..max
	public static float Clamp(this float val, (float min, float max) range)
		=> val.Clamp(range.min, range.max);

	/// Clamp between 0..1
	public static float Clamp01(this float val) => val.Clamp(0, 1);

	/// Clamp between -1..1
	public static float ClampN1P1(this float val) => val.Clamp(-1, 1);

	/// val.Clamp(-plusOrMinus, plusOrMinus)
	public static float ClampAroundZero(this float val, float plusOrMinus)
		=> val.Clamp(-plusOrMinus, plusOrMinus);


	/// (mid - plusMinus, mid + plusMinus)
	public static (float, float) PlusMinus(this float mid, float plusMinus)
		=> (mid - plusMinus, mid + plusMinus);

	/// round to nearest (skip if zero)
	public static float TryRoundTo(this float val, float amount)
	{
		if (amount == 0) return val;
		return Mathf.Round(val / amount) * amount;
	}

	/// round to nearest
	public static float RoundTo(this float val, float amount) => Mathf.Round(val / amount) * amount;

	/// round to nearest whole number
	public static float RoundWhole(this float val) => Mathf.Round(val);

	/// round to nearest .1f
	public static float Round1Decimal(this float val) => Mathf.Round(val / .1f) * .1f;

	/// round to nearest .01f
	public static float Round2Decimal(this float val) => Mathf.Round(val / .01f) * .01f;

	/// round to nearest .001f
	public static float Round3Decimal(this float val) => Mathf.Round(val / .001f) * .001f;

	/// round to nearest .0001f
	public static float Round4Decimal(this float val) => Mathf.Round(val / .0001f) * .0001f;

	/// round to nearest .00001f
	public static float Round5Decimal(this float val) => Mathf.Round(val / .00001f) * .00001f;

	/// round to nearest .000001f
	public static float Round6Decimal(this float val) => Mathf.Round(val / .000001f) * .000001f;

	/// round to nearest whole number
	public static float R0(this float val) => Mathf.Round(val);

	/// round to nearest .1f
	public static float R1(this float val) => Mathf.Round(val / .1f) * .1f;

	/// round to nearest .01f
	public static float R2(this float val) => Mathf.Round(val / .01f) * .01f;

	/// round to nearest .001f
	public static float R3(this float val) => Mathf.Round(val / .001f) * .001f;

	/// round to nearest .0001f
	public static float R4(this float val) => Mathf.Round(val / .0001f) * .0001f;

	/// round to nearest .00001f
	public static float R5(this float val) => Mathf.Round(val / .00001f) * .00001f;

	/// round to nearest .000001f
	public static float R6(this float val) => Mathf.Round(val / .000001f) * .000001f;

	/// Maps from range to range
	public static float Map(
		this float val,
		(float min, float max) from,
		(float min, float max) to,
		bool clamp = true
	)
	{
		var percent = (val - from.min) / from.Range();
		var mapped = to.Range() * percent + to.min;
		return clamp ? mapped.Clamp(to) : mapped;
	}

	/// Maps min..max value to 0..1 range
	public static float Map01(this float val, float min, float max, bool clamp = true)
		=> val.Map((min, max), (0, 1), clamp);


	/// Maps 0..max value to 0..1 range
	public static float Map01(this float val, float max, bool clamp = true)
		=> val.Map((0, max), (0, 1), clamp);

	/// Maps min..max value to -1..1 range
	public static float MapN1P1(this float val, float min, float max, bool clamp = true)
		=> val.Map((min, max), (-1, 1), clamp);

	/// Maps 0..max value to -1..1 range
	public static float MapN1P1(this float val, float max, bool clamp = true)
		=> val.Map((0, max), (-1, 1), clamp);

	/// Maps 0..1 value to min..max range
	public static float MapFrom01(this float val, float min, float max, bool clamp = true)
		=> val.Map((0, 1), (min, max), clamp);

	/// Maps -1..1 value to min..max range
	public static float MapFromN1P1(this float val, float min, float max, bool clamp = true)
		=> val.Map((-1, 1), (min, max), clamp);

	/// max - min
	public static float Range(this (float min, float max) vals) => vals.max - vals.min;

	public static (float val01, float val) ClampMap01(this float val, float min, float max)
	{
		var percent = (val - min) / (max - min);
		if (percent < 0) return (0, min);
		if (percent > 1) return (1, max);
		return (percent, val);
	}

	public static (float val01, float val) ClampMapN1P1(this float val, float min, float max)
	{
		var percent = (val - min) / (max - min);
		var mapped = 2 * percent - 1;
		if (mapped < -1) return (-1, min);
		if (mapped > 1) return (1, max);
		return (mapped, val);
	}

	const float ONE_HALF = 1 / 2f;
	const float ONE_THIRD = 1 / 3f;
	const float TWO_THIRD = 2 / 3f;
	const float ONE_FOURTH = 1 / 4f;
	const float TWO_FOURTH = 2 / 4f;
	const float THREE_FOURTH = 3 / 4f;

	/// <remarks>HALF</remarks>
	/// 0.0 [-- A --] 0.5 [-- B --] 1.0
	public static T MapOn<T>(this float val01, T a, T b) => val01 < ONE_HALF ? a : b;

	/// <remarks>THIRDS</remarks>
	/// 0.0 [-- A --] 0.33333334 [-- B --]  0.6666667 [-- C --] 1.0
	public static T MapOn<T>(this float val01, T a, T b, T c)
		=> val01 switch {
			< ONE_THIRD => a,
			< TWO_THIRD => b,
			_ => c,
		};

	/// <remarks>FOURTHS</remarks>
	/// 0.0 [-- A --] 0.25 [-- B --]  0.5 [-- C --] .75 [-- D --] 1.0
	public static T MapOn<T>(this float val01, T a, T b, T c, T d)
		=> val01 switch {
			< ONE_FOURTH => a,
			< TWO_FOURTH => b,
			< THREE_FOURTH => c,
			_ => d,
		};

	/// TODO: untested
	public static T MapOn<T>(this float val01, List<T> list)
	{
		var count = list.Count;
		if (count == 0) return default;

		var index = (val01 * count).FloorToInt();
		if (index < 0) index = 0;
		if (index >= 1) index = count - 1;

		return list[index];
	}

	/// V3.LerpUnclamped(start, end, val01)
	public static V3 Map(this float val01, V3 start, V3 end)
		=> V3.LerpUnclamped(
			start,
			end,
			val01
		);

	/// 1 - val01
	public static float Inverse01(this float val01) => 1 - val01;


	/// from 0..360 to -180..180
	public static float To180Angle(this float val) => val < 180f ? val : val - 360f;

	public static float Radian(this float degrees) => degrees * Mathf.Deg2Rad;
	public static float Degree(this float radians) => radians * Mathf.Rad2Deg;


	// TODO: cleanup
	// TODO: cleanup
	// TODO: cleanup
	// TODO: cleanup

	/// <summary>
	/// Returns true if value is within min and max.
	/// </summary>
	public static bool IsWithin(this float value, float min, float max)
	{
		return value >= min && value <= max;
	}

	/// Returns absolute distance between two floats.
	public static float DistanceTo(this float floatA, float floatB)
	{
		return Mathf.Abs(floatA - floatB);
	}

	/// this function already multiplies by deltaTime
	public static float MoveTowards(this float from, float to, float speedPerSec)
		=> Mathf.MoveTowards(from, to, speedPerSec * Time.deltaTime);

	/// Returns int of rounded float.
	public static int RoundToInt(this float thisFloat)
	{
		return Mathf.RoundToInt(thisFloat);
	}

	/// Returns int of floored float.
	public static int FloorToInt(this float thisFloat)
	{
		return Mathf.FloorToInt(thisFloat);
	}

	/// Returns int of floored float.
	public static int CeilToInt(this float thisFloat)
	{
		return Mathf.CeilToInt(thisFloat);
	}

	/// Returns float multiplied by multiplier.
	public static float MultiplyBy(this float thisFloat, float multiplier)
	{
		return thisFloat * multiplier;
	}


	/// Returns target float clamped between min and max, but adds float first
	public static float ClampAdd(
		this float thisFloat,
		float toAdd,
		float min = 0,
		float max = 1
	)
	{
		return (thisFloat + toAdd).Clamp(min, max);
	}

	/// Returns target float clamped between min and max, but subtracts float first
	public static float ClampSubtract(
		this float thisFloat,
		float toSubtract,
		float min = 0,
		float max = 1
	)
	{
		return (thisFloat - toSubtract).Clamp(min, max);
	}

	/// If positive: 1, if negative: -1, else 0. Uses significantZeroValue to avoid floating point errors
	public static int ConvertToSign(
		this float thisFloat,
		float significantZeroValue = 0.0001f
	)
	{
		if (thisFloat > significantZeroValue) return +1;
		if (thisFloat < -significantZeroValue) return -1;
		return 0;
	}

	/// 1 - {0..1} (for reversing a percentage, .2 => .8) 
	public static float Complement(this float percent) => 1 - percent;


	/// 
	public static float RoundToHalf(this float thisFloat)
	{
		return (float)Math.Round(thisFloat * 2, MidpointRounding.AwayFromZero) / 2;
	}

	// /// Maps from range to range
	// public static float MapFromTo(
	// 	this float val, 
	// 	float fromMin, 
	// 	float fromMax, 
	// 	float toMin, 
	// 	float toMax,
	// 	bool clamp = true
	// ) {
	// 	var fromMagnitude = fromMax - fromMin;
	// 	var toMagnitude = toMax - toMin;
	// 	var flattenedVal = val - fromMin;
	// 	var mapped = toMagnitude * (flattenedVal / fromMagnitude) + toMin;
	// 	return clamp ? mapped.Clamp(toMin, toMax) : mapped;
	// }

	#region right triangle helpers

	public static float Sin(this float val) => Mathf.Sin(val);
	public static float Asin(this float val) => Mathf.Asin(val);
	public static float ClampAsin(this float val) => Mathf.Asin(val.ClampN1P1());

	public static float Cos(this float val) => Mathf.Cos(val);
	public static float Acos(this float val) => Mathf.Acos(val);
	public static float ClampAcos(this float val) => Mathf.Acos(val.ClampN1P1());

	public static float Tan(this float val) => Mathf.Tan(val);

	public static float Atan(this float val) => Mathf.Atan(val);
	// public static float ClampAtan(this float val) => Mathf.Atan(val.ClampN1P1());

	/// c = sqrt( a^2 + b^2 )
	public static float Pythag(this (float a, float b) t) => (t.a.Squared() + t.b.Squared()).Sqrt();

	public static float Adj_OppHyp(this (float opp, float hyp) t) => (t.hyp, -t.opp).Pythag();

	public static float Adj_DegreeHyp(this (float degree, float hyp) t)
		=> t.degree.Radian().Cos() * t.hyp;

	public static float Adj_RadianHyp(this (float radian, float hyp) t) => t.radian.Cos() * t.hyp;

	public static float Adj_DegreeOpp(this (float degree, float opp) t)
		=> t.opp / t.degree.Radian().Tan();

	public static float Adj_RadianOpp(this (float radian, float opp) t) => t.opp / t.radian.Tan();

	public static float Hyp_OppAdj(this (float opp, float adj) t) => t.Pythag();

	public static float Hyp_DegreeAdj(this (float degree, float adj) t)
		=> t.adj / t.degree.Radian().Cos();

	public static float Hyp_RadianAdj(this (float radian, float adj) t) => t.adj / t.radian.Cos();

	public static float Hyp_DegreeOpp(this (float degree, float opp) t)
		=> t.opp / t.degree.Radian().Sin();

	public static float Hyp_RadianOpp(this (float radian, float opp) t) => t.opp / t.radian.Sin();

	public static float Opp_AdjHyp(this (float adj, float hyp) t) => (t.hyp, -t.adj).Pythag();

	public static float Opp_DegreeAdj(this (float degree, float adj) t)
		=> t.degree.Radian().Tan() * t.adj;

	public static float Opp_DegreeHyp(this (float degree, float hyp) t)
		=> t.degree.Radian().Sin() * t.hyp;

	public static float Opp_RadianAdj(this (float radian, float adj) t) => t.radian.Tan() * t.adj;
	public static float Opp_RadianHyp(this (float radian, float hyp) t) => t.radian.Sin() * t.hyp;

	public static float Degree_AdjHyp(this (float adj, float hyp) t)
		=> (t.adj / t.hyp).ClampAcos().Degree();

	public static float Radian_AdjHyp(this (float adj, float hyp) t) => (t.adj / t.hyp).ClampAcos();

	public static float Degree_OppAdj(this (float opp, float adj) t)
		=> (t.opp / t.adj).Atan().Degree();

	public static float Radian_OppAdj(this (float opp, float adj) t) => (t.opp / t.adj).Atan();

	public static float Degree_OppHyp(this (float opp, float hyp) t)
		=> (t.opp / t.hyp).ClampAsin().Degree();

	public static float Radian_OppHyp(this (float opp, float hyp) t) => (t.opp / t.hyp).ClampAsin();

	#endregion
}


public class FloatEquality : EqualityComparer<float>
{
	public static readonly EqualityComparer<float> I = new FloatEquality();

	public override bool Equals(float x, float y) => Mathf.Approximately(x, y);
	public override int GetHashCode(float x) => x.GetHashCode();
}
}