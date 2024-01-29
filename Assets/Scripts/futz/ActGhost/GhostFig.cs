using System.Collections.Generic;
using UnityEngine;

namespace futz.ActGhost
{
[CreateAssetMenu(menuName = "Act Figs/" + nameof(GhostFig))]
public class GhostFig : ScriptableObject
{

	[Header("Config")]
	public int NumHints = 5;
	public int MinActorCount = 2;
	public float RoomTimeSec = 120f;
	public float TimerSlowThreshold = 5;
	public float TimerSlowMulti = .5f;

	public List<string> GhostNames = new();

	[Header("Room Gen")]
	public int NumOfWallObjs = 2;
	public int NumOfFloorObjs = 6;
	public float WantedChance = .7f;
	public int NumOfGhosts = 3;
	public int GhostOptions = 3;
	public int ExitOptions = 3;
	public List<GameObject> WallObjPrefabs = new();
	public List<GameObject> FloorObjPrefabs = new();
	public List<GameObject> GhostPrefabs = new();
	
	// TEMP
	[Header("Temp Strings")]
	public List<string> TestClues = new();
	
	[Header("Temp Strings")]
	public string StrLoading = "Loading";
	public string StrWaitingOthers = "Waiting for others";
	public string StrPhaseTitle_WaitingToStart = "Need 2 or more players";
	public string StrPhaseDesc_WaitingToStart = "";
	public string StrPhaseTitle_RoundIntro = "TODO: Round Intro";
	public string StrPhaseDesc_RoundIntro = "";
	public string StrPhaseTitle_PlayingRoom = "TODO: Playing Room";
	public string StrPhaseDesc_PlayingRoom = "";
	public string StrPhaseTitle_RoundSummary = "TODO: Room Summary";
	public string StrPhaseDesc_RoundSummary = "";
	public string StrPhaseTitle_GameComplete = "TODO: Game Complete";
	public string StrPhaseDesc_GameComplete = "";
}
}