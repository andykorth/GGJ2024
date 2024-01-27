using System;

namespace Weasel
{
public enum Easing
{
	Linear,
	QuadIn,
	QuadOut,
	QuadInOut,
	CubicIn,
	CubicOut,
	CubicInOut,
	QuartIn,
	QuartOut,
	QuartInOut,
	QuintIn,
	QuintOut,
	QuintInOut,
	SineIn,
	SineOut,
	SineInOut,
	ExpoIn,
	ExpoOut,
	ExpoInOut,
	CircIn,
	CircOut,
	CircInOut,
	BackIn,
	BackOut,
	BackInOut,
	ElasticIn,
	ElasticOut,
	ElasticInOut,
	BounceIn,
	BounceOut,
	BounceInOut,
}

/// applies easing to fraction
public delegate float FnEasing(float frac);

public static class WeaseFns
{
	public static FnEasing Linear = static frac => EasingMath.Linear(frac);
	public static FnEasing QuadIn = static frac => EasingMath.QuadIn(frac);
	public static FnEasing QuadOut = static frac => EasingMath.QuadOut(frac);
	public static FnEasing QuadInOut = static frac => EasingMath.QuadInOut(frac);
	public static FnEasing CubicIn = static frac => EasingMath.CubicIn(frac);
	public static FnEasing CubicOut = static frac => EasingMath.CubicOut(frac);
	public static FnEasing CubicInOut = static frac => EasingMath.CubicInOut(frac);
	public static FnEasing QuartIn = static frac => EasingMath.QuartIn(frac);
	public static FnEasing QuartOut = static frac => EasingMath.QuartOut(frac);
	public static FnEasing QuartInOut = static frac => EasingMath.QuartInOut(frac);
	public static FnEasing QuintIn = static frac => EasingMath.QuintIn(frac);
	public static FnEasing QuintOut = static frac => EasingMath.QuintOut(frac);
	public static FnEasing QuintInOut = static frac => EasingMath.QuintInOut(frac);
	public static FnEasing SineIn = static frac => EasingMath.SineIn(frac);
	public static FnEasing SineOut = static frac => EasingMath.SineOut(frac);
	public static FnEasing SineInOut = static frac => EasingMath.SineInOut(frac);
	public static FnEasing ExpoIn = static frac => EasingMath.ExpoIn(frac);
	public static FnEasing ExpoOut = static frac => EasingMath.ExpoOut(frac);
	public static FnEasing ExpoInOut = static frac => EasingMath.ExpoInOut(frac);
	public static FnEasing CircIn = static frac => EasingMath.CircIn(frac);
	public static FnEasing CircOut = static frac => EasingMath.CircOut(frac);
	public static FnEasing CircInOut = static frac => EasingMath.CircInOut(frac);
	public static FnEasing BackIn = static frac => EasingMath.BackIn(frac);
	public static FnEasing BackOut = static frac => EasingMath.BackOut(frac);
	public static FnEasing BackInOut = static frac => EasingMath.BackInOut(frac);
	public static FnEasing ElasticIn = static frac => EasingMath.ElasticIn(frac);
	public static FnEasing ElasticOut = static frac => EasingMath.ElasticOut(frac);
	public static FnEasing ElasticInOut = static frac => EasingMath.ElasticInOut(frac);
	public static FnEasing BounceIn = static frac => EasingMath.BounceIn(frac);
	public static FnEasing BounceOut = static frac => EasingMath.BounceOut(frac);
	public static FnEasing BounceInOut = static frac => EasingMath.BounceInOut(frac);

	public static FnEasing GetFn(this Easing easing)
		=> easing switch {
			Easing.Linear => Linear,
			Easing.QuadIn => QuadIn,
			Easing.QuadOut => QuadOut,
			Easing.QuadInOut => QuadInOut,
			Easing.CubicIn => CubicIn,
			Easing.CubicOut => CubicOut,
			Easing.CubicInOut => CubicInOut,
			Easing.QuartIn => QuartIn,
			Easing.QuartOut => QuartOut,
			Easing.QuartInOut => QuartInOut,
			Easing.QuintIn => QuintIn,
			Easing.QuintOut => QuintOut,
			Easing.QuintInOut => QuintInOut,
			Easing.SineIn => SineIn,
			Easing.SineOut => SineOut,
			Easing.SineInOut => SineInOut,
			Easing.ExpoIn => ExpoIn,
			Easing.ExpoOut => ExpoOut,
			Easing.ExpoInOut => ExpoInOut,
			Easing.CircIn => CircIn,
			Easing.CircOut => CircOut,
			Easing.CircInOut => CircInOut,
			Easing.BackIn => BackIn,
			Easing.BackOut => BackOut,
			Easing.BackInOut => BackInOut,
			Easing.ElasticIn => ElasticIn,
			Easing.ElasticOut => ElasticOut,
			Easing.ElasticInOut => ElasticInOut,
			Easing.BounceIn => BounceIn,
			Easing.BounceOut => BounceOut,
			Easing.BounceInOut => BounceInOut,
			_ => throw new ArgumentOutOfRangeException(nameof(easing), easing, null)
		};
}
}