using System;
using System.Reflection;
using Regent.Staging;
using Regent.Workers;
using Swoonity.Collections;
using Swoonity.MHasher;

namespace Regent.WorkerFacts
{
[Serializable]
public class WorkerFact : IMHashable
{
	public string Name; // name of method
	public MHash HashId;
	public MHash GetHash() => HashId;
	public string ParentTypeName;
	public string[] RequiredTypeNames;
	public string[] RequiredTypeNamesShort;
	public MHash[] RequiredTypeHashes;

	public UnityCycle Cycle;
	public string StageName;
	public MHash StageHash;
	public WorkerTrigger WorkerTrigger;
	public WorkerMemberType WorkerMemberType;
	public string MakerString;

	public RegentDomain Domain;
	public bool RequiresAuthor;
	public bool RequiresRemote;
	public bool IsInstant;

	public string SyncerName; // Syncer worker
	public float TimedSeconds; // Timed worker


	public override string ToString() => $"{ParentTypeName}.{Name}({WorkerTrigger})";
}

public static class WorkerFactMakers
{
	/// happens OnValidate (devtime)
	public static WorkerFact MakeWorkerFact(
		MemberInfo info,
		BaseWorkerAttribute attribute
	)
	{
		var fact = new WorkerFact();
		fact.HashId = MHash.Hash(info.DeclaringType.FullName, info.Name);
		fact.Name = info.Name;
		fact.ParentTypeName = info.DeclaringType?.Name;

		var requiredTypes = attribute.GetTypes(info);

		fact.RequiredTypeNames = requiredTypes.Map(static t => t.AssemblyQualifiedName);
		fact.RequiredTypeNamesShort = requiredTypes.Map(static t => t.Name);
		fact.RequiredTypeHashes = requiredTypes.Map(static t => MHash.Hash(t));

		(fact.Cycle, fact.StageName) =
			StageCreation.PlaceholderToCycleName(attribute.GetStagePlaceholder());

		fact.StageHash = MHash.Hash(fact.StageName);
		fact.WorkerTrigger = attribute.GetTrigger();
		fact.WorkerMemberType = attribute.GetMemberType();
		fact.MakerString = fact.WorkerTrigger.GetMakerString(fact.RequiredTypeNames.Length);

		fact.Domain = attribute.GetDomain();
		fact.RequiresAuthor = fact.Domain == RegentDomain.AUTHOR;
		fact.RequiresRemote = fact.Domain == RegentDomain.REMOTE;
		fact.IsInstant = fact.StageName == "INSTANT"; // HACK

		fact.SyncerName = attribute.GetSyncer();
		fact.TimedSeconds = attribute.GetTimedSeconds();

		attribute.ThrowIfMisconfigured(fact, requiredTypes);
		return fact;
	}
}
}