using System;
using System.Collections.Generic;
using Regent.Cogs;
using Regent.SyncerFacts;
using Swoonity.Collections;
using Swoonity.CSharp;
using Swoonity.MHasher;

namespace Regent.CogFacts
{
/// Generated at devtime
[Serializable]
public class CogFact : IMHashable
{
	public string Name;
	public MHash HashId;
	public MHash GetHash() => HashId;

	public SyncerFact[] SyncerFacts;

	public override string ToString() => $"cogFact[{Name}]";
}

[Serializable]
public class CogInfo : IMHashable
{
	public string Name;
	public CogFact Fact;
	public MHash GetHash() => Fact.HashId;

	public SyncerInfo[] SyncerInfos;

	public override string ToString() => $"cogStat[{Name}]";
}

public static class CogFactMakers
{
	public static List<CogFact> MakeCogFacts()
	{
		return AppDomain.CurrentDomain
		   .TypesWithInterface<ICog>()
		   .Map(MakeCogFact)
		   .ToList();
	}

	public static CogFact MakeCogFact(Type cogType)
		=> new() {
			HashId = MHash.Hash(cogType),
			Name = cogType.Name,
			SyncerFacts = SyncerFactMakers.MakeSyncerFacts(cogType),
		};

	public static CogInfo MakeCogInfo(CogFact cogFact)
		=> new() {
			Fact = cogFact,
			Name = cogFact.Name,
			SyncerInfos = cogFact.SyncerFacts
			   .Map(
					static syncerFact => new SyncerInfo {
						Name = syncerFact.Name,
						Fact = syncerFact,
					}
				),
		};

	public static SyncerInfo GetSyncerInfoByName(this CogInfo cogInfo, string syncerName)
	{
		var syncerStat = cogInfo.SyncerInfos.FirstOrNull(s => s.Fact.Name == syncerName);
		if (syncerStat == null)
			throw new Exception($"{cogInfo} missing syncer: {syncerName} (check worker Attribute)");
		return syncerStat;
	}
}
}