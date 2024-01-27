using UnityEngine;

namespace Swoonity.Unity
{
public static class RectUtils
{
	/// rect with new width value
	public static Rect WithWidth(this Rect rect, float val)
	{
		rect.width = val;
		return rect;
	}

	/// rect with new height value
	public static Rect WithHeight(this Rect rect, float val)
	{
		rect.height = val;
		return rect;
	}

	/// rect with new x value
	public static Rect WithX(this Rect rect, float val)
	{
		rect.x = val;
		return rect;
	}

	/// rect with new y value
	public static Rect WithY(this Rect rect, float val)
	{
		rect.y = val;
		return rect;
	}

	/// rect with width += val
	public static Rect AddWidth(this Rect rect, float val)
	{
		rect.width += val;
		return rect;
	}

	/// rect with height += val
	public static Rect AddHeight(this Rect rect, float val)
	{
		rect.height += val;
		return rect;
	}

	/// rect with x += val
	public static Rect AddX(this Rect rect, float val)
	{
		rect.x += val;
		return rect;
	}

	/// rect with y += val
	public static Rect AddY(this Rect rect, float val)
	{
		rect.y += val;
		return rect;
	}
}
}