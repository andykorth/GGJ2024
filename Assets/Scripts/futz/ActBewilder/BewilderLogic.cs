using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Foundational;
using FutzSys;
using Idealist;
using Lumberjack;
using Regent.Syncers;
using Swoonity.CSharp;
using static UnityEngine.Debug;
using Phase = ActBewilder.BewilderActivity.PhaseEnum;
using Status = ActBewilder.BewilderActor.StatusEnum;
using Act = ActBewilder.BewilderActivity;
using Actor = ActBewilder.BewilderActor;
using Card = ActBewilder.BewilderCard;

namespace ActBewilder
{
public static class BewilderLogic
{
	static CancellationTokenSource _cancelSource;

	// ReSharper disable once InconsistentNaming
	/// wraps Delay and adds cancellationToken
	static UniTask __WAIT__(int ms) => UniTask.Delay(ms, cancellationToken: _cancelSource.Token);


	public static void Initialize(Act act)
	{
		var fig = act.Fig;

		for (var i = 0; i < fig.NumOfCards; i++) {
			var card = UnityEngine.Object.Instantiate(act.Fab_Card, act.Tform);
			card.CardId = i;
			card.name = $"{i} card";
		}

		act.WordBag.Capacity = fig.AllPossibleWords.Count;

		NewGame(act);
	}

	public static void CleanUp(Act act)
	{
		Log($"".LgTodo());
		_cancelSource.CancelDispose();
		_cancelSource = null;
	}

	public static void ActorAdded(Act act, BewilderActor actor)
	{
		var fig = act.Fig;
		actor.Status.Change(Status.READY);
		actor.Agent.Status.Change("waiting");
		act.PkWaiting.SendTo(actor, new Pk_Waiting { Msg = fig.StrWaitingOthers });

		switch (act.Phase.Current) {
			case Phase.UNINITIALIZED: return;
			case Phase.WAITING_TO_START:
				if (act.Actors.Count >= fig.MinActorCount) {
					BeginRound(act).Forget();
				}

				return;
			case Phase.ROUND_INTRO: return;
			case Phase.WRITING_CLUES:
				AssignCards(act, actor); // TEMP?
				return;
			case Phase.GUESSING:
				// TODO: allow guessing?
				return;
			case Phase.ROUND_SUMMARY: return;
			case Phase.GAME_COMPLETE: return;
			default: throw new ArgumentOutOfRangeException();
		}
	}

	public static void ActorRemoved(Act act, BewilderActor actor)
	{
		Log($"removed {actor}".LgTodo());
		var fig = act.Fig;

		if (act.Actors.Count < fig.MinActorCount) {
			NewGame(act);
			return; //>> below min players, restart
		}

		switch (act.Phase.Current) {
			case Phase.UNINITIALIZED: return;
			case Phase.WAITING_TO_START: return;
			case Phase.ROUND_INTRO: return;
			case Phase.WRITING_CLUES:
				CheckClueCount(act);
				return;
			case Phase.GUESSING:
				if (actor == act.ActiveActorTurn) {
					NextGuessing(act).Forget();
					return; //>> active actor left
				}

				CheckGuessCount(act);
				return;
			case Phase.ROUND_SUMMARY: return;
			case Phase.GAME_COMPLETE: return;
			default: throw new ArgumentOutOfRangeException();
		}
	}

	public static void ChangePhase(Act act, Phase phase)
	{
		Log($"<><><><><><> phase: {phase}".LgOrange(), act);
		var fig = act.Fig;

		var (title, desc) = phase switch {
			Phase.UNINITIALIZED => ("", ""),
			Phase.WAITING_TO_START => (fig.StrPhaseTitle_WaitingToStart,
				fig.StrPhaseDesc_WaitingToStart),
			Phase.ROUND_INTRO => (fig.StrPhaseTitle_RoundIntro, fig.StrPhaseDesc_RoundIntro),
			Phase.WRITING_CLUES => (fig.StrPhaseTitle_WritingClues, fig.StrPhaseDesc_WritingClues),
			Phase.GUESSING => (fig.StrPhaseTitle_Guessing, fig.StrPhaseDesc_Guessing),
			Phase.ROUND_SUMMARY => (fig.StrPhaseTitle_RoundSummary, fig.StrPhaseDesc_RoundSummary),
			Phase.GAME_COMPLETE => (fig.StrPhaseTitle_GameComplete, fig.StrPhaseDesc_GameComplete),
			_ => throw new ArgumentOutOfRangeException(nameof(phase), phase, null)
		};

		act.PhaseTitle.Change(title);
		act.PhaseDesc.Change(desc);
		act.Phase.Change(phase);
	}

	public static void NewGame(Act act)
	{
		var fig = act.Fig;
		if (act.Phase.Current == Phase.WAITING_TO_START) return; //>> already made new game

		_cancelSource = _cancelSource.Remake();

		ChangePhase(act, Phase.WAITING_TO_START);
		if (act.Actors.Count >= fig.MinActorCount) {
			BeginRound(act).Forget();
		}
	}

