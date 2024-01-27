using System;
using System.Collections.Generic;
using UnityEngine;

namespace ActBewilder
{
[CreateAssetMenu(menuName = "Act Figs/" + nameof(BewilderFig))]
public class BewilderFig : ScriptableObject
{
	[Header("Config")]
	public int NumOfCards = 9;
	public int ChoiceMin = 2;
	public int ChoiceMax = 2;
	public int NumOfLocked = 1;
	public int MinActorCount = 2;
	public List<string> AllPossibleWords => TestWords.Words; // TEMP

	[Header("Animation")]
	public int RevealStartMs = 500;
	public int RevealDelayMs = 50;
	public int GuessStartMs = 1000;
	public int GuessIntervalMs = 400;
	public int GuessEndMs = 2000;
	public int ScoreIntervalMs = 300;
	public int ScoreEndMs = 5000;

	[Header("Scoring")]
	public BewilderScoring Scoring;

	// TEMP
	[Header("Temp Strings")]
	public string StrLoading = "Loading";
	public string StrWaitingOthers = "Waiting for others";
	public string StrOthersGuessing = "Others are guessing your words";
	public string StrPhaseTitle_WaitingToStart = "Need 2 or more players";
	public string StrPhaseDesc_WaitingToStart;
	public string StrPhaseTitle_RoundIntro = "TODO: Round Intro";
	public string StrPhaseDesc_RoundIntro;
	public string StrPhaseTitle_WritingClues = "Write Clues!";
	public string StrPhaseDesc_WritingClues = "Write a clue to get others to guess your words.";
	public string StrPhaseTitle_Guessing = "Guess Words!";
	public string StrPhaseDesc_Guessing = "Pick words based on the clue.";
	public string StrPhaseTitle_RoundSummary = "TODO: Round Summary";
	public string StrPhaseDesc_RoundSummary;
	public string StrPhaseTitle_GameComplete = "TODO: Game Complete";
	public string StrPhaseDesc_GameComplete;
}

[Serializable]
public class BewilderScoring
{
	[Header("Others guess your clue")]
	public int ClueHit = 10;
	public int CluePerfectPerPlayer = 5;
	public int CluePerfectAll = 10;

	[Header("You guess others")]
	public int GuessHit = 10;
	public int GuessHitPerfect = 10;
}
}