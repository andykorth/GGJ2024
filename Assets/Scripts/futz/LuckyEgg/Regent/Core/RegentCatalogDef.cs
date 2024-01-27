using System;
using System.Collections.Generic;
using System.Linq;
using Regent.BaronFacts;
using Regent.CogFacts;
using Regent.Cogs;
using Regent.Logging;
using Regent.StageFacts;
using Regent.Staging;
using Regent.SyncerFacts;
using Regent.WorkerFacts;
using Swoonity.Collections;
using Swoonity.MHasher;
using Swoonity.Unity;
using UnityEngine;
using static UnityEngine.Debug;

// ReSharper disable InconsistentNaming

namespace Regent.Catalog
{
public abstract class RegentCatalogDef : ScriptableObject
{
	// TODO: button
	[Header("Config")]
	[Tooltip("Reloads with editor OnValidate")]
	public bool AutoLoad = true;

	public bool ClickToTriggerAutoLoad = false;

	[Header("Devtime facts")]
	public List<BaronFact> BaronFacts;

	public List<WorkerFact> WorkerFacts;
	public List<CogFact> CogFacts;
	public List<SyncerFact> SyncerFacts;

	[Header("Stage facts")]
	public List<StageFact> SpawnStages;

	public List<StageFact> UpdateStages;
	public List<StageFact> LateStages;
	public List<StageFact> FixedStages;
	public List<StageFact> CustomStages;
	public List<StageFact> AllStages;


	void OnValidate()
	{
		if (AutoLoad) RegentCatalogDevtime.DevtimeInitialize(this);
		ClickToTriggerAutoLoad = false;
	}

	[ContextMenu("Force Load/Validate")]
	public void ForceLoadAndValidate()
	{
		RegentCatalogDevtime.DevtimeInitialize(this);
	}

	public abstract Type GetStageSourceType();
}

public static class RegentCatalogDevtime
{
	public static void DevtimeInitialize(RegentCatalogDef catalog)
	{
		Log($"Devtime Initialize Regent Catalog"._RLog(RLog.Important));

		(
			catalog.SpawnStages,
			catalog.UpdateStages,
			catalog.LateStages,
			catalog.FixedStages,
			catalog.CustomStages,
			catalog.AllStages
		) = StageCreation.MakeStageFacts(catalog.GetStageSourceType());

		catalog.BaronFacts = BaronFactMakers
		   .MakeBaronFacts()
		   .OrderBy(static f => f.Name)
		   .ToList();

		catalog.WorkerFacts = catalog.BaronFacts
		   .SelectMany(static b => b.WorkerFacts)
		   .OrderBy(static f => f.Name)
		   .ToList();

		catalog.CogFacts = CogFactMakers
		   .MakeCogFacts()
		   .OrderBy(static f => f.Name)
		   .ToList();

		catalog.SyncerFacts = catalog.CogFacts
		   .SelectMany(static c => c.SyncerFacts)
		   .OrderBy(static f => f.Name)
		   .ToList();

		catalog.SetDirtyIfEditor();

		Log(
			(
				$"Loaded: "
			  + $"{catalog.AllStages.Count} stages, "
			  + $"{catalog.BaronFacts.Count} barons, "
			  + $"{catalog.WorkerFacts.Count} workers, "
			  + $"{catalog.CogFacts.Count} cogs, "
			  + $"{catalog.SyncerFacts.Count} syncers"
			  + $" | {DateTime.Now.ToShortTimeString()}"
			)._RLog(RLog.Important)
		);
	}
}

public static class RegentCatalogRuntime
{
	public static RegentCatalogDef __DefInstance;

	public static MHashLup<BaronFact> BaronFactLup = new();
	public static MHashLup<CogInfo> CogInfoLup = new();
	public static MHashLup<SyncerInfo> SyncerInfoLup = new();

	public static List<StageState> StageStates = new(); // TODO

	public static void RuntimeInitialize(RegentCatalogDef catalog)
	{
		// Log($"Runtime Initialize Regent Catalog"._RLog(RLog.Important));

		__DefInstance = catalog;

		BaronFactLup.Clear();
		CogInfoLup.Clear();
		SyncerInfoLup.Clear();
		StageStates.Clear();

		BaronFactLup.AddRange(catalog.BaronFacts);

		foreach (var stageFact in catalog.AllStages) {
			StageStates.Add(
				new StageState {
					Name = stageFact.Name,
					Fact = stageFact,
				}
			);
		}

		foreach (var cogFact in catalog.CogFacts) {
			var cogInfo = CogFactMakers.MakeCogInfo(cogFact);
			CogInfoLup.Set(cogFact.HashId, cogInfo);

			foreach (var syncerInfo in cogInfo.SyncerInfos) {
				SyncerInfoLup.Set(syncerInfo.Fact.HashId, syncerInfo);
			}
		}
	}

	public static BaronFact GetBaronFact(MHash hash)
		=> BaronFactLup.GetOrThrow(hash, CatalogErrors.MissingBaronFact);

	public static CogInfo GetCogInfo(MHash hash)
		=> CogInfoLup.GetOrThrow(hash, CatalogErrors.MissingCogInfo);

	public static SyncerInfo GetSyncerInfo(MHash hash)
		=> SyncerInfoLup.GetOrThrow(hash, CatalogErrors.MissingSyncerInfo);
}


/*
	TODO: look at these again
*/
public static class RegentValidators
{
	/// The idea here is HashIds will be deterministically set at edit-time.
	/// TODO: use this to check & kick off Entity caching?
	public static void ValidateCog(ICog cog)
	{
		var cogType = cog.GetType();
		var current = cog.GetHash();
		var hash = MHash.Hash(cogType);

		if (current != hash) {
			cog.SetHashId(hash);
			// TODO: cache here? (implies maybe cog was just added)
		}
	}
}

public static class DevtimeHashChecks
{
	// TODO: re-enable maybe?
	// public static RHash.HashTypes BaronHashTypes = new RHash.HashTypes(typeof(Baron));
	// public static RHash.HashTypes CogHashTypes = new RHash.HashTypes(typeof(ICog));
	//
	// public static void CheckBaron(Type type) => BaronHashTypes.Check(type);
	// public static void CheckCog(Type type) => CogHashTypes.Check(type);
}

public static class CatalogErrors
{
	public static Func<MHash, Exception> MissingBaronFact = static hash => new Exception(
		$"missing BaronFact hash: {hash} "
	  + $"| usually caused by __todo__"
	);

	public static Func<MHash, Exception> MissingCogInfo = static hash => new Exception(
		hash == 0
			? $"hash is 0 | did you mistakenly override OnValidate?"
			: $"missing CogRunInfo hash: {hash} "
	);

	public static Func<MHash, Exception> MissingSyncerInfo = static hash => new Exception(
		hash == 0
			? $"hash is 0 | did you mistakenly override OnValidate?"
			: $"missing SyncerRunInfo hash: {hash} "
	);
}
}