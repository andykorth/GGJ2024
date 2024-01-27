using System.Collections.Generic;
using FutzSys;
using Regent.Core;
using Regent.Syncers;
using Swoonity.Unity;
using UnityEngine;
using Sz = System.SerializableAttribute;

namespace ActBewilder
{
public class BewilderActivity : Activity<BewilderActor>
{
	[Header("Config")]
	public BewilderFig Fig;
	public BewilderCard Fab_Card;

	[Header("Working State")]
	public List<string> WordBag = new();
	public List<string> CurrentWords = new();
	public string CurrentWordsString;
	public List<BewilderCard> CardBag = new();
	public List<BewilderActor> PendingActorTurn = new();
	public BewilderActor ActiveActorTurn;

	[Header("Observable State")]
	public Track<PhaseEnum> Phase = new();
	public Track<string> PhaseTitle = new();
	public Track<string> PhaseDesc = new();
	public TrackEvt ForceNextRound = new();
	public Track<int> GuessingActorSlotId = new();
	public Track<string> GuessingClue = new();

	public Registry<BewilderCard> Cards => BewilderBaron.Cards_; // TEMP
	public override Registry<BewilderActor> GetRegistry() => BewilderBaron.Actors_; // TEMP

	//## PACKETS	
	public Pk<Pk_Waiting> PkWaiting = new(); //## outgoing
	public Pk<Pk_BewilderRoundStart> PkRoundStart = new(); //## outgoing
	public Pk<Pk_BewilderClue> PkClue = new();
	public Pk<Pk_BewilderGuessing> PkGuessing = new(); //## outgoing
	public Pk<Pk_BewilderGuess> PkGuess = new();

	[TextArea(20, 20)]
	[Btn(nameof(SendTestAi))]
	public string PromptStart;
	
	public enum PhaseEnum
	{
		UNINITIALIZED,
		WAITING_TO_START,
		ROUND_INTRO,
		WRITING_CLUES,
		GUESSING,
		ROUND_SUMMARY,
		GAME_COMPLETE,
	}

	public void SendTestAi()
	{
		OpenAiBaron.Send(PromptStart + CurrentWordsString); // TEMP HACK
	}
}

[Sz] public struct Pk_BewilderRoundStart : IPk
{
	public int Min;
	public int Max;
	public string Words; // CSV
	public string Lock; // CSV
}

[Sz] public struct Pk_BewilderClue : IPk
{
	public string Clue;
	public string Chosen; // CSV
}

[Sz] public struct Pk_BewilderGuessing : IPk
{
	public int Number;
	public int ActiveSlotId; // actor turn
	public string Clue;
	public string Words; // CSV
}

[Sz] public struct Pk_BewilderGuess : IPk
{
	public string Guess; // CSV of card IDs
}
}