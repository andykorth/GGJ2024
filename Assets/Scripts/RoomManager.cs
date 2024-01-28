using System;
using System.Collections.Generic;
using Foundational;
using futz.ActGhost;
using Idealist;
using Swoonity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class RoomManager : MonoBehaviour
{
	[Header("Config")] 
	public GhostFig Fig;
	public ExitDoor Door;
	public List<Transform> PossibleGhostSpawns = new();
	public List<Transform> PossibleWallSpawns = new();
	public List<Transform> PossibleFloorSpawns = new();

	[Header("State")]
	public bool UnblockDoor;
	public RoomState StateToExit;
	public List<Ghost> Ghosts = new();

	public List<ObjCriteria> AllCriteria = new();

	public List<InteractableObject> RoomInteractables = new();
	public Transform InteractablesRoot;
	public Transform GhostsRoot;

	public void CreateRoom()
	{
		var act = GameSysClip.I.GhostAct.Current;
		var fig = Fig;

		UnblockDoor = false;

		RoomInteractables.Clear();
		if (InteractablesRoot) Destroy(InteractablesRoot.gameObject);
		InteractablesRoot = new GameObject("_INTERACTABLES").transform;

		var availableWallObjs = new List<GameObject>(fig.WallObjPrefabs);
		var availableFloorObjs = new List<GameObject>(fig.FloorObjPrefabs);
		var availableWallSpawns = new List<Transform>(PossibleWallSpawns);
		var availableFloorSpawns = new List<Transform>(PossibleFloorSpawns);

		for (var i = 0; i < fig.NumOfWallObjs; i++)
		{
			var interactable = Instantiate(
				availableWallObjs.GrabRandom(),
				InteractablesRoot
			).GetComponent<InteractableObject>();
			interactable.transform.position = availableWallSpawns.GrabRandom().position;
			RoomInteractables.Add(interactable);
		}

		for (var i = 0; i < fig.NumOfFloorObjs; i++)
		{
			var interactable = Instantiate(
				availableFloorObjs.GrabRandom(),
				InteractablesRoot
			).GetComponent<InteractableObject>();
			interactable.transform.position = availableFloorSpawns.GrabRandom().position;
			RoomInteractables.Add(interactable);
		}

		
		
		Ghosts.Clear();
		if (GhostsRoot) Destroy(GhostsRoot.gameObject);
		GhostsRoot = new GameObject("_GHOSTS").transform;
		
		var availableGhostPrefabs = new List<GameObject>(fig.GhostPrefabs);
		var availableGhostSpawns = new List<Transform>(PossibleGhostSpawns);

		AllCriteria.Clear();

		StateToExit = RoomLogic.GenerateExitDesire(this);
		AllCriteria.AddRange(StateToExit.Criteria);

		for (var i = 0; i < fig.NumOfGhosts; i++)
		{
			var ghost = Instantiate(
				availableGhostPrefabs.GrabRandom(),
				GhostsRoot
			).GetComponent<Ghost>();
			ghost.transform.position = availableGhostSpawns.GrabRandom().position;
			
			ghost.Name = $"Ghost {i}";
			ghost.DesiredRoomState = RoomLogic.GenerateGhostDesire(this);
			AllCriteria.AddRange(ghost.DesiredRoomState.Criteria);
			
			Ghosts.Add(ghost);
		}

		act.DebugString.Change(Stringification.Join(AllCriteria, c => c.Hint, "\n"));
	}
}


[Serializable]
public class RoomState
{
	public List<ObjCriteria> Criteria = new();
	public bool IsMet;
	public float Percent;

	// public bool PrevCheck; // debugging
	// public bool Assess() => PrevCheck = Criteria.All(c => c.Assess());
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