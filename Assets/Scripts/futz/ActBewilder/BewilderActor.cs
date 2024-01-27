using System.Collections.Generic;
using FutzSys;
using Regent.Syncers;

namespace ActBewilder
{
public class BewilderActor : ActorBase
{
	public Track<StatusEnum> Status = new();
	public List<BewilderCard> AssignedCards = new();
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