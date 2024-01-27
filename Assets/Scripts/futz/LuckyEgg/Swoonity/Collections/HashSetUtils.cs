using System.Collections.Generic;

namespace Swoonity.Collections
{
public static class HashSetUtils
{
	public static bool IsEmpty<T>(this HashSet<T> hash) => hash.Count == 0;
	public static bool IsNotEmpty<T>(this HashSet<T> hash) => hash.Count > 0;

	public static string Join<T>(this HashSet<T> hash, string delimiter = ", ")
	{
		return string.Join(delimiter, hash);
	}
}
}