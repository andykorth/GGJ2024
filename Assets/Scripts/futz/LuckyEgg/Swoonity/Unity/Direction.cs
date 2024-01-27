using System;
using V3 = UnityEngine.Vector3;

namespace Swoonity.Unity
{
public enum Dir3
{
	/// 0, 0, 0
	NONE,

	/// -1, 0, 0
	LEFT,

	/// +1, 0, 0
	RIGHT,

	/// 0, -1, 0
	DOWN,

	/// 0, +1, 0
	UP,

	/// 0, 0, -1
	BACK,

	/// 0, 0, +1
	FORWARD,
}

public static class DirectionUtils
{
	public static V3 ToV3(this Dir3 dir)
		=> dir switch {
			Dir3.NONE => V3.zero,
			Dir3.LEFT => V3.left,
			Dir3.RIGHT => V3.right,
			Dir3.DOWN => V3.down,
			Dir3.UP => V3.up,
			Dir3.BACK => V3.back,
			Dir3.FORWARD => V3.forward,
			_ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
		};
}
}