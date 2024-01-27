using System;
using Regent.StageFacts;
using Regent.Staging;
using UnityEngine;

namespace Regent.Core
{
public class StageDebugRef : MonoBehaviour
{
	public StageState State;

	public Transform NativeWorkerRefs;
	public Transform ServerWorkerRefs;
	public Transform ClientWorkerRefs;
	public Transform AuthorWorkerRefs;
	public Transform RemoteWorkerRefs;


	public Transform GetTransformByDomain(RegentDomain domain)
		=> domain switch {
			RegentDomain.NATIVE => NativeWorkerRefs,
			RegentDomain.SERVER => ServerWorkerRefs,
			RegentDomain.CLIENT => ClientWorkerRefs,
			RegentDomain.AUTHOR => AuthorWorkerRefs,
			RegentDomain.REMOTE => RemoteWorkerRefs,
			_ => throw new ArgumentOutOfRangeException(nameof(domain), domain, null)
		};
}
}