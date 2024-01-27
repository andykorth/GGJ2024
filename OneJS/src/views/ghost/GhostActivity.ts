import {Track, TrackEvt, TrackList} from '../../util/Track';
import {T_Actor} from '../../types/foundational';

export interface T_GhostActivity {
	Actors: TrackList<T_GhostActor>;
	
	Phase: Track<E_GhostPhaseEnum>;
	PhaseTitle: Track<string>;
	PhaseDesc: Track<string>;
	// ForceNextRound: TrackEvt;
}

export enum E_GhostPhaseEnum {
	UNINITIALIZED,
	WAITING_TO_START,
	ROUND_INTRO,
	PLAYING_ROOM,
	ROOM_SUMMARY,
	GAME_COMPLETE,
}


export enum E_GhostActorStatusEnum {
	UNSET,
	READY,
}

export interface T_GhostActor extends T_Actor {
	Status: Track<E_GhostActorStatusEnum>;
}

export interface P_GhostView {
	act: T_GhostActivity;
}