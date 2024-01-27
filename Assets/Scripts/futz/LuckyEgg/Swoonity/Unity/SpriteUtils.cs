using System.Collections.Generic;
using UnityEngine;

namespace Swoonity.Unity
{
public static class SpriteUtils
{
	/// if sprite == null, return or
	public static Sprite Or(this Sprite sprite, Sprite or) => sprite ? sprite : or;

	/// (each in list).color = color
	public static void SetColor(this List<SpriteRenderer> list, Color color)
	{
		foreach (var spriteRenderer in list) {
			spriteRenderer.color = color;
		}
	}
}
}