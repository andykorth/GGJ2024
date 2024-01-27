using Regent.WorkerFacts;
using UnityEngine;

namespace Regent.Workers
{
public class WorkerRef : MonoBehaviour
{
	public override string ToString() => $"worker[{name}]";

	[Header("Specs")]
	public WorkerFact Fact;

	[Header("State")]
	public BaseWorker Worker;
}
}