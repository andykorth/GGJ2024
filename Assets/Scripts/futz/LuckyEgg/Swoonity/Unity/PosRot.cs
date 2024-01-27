using UnityEngine;

namespace Swoonity.Unity
{
/// struct wrapper of transform.position & transform.rotation
public struct PosRot
{
	public Vector3 Position;
	public Quaternion Rotation;

	public PosRot(Vector3 pos = default, Quaternion rot = default)
	{
		Position = pos;
		Rotation = rot;
	}

	public PosRot(Transform tform)
	{
		Position = tform.position;
		Rotation = tform.rotation;
	}

	public static PosRot Make(Transform tform)
		=> new PosRot { Position = tform.position, Rotation = tform.rotation, };

	public static PosRot Above(Transform tform, float addY)
		=> new PosRot { Position = tform.position.WithAddedY(addY), Rotation = tform.rotation, };

	public static PosRot Around(Transform tform, float distance)
		=> new PosRot {
			Position = tform.position.RandomAroundXZ(distance), Rotation = Quaternion.identity,
		};
}

public static class PosRotUtils
{
	public static PosRot PosRot(this Transform tform) => new PosRot(tform);
}
}