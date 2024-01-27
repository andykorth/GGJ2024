using UnityEngine.UI;

namespace Swoonity.Unity
{
public static class ImageUtils
{
	/// <summary>
	/// Sets image alpha
	/// </summary>
	public static void SetAlpha(this Image image, float alpha)
	{
		image.color = image.color.WithAlpha(alpha);
	}

	public static void On(this Image image)
	{
		image.gameObject.On();
	}

	public static void Off(this Image image)
	{
		image.gameObject.Off();
	}

	public static void SetActive(this Image image, bool isActive)
	{
		image.gameObject.SetActive(isActive);
	}
}
}