using System;

namespace Swoonity.CSharp
{
public static class IntUtils
{
	/// return greater of both values
	public static int Greater(this int a, int b) => a < b ? b : a;

	/// return cap if val is over
	public static int Cap(this int val, int cap) => val > cap ? cap : val;

	public static int ParseOr(this string str, int or = 0) => int.TryParse(str, out var i) ? i : or;

	/// val.ToString($"D{digits}")
	public static string Str(this int val, int digits) => val.ToString($"D{digits}");

	/// val.ToString($"D{digits}")
	public static string StringPad(this int val, int digits) => val.ToString($"D{digits}");

	/// if val is zero, return new val instead
	public static int Or(this int val, int or) => val == 0 ? or : val;

	/// return min if val is under
	public static int OrMin(this int val, int min) => val < min ? min : val;

	/// return max if val is over
	public static int OrMax(this int val, int max) => val > max ? max : val;

	public static byte ToByte(this int value) => Convert.ToByte(value);
	public static ushort ToUShort(this int value) => Convert.ToUInt16(value);

	/// value == min OR min..max OR max (inclusive)
	public static bool IsWithin(this int value, int min, int max) => value >= min && value <= max;

	/// value + add
	public static int Add(this int value, int add) => value + add;

	/// value++
	public static int Inc(this int value) => value + 1;

	/// clamps (min, max-1) but with wrapping
	public static int Wrap(this int value, int min, int maxExcl)
	{
		if (value < min) return maxExcl - 1;
		if (value >= maxExcl) return min;
		return value;
	}

	/// clamps (0, max-1) but with wrapping
	public static int Wrap(this int value, int maxExcl) => value.Wrap(0, maxExcl);

	/// clamps (min, max) but with wrapping
	public static int WrapInclusive(this int value, int min, int max)
	{
		if (value < min) return max;
		if (value > max) return min;
		return value;
	}

	/// clamps (0, max) but with wrapping
	public static int WrapInclusive(this int value, int max) => value.Wrap(0, max);

	public static int IndexClamp(this int index, int count, bool canWrap = false)
	{
		if (index < 0) return canWrap ? count - 1 : 0;
		if (index >= count) return canWrap ? 0 : count - 1;
		return index;
	}

	// /// <summary>
	// /// Invokes a callback N number of times
	// /// </summary>
	// public static void Times(this int numberOfTimes, Action<int> callback) {
	// 	for (var i = 0; i < numberOfTimes; i++) {
	// 		callback(i);
	// 	}
	// }

	/// <summary>
	/// Tests a condition func up to N number of times until it succeeds or runs out of tries
	/// </summary>
	public static bool Tries(this int numberOfTries, Func<bool> condition)
	{
		while (numberOfTries > 0) {
			if (condition()) {
				return true;
			}

			numberOfTries--;
		}

		return false;
	}

	/// Returns target int clamped between min and max (incl)
	public static int Clamp(this int thisInt, int min, int max = Int32.MaxValue)
	{
		if (thisInt < min) return min;
		if (thisInt > max) return max;
		return thisInt;
	}

	public static int Pow(this int thisInt, int toPowerOf)
	{
		var result = 1;
		for (int i = 0; i < toPowerOf; i++) {
			result *= thisInt;
		}

		return result;
	}

	public static bool IsEven(this int thisInt)
	{
		return thisInt % 2 == 0;
	}

	public static bool IsOdd(this int thisInt)
	{
		return thisInt % 2 != 0;
	}

	public static int ToHundreds(this int val) => val / 100 * 100;
	public static int ToThousands(this int val) => val / 1000 * 1000;
	public static int ToMillions(this int val) => val / 1000000 * 1000000;
}
}