import {Track, TrackChoice, TrackEvt, TrackList, TrackToggle} from '../util/Track';
import {uList} from '../util/buckle/UnityTypeHelpers';
import {Color} from 'UnityEngine';
// import {Registry} from '../util/Registry';
import {List} from 'System/Collections/Generic';

// TODO: convert to interface types?

export class GameSysClip {
	// extends ClipNative implements IClip, IMHashable, IHasEntity, ICog
	// FutzConfig: FutzConfig
	// FutzHost: FutzHost
	RoomIdf: Track<string>;
	Status: Track<string>;
	ActivityChoice: TrackChoice<ActivityDef>;
	Agents: TrackList<Agent>;
	CurrentActivity: Track<any>;
	// MatcherActivity: MatcherActivity
	// SystemActivity: SystemActivity
	// LoadedActivityDef: Track<ActivityDef>
	TestStr: Track<string>;
	TestInt: Track<int>;
	ChangeTestString = (str: string): void => {};
	
	TestObjList: uList<TestObj>;
	TestObjArray: TestObj[];
	TestStringList: uList<string>;
	TestStringArray: string[];
	TestTrackList: TrackList<string>;
	
	EvtTest: (number) => void;
}

export class CoreUiClip {
	ShowActivitySelect: TrackToggle;
	ShowAgentList: TrackToggle;
	ShowScore: TrackToggle;
	OptionsMenu: TrackEvt;
	ToggleHud: TrackEvt;
	Cancel: TrackEvt;
	Console: TrackEvt;
	FKeys: Track<number>;
}

export class ActivityDef {
	Idf: string;
	Name: string;
	// Fab_Activity: ActivityBase
	// Fab_Actor: ActorBase
	// PacketFacts: List<PacketFact>
	// SystemOnly: boolean
	// IdOffset: number
	// MakePacketFacts(): void
	// SpawnActivity(): ActivityBase
}

export class TestObj {
	Name: string;
}

export class VersionNumber {
	VersionName: string;
	AutoSet: boolean;
	Year: string;
	Month: string;
	Day: string;
	Hour: string;
	Minute: string;
	Version: string;
	NameAndVersion: string;
}

export class Agent {
	EntityId: number;
	Info: Track<AgentInfo>;
	Status: Track<string>;
	Color: Track<Color>;
	Score: Track<number>;
	// CurrentActivity: ActivityBase
	// CurrentActor: ActorBase
	// SlotId: number
	// Nickname: string
}

export class AgentInfo {
	SlotId: number;
	GlobalId: number;
	Nickname: string;
	Uuid: string;
}

export interface T_Actor {
	EntityId: number;
	Agent: Agent;
	Nickname: string;
	Color: Color;
}


// export type ValidGluTypes =
// 	boolean
// 	| int
// 	| float
// 	| string
// // TODO
// 	;
//
// export interface Glu<TVal extends ValidGluTypes> {
// 	Value: TVal;
//
// 	OnValueChanged(value: TVal): void;
// }
//
//
// export function useGlu<
// 	TGlu extends Glu<any>,
// 	TVal extends GetGluType<TGlu>,
// >(
// 	glu: TGlu,
// ): [TVal, StateUpdater<TVal>] {
// 	return useEventfulState(glu, 'Value');
// }
//
// export function useGluVal<
// 	TGlu extends Glu<any>,
// 	TVal extends GetGluType<TGlu>,
// >(
// 	glu: TGlu,
// ): TVal {
// 	const [val, _] = useEventfulState(glu, 'Value');
// 	return val;
// }
//
// type GetGluType<TGlu extends Glu<ValidGluTypes>>
// 	= TGlu extends Glu<infer TVal> ? TVal : unknown;


