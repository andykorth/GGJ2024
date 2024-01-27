using System;

namespace Swoonity.CSharp
{
public static class ShortUtils
{
	/// Returns if ushort is within min and max
	public static bool IsWithin(this ushort value, ushort min, ushort max)
	{
		return value >= min && value <= max;
	}

	/// Invokes a callback N number of times
	public static void Times(this ushort numberOfTimes, Action<ushort> callback)
	{
		for (ushort i = 0; i < numberOfTimes; i++) {
			callback(i);
		}
	}
}
}