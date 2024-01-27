using System;
using System.Collections.Generic;
using Regent.WorkerFacts;
using Regent.Workers;
using Swoonity.Collections;
using Swoonity.CSharp;
using Swoonity.MHasher;

namespace Regent.BaronFacts
{
[Serializable]
public class BaronFact : IMHashable
{
	public string Name;
	public string TypeName;
	public MHash HashId;
	public MHash GetHash() => HashId;

	public WorkerFact[] WorkerFacts;

	public override string ToString() => $"{Name} {HashId}";
}

public static class BaronFactMakers
{
	public static List<BaronFact> MakeBaronFacts()
		=> AppDomain.CurrentDomain
		   .SubtypesOf<Barons.Baron>()
		   .Map(MakeBaronFact)
		   .ToList();

	public static BaronFact MakeBaronFact(Type baronType)
		=> new() {
			Name = baronType.Name,
			TypeName = baronType.AssemblyQualifiedName,
			HashId = MHash.Hash(baronType),
			WorkerFacts = baronType
			   .GetMembersWithAttribute<BaseWorkerAttribute>()
			   .MapDown(WorkerFactMakers.MakeWorkerFact)
		};
}
}