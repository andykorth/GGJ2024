using UnityEngine;

namespace Swoonity.Unity
{
public static class CanvasUtils
{
	/// Returns the width of the Canvas (scaled by scaleFactor)
	public static float WidthScaled(this Canvas canvas)
	{
		return canvas.pixelRect.width / canvas.scaleFactor;
	}

	/// Returns the height of the Canvas (scaled by scaleFactor)
	public static float HeightScaled(this Canvas canvas)
	{
		return canvas.pixelRect.height / canvas.scaleFactor;
	}
}
}