using System;

namespace Swoonity.CSharp
{
public static class ByteUtils
{
	/// 0..255 -> 0f..1f
	public static float AsPercent(this byte b) => b / 255f;

	/// <summary>
	/// Invokes a callback N number of times
	/// </summary>
	public static void Times(this byte numberOfTimes, Action<byte> callback)
	{
		for (byte i = 0; i < numberOfTimes; i++) {
			callback(i);
		}
	}
}
}