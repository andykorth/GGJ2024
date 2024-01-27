using UnityEngine;

namespace Swoonity.Unity
{
public static class RayUtils
{
	public static Ray RayForward(this Transform tf) => new Ray(tf.position, tf.forward);
}
}