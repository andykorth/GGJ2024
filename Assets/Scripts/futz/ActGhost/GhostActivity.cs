using System.Collections.Generic;
using FutzSys;
using Regent.Syncers;
using Swoonity.Unity;
using UnityEngine;
using Sz = System.SerializableAttribute;

namespace futz.ActGhost
{
public class GhostActivity : Activity<GhostActor>
{
	[Header("Config")]
	public GhostFig Fig;

	[Header("Observable State")]
	public float TimeLeftSec;
	public Track<PhaseEnum> Phase = new();
	public Track<string> PhaseTitle = new();
	public Track<string> PhaseDesc = new();

	public Track<string> TimerString = new();
	// public TrackEvt ForceNextRound = new();

	//## PACKETS	
	public Pk<Pk_Waiting> PkWaiting = new(); //## outgoing
	public Pk<Pk_GhostHints> PkHints = new();
	// public Pk<Pk_BewilderRoundStart> PkRoundStart = new(); //## outgoing
	// public Pk<Pk_BewilderClue> PkClue = new();
	// public Pk<Pk_BewilderGuessing> PkGuessing = new(); //## outgoing
	// public Pk<Pk_BewilderGuess> PkGuess = new();
	
	
	public enum PhaseEnum
	{
		UNINITIALIZED,
		WAITING_TO_START,
		ROUND_INTRO,
		PLAYING_ROOM,
		ROOM_SUMMARY,
		GAME_COMPLETE,
	}
}

[Sz] public struct Pk_GhostHints : IPk
{
	public string Hints; // delimiter: |
}

// [Sz] public struct Pk_BewilderRoundStart : IPk
// {
// 	public int Min;
// 	public int Max;
// 	public string Words; // CSV
// 	public string Lock; // CSV
// }
//
// [Sz] public struct Pk_BewilderClue : IPk
// {
// 	public string Clue;
// 	public string Chosen; // CSV
// }
//
// [Sz] public struct Pk_BewilderGuessing : IPk
// {
// 	public int Number;
// 	public int ActiveSlotId; // actor turn
// 	public string Clue;
// 	public string Words; // CSV
// }
//
// [Sz] public struct Pk_BewilderGuess : IPk
// {
// 	public string Guess; // CSV of card IDs
// }
}