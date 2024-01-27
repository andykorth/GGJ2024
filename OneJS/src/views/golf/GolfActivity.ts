import {Registry} from '../../util/Registry';
import {T_Actor} from '../../types/foundational';


export interface T_GolfActivity {
	Actors: Registry<T_GolfActor>;
	
}

export interface T_GolfActor extends T_Actor {

}

export interface P_GolfView {
	act: T_GolfActivity;
}