	public static async UniTask BeginRound(Act act)
	{
		var fig = act.Fig;
		Log($"<><><><><><> {act} Round Begin".LgTodo(), act);
		ChangePhase(act, Phase.ROUND_INTRO);
		SetStatusAll(act, "", Status.READY);
		act.PkWaiting.SendToAllAgents(new Pk_Waiting { Msg = fig.StrLoading });

		var currentWords = act.CurrentWords;
		currentWords.GrabBag(
			act.WordBag,
			fig.NumOfCards,
			fig.AllPossibleWords,
			true
		);

		act.CurrentWordsString = currentWords.Join(",");

		await __WAIT__(fig.RevealStartMs);

		foreach (var card in act.Cards.Value) {
			card.Word.Change(currentWords[card.CardId]);
			card.IsCorrect.Change(false);
			card.PickedByActorSlotIds.Clear();
			card.ShowCount.Change(0);
			card.IsRevealed.Change(true);
			await __WAIT__(fig.RevealDelayMs);
		}

		ChangePhase(act, Phase.WRITING_CLUES);

		Log($"  NEW ROUND:      <b>{act.CurrentWordsString}</b>".LgOrange(skipPrefix: true));

		foreach (var actor in act.Actors.Value) {
			AssignCards(act, actor);
		}
	}

	public static void AssignCards(Act act, Actor actor)
	{
		var fig = act.Fig;
		actor.AssignedCards.GrabBag(
			act.CardBag,
			fig.NumOfLocked,
			act.Cards.Value,
			true
		);

		act.PkRoundStart.SendTo(
			actor,
			new Pk_BewilderRoundStart {
				Min = fig.ChoiceMin,
				Max = fig.ChoiceMax,
				Words = act.CurrentWordsString,
				Lock = actor.AssignedCards.Join(static c => c.CardId.ToString()),
			}
		);

		actor.Status.Change(Status.WRITING_CLUE);
		actor.Agent.Status.Change("writing clue");
		actor.Clue.Change("");
		actor.ClueCardIds.Clear();
	}

	static void CheckClueCount(Act act)
	{
		var allSubmitted =
			act.Actors.Value.All(static a => a.Status.Current != Status.WRITING_CLUE);

		if (allSubmitted) {
			BeginGuessing(act).Forget();
		}
	}

	public static async UniTask BeginGuessing(Act act)
	{
		var fig = act.Fig;
		Log($"<><><><><><> {act} Begin Guessing".LgTodo(), act);
		ChangePhase(act, Phase.GUESSING);
		SetStatusAll(act, "", Status.READY);
		act.PkWaiting.SendToAllAgents(new Pk_Waiting { Msg = fig.StrLoading });

		var actors = act.Actors.Value;

		act.PendingActorTurn
		   ._Clear()
		   .ShuffleAdd(actors);

		NextGuessing(act).Forget();
	}

	static void CheckGuessCount(Act act)
	{
		var allSubmitted =
			act.Actors.Value.All(static a => a.Status.Current != Status.GUESSING);

		if (allSubmitted) {
			ShowGuessResult(act).Forget();
		}
	}

	public static async UniTask NextGuessing(Act act)
	{
		var fig = act.Fig;
		if (act.PendingActorTurn.Nil()) {
			await EndGuessing(act);
			return; //>> all actor turns done
		}

		var activeActor = act.PendingActorTurn.GrabLast();
		act.ActiveActorTurn = activeActor;
		var clue = activeActor.Clue.Current;

		if (clue.Nil()) {
			NextGuessing(act).Forget();
			return; //>> actor missing clue
		}

		foreach (var card in act.Cards.Value) {
			card.IsCorrect.Change(false);
			card.PickedByActorSlotIds.Clear();
			card.ShowCount.Change(0);
		}

		var pkGuessing = new Pk_BewilderGuessing {
			Number = fig.ChoiceMax + fig.NumOfLocked,
			ActiveSlotId = activeActor.SlotId,
			Clue = clue,
			Words = act.CurrentWordsString,
		};

		foreach (var actor in act.Actors.Value) {
			actor.GuessedCardIds.Clear();

			if (actor == activeActor) {
				//## don't guess on your own clue
				actor.Status.Change(Status.SUBMITTED_GUESS);
				actor.Agent.Status.Change("sweating");
				act.PkWaiting.SendTo(actor, new Pk_Waiting { Msg = fig.StrOthersGuessing });
			}
			else {
				actor.Status.ChangeDiff(Status.GUESSING);
				actor.Agent.Status.Change("guessing");
				act.PkGuessing.SendTo(actor, pkGuessing); // OPTIMIZE: sending to all but 1
			}
		}

		act.GuessingActorSlotId.Change(activeActor.SlotId);
		act.GuessingClue.Change(clue);
		act.PhaseTitle.Change($"Clue: {clue.ToUpper()}");
		act.PhaseDesc.Change($"Clue Giver: {activeActor.Nickname}");
	}

