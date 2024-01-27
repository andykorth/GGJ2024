using LapUtil;
using Regent.Logging;
using UnityEngine;

namespace Foundational
{
/// they can go here for now
public static class GlobalMiscSystemInitializers
{
	public static void Initialize(MonoBehaviour compForCoroutines)
	{
		Lumberjack.Lumberjack.FnGetGlobalPrefix
			= static () => $"<color=#E0FFEF>{RLog.CurrentCycle}.{RLog.CurrentStage}</color>|";

		Idealist.Randomization.Range = UnityEngine.Random.Range;

		compForCoroutines.StartCoroutine(Lap.Timing());
	}
}
}