// type KeysExtending<TObj, TTsp> = {
// 	[TKey in keyof TObj]: TObj[TKey] extends TTsp ? TKey : never;
// };
// type ReturnOfTsp<TTsp> = TTsp extends Tsp<infer TVal> ? TVal : unknown;
// type GetTspType<T> = T extends Tsp<infer TVal> ? Tsp<TVal> : unknown;
// type GetTspValType<T> = T extends Tsp<infer TVal> ? TVal : unknown;
// type GetTspType<T> = T extends Tsp<infer TVal> ? Tsp<TVal> : never;
// type GetTspValType<T> = T extends Tsp<infer TVal> ? TVal : never;
//
// export function useTsp<
// 	TObj,
// 	TKey extends keyof TObj
// >(obj: TObj, key: TKey): [TObj[TKey], StateUpdater<TObj[TKey]>] {
//
// }
//
// export function useTsp<
// 	TObj,
// 	TKey extends keyof TObj,
// 	TTsp extends GetTspType<TObj[TKey]>,
// 	TVal extends GetTspValType<TTsp>,
// 	// TTsp extends ReturnOfTsp<TObj[TKey]> ? TObj[TKey] : never
// 	// TTsp extends (Tsp<infer TVal> ? TVal : unknown),
// >(obj: TObj, key: TKey): [TVal, StateUpdater<TVal>] {
// // >(obj: TObj, key: TKey): TVal {
// 	const tsp = obj[key] as TTsp;
// 	const [val, setVal] = useEventfulState(obj[key], 'Value');
// 	// return val as TVal;
// 	return [val as TVal, setVal as StateUpdater<TVal>];
// 	// const tsp = obj[key] as ReturnOfTsp<TObj[TKey]>;
// 	// return actualUseTsp(tsp);
// }
//
// function actualUseTsp<
// 	TVal extends ValidTspTypes
// >(
// 	tsp: Tsp<TVal>
// ): [Tsp<TVal>[keyof Tsp<TVal>], StateUpdater<Tsp<TVal>[keyof Tsp<TVal>]>] {
// 	return useEventfulState(tsp, 'Value');
// }
//
// export function useTsp<
// 	TObj,
// 	TKey extends keyof TObj,
// 	TTsp extends Tsp<ValidTspTypes>,
// 	TVal extends ReturnOfTsp<TObj[TKey]>,
// 	// TVal extends KeysExtending<TObj, Tsp<ValidTspTypes>>,
// 	// TVal extends GetValType<TObj[TKey]>,
// >(
// 	obj: TObj,
// 	key: TKey,
// ): [TVal, StateUpdater<TVal>] {
// 	const tsp = obj[key];
// 	const [val, setVal] = useEventfulState(tsp, 'Value');
// 	return [val, setVal];
// 	// return useEventfulState(obj[key], 'Value');
// }
//
//
// export function useTspString<TObj, TKey extends keyof TObj>(
// 	obj: TObj,
// 	key: TKey,
// ): [string, StateUpdater<string>] {
// 	return useTsp(obj, key);
// }
//
// export function useTsp1234<
// 	TObj,
// 	TKey extends keyof TObj,
// 	TVal extends ValidTspTypes,
// >(
// 	obj: TObj,
// 	key: TKey,
// ): [TVal, StateUpdater<TVal>] {
// 	const result = useEventfulState(obj[key] as Tsp<TVal>, 'Value');
// 	return result as [TVal, StateUpdater<TVal>];
// }
//
// export function useTsp<
// 	TObj,
// 	TKey extends keyof TObj & keyof Extract<TObj, Tsp<ValidTspTypes>>,
// 	TTsp extends Tsp<ValidTspTypes> & TObj[TKey],
// 	TVal extends GetValType<TObj[TKey]>,
// >(
// 	obj: TObj,
// 	key: TKey,
// ): [TVal, StateUpdater<TVal>] {
// 	// const [val, setVal] = useEventfulState(obj[key] as Tsp<TVal>, 'Value');
// 	// return [val, setVal];
// 	return useEventfulState(obj[key] as Tsp<TVal>, 'Value');
// }
//
// const tsp = obj[key] as Tsp<TVal>;
// const [value, setValue] = useEventfulState(tsp, 'Value');
// // const setValue = (newVal: TVal) => tsp.Value = newVal;
// return [value, setValue];
//
// type TspProps<TObj, TVal> = Extract<TObj, Tsp<TVal> >;
//
// type PropOf<TObj, TPropName extends keyof TObj, TVal> = Tsp<TVal>;
//
// type Entries<TObj> = {
// 	[TKey in keyof TObj]: [TKey, TObj[TKey]];
// }[keyof TObj][];
//
// type Thing<TObj, TTsp> = {
// 	[TKey in keyof TObj]: TObj[TKey] extends TTsp ? TKey : never
// }[keyof TObj]
// 	;
//
// export function useTsp<
// 	TObj,
// 	TKey extends keyof TObj,
// 	TVal,
// 	TTsp extends Tsp<TVal>
//
// >(
// 	obj: TObj,
// 	propertyName: TKey,
// ): [TVal, StateUpdater<TVal>] {
// const tsp: Tsp<TVal> = obj[propertyName];
//
//
// const [value] = useEventfulState(tsp, 'Value');
// const setValue = (newVal: TVal) => tsp.Value = newVal;
// return [value, setValue];
// }