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
	
	public List<GhostCard> AssignedCards = new();
	public Track<string> Clue = new();
	public List<int> ClueCardIds = new();
	public List<int> GuessedCardIds = new();
	public int PendingScore;

	public enum StatusEnum
	{
		UNSET,
		OBSERVING, // I guess?
		READY,
		WRITING_CLUE,
		SUBMITTED_CLUE,
		GUESSING,
		SUBMITTED_GUESS,
		TODO,
	}
}
}