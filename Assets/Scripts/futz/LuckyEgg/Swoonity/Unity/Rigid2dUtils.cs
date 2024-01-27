using Swoonity.CSharp;
using UnityEngine;
using Rigid = UnityEngine.Rigidbody2D;
using V2 = UnityEngine.Vector2;

namespace Swoonity.Unity
{
public static class Rigid2dUtils
{
	// ForceMode2D.Force: velocity += (v * deltaTime) / mass
	// ForceMode2D.Impulse: velocity += v / mass

	public static void Force(
		this Rigid rigid,
		V2 force,
		bool useMass = true,
		bool useDeltaTime = true
	)
		=> rigid.AddForce(
			useMass ? force : force * rigid.mass,
			useDeltaTime ? ForceMode2D.Force : ForceMode2D.Impulse
		);

	public static void Force(
		this Rigid rigid,
		V2 force,
		V2 atPosition,
		bool useMass = true,
		bool useDeltaTime = true
	)
		=> rigid.AddForceAtPosition(
			useMass ? force : force * rigid.mass,
			atPosition,
			useDeltaTime ? ForceMode2D.Force : ForceMode2D.Impulse
		);

	/// positive: counter-clockwise, negative: clockwise (thanks Unity)
	public static void Torque(
		this Rigid rigid,
		float torqueCCW,
		bool useMass = true,
		bool useDeltaTime = true,
		bool useDegrees = false
	)
	{
		if (useDegrees) torqueCCW = torqueCCW.Radian() * rigid.inertia;
		rigid.AddTorque(
			useMass ? torqueCCW : torqueCCW * rigid.mass,
			useDeltaTime ? ForceMode2D.Force : ForceMode2D.Impulse
		);
	}
}
}