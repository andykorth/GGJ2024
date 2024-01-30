using System.Collections.Generic;
using UnityEngine;
using Foundational;
using futz.ActGhost;
using Cysharp.Threading.Tasks;
using Idealist;
using Lumberjack;
using Swoonity.CSharp;
using UnityEngine.Serialization;

public class GhostPlayerManager : Singleton<GhostPlayerManager>
{
	[HideInInspector] public bool readyToBegin = false;
	public GameObject pressPlayToBeginMsg;
	public GameObject needTwoPhonePlayersMsg;
	public GameObject exitGoalMsg;

	public Transform interactMarker;
	public TMPro.TMP_Text interactText;

	public TMPro.TMP_Text ghostText;
	public TMPro.TMP_Text goalDebugText;
	public TMPro.TMP_Text exitGoalText;
	public GameObject InstructionsObj;
	public Transform GhostFlyTarget;

	public List<DEPRECATED_GhostPlayer> ghostPlayers;

	public List<DEPRECATED_Goal> allSharedGoals;
	public List<DEPRECATED_Goal> assignedGoals;
	public List<DEPRECATED_Goal> possibleExitGoals;

	private DEPRECATED_Goal selectedExitGoal1, selectedExitGoal2;

	public string[] spiritNames;

	[Header("Room State stuff")] public RoomManager RoomManager;

	public const int SPIRIT_COUNT = 3;

	// public void CreateAllGoals()
	// {
	// 	allSharedGoals = new List<DEPRECATED_Goal>
	// 	{
	// 		new DEPRECATED_Goal
	// 		{
	// 			goalString = "I hate the smell of burning cauldrons.",
	// 			goalAction = () => MatchesInteraction(InterType.Cauldron) == 0,
	// 			spiritIndex = 0,
	// 		},
	// 		new DEPRECATED_Goal
	// 		{
	// 			goalString = "But I can't stand the dark.",
	// 			goalAction = () => MatchesInteraction(InterType.Candle) >= 1,
	// 			spiritIndex = 0,
	// 		},
	// 		new DEPRECATED_Goal
	// 		{
	// 			goalString = "Ooooh that vase reminds me of limes!",
	// 			goalAction = () => MatchesInteraction(InterType.Vase, InterColor.Green) >= 1,
	// 			spiritIndex = 0,
	// 		},
	//
	//
	// 		new DEPRECATED_Goal
	// 		{
	// 			goalString = "I always loved pottery.",
	// 			goalAction = () =>
	// 				MatchesInteraction(InterType.Vase, InterColor.Green) >= 1
	// 				&& MatchesInteraction(InterType.Vase, InterColor.Purple) >= 1,
	// 			spiritIndex = 1,
	// 		},
	// 		new DEPRECATED_Goal
	// 		{
	// 			goalString = "That blue vase is an embarrassment.",
	// 			goalAction = () =>
	// 				MatchesInteraction(InterType.Vase,
	// 					InterColor.Blue) == 0,
	// 			spiritIndex = 1,
	// 		},
	// 		new DEPRECATED_Goal
	// 		{
	// 			goalString = "I do love blue though!",
	// 			goalAction = () => MatchesInteraction(InterType.Cauldron, InterColor.Blue) >= 1,
	// 			spiritIndex = 1,
	// 		},
	//
	//
	// 		new DEPRECATED_Goal
	// 		{
	// 			goalString = "Ewww, purple!",
	// 			goalAction = () => MatchesInteraction(InterType.Cauldron, InterColor.Purple) <= 0,
	// 			spiritIndex = 2,
	// 		},
	// 		new DEPRECATED_Goal
	// 		{
	// 			goalString = "I didn't know there was a painting of me.",
	// 			goalAction = () => MatchesInteraction(InterType.Portrait, InterColor.Blue) > 0,
	// 			spiritIndex = 2,
	// 		},
	// 		new DEPRECATED_Goal
	// 		{
	// 			goalString = "These vases are all hideous!",
	// 			goalAction = () => MatchesInteraction(InterType.Vase) <= 0,
	// 			spiritIndex = 2,
	// 		},
	// 	};
	//
	// 	possibleExitGoals = new List<DEPRECATED_Goal>
	// 	{
	// 		new DEPRECATED_Goal
	// 		{
	// 			goalString = $"Only {spiritNames[0]} can open the exit!",
	// 			goalAction = () => spiritGoalComplete[0] == spiritGoalCount[0]
	// 		},
	// 		new DEPRECATED_Goal
	// 		{
	// 			goalString = $"We need one disordered painting to leave.",
	// 			goalAction = () => MatchesInteraction(InterType.Portrait, InterColor.Blue) < 2
	// 		},
	// 		new DEPRECATED_Goal
	// 		{
	// 			goalString = $"You cannot leave without {spiritNames[1]}!",
	// 			goalAction = () => spiritGoalComplete[1] == spiritGoalCount[1]
	// 		},
	// 		new DEPRECATED_Goal
	// 		{
	// 			goalString = $"The candle on the table will light the way",
	// 			goalAction = () => MatchesInteraction(InterType.Candle, InterColor.Blue) >= 1
	// 		},
	// 		new DEPRECATED_Goal
	// 		{
	// 			goalString = $"Do not unleash {spiritNames[2]}!",
	// 			goalAction = () => spiritGoalComplete[2] != spiritGoalCount[2]
	// 		},
	// 		new DEPRECATED_Goal
	// 		{
	// 			goalString = $"Believe in yourself to open the exit!",
	// 			goalAction = () => true
	// 		},
	// 	};
	// }
	//
	// internal int MatchesInteraction(InterType interactionType, InterColor color = InterColor.Any)
	// {
	// 	int count = 0;
	// 	foreach (var v in InteractableObject.allInteractables)
	// 	{
	// 		bool matchType = v.interactableType == interactionType;
	// 		bool matchColor = color == InterColor.Any
	// 		                  || v.interactableColor == color;
	// 		if (matchType && matchColor && v.hasBeenEnabledByPlayer)
	// 		{
	// 			count += 1;
	// 		}
	// 	}
	//
	// 	return count;
	// }

