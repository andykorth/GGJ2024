using System;
using Regent.Core;
using Regent.Staging;
using Regent.Workers;
using Swoonity.CSharp;
using Swoonity.MHasher;

namespace Regent.StageFacts
{
[Serializable]
public class StageFact : IMHashable
{
	public string Name;
	public UnityCycle Cycle;
	public MHash HashId;
	public MHash GetHash() => HashId;

	public override string ToString() => $"{Cycle.ToString().ToCapitalize()}.{Name}";
}

[Serializable]
public class StageState : IMHashable
{
	public string Name;
	public StageFact Fact;
	public MHash GetHash() => Fact.HashId;

	public int Count = 0;
	public StageDebugRef DebugRef;

	public WorkerLup NativeWorkers = new();
	public WorkerLup ServerWorkers = new();
	public WorkerLup ClientWorkers = new();
	public WorkerLup AuthorWorkers = new();
	public WorkerLup RemoteWorkers = new();

	public void AddWorker(BaseWorker worker)
	{
		GetLupByDomain(worker.Fact.Domain).Set(worker);
		++Count;
	}

	public void CutWorker(BaseWorker worker)
	{
		GetLupByDomain(worker.Fact.Domain).Cut(worker);
		--Count;
	}

	public WorkerLup GetLupByDomain(RegentDomain domain)
		=> domain switch {
			RegentDomain.NATIVE => NativeWorkers,
			RegentDomain.SERVER => ServerWorkers,
			RegentDomain.CLIENT => ClientWorkers,
			RegentDomain.AUTHOR => AuthorWorkers,
			RegentDomain.REMOTE => RemoteWorkers,
			_ => throw new ArgumentOutOfRangeException(nameof(domain), domain, null)
		};


	public void Workers_TryExecuteAll(bool isServer, bool isClient)
	{
		NativeWorkers.Workers_TryExecuteAll();
		if (isServer) ServerWorkers.Workers_TryExecuteAll();
		if (isClient) ClientWorkers.Workers_TryExecuteAll();
		AuthorWorkers.Workers_TryExecuteAll();
		RemoteWorkers.Workers_TryExecuteAll();
	}

	public override string ToString() => $"[{Fact} {Count}]";
}

/*
	Stage Fact makers are in StageCreation
*/
}