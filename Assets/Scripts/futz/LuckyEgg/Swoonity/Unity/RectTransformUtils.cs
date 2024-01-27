using UnityEngine;

namespace Swoonity.Unity
{
public static class RectTransformUtils
{
	/// <summary>
	/// Set RectTransform width.
	/// </summary>
	public static void SetWidth(this RectTransform rect, float width)
	{
		rect.SetSizeDelta(rect.sizeDelta.WithX(width));
	}

	/// <summary>
	/// Set RectTransform height.
	/// </summary>
	public static void SetHeight(this RectTransform rect, float height)
	{
		rect.SetSizeDelta(rect.sizeDelta.WithY(height));
	}

	/// <summary>
	/// Set RectTransform size delta.
	/// </summary>
	public static void SetSizeDelta(this RectTransform rect, Vector2 vector)
	{
		rect.sizeDelta = vector;
	}

	/// <summary>
	/// Set RectTransform size delta.
	/// </summary>
	public static void SetSizeDelta(this RectTransform rect, float x, float y)
	{
		rect.sizeDelta = new Vector2(x, y);
	}

	/// <summary>
	/// Returns width of rectTransform.
	/// </summary>
	public static float GetWidth(this RectTransform rect)
	{
		return rect.rect.width;
	}

	/// <summary>
	/// Returns height of rectTransform.
	/// </summary>
	public static float GetHeight(this RectTransform rect)
	{
		return rect.rect.height;
	}

	/// <summary>
	/// Set RectTransform's anchored position X
	/// </summary>
	public static void SetAnchoredX(this RectTransform rect, float x)
	{
		rect.anchoredPosition = rect.anchoredPosition.WithX(x);
	}

	/// <summary>
	/// Set RectTransform's anchored position Y
	/// </summary>
	public static void SetAnchoredY(this RectTransform rect, float y)
	{
		rect.anchoredPosition = rect.anchoredPosition.WithY(y);
	}

	/// <summary>
	/// Set RectTransform's anchored position X and Y
	/// </summary>
	public static void SetAnchoredPos(this RectTransform rect, float x, float y)
	{
		rect.anchoredPosition = rect.anchoredPosition.WithX(x).WithY(y);
	}

	/// <summary>
	/// Set RectTransform's anchored position X and Y
	/// </summary>
	public static void SetAnchoredPos(this RectTransform rect, Vector2 vector)
	{
		rect.anchoredPosition = rect.anchoredPosition.WithX(vector.x).WithY(vector.y);
	}

	/// <summary>
	/// Set RectTransform's minimum Anchor
	/// </summary>
	public static void SetAnchorMin(this RectTransform rect, Vector2 vector)
	{
		rect.anchorMin = vector;
	}

	/// <summary>
	/// Set RectTransform's maximum Anchor
	/// </summary>
	public static void SetAnchorMax(this RectTransform rect, Vector2 vector)
	{
		rect.anchorMax = vector;
	}

	/// <summary>
	/// Set RectTransform's minimum Anchor X
	/// </summary>
	public static void SetAnchorMinX(this RectTransform rect, float x)
	{
		rect.anchorMin = rect.anchorMin.WithX(x);
	}

	/// <summary>
	/// Set RectTransform's minimum Anchor Y
	/// </summary>
	public static void SetAnchorMinY(this RectTransform rect, float y)
	{
		rect.anchorMin = rect.anchorMin.WithY(y);
	}

	/// <summary>
	/// Set RectTransform's maximum Anchor X
	/// </summary>
	public static void SetAnchorMaxX(this RectTransform rect, float x)
	{
		rect.anchorMax = rect.anchorMax.WithX(x);
	}

	/// <summary>
	/// Set RectTransform's maximum Anchor Y
	/// </summary>
	public static void SetAnchorMaxY(this RectTransform rect, float y)
	{
		rect.anchorMax = rect.anchorMax.WithY(y);
	}
}
}