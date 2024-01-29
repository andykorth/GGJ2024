using System.Collections.Generic;
using Idealist;
using Swoonity.CSharp;
using uRandom = UnityEngine.Random;

public static class RoomLogic
{
	public static void UpdateRoomState(RoomState room)
	{
		var numMet = 0;

		foreach (var crit in room.Criteria)
		{
			var isMet = crit.Assess();
			if (isMet) numMet++;
		}

		if (numMet >= room.Criteria.Count)
		{
			room.IsMet = true;
			room.Percent = 1f;
		}
		else
		{
			room.IsMet = false;
			room.Percent = numMet / (float)room.Criteria.Count;
		}
	}


	static List<InteractableObject> _availableObjs = new();

	public static RoomState GenerateGhostDesire(RoomManager room, string name)
	{
		var fig = room.Fig;

		_availableObjs.Clear();
		_availableObjs.AddRange(room.RoomInteractables);

		var state = new RoomState();
		state.Name = name;

		for (var i = 0; i < fig.GhostOptions; i++)
		{
			var crit = new ObjCriteria();

			crit.Obj = _availableObjs.GrabRandom();
			crit.IsWanted = uRandom.value <= fig.WantedChance;
			(crit.StateOption, crit.TargetStateId) = crit.Obj.Options.GetRandomAndIndex();
			crit.Hint = MakeHintString(
				crit.Obj,
				crit.StateOption,
				crit.IsWanted
					? crit.StateOption.Wants.GetRandom()
					: crit.StateOption.Hates.GetRandom(),
				$"{name}: "
			);

			state.Criteria.Add(crit);
		}

		return state;
	}


	public static RoomState GenerateExitDesire(RoomManager room)
	{
		var fig = room.Fig;

		_availableObjs.Clear();
		_availableObjs.AddRange(room.RoomInteractables);

		var state = new RoomState();
		state.Name = "Exit";

		for (var i = 0; i < fig.ExitOptions; i++)
		{
			var crit = new ObjCriteria();

			crit.Obj = _availableObjs.GrabRandom();
			crit.IsWanted = uRandom.value <= fig.WantedChance;
			(crit.StateOption, crit.TargetStateId) = crit.Obj.Options.GetRandomAndIndex();
			crit.Hint = MakeHintString(
				crit.Obj,
				crit.StateOption,
				crit.IsWanted
					? crit.StateOption.ExitWants.GetRandom()
					: crit.StateOption.ExitHates.GetRandom()
			);

			state.Criteria.Add(crit);
		}

		return state;
	}

	const string STR_COLOR = "{color}";
	const string STR_OBJ_NAME = "{name}";
	const string STR_OPTION_LABEL = "{label}";

	public static string MakeHintString(
		InteractableObject interactableObject,
		StateOption stateOption,
		string str,
		string prefix = ""
	) =>
		str
			.Prefix(prefix)
			// .Replace(STR_COLOR, interactableObject.interactableColor.ToString().ToLower())
			.Replace(STR_COLOR, interactableObject.ColorText.ToLower())
			.Replace(STR_OBJ_NAME, interactableObject.Name.ToLower())
			.Replace(STR_OPTION_LABEL, stateOption.Label.ToLower());
}