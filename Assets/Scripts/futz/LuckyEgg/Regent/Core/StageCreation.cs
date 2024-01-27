using System;
using System.Collections.Generic;
using Regent.StageFacts;
using Swoonity.CSharp;
using Swoonity.MHasher;
using static UnityEngine.Debug;

namespace Regent.Staging
{
public enum RegentDomain
{
	UNSET = 0,

	NATIVE = 1_000, // runs on anything

	SERVER = 5_000, // worker runs if this is the Server
	CLIENT = 6_000, // worker runs if this is a Client

	AUTHOR = 7_000, // worker runs if we have authority over target Entity (can be server or client)
	REMOTE = 8_000, // worker runs if we do NOT have authority over target Entity
}

public enum UnityCycle
{
	UNSET,

	SPAWN,
	UPDATE,
	LATE,
	FIXED,
	CUSTOM,
}

public static class StageCreation
{
	/// splits on STAGE_MARKER
	/// [0] = cycle
	/// [1] = stage name
	public const string STAGE_MARKER = "___REGENT_STAGE___";

	public static class StageMarker
	{
		public const string Spawn = nameof(UnityCycle.SPAWN) + STAGE_MARKER;
		public const string Update = nameof(UnityCycle.UPDATE) + STAGE_MARKER;
		public const string Late = nameof(UnityCycle.LATE) + STAGE_MARKER;
		public const string Fixed = nameof(UnityCycle.FIXED) + STAGE_MARKER;
		public const string Custom = nameof(UnityCycle.CUSTOM) + STAGE_MARKER;

		public const string __SKIP_STAGE__ = Custom + nameof(__SKIP_STAGE__); //?? + STAGE_MARKER ??
	}

	public static (UnityCycle, string) PlaceholderToCycleName(string ph)
	{
		// Log($"Stage PlaceholderToCycleName: {ph}"._RLog(RLog.Important));

		var split = ph.Split(STAGE_MARKER, StringSplitOptions.RemoveEmptyEntries);

		if (split == null || split.Length != 2)
			return (UnityCycle.UNSET, "");

		return (split[0].ToEnum<UnityCycle>(), split[1]);
	}

	public static (
		List<StageFact> spawnStages,
		List<StageFact> updateStages,
		List<StageFact> lateStages,
		List<StageFact> fixedStages,
		List<StageFact> customStages,
		List<StageFact> allStages
		) MakeStageFacts(Type stageSourceType)
	{
		var spawnStages = new List<StageFact>();
		var updateStages = new List<StageFact>();
		var lateStages = new List<StageFact>();
		var fixedStages = new List<StageFact>();
		var customStages = new List<StageFact>();
		var allStages = new List<StageFact>();

		var constantFields = stageSourceType.GetConstantFields<string>();

		foreach (var (field, ph) in constantFields) {
			var (cycle, stageName) = PlaceholderToCycleName(ph);

			if (cycle == UnityCycle.UNSET) continue; //## not a stage, skip

			if (stageName != field.Name) throw StageErrors.NameMismatch(stageName, ph, field.Name);

			var stage = new StageFact {
				HashId = MHash.Hash(stageName),
				Name = stageName,
				Cycle = cycle
			};

			var cycleList = cycle switch {
				UnityCycle.SPAWN => spawnStages,
				UnityCycle.UPDATE => updateStages,
				UnityCycle.LATE => lateStages,
				UnityCycle.FIXED => fixedStages,
				UnityCycle.CUSTOM => customStages,
				UnityCycle.UNSET => throw StageErrors.BadCycle(cycle),
				_ => throw StageErrors.BadCycle(cycle),
			};
			cycleList.Add(stage);
			allStages.Add(stage);
		}

		return (
			spawnStages,
			updateStages,
			lateStages,
			fixedStages,
			customStages,
			allStages
		);
	}
}

public static class StageErrors
{
	public static Exception NameMismatch(string stageName, string ph, string fieldName)
		=> new($"Stage name mismatch {stageName} ({ph} vs {fieldName})");

	public static Exception BadCycle(UnityCycle cycle) => new ArgumentOutOfRangeException();
}
}