using UnityEngine;

namespace Swoonity.CSharp
{
[System.Serializable]
public struct FloatRange
{
	public float Min;
	public float Max;

	public FloatRange(float min, float max)
	{
		Min = min;
		Max = max;
	}

	public float Random()
	{
		return UnityEngine.Random.Range(Min, Max);
	}

	public float Clamp(float val)
	{
		return Mathf.Clamp(val, Min, Max);
	}

	public bool IsWithin(float val)
	{
		return val >= Min && val <= Max;
	}
}

public static class FloatRangeUtils
{
	public static bool IsWithin(this float val, FloatRange floatRange)
	{
		return floatRange.IsWithin(val);
	}

	public static bool IsOutOfBounds(this float val, FloatRange floatRange)
	{
		return !floatRange.IsWithin(val);
	}
}
}