	public void Start()
	{
		ghostPlayers = new List<DEPRECATED_GhostPlayer>();
		// CreateAllGoals();

		pressPlayToBeginMsg.SetActive(false);
		needTwoPhonePlayersMsg.SetActive(true);
		exitGoalMsg.SetActive(false);
		readyToBegin = false;
		Player.i.gameObject.SetActive(false);
	}

	private float timeTilUpdate = 1.0f;

	public void Update()
	{
		var act = GameSysClip.I.GhostAct.Current;
		var player = Player.i;

		if (Input.GetKeyDown(KeyCode.C))
		{
			readyToBegin = false;
			Player.i.transform.position = Vector3.zero;
			needTwoPhonePlayersMsg.SetActive(false);
			pressPlayToBeginMsg.SetActive(false);
			GameStarted();
		}

		if (Input.GetKeyDown(KeyCode.F1))
		{
			act.ShowDebug.Change(!act.ShowDebug.Current);
			goalDebugText.enabled = !goalDebugText.enabled;
		}

		if (Input.GetKeyDown(KeyCode.F7))
		{
			act.TimeLeftSec -= 10;
		}
		else if (Input.GetKeyDown(KeyCode.F8))
		{
			act.TimeLeftSec += 60;
		}

		if (readyToBegin)
		{
			needTwoPhonePlayersMsg.SetActive(false);
			pressPlayToBeginMsg.SetActive(true);
			if (Input.GetKeyDown(KeyCode.Space))
			{
				GameStarted();
			}
		}

		timeTilUpdate -= Time.deltaTime;
		if (timeTilUpdate < 0f)
		{
			AssessRoomStates();
			// EvaluateGoals();

			timeTilUpdate = 1.0f;
		}


		string s = $"Ghosts: {ghostPlayers.Count}\n";
		foreach (var ghost in ghostPlayers)
		{
			s += "   " + ghost.ToString() + "\n";
		}

		ghostText.text = s;


		var touchingObj = player.mostRecentTouch;
		if (touchingObj)
		{
			// interactMarker.position = touchingObj.transform.position + touchingObj.markerOffset;
			interactMarker.position = touchingObj.mainSprite.bounds.center + touchingObj.TextOffset;
			interactText.text = touchingObj.Options[touchingObj.CurrentStateId].VerbToNext.Or("poke");
		}
		else
		{
			interactMarker.position = Vector3.right * 99999;
		}
	}

	public string goalDebugString;
	private int[] spiritGoalCount = new int[SPIRIT_COUNT];
	private int[] spiritGoalComplete = new int[SPIRIT_COUNT];

