using System;
using UnityEngine;

namespace Sonic
{
public static class SonicSugarFns
{
	/// required for Play sugar
	public static Action<SonicSfx, Vector3> FnPlayAt = static (_, _)
		=> throw new Exception($"Missing required SonicSugarFns");
	
	/// required for Play sugar
	public static Action<SonicSfx> FnPlayUi = static _
		=> throw new Exception($"Missing required SonicSugarFns");

	/// null checks SonicSfx, then plays at the position
	/// <remarks>Requires SonicSugarFns</remarks>
	public static void PlayAt(this SonicSfx sfx, Vector3 position)
	{
		if (sfx) FnPlayAt(sfx, position);
	}

	/// null checks SonicSfx and Transform, then plays at its position
	/// <remarks>Requires SonicSugarFns</remarks>
	public static void PlayAt(this SonicSfx sfx, Transform tf)
	{
		if (sfx && tf) FnPlayAt(sfx, tf.position);
	}

	/// null checks SonicSfx and MonoBehaviour, then plays at its position
	/// <remarks>Requires SonicSugarFns</remarks>
	public static void PlayAt(this SonicSfx sfx, MonoBehaviour mb)
	{
		if (sfx && mb) FnPlayAt(sfx, mb.transform.position);
	}
	
	/// null checks SonicSfx, then plays in UI (non-spatial)
	/// <remarks>Requires SonicSugarFns</remarks>
	public static void PlayUi(this SonicSfx sfx)
	{
		if (sfx) FnPlayUi(sfx);
	}
}
}