using System.Collections.Generic;
using FutzSys;
using Regent.Syncers;
using Swoonity.Unity;
using UnityEngine.Serialization;

namespace futz.ActGhost
{
public class GhostActor : ActorBase
{
	public Track<StatusEnum> Status = new();

	public TrackList<Hint> AssignedHints = new();

	[Btn(nameof(AddTestHint))]
	public string TestHintAdder;
	public void AddTestHint() => AssignedHints.Add(new Hint { Message = TestHintAdder});

	public enum StatusEnum
	{
		UNSET,
		READY,
	}
}
}