using System.Collections.Generic;
using Idealist;
using UnityEngine;

public static class RoomLogic
{
	private const string STR_COLOR = "{color}";
	
	static List<InteractableObject> _availableObjs = new();

	public static RoomState GenerateGhostDesire(RoomManager room)
	{
		var fig = room.Fig;

		_availableObjs.Clear();
		_availableObjs.AddRange(room.AllInteractables);

		var state = new RoomState();

		for (var i = 0; i < fig.GhostOptions; i++)
		{
			var crit = new ObjCriteria();

			crit.Obj = _availableObjs.GrabRandom();
			crit.IsWanted = Random.value <= fig.WantedChance;
			var (option, stateId) = crit.Obj.Options.GetRandomAndIndex();
			crit.TargetStateId = stateId;
			var hint = crit.IsWanted ? option.Wants.GetRandom() : option.Hates.GetRandom();
			hint = hint.Replace(STR_COLOR, crit.Obj.interactableColor.ToString().ToLower());
			crit.Hint = hint;

			state.Criteria.Add(crit);
		}

		return state;
	}


	public static RoomState GenerateExitDesire(RoomManager room)
	{
		var fig = room.Fig;

		_availableObjs.Clear();
		_availableObjs.AddRange(room.AllInteractables);

		var state = new RoomState();

		for (var i = 0; i < fig.ExitOptions; i++)
		{
			var crit = new ObjCriteria();

			crit.Obj = _availableObjs.GrabRandom();
			crit.IsWanted = Random.value <= fig.WantedChance;
			var (option, stateId) = crit.Obj.Options.GetRandomAndIndex();
			crit.TargetStateId = stateId;
			var hint = crit.IsWanted ? option.ExitWants.GetRandom() : option.ExitHates.GetRandom();
			hint = hint.Replace(STR_COLOR, crit.Obj.interactableColor.ToString().ToLower());
			crit.Hint = hint;

			state.Criteria.Add(crit);
		}

		return state;
	}
}