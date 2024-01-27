import {Registry} from '../../util/Registry';
import {Track, TrackEvt, TrackList} from '../../util/Track';
import {T_Actor} from '../../types/foundational';

export interface T_BewilderActivity {
	Actors: Registry<T_BewilderActor>;
	Cards: Registry<T_BewilderCard>;
	
	Phase: Track<E_BewilderPhaseEnum>;
	PhaseTitle: Track<string>;
	PhaseDesc: Track<string>;
	GuessingActorSlotId: Track<number>;
	GuessingClue: Track<string>;
	ForceNextRound: TrackEvt;
}

export enum E_BewilderPhaseEnum {
	UNINITIALIZED,
	WAITING_TO_START,
	ROUND_INTRO,
	WRITING_CLUES,
	GUESSING,
	ROUND_SUMMARY,
	GAME_COMPLETE,
}

export enum E_BewilderActorStatusEnum {
	UNSET,
	OBSERVING,
	READY,
	WRITING_CLUE,
	SUBMITTED_CLUE,
	SUBMITTED_GUESS,
}

export interface T_BewilderActor extends T_Actor {
	Status: Track<E_BewilderActorStatusEnum>;
	Clue: Track<string>;
}

export interface T_BewilderCard {
	EntityId: number;
	CardId: number;
	IsRevealed: Track<boolean>;
	Word: Track<string>;
	IsCorrect: Track<boolean>;
	PickedByActorSlotIds: TrackList<int>;
	ShowCount: Track<int>;
}

export interface P_BewilderView {
	act: T_BewilderActivity;
}