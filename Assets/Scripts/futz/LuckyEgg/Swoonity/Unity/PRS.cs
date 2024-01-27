using System;
using UnityEngine;

namespace Swoonity.Unity
{
/// struct of position, rotation, scale
[Serializable]
public struct PRS
{
	public Vector3 Pos;
	public Quaternion Rot;
	public Vector3 Sca;

	public PRS(Vector3 pos)
	{
		Pos = pos;
		Rot = Quaternion.identity;
		Sca = Vector3.one;
	}

	public PRS(Vector3 pos, Quaternion rot)
	{
		Pos = pos;
		Rot = rot;
		Sca = Vector3.one;
	}

	public PRS(Vector3 pos, Quaternion rot, Vector3 sca)
	{
		Pos = pos;
		Rot = rot;
		Sca = sca;
	}

	public void Apply(Transform tform)
	{
		tform.position = Pos;
		tform.rotation = Rot;
		tform.localScale = Sca;
	}

	public void ApplyLocal(Transform tform)
	{
		tform.localPosition = Pos;
		tform.localRotation = Rot;
		tform.localScale = Sca;
	}

	public static PRS From(Transform tform)
		=> new PRS {
			Pos = tform.position,
			Rot = tform.rotation,
			Sca = tform.localScale,
		};

	public static PRS FromLocal(Transform tform)
		=> new PRS {
			Pos = tform.localPosition,
			Rot = tform.localRotation,
			Sca = tform.localScale,
		};
}

public static class PrsUtils
{
	public static PRS ToPrs(this Transform tform) => PRS.From(tform);
	public static PRS ToPrsLocal(this Transform tform) => PRS.FromLocal(tform);

	public static void Apply(this Transform tform, PRS prs) => prs.Apply(tform);
	public static void ApplyLocal(this Transform tform, PRS prs) => prs.ApplyLocal(tform);
}
}