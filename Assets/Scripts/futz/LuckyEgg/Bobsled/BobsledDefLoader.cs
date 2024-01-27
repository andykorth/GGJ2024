using System.Collections.Generic;
using System.Linq;
using Bobsled.Defs;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Bobsled.DefLoader
{
public static class BobsledDefLoader
{
	public static List<Definition> FindAll()
	{
#if UNITY_EDITOR

		return AssetDatabase.FindAssets($"t:{nameof(Definition)}")
		   .Select(
				guid =>
					AssetDatabase.LoadAssetAtPath<Definition>(AssetDatabase.GUIDToAssetPath(guid))
			)
		   .ToList();
#else
			return null;
#endif
	}
}
}