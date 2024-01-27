using UnityEngine;

namespace Swoonity.Unity
{
public static class ColorUtils
{
	static Color DEFAULT_COLOR = default;

	/// color != default
	public static bool IsValid(this Color color) => color != DEFAULT_COLOR;
	public static bool IsDefault(this Color color) => color == DEFAULT_COLOR;

	/// color != default ? color : or;
	public static Color Or(this Color color, Color or) => color != DEFAULT_COLOR ? color : or;

	/// Returns color with new alpha
	public static Color32 WithA(this Color32 color, float alpha)
	{
		color.a = (byte)Mathf.RoundToInt(255f * alpha);
		return color;
	}

	/// Converts color to 3 floats
	public static Vector4 ToVector(this Color color)
	{
		return new Vector4(color.r, color.g, color.b, color.a);
	}

	/// Color => "000000"
	public static string ToHtml(this Color color, bool hashtag = false)
		=> hashtag
			? $"#{ColorUtility.ToHtmlStringRGB(color)}"
			: ColorUtility.ToHtmlStringRGB(color);

	public static Color ToColor(this string hexString)
	{
		ColorUtility.TryParseHtmlString(hexString, out var color);
		return color;
	}

	/// Sets alpha and returns the Color.
	public static Color WithAlpha(this Color color, float newAlpha)
	{
		color.a = newAlpha;
		return color;
	}

	/// Sets alpha 0..100 and returns the Color.
	public static Color Alpha(this Color color, int percent)
	{
		color.a = percent / 100f;
		return color;
	}
}
}