	public void AssessRoomStates()
	{
		var act = GameSysClip.I.GhostAct.Current;
		RoomLogic.UpdateRoomState(RoomManager.StateToExit);
		// $"check: {RoomManager.StateToExit.PrevCheck}".LgOrange0();


		foreach (var ghost in RoomManager.Ghosts)
		{
			if (ghost.IsRescued) continue;

			RoomLogic.UpdateRoomState(ghost.DesiredRoomState);

			var percent = ghost.DesiredRoomState.Percent;
			ghost.SpriteRenderer.color = ghost.SpriteRenderer.color.WithAlpha(percent);

			if (!ghost.DesiredRoomState.IsMet) continue;

			$"RESCUED: {ghost.Name}".LgOrange0();
			ghost.IsRescued = true;
			RoomManager.UnblockDoor = true;
			act.GhostsRescued.Change(act.GhostsRescued.Current + 1);
			ghost.transform.localScale = Vector3.one * 2; // TODO

			var startPos = ghost.transform.position;
			var endPos = GhostFlyTarget.position;

			this.AddTween(2f,
				a => ghost.transform.position = Vector3.Lerp(startPos, endPos, a)
			);
		}

		var door = RoomManager.Door;

		RoomLogic.UpdateRoomState(RoomManager.StateToExit);
		door.SetDoor(RoomManager.StateToExit.IsMet, !RoomManager.UnblockDoor);

		act.DebugString.Change(RoomManager.AllCriteria.Join(
			c => c.PrevCheck ? $"YES {c.Hint}" : $"NO {c.Hint}",
			"\n")
		);
	}

	// public void EvaluateGoals()
	// {
	// 	if (assignedGoals == null) return;
	//
	// 	goalDebugString = "Debugging String:\n";
	//
	// 	foreach (DEPRECATED_Goal goal in assignedGoals)
	// 	{
	// 		spiritGoalCount[goal.spiritIndex] += 1;
	// 		bool b = goal.Evaluate();
	// 		goalDebugString += (b ? "<color=green>YES" : "<color=red>NO") + $"</color> spirit {goal.spiritIndex}: {goal.goalString}\n";
	// 		if (b)
	// 		{
	// 			spiritGoalComplete[goal.spiritIndex] += 1;
	// 		}
	// 	}
	//
	// 	// update spirits:
	// 	for (int i = 0; i < SPIRIT_COUNT; i++)
	// 	{
	// 		Color c = spiritsToFade[i].color;
	// 		float alpha = spiritGoalComplete[i] / (float)spiritGoalCount[i];
	// 		spiritsToFade[i].color = c.WithAlpha(alpha);
	// 		// Debug.Log($"At {Time.time} spirit {i} = {alpha}");
	// 	}
	//
	// 	if (!exitGoalMsg.activeSelf && GameSysClip.I.GhostAct.Current.TimeLeftSec < 100)
	// 	{
	// 		exitGoalText.text = selectedExitGoal1.goalString + "\n" + selectedExitGoal2.goalString;
	// 		exitGoalMsg.SetActive(true);
	// 	}
	//
	// 	if (exitGoalMsg.activeSelf)
	// 	{
	// 		if (selectedExitGoal1.Evaluate() && selectedExitGoal2.Evaluate())
	// 		{
	// 			exitDoor.SetDoor(true);
	// 		}
	// 	}
	//
	// 	if (goalDebugText != null) goalDebugText.text = goalDebugString;
	// }


	public void GameStarted()
	{
		InstructionsObj.SetActive(false);

		RoomManager.CreateRoom();

		readyToBegin = false;
		Player.i.gameObject.SetActive(true);
		pressPlayToBeginMsg.SetActive(false);
		GhostLogic.BeginRoom(GameSysClip.I.GhostAct.Current).Forget();

		var actors = GameSysClip.I.GhostAct.Current.Actors.Current;
		var allCriteria = new List<ObjCriteria>(RoomManager.AllCriteria);

		foreach (var phonePlayer in actors)
		{
			phonePlayer.AssignedHints.Clear();
		}

		if (actors.Count == 0) return;

		var playerIndex = 0;
		while (allCriteria.Count > 0)
		{
			var crit = allCriteria.GrabRandom();
			actors[playerIndex].AssignedHints.Add(crit);

			playerIndex++;
			if (playerIndex >= actors.Count) playerIndex = 0;
		}

		AssessRoomStates();
	}
}

public class DEPRECATED_GhostPlayer
{
	public string name;
	public int goalsComplete = 0;
	public List<DEPRECATED_Goal> activeGoals = new List<DEPRECATED_Goal>();
	internal GhostActor actor;

	public override string ToString() => $"{name} score: {goalsComplete}";
}

public class DEPRECATED_Goal
{
	public string goalString;
	public System.Func<bool> goalAction;
	public bool hasBeenFinished = false;
	public int spiritIndex;


	public bool Evaluate()
	{
		bool b = goalAction();
		// Debug.Log("Goal " + goalString + " = " + b);

		if (b)
		{
			if (!hasBeenFinished)
			{
				// first time finishing this goal!
				Debug.Log($"Finished goal for spirit {spiritIndex}: {goalString}");
			}

			hasBeenFinished = true;
		}

		return b;
	}
}