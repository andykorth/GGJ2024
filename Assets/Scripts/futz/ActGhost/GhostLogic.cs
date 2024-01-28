using System;
using System.Diagnostics;
using System.Threading;
using Cysharp.Threading.Tasks;
using Foundational;
using FutzSys;
using Idealist;
using Lumberjack;
using static UnityEngine.Debug;
using Phase = futz.ActGhost.GhostActivity.PhaseEnum;
using Status = futz.ActGhost.GhostActor.StatusEnum;

namespace futz.ActGhost
{
	public static class GhostLogic
	{
		static CancellationTokenSource _cancelSource;

		// ReSharper disable once InconsistentNaming
		/// wraps Delay and adds cancellationToken
		static UniTask __WAIT__(int ms) => UniTask.Delay(ms, cancellationToken: _cancelSource.Token);


		public static void Initialize(GhostActivity act)
		{
			var fig = act.Fig;

			$"Initialize GHOST activity".LgOrange0();

			NewGame(act);
		}

		public static void CleanUp(GhostActivity act)
		{
			Log($"".LgTodo());
			_cancelSource.CancelDispose();
			_cancelSource = null;
		}

		public static void ActorAdded(GhostActivity act, GhostActor actor)
		{
			var fig = act.Fig;
			actor.Status.Change(Status.READY);
			actor.Agent.Status.Change("waiting");
			act.PkWaiting.SendTo(actor, new Pk_Waiting { Msg = fig.StrWaitingOthers });

			switch (act.Phase.Current)
			{
				case Phase.UNINITIALIZED: return;
				case Phase.WAITING_TO_START:
				Log("PHASE WIASTING TOP START");
					GhostPlayerManager.i.readyToBegin = act.Actors.Count >= fig.MinActorCount;
					return;
				case Phase.ROUND_INTRO: return;
				case Phase.PLAYING_ROOM:
					AssignHints(act); // TEMP
					return;
				case Phase.ROOM_SUMMARY: return;
				case Phase.GAME_COMPLETE: return;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		public static void ActorRemoved(GhostActivity act, GhostActor actor)
		{
			Log($"removed {actor}".LgTodo());
			var fig = act.Fig;

			if (act.Actors.Count < fig.MinActorCount)
			{
				NewGame(act);
				return; //>> below min players, restart
			}

			switch (act.Phase.Current)
			{
				case Phase.UNINITIALIZED: return;
				case Phase.WAITING_TO_START: return;
				case Phase.ROUND_INTRO: return;
				case Phase.PLAYING_ROOM: return;
				case Phase.ROOM_SUMMARY: return;
				case Phase.GAME_COMPLETE: return;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		public static void ChangePhase(GhostActivity act, Phase phase)
		{
			Log($"<><><><><><> phase: {phase}".LgOrange(), act);
			var fig = act.Fig;

			var (title, desc) = phase switch
			{
				Phase.UNINITIALIZED => ("", ""),
				Phase.WAITING_TO_START => (fig.StrPhaseTitle_WaitingToStart, fig.StrPhaseDesc_WaitingToStart),
				Phase.ROUND_INTRO => (fig.StrPhaseTitle_RoundIntro, fig.StrPhaseDesc_RoundIntro),
				Phase.PLAYING_ROOM => (fig.StrPhaseTitle_PlayingRoom, fig.StrPhaseDesc_PlayingRoom),
				Phase.ROOM_SUMMARY => (fig.StrPhaseTitle_RoundSummary, fig.StrPhaseDesc_RoundSummary),
				Phase.GAME_COMPLETE => (fig.StrPhaseTitle_GameComplete, fig.StrPhaseDesc_GameComplete),
				_ => throw new ArgumentOutOfRangeException(nameof(phase), phase, null)
			};

			act.PhaseTitle.Change(title);
			act.PhaseDesc.Change(desc);
			act.Phase.Change(phase);
		}

		public static void NewGame(GhostActivity act)
		{
			var fig = act.Fig;
			if (act.Phase.Current == Phase.WAITING_TO_START) return; //>> already made new game

			_cancelSource = _cancelSource.Remake();

			ChangePhase(act, Phase.WAITING_TO_START);
			if (act.Actors.Count >= fig.MinActorCount)
			{
				BeginRoom(act).Forget();
			}
		}

		public static async UniTask BeginRoom(GhostActivity act)
		{
			var fig = act.Fig;
			Log($"<><><><><><> {act} Room Begin".LgTodo(), act);
			ChangePhase(act, Phase.ROUND_INTRO);
			SetStatusAll(act, "", Status.READY);
			act.PkWaiting.SendToAllAgents(new Pk_Waiting { Msg = fig.StrLoading });

			// await __WAIT__(fig.RevealStartMs);

			ChangePhase(act, Phase.PLAYING_ROOM);
			act.TimeLeftSec = fig.RoomTimeSec;

			Log($"  NEW ROOM:      <b>TODO</b>".LgOrange(skipPrefix: true));

			AssignHints(act);
		}

		public static void AssignHints(GhostActivity act)
		{
			var fig = act.Fig;

			foreach (var actor in act.Actors.Current)
			{
				actor.AssignedHints.Clear();

				for (var i = 0; i < fig.NumHints; i++)
				{
					actor.AssignedHints.Add(new Hint
					{
						Message = fig.TestClues.GetRandom(),
					});
				}
			}
		}

		public static void DrainPackets(GhostActivity act)
		{
			// act.PkClue
			// 	.OnlyIf(act.Phase.Current == Phase.WRITING_CLUES)
			// 	.DrainAsActor(
			// 		act,
			// 		static (act, actor, pk) =>
			// 		{
			// 			var fig = act.Fig;
			// 			actor.Status.Change(Status.SUBMITTED_CLUE);
			// 			actor.Agent.Status.Change("submitted clue");
			// 			actor.Clue.Change(pk.Clue);
			//
			// 			var clueCardIds = actor.ClueCardIds._Clear();
			// 			actor.AssignedCards.MapInto(clueCardIds, static c => c.CardId);
			// 			pk.Chosen.SplitIndexesInto(clueCardIds);
			//
			// 			act.PkWaiting.SendTo(actor, new Pk_Waiting { Msg = fig.StrWaitingOthers });
			// 			CheckClueCount(act);
			// 		}
			// 	);
		}

		static void SetStatusAll(GhostActivity act, string agentStatus, Status actorStatus = Status.UNSET)
		{
			if (actorStatus != Status.UNSET)
			{
				foreach (var actor in act.Actors.Current)
				{
					actor.Agent.Status.Change(agentStatus);
					actor.Status.Change(actorStatus);
				}
			}
			else
			{
				foreach (var actor in act.Actors.Current)
				{
					actor.Agent.Status.Change(agentStatus);
				}
			}
		}
	}
}