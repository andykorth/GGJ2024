using System;
using System.Runtime.CompilerServices;

namespace Weasel
{
public static class EasingMath
{
	const float PI = 3.14159265358f;
	const float TAU = 6.28318530718f;

	const MethodImplOptions INLINE = MethodImplOptions.AggressiveInlining;

	[MethodImpl(INLINE)] static bool Hit0(float v) => v <= 0;
	[MethodImpl(INLINE)] static bool Hit1(float v) => v >= 1;
	[MethodImpl(INLINE)] static bool BelowHalf(float v) => v < 0.5f;
	[MethodImpl(INLINE)] static bool AboveHalf(float v) => v >= 0.5f;
	[MethodImpl(INLINE)] static float Cos(float x) => (float)Math.Cos(x);
	[MethodImpl(INLINE)] static float Sin(float x) => (float)Math.Sin(x);
	[MethodImpl(INLINE)] static float Tan(float x) => (float)Math.Tan(x);
	[MethodImpl(INLINE)] static float Sqrt(float x) => (float)Math.Sqrt(x);
	[MethodImpl(INLINE)] static float Pow(float v, float p) => (float)Math.Pow(v, p);
	[MethodImpl(INLINE)] static float Pow2(float v) => (float)Math.Pow(v, 2);
	[MethodImpl(INLINE)] static float Pow3(float v) => (float)Math.Pow(v, 3);
	[MethodImpl(INLINE)] static float Pow4(float v) => (float)Math.Pow(v, 4);
	[MethodImpl(INLINE)] static float Pow5(float v) => (float)Math.Pow(v, 5);


	public static float Linear(float frac) => frac;

	public static float QuadIn(float frac)
	{
		return Pow2(frac);
	}

	public static float QuadOut(float frac)
	{
		return 1f - (1f - frac) * (1f - frac);
	}

	public static float QuadInOut(float frac)
	{
		return BelowHalf(frac)
			? 2f * Pow2(frac)
			: 1f - Pow2(-2f * frac + 2) / 2f;
	}

	public static float CubicIn(float frac)
	{
		return Pow3(frac);
	}

	public static float CubicOut(float frac)
	{
		return 1f - Pow3(1f - frac);
	}

	public static float CubicInOut(float frac)
	{
		return BelowHalf(frac)
			? 4 * Pow3(frac)
			: 1f - Pow3(-2f * frac + 2) / 2f;
	}

	public static float QuartIn(float frac)
	{
		return Pow4(frac);
	}

	public static float QuartOut(float frac)
	{
		return 1f - Pow4(1f - frac);
	}

	public static float QuartInOut(float frac)
	{
		return BelowHalf(frac)
			? 8 * Pow4(frac)
			: 1f - Pow4(-2f * frac + 2) / 2f;
	}

	public static float QuintIn(float frac)
	{
		return Pow5(frac);
	}

	public static float QuintOut(float frac)
	{
		return 1f - Pow5(1f - frac);
	}

	public static float QuintInOut(float frac)
	{
		return BelowHalf(frac)
			? 16 * Pow5(frac)
			: 1f - Pow5(-2f * frac + 2) / 2f;
	}

	public static float SineIn(float frac)
	{
		return 1f - Cos((frac * PI) / 2f);
	}

	public static float SineOut(float frac)
	{
		return Sin((frac * PI) / 2f);
	}

	public static float SineInOut(float frac)
	{
		return -(Cos(PI * frac) - 1) / 2f;
	}

	public static float ExpoIn(float frac)
	{
		if (Hit0(frac)) return 0f;
		return Pow(2f, 10f * frac - 10f);
	}

	public static float ExpoOut(float frac)
	{
		if (Hit1(frac)) return 1f;
		return 1f - Pow(2, -10f * frac);
	}

	public static float ExpoInOut(float frac)
	{
		if (Hit0(frac)) return 0f;
		if (Hit1(frac)) return 1f;
		return BelowHalf(frac)
			? Pow(2f, 20f * frac - 10f) / 2f
			: (2f - Pow(2f, -20 * frac + 10f)) / 2f;
	}

