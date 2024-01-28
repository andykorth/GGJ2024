using System;
using System.Collections.Generic;
using Foundational;
using futz.ActGhost;
using Idealist;
using UnityEngine;
using UnityEngine.Serialization;

public class RoomManager : MonoBehaviour
{
	[Header("Config")] public GhostFig Fig;

	[Header("State")] public RoomState StateToExit;
	public List<RoomState> Ghosts = new();

	public List<ObjCriteria> AllCriteria = new();

	public List<InteractableObject> AllInteractables = new();

	public void CreateRoom()
	{
		var act = GameSysClip.I.GhostAct.Current;
		var fig = Fig;

		// TODO: gen interactables
		// AllInteractables = InteractableObject.allInteractables;


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