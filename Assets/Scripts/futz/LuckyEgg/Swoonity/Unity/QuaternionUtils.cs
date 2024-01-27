using UnityEngine;
using Quat = UnityEngine.Quaternion;
using V3 = UnityEngine.Vector3;

namespace Swoonity.Unity
{
public static class QuaternionUtils
{
	public static Quat Inverse(this Quat quat) => Quaternion.Inverse(quat);

	public static Quat Difference(this Quat quat1, Quat quat2) => quat1 * quat2.Inverse();

	public static V3 DifferenceEuler(this Quat quat1, Quat quat2, bool normalize)
		=> normalize
			? quat1.Difference(quat2).eulerAngles.normalized
			: quat1.Difference(quat2).eulerAngles;

	public static V3 Right(this Quat quat, float magnitude = 1) => quat * V3.right * magnitude;
	public static V3 Up(this Quat quat, float magnitude = 1) => quat * V3.up * magnitude;
	public static V3 Forward(this Quat quat, float magnitude = 1) => quat * V3.forward * magnitude;

	public static (float angle, V3 axis) AngleAxis(this Quat quat)
	{
		quat.ToAngleAxis(out var angle, out var axis);
		return (angle, axis);
	}
}
}