	public static float CircIn(float frac)
	{
		return 1f - Sqrt(1f - Pow2(frac));
	}

	public static float CircOut(float frac)
	{
		return Sqrt(1f - Pow2(frac - 1));
	}

	public static float CircInOut(float frac)
	{
		return BelowHalf(frac)
			? (1f - Sqrt(1f - Pow2(2f * frac))) / 2f
			: (Sqrt(1f - Pow2(-2f * frac + 2f)) + 1f) / 2f;
	}

	const float MAGIC_BACK1 = 1.70158f; // minimum of -10%
	const float MAGIC_BACK2 = MAGIC_BACK1 * 1.525f;
	const float MAGIC_BACK3 = MAGIC_BACK1 + 1f;

	public static float BackIn(float frac)
	{
		return MAGIC_BACK3 * Pow3(frac) - MAGIC_BACK1 * frac * frac;
	}

	public static float BackOut(float frac)
	{
		return 1f + MAGIC_BACK3 * Pow3(frac - 1) + MAGIC_BACK1 * Pow2(frac - 1);
	}

	public static float BackInOut(float frac)
	{
		return BelowHalf(frac)
			? (Pow2(2f * frac) * ((MAGIC_BACK2 + 1) * 2f * frac - MAGIC_BACK2)) / 2f
			: (Pow2(2f * frac - 2) * ((MAGIC_BACK2 + 1) * (frac * 2f - 2f) + MAGIC_BACK2) + 2f)
			/ 2f;
	}

	const float MAGIC_ELASTIC1 = TAU / 3f;
	const float MAGIC_ELASTIC2 = TAU / 4.5f;

	public static float ElasticIn(float frac)
	{
		if (Hit0(frac)) return 0f;
		if (Hit1(frac)) return 1f;
		return -Pow(2, 10 * frac - 10) * Sin((frac * 10 - 10.75f) * MAGIC_ELASTIC1);
	}

	public static float ElasticOut(float frac)
	{
		if (Hit0(frac)) return 0f;
		if (Hit1(frac)) return 1f;
		return Pow(2, -10 * frac) * Sin((frac * 10 - 0.75f) * MAGIC_ELASTIC1) + 1f;
	}

	public static float ElasticInOut(float frac)
	{
		if (Hit0(frac)) return 0f;
		if (Hit1(frac)) return 1f;
		return BelowHalf(frac)
			? -(Pow(2, 20 * frac - 10) * Sin((20 * frac - 11.125f) * MAGIC_ELASTIC2)) / 2f
			: (Pow(2, -20 * frac + 10) * Sin((20 * frac - 11.125f) * MAGIC_ELASTIC2)) / 2f
			+ 1f;
	}

	const float MAGIC_BOUNCE1 = 2.75f;
	const float MAGIC_BOUNCE2 = 7.5625f;

	public static float BounceIn(float frac)
	{
		return 1f - BounceOut(1f - frac);
	}

	public static float BounceOut(float frac)
	{
		if (frac < 1f / MAGIC_BOUNCE1) {
			return MAGIC_BOUNCE2 * frac * frac;
		}

		if (frac < 2f / MAGIC_BOUNCE1) {
			var b1 = frac - 1.5f / MAGIC_BOUNCE1;
			return MAGIC_BOUNCE2 * b1 * b1 + 0.75f;
		}

		if (frac < 2.5f / MAGIC_BOUNCE1) {
			var b2 = frac - 2.25f / MAGIC_BOUNCE1;
			return MAGIC_BOUNCE2 * b2 * b2 + 0.9375f;
		}

		var b3 = frac - 2.625f / MAGIC_BOUNCE1;
		return MAGIC_BOUNCE2 * b3 * b3 + 0.984375f;
	}

	public static float BounceInOut(float frac)
	{
		return BelowHalf(frac)
			? (1f - BounceOut(1f - 2f * frac)) / 2f
			: (1f + BounceOut(2f * frac - 1)) / 2f;
	}
}
}