using System;

namespace Swoonity.CSharp
{
public static class BinaryUtils
{
	/// get binary string: 5 => "101"
	public static string Bits(this int val) => Convert.ToString(val, 2);

	/// get binary string: 5 => "101"
	public static string Bits(this uint val) => Convert.ToString((int)val, 2);

	/// get binary string: 5 => "101"
	public static string Bits(this long val) => Convert.ToString(val, 2);

	/// get binary string: 5 => "101"
	public static string Bits(this ulong val) => Convert.ToString((long)val, 2);


	public static bool Intersects(this int mask, int other) => (mask & other) != 0;
	public static bool Intersects(this uint mask, uint other) => (mask & other) != 0;
	public static bool Intersects(this long mask, long other) => (mask & other) != 0;
	public static bool Intersects(this ulong mask, ulong other) => (mask & other) != 0;

	public static bool BitAny(this int mask, int other) => (mask & other) != 0;
	public static bool BitAny(this uint mask, uint other) => (mask & other) != 0;
	public static bool BitAny(this long mask, long other) => (mask & other) != 0;
	public static bool BitAny(this ulong mask, ulong other) => (mask & other) != 0;

	public static ulong ToBitMask64(this byte[] values, bool checkValidRange = false)
	{
		var mask = 0UL;

		if (checkValidRange) {
			foreach (var val in values) {
				if (val >= 64)
					throw new ArgumentOutOfRangeException($"ToBitMask64: requires value < 64");
				mask |= 1UL << val;
			}

			return mask;
		}

		foreach (var val in values) {
			mask |= 1UL << val;
		}

		return mask;
	}
}

/*

See: FlagUtils

*/
}