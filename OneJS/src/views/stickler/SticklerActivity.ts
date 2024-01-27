import {Registry} from '../../util/Registry';
import {Track, TrackEvt, TrackList} from '../../util/Track';
import {T_Actor} from '../../types/foundational';

export interface T_SticklerActivity {
	Actors: Registry<T_SticklerActor>;
	
	Phase: Track<E_SticklerPhaseEnum>;
}

export enum E_SticklerPhaseEnum {
	UNINITIALIZED,
	WAITING_TO_START,
}

export enum E_SticklerActorStatusEnum {
	UNSET,
	OBSERVING,
}

export interface T_SticklerActor extends T_Actor {
	Status: Track<E_SticklerActorStatusEnum>;
	Clue: Track<string>;
}

export interface P_SticklerView {
	act: T_SticklerActivity;
}