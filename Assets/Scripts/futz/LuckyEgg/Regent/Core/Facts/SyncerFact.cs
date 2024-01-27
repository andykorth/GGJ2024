using System;
using System.Linq;
using System.Reflection;
using Regent.Cogs;
using Regent.Syncers;
using Regent.Workers;
using Swoonity.Collections;
using Swoonity.CSharp;
using Swoonity.MHasher;

namespace Regent.SyncerFacts
{
[Serializable]
public class SyncerFact
{
	public string Name;
	public MHash HashId;
	public ClientAuthType AuthType;
	public string CogName;
	public MHash CogHash;

	public bool NeedsVerify => AuthType != ClientAuthType.Native;

	public override string ToString() => $"syncerFact[{CogName}.{Name}]";
}

[Serializable]
public class SyncerInfo : IMHashable
{
	public string Name;
	public SyncerFact Fact;
	public MHash GetHash() => Fact.HashId; // CogName+SyncerName

	public bool IsInitialized;
	public BaseVerifyForwarder VerifyFwd; // forwards cues to VerifyWorker
	public BaseReactForwarder ReactFwd; // forwards cues to ReactWorker list

	public override string ToString() => $"syncerInfo[{Name}]";
}


public static class SyncerFactMakers
{
	public static SyncerFact[] MakeSyncerFacts(Type cogType)
		=> cogType
		   .GetFieldsOfType<ISyncer>()
		   .Map(MakeSyncerFact, cogType)
		   .ToArray();

	public static SyncerFact MakeSyncerFact(FieldInfo field, Type cogType)
		=> new() {
			Name = field.Name,
			HashId = MHash.Hash(field.DeclaringType.FullName, field.Name),
			AuthType = GetAuthTypeFromType(field.FieldType),
			CogName = cogType.Name,
			CogHash = MHash.Hash(cogType),
		};

	/// fragile, but whatevs
	public static ClientAuthType GetAuthTypeFromType(Type type)
	{
		var name = type.Name;

		if (name.Has($"{ClientAuthType.Control}")) return ClientAuthType.Control;
		if (name.Has($"{ClientAuthType.Respect}")) return ClientAuthType.Respect;
		if (name.Has($"{ClientAuthType.Propose}")) return ClientAuthType.Propose;
		if (name.Has($"{ClientAuthType.Declare}")) return ClientAuthType.Declare;
		if (name.Has($"{ClientAuthType.Monitor}")) return ClientAuthType.Monitor;

		return ClientAuthType.Native;
	}

	public static void DevtimeInitializeCogSyncers(ICog cog)
	{
		var entity = cog.GetEntity();
		var cogType = cog.GetType();

		var fields = cogType
		   .GetFieldsOfType<ISyncer>();

		foreach (var field in fields) {
			var syncer = field.GetFieldValue<ISyncer>(cog);
			var fact = MakeSyncerFact(field, cogType);
			syncer.__ValidateFromCog(entity, fact);
		}
	}


	/// will create VerifyFwd/ReactFwd (unless already initialized)
	public static void CheckSyncerInfoInit<TVal>(SyncerInfo info)
	{
		if (info.IsInitialized) return;

		if (info.Fact.NeedsVerify) {
			info.VerifyFwd = new VerifyForwarder<TVal>();
		}

		info.ReactFwd = new ReactForwarder<TVal>();
		info.IsInitialized = true;
	}
}
}