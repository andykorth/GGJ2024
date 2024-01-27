using System;

namespace Swoonity.CSharp
{
[Serializable]
public struct IntRange
{
	public int Min;
	public int Max;

	public IntRange(int min, int max)
	{
		Min = min;
		Max = max;
	}

	public int Random()
	{
		return UnityEngine.Random.Range(Min, Max + 1);
	}
}
}