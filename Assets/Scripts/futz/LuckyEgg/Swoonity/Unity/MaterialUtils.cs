using UnityEngine;

namespace Swoonity.Unity
{
public static class MaterialUtils
{
	/// mat ? mat : or
	public static Material Or(this Material mat, Material or) => mat ? mat : or;
}
}