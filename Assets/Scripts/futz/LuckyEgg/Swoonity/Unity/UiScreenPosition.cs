using UnityEngine;
using UnityEngine.UI;

namespace Swoonity.Unity
{
public static class UiScreenPosition
{
	public enum UiScreenQuadrant
	{
		Unset,
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight,
	}

	public static void SetScreenByWorld(
		this RectTransform rect,
		Vector3 worldPosition,
		Camera camera
	)
	{
		rect.position = camera.WorldToScreenPoint(worldPosition);
	}

	public static UiScreenQuadrant CalculateScreenQuadrant(this RectTransform rect, Camera camera)
	{
		var viewportPosition = camera.ScreenToViewportPoint(rect.position);
		if (viewportPosition.y < .5f) {
			if (viewportPosition.x < .5f) {
				return UiScreenQuadrant.BottomLeft;
			}
			else {
				return UiScreenQuadrant.BottomRight;
			}
		}
		else if (viewportPosition.x < .5f) {
			return UiScreenQuadrant.TopLeft;
		}

		return UiScreenQuadrant.TopRight;
	}

	public static void SetTextSideByQuadrant(this Text text, UiScreenQuadrant quadrant)
	{
		switch (quadrant) {
			default:
			case UiScreenQuadrant.TopLeft:
			case UiScreenQuadrant.BottomLeft:
				text.rectTransform.pivot = new Vector2(0, .5f);
				text.rectTransform.anchorMin = new Vector2(1, .5f);
				text.rectTransform.anchorMax = new Vector2(1, .5f);
				text.alignment = TextAnchor.MiddleLeft;
				break;
			case UiScreenQuadrant.TopRight:
			case UiScreenQuadrant.BottomRight:
				text.rectTransform.pivot = new Vector2(1, .5f);
				text.rectTransform.anchorMin = new Vector2(0, .5f);
				text.rectTransform.anchorMax = new Vector2(0, .5f);
				text.alignment = TextAnchor.MiddleRight;
				break;
		}
	}

	public static void SetPivotByQuadrant(
		this RectTransform rect,
		UiScreenQuadrant quadrant,
		float offsetX = 0,
		float offsetY = 0
	)
	{
		switch (quadrant) {
			default:
			case UiScreenQuadrant.TopLeft:
				rect.pivot = new Vector2(0, 1);
				rect.anchoredPosition = new Vector2(offsetX, -offsetY);
				break;
			case UiScreenQuadrant.TopRight:
				rect.pivot = new Vector2(1, 1);
				rect.anchoredPosition = new Vector2(-offsetX, -offsetY);
				break;
			case UiScreenQuadrant.BottomLeft:
				rect.pivot = new Vector2(0, 0);
				rect.anchoredPosition = new Vector2(offsetX, offsetY);
				break;
			case UiScreenQuadrant.BottomRight:
				rect.pivot = new Vector2(1, 0);
				rect.anchoredPosition = new Vector2(-offsetX, offsetY);
				break;
		}
	}
}
}