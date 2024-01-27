using Regent.WorkerFacts;
using UnityEngine;

namespace Regent.Workers
{
public class WorkerDebugRef : MonoBehaviour
{
	public WorkerFact Fact;
	public WorkerRef WorkerRef;
	public BaseWorker Worker;


	public override string ToString() => $"worker[{name}]";
}
}