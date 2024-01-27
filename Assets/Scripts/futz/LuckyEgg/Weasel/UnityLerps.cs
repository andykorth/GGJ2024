using UnityEngine;

namespace Weasel
{
public static class UnityLerps
{
	public static void DefineAll()
	{
		LerpFunctions.DefineType(FnFloat);
		LerpFunctions.DefineType(FnInt);
		LerpFunctions.DefineType(FnVector2);
		LerpFunctions.DefineType(FnVector3);
		LerpFunctions.DefineType(FnQuaternion);
	}

	public static FnLerp<float> FnFloat = (a, b, f) => a + ((b - a) * f);
	public static FnLerp<int> FnInt = (a, b, f) => a + Mathf.RoundToInt((b - a) * f);
	public static FnLerp<Vector2> FnVector2 = (a, b, f) => Vector2.Lerp(a, b, f);
	public static FnLerp<Vector3> FnVector3 = (a, b, f) => Vector3.Lerp(a, b, f);
	public static FnLerp<Quaternion> FnQuaternion = (a, b, f) => Quaternion.Lerp(a, b, f);
}
}