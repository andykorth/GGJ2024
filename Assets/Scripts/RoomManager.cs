using System;
using System.Collections.Generic;
using Foundational;
using futz.ActGhost;
using Idealist;
using UnityEngine;
using UnityEngine.Serialization;

public class RoomManager : MonoBehaviour
{
	[Header("Config")] 
	public GhostFig Fig;
	public List<InteractableObject> PossibleWallObjs = new();
	public List<InteractableObject> PossibleFloorObjs = new();
	public List<Transform> PossibleWallSpawns = new();
	public List<Transform> PossibleFloorSpawns = new();

	[Header("State")] public RoomState StateToExit;
	public List<RoomState> Ghosts = new();

	public List<ObjCriteria> AllCriteria = new();

	[FormerlySerializedAs("AllInteractables")] public List<InteractableObject> RoomInteractables = new();

	public void CreateRoom()
	{
		var act = GameSysClip.I.GhostAct.Current;
		var fig = Fig;

		foreach (var wall in PossibleWallObjs) wall.gameObject.SetActive(false);
		foreach (var floor in PossibleFloorObjs) floor.gameObject.SetActive(false);

		var availableWallObjs = new List<InteractableObject>(PossibleWallObjs);
		var availableFloorObjs = new List<InteractableObject>(PossibleFloorObjs);
		var availableWallSpawns = new List<Transform>(PossibleWallSpawns);
		var availableFloorSpawns = new List<Transform>(PossibleFloorSpawns);

		for (var i = 0; i < fig.NumOfWallObjs; i++)
		{
			var interactable = availableWallObjs.GrabRandom();
			interactable.transform.position = availableWallSpawns.GrabRandom().position;
			interactable.gameObject.SetActive(true);
		}
		
		for (var i = 0; i < fig.NumOfFloorObjs; i++)
		{
			var interactable = availableFloorObjs.GrabRandom();
			interactable.transform.position = availableFloorSpawns.GrabRandom().position;
			interactable.gameObject.SetActive(true);
		}
		

		Ghosts.Clear();
		AllCriteria.Clear();

		StateToExit = RoomLogic.GenerateExitDesire(this);
		StateToExit.Label = "Exit";
		AllCriteria.AddRange(StateToExit.Criteria);

		for (var i = 0; i < fig.NumOfGhosts; i++)
		{
			var ghostState = RoomLogic.GenerateGhostDesire(this);
			ghostState.Label = $"Ghost {i}";
			Ghosts.Add(ghostState);
			AllCriteria.AddRange(ghostState.Criteria);
		}
		
		act.DebugString.Change(AllCriteria.Join(c => c.Hint, "\n"));
	}
}


[Serializable]
public class RoomState
{
	public string Label;
	public List<ObjCriteria> Criteria = new();

	public bool PrevCheck; // debugging
	public bool Assess() => PrevCheck = Criteria.All(c => c.Assess());
}

[Serializable]
public class ObjCriteria
{
	public InteractableObject Obj;
	public StateOption StateOption;
	public bool IsWanted;
	public int TargetStateId;
	public string Hint;

	public bool PrevCheck; // debugging

	public bool Assess() =>
		PrevCheck = IsWanted
			? Obj.CurrentStateId == TargetStateId
			: Obj.CurrentStateId != TargetStateId;
}