	static async UniTask ShowGuessResult(Act act)
	{
		var fig = act.Fig;
		var actors = act.Actors.Value;
		var activeActor = act.ActiveActorTurn;
		var cards = act.Cards.Value;
		var scoring = fig.Scoring;
		var hitsGoal = fig.ChoiceMax + fig.NumOfLocked;
		var perfectsGoal = actors.Count;

		//## clear actor stuff
		foreach (var actor in actors) {
			actor.Status.ChangeDiff(Status.READY);
			actor.Agent.Status.Change("");
			actor.PendingScore = 0;
		}

		//## set which cards will be correct
		foreach (var card in cards) {
			card.PendingIsCorrect = activeActor.ClueCardIds.Contains(card.CardId);
		}

		await __WAIT__(fig.GuessStartMs);

		var perfects = 0;

		//## apply actor's guesses to cards
		foreach (var actor in actors) {
			var hits = 0;

			foreach (var actorGuessedCardId in actor.GuessedCardIds) {
				var card = cards[actorGuessedCardId];
				card.PickedByActorSlotIds.Add(actor.SlotId);
				if (card.PendingIsCorrect) {
					++hits;
					actor.PendingScore += scoring.GuessHit;
					activeActor.PendingScore += scoring.ClueHit;
				}
			}

			if (hits >= hitsGoal) {
				++perfects;
				actor.PendingScore += scoring.GuessHitPerfect;
				activeActor.PendingScore += scoring.CluePerfectPerPlayer;
			}
		}

		if (perfects >= perfectsGoal) {
			activeActor.PendingScore += scoring.CluePerfectAll;
		}

		//## show guesses on each card
		foreach (var card in cards) {
			foreach (var _ in card.PickedByActorSlotIds.Current) {
				await __WAIT__(fig.GuessIntervalMs);
				card.ShowCount.Increment();
			}
		}

		//## show correct cards
		foreach (var card in cards) {
			if (card.PendingIsCorrect) {
				await __WAIT__(fig.GuessIntervalMs);
				card.IsCorrect.Change(true);
			}
		}

		await __WAIT__(fig.GuessEndMs);

		//## apply scores
		foreach (var actor in actors) {
			await __WAIT__(fig.ScoreIntervalMs);
			actor.Agent.Status.Change($"+{actor.PendingScore}");
			actor.Agent.Score.Increment(actor.PendingScore);
		}

		await __WAIT__(fig.ScoreEndMs);

		SetStatusAll(act, "");

		NextGuessing(act).Forget();
	}

	public static async UniTask EndGuessing(Act act)
	{
		// TODO: EndRound / Round summary?
		await EndRound(act);
	}

	public static async UniTask EndRound(Act act)
	{
		Log($"<><><><><><> {act} Round End".LgTodo(), act);
		var fig = act.Fig;
		ChangePhase(act, Phase.ROUND_SUMMARY); // TODO: round summary?
		SetStatusAll(act, "");
		act.PkWaiting.SendToAllAgents(new Pk_Waiting { Msg = fig.StrLoading });

		var cards = act.Cards.Value;

		for (var i = cards.Count - 1; i >= 0; i--) {
			cards[i].IsRevealed.Change(false);
			await __WAIT__(fig.RevealDelayMs);
		}

		BeginRound(act).Forget();
	}

	public static void DrainPackets(Act act)
	{
		act.PkClue
		   .OnlyIf(act.Phase.Current == Phase.WRITING_CLUES)
		   .DrainAsActor(
				act,
				static (act, actor, pk) => {
					var fig = act.Fig;
					actor.Status.Change(Status.SUBMITTED_CLUE);
					actor.Agent.Status.Change("submitted clue");
					actor.Clue.Change(pk.Clue);

					var clueCardIds = actor.ClueCardIds._Clear();
					actor.AssignedCards.MapInto(clueCardIds, static c => c.CardId);
					pk.Chosen.SplitIndexesInto(clueCardIds);

					act.PkWaiting.SendTo(actor, new Pk_Waiting { Msg = fig.StrWaitingOthers });
					CheckClueCount(act);
				}
			);

		act.PkGuess
		   .OnlyIf(act.Phase.Current == Phase.GUESSING)
		   .DrainAsActor(
				act,
				static (act, actor, pk) => {
					var fig = act.Fig;
					actor.Status.Change(Status.SUBMITTED_GUESS);
					actor.Agent.Status.Change("submitted guess");
					pk.Guess.SplitIndexesInto(actor.GuessedCardIds);

					act.PkWaiting.SendTo(actor, new Pk_Waiting { Msg = fig.StrWaitingOthers });
					CheckGuessCount(act);
				}
			);
	}

	public static async UniTaskVoid ForceNextRound(Act act)
	{
		Log($"FORCE NEXT ROUND ----------".LgRed(), act);

		await EndRound(act);
		// await BeginRound(act);
	}

	static void SetStatusAll(Act act, string agentStatus, Status actorStatus = Status.UNSET)
	{
		if (actorStatus != Status.UNSET) {
			foreach (var actor in act.Actors.Value) {
				actor.Agent.Status.Change(agentStatus);
				actor.Status.Change(actorStatus);
			}
		}
		else {
			foreach (var actor in act.Actors.Value) {
				actor.Agent.Status.Change(agentStatus);
			}
		}
	}
}
}