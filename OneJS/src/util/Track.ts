import {MutableRef, StateUpdater, useCallback, useEffect, useRef, useState} from 'preact/hooks';
import {uList} from './buckle/UnityTypeHelpers';
import {lg} from './lg';
import {Dom} from 'OneJS/Dom';

const valueKey = 'Current';
const addEventKey = 'add_EventValueChanged';
const removeEventKey = 'remove_EventValueChanged';

export type ValidTrackTypes =
	boolean
	| int
	| float
	| string
	| uList<ValidTrackTypes>
	| any // ???
// TODO
	;

export type GetTrackType<TTrack extends Track<ValidTrackTypes>>
	= TTrack extends Track<infer TVal> ? TVal : unknown;

export interface Track<TVal extends ValidTrackTypes> {
	Current: TVal;
	
	Change(value: TVal): void;
	
	ChangeDiff(value: TVal): void;
}

export function useTrack<
	TTrack extends Track<any>,
	TVal extends GetTrackType<TTrack>,
>(
	track: TTrack,
): TVal {
	// lgRender(this);
	const currentVal = track[valueKey];
	const [v, setV] = useState(1);
	
	const onValueChangedCallback = useCallback(() => {
		setV(v => v + 1);
	}, []);
	
	useEffect(() => {
		const addEvent = track[addEventKey] as Function;
		const removeEvent = track[removeEventKey] as Function;
		
		addEvent.call(track, onValueChangedCallback);
		// lg(`${track} (useEffect) addEvent called`, this);
		onEngineReload(() => removeEvent.call(track, onValueChangedCallback));
		
		if (currentVal != track[valueKey]) {
			lg(`${track} (useEffect) initial value changed (SHOULD BE RARE)`, this);
			onValueChangedCallback();
		}
		
		return () => {
			removeEvent.call(track, onValueChangedCallback);
			// TODO: unregisterOnEngineReload?
		};
	}, [track]);
	
	// lg(`${track} (render) value: ${track[valueKey]}`, this);
	
	return track[valueKey] as TVal;
}


/* just use track.Change(v) directly */
// export function useTrackSet<
// 	TTrack extends Track<any>,
// 	TVal extends GetTrackType<TTrack>,
// >(
// 	track: TTrack,
// ): [TVal, StateUpdater<TVal>] {
// 	const val = useTrack(track);
// 	const setVal = useCallback((val) => track.ChangeDiff(val), [track]);
// 	return [val, setVal];
// }


export function useTrackList<
	TTrack extends TrackList<any>,
	TVal extends GetTrackType<TTrack>,
>(
	trackList: TTrack,
): TVal {
	const [v, setV] = useState(1);
	
	const onValueChangedCallback = useCallback(() => {
		setV(v => v + 1);
	}, []);
	
	useEffect(() => {
		const addEvent = trackList[addEventKey] as Function;
		const removeEvent = trackList[removeEventKey] as Function;
		
		addEvent.call(trackList, onValueChangedCallback);
		onEngineReload(() => removeEvent.call(trackList, onValueChangedCallback));
		
		return () => {
			removeEvent.call(trackList, onValueChangedCallback);
			// TODO: unregisterOnEngineReload?
		};
	}, [trackList]);
	
	return trackList[valueKey] as TVal;
}

export interface TrackList<TVal> extends Track<uList<TVal>> {
	Current: uList<TVal>;
	// TODO: changing, Add, etc.
}


export interface TrackChoice<TVal> {
	Current: TVal;
	CurrentIndex: number;
	Choices: uList<TVal>;
	
	ChangeDiff(index: number): void;
}

type GetTrackChoiceType<TTrack extends TrackChoice<any>>
	= TTrack extends TrackChoice<infer TVal> ? TVal : unknown;

export function useTrackChoiceList<
	TTrack extends TrackChoice<any>,
	TVal extends GetTrackChoiceType<TTrack>,
>(
	track: TTrack,
): [uList<TVal>, number, TVal, StateUpdater<number>] {
	const [index, setIndex] = useState(track.CurrentIndex);
	
	const [, updateState] = useState({});
	const forceUpdate = useCallback(() => updateState({}), []);
	
	const addEvent = track[addEventKey] as Function;
	const removeEvent = track[removeEventKey] as Function;
	
	const onValueChangedCallback = function (nextVal) {
		setIndex(nextVal);
		forceUpdate();
	};
	
	useEffect(() => {
		addEvent.call(track, onValueChangedCallback);
		onEngineReload(() => removeEvent.call(track, onValueChangedCallback));
		return () => removeEvent.call(track, onValueChangedCallback);
	}, []);
	
	// lg(`${index} ${track.Current?.Name}`, this);
	
	const setIndexWrapper = (i) => track.ChangeDiff(i);
	return [track.Choices, index, track.Current, setIndexWrapper];
}

export function useTrackChoice<
	TTrack extends TrackChoice<any>,
	TVal extends GetTrackChoiceType<TTrack>,
>(
	track: TTrack,
): TVal {
	const [, , val] = useTrackChoiceList(track);
	return val;
}


export interface TrackEvt extends Track<int> {
	Trigger(): void;
	
	add_EventValueChanged(fn: Function): void;
	
	remove_EventValueChanged(fn: Function): void;
}

export function useTrackEvt<TTrack extends TrackEvt>(
	track: TTrack,
): () => void {
	
	const [, updateState] = useState({});
	const forceUpdate = useCallback(() => updateState({}), []);
	
	const handler = function () {
		forceUpdate();
	};
	
	useEffect(() => {
		track.add_EventValueChanged(handler);
		onEngineReload(() => track.remove_EventValueChanged(handler));
		return () => track.remove_EventValueChanged(handler);
	}, []);
	
	return () => track.Trigger();
}


export interface TrackToggle {
	IsOn: boolean;
	
	Toggle(): void;
	
	add_EventValueChanged(fn: Function): void;
	
	remove_EventValueChanged(fn: Function): void;
}

export function useTrackToggle<TTrack extends TrackToggle>(
	track: TTrack,
): [boolean, () => void] {
	
	const [, updateState] = useState({});
	const forceUpdate = useCallback(() => updateState({}), []);
	
	const handler = function () {
		// lg(`useTrackToggle.handler`, this);
		forceUpdate();
	};
	
	useEffect(() => {
		track.add_EventValueChanged(handler);
		onEngineReload(() => track.remove_EventValueChanged(handler));
		return () => track.remove_EventValueChanged(handler);
	}, []);
	
	return [track.IsOn, () => track.Toggle()];
}




export function useTrackFn<
	TTrack extends Track<any>,
	TVal extends GetTrackType<TTrack>
>(
	track: TTrack,
	fn: (val: TVal) => void,
) {
	const callFn = useCallback(() => fn(track[valueKey]), []);
	useTrackEffect(track, callFn);
}

export function useTrackRefFn<
	TTrack extends Track<any>,
	TVal extends GetTrackType<TTrack>
>(
	track: TTrack,
	fn: (val: TVal, dom: Dom) => void,
): MutableRef<Dom> {
	const ref = useRef<Dom>();
	
	const callFn = useCallback(() => fn(track[valueKey], ref.current), []);
	useTrackEffect(track, callFn);
	
	return ref;
}

export function useTrackBoolVisible<TTrack extends Track<boolean>>(
	track: TTrack,
): MutableRef<Dom> {
	const ref = useRef<Dom>();
	
	const callFn = useCallback(
		() => {
			ref.current.style.display = track[valueKey] ? 'Flex' : 'None';
		},
		[],
	);
	useTrackEffect(track, callFn);
	
	return ref;
}


export function useTrackEvtFn<TTrack extends TrackEvt>(
	track: TTrack,
	fn: (dom: Dom) => void,
): MutableRef<Dom> {
	const ref = useRef<Dom>();
	
	const callFn = useCallback(() => fn(ref.current), []);
	useTrackEffect(track, callFn);
	
	return ref;
}


export function useTrackEffect<TTrack extends Track<any>>(
	track: TTrack,
	callFn: () => void,
) {
	useEffect(() => {
		const addEvent = track[addEventKey] as Function;
		const removeEvent = track[removeEventKey] as Function;
		
		callFn();
		addEvent.call(track, callFn);
		
		onEngineReload(removeHandler);
		
		return () => {
			removeHandler();
			unregisterOnEngineReload(removeHandler);
		};
		
		function removeHandler() {
			removeEvent.call(track, callFn);
		}
		
	}, [track]);
}





// https://github.com/DragonGround/ScriptLib/blob/main/onejs/index.ts
// export function useEventfulState<T, K extends keyof T>(obj: T, propertyName: K, eventName?: string): [T[K], StateUpdater<T[K]>] {
// 	const [val, setVal] = useState(obj[propertyName] as unknown as T[K])
// 	const [, updateState] = useState({})
// 	const forceUpdate = useCallback(() => updateState({}), [])
//
// 	eventName = eventName || "On" + String(propertyName) + "Changed"
// 	let addEventFunc = obj[`add_${eventName}`] as Function
// 	let removeEventFunc = obj[`remove_${eventName}`] as Function
//
// 	if (!addEventFunc || !removeEventFunc)
// 		throw new Error(`[useEventfulState] The object does not have an event named ${eventName}`)
//
// 	let onValueChangedCallback = function (v) {
// 		setVal(v)
// 		forceUpdate()
// 	}
//
// 	useEffect(() => {
// 		addEventFunc.call(obj, onValueChangedCallback)
// 		onEngineReload(() => {
// 			removeEventFunc.call(obj, onValueChangedCallback)
// 		})
// 		return () => {
// 			removeEventFunc.call(obj, onValueChangedCallback)
// 		}
// 	}, [])
// 	const setValWrapper = (v) => {
// 		obj[propertyName] = v
// 		// setVal(v) // No need to set the state here in JS. The event handling stuff above will do.
// 	}
// 	return [val, setValWrapper]
// }
//
// // is the val useState actually needed?
// export function useTrackSet2<
// 	TTrack extends Track<any>,
// 	TVal extends GetTrackType<TTrack>,
// >(
// 	track: TTrack,
// ): [TVal, StateUpdater<TVal>] {
// 	const [val, setVal] = useState(track[valueKey] as TVal);
//
// 	const [, updateState] = useState({});
// 	const forceUpdate = useCallback(() => updateState({}), []);
//
// 	const addEvent = track[addEventKey] as Function;
// 	const removeEvent = track[removeEventKey] as Function;
//
// 	const onValueChangedCallback = function (nextVal) {
// 		setVal(nextVal);
// 		forceUpdate();
// 	};
//
// 	useEffect(() => {
// 		addEvent.call(track, onValueChangedCallback);
// 		onEngineReload(() => removeEvent.call(track, onValueChangedCallback));
// 		return () => removeEvent.call(track, onValueChangedCallback);
// 	}, []);
//
// 	const setValWrapper = (val) => track.ChangeDiff(val);
// 	return [val, setValWrapper];
// }
//
// export function useTrackSet22<
// 	TTrack extends Track<any>,
// 	TVal extends GetTrackType<TTrack>,
// >(
// 	track: TTrack,
// ): [TVal, StateUpdater<TVal>] {
// 	lg(`${track} useTrackSet`);
//
// 	const [v, setV] = useState(1);
//
// 	const onValueChangedCallback = useCallback(() => {
// 		lg(`${track} onValueChangedCallback version: ${v} -> ${v + 1}`);
// 		setV(v => v + 1);
// 	}, []);
//
// 	useEffect(() => {
// 		lg(`${track} useEffect run`);
//
// 		const addEvent = track[addEventKey] as Function;
// 		const removeEvent = track[removeEventKey] as Function;
//
// 		addEvent.call(track, onValueChangedCallback);
// 		onEngineReload(() => removeEvent.call(track, onValueChangedCallback));
//
// 		return () => {
// 			lg(`${track} removeEvent (useEffect end)`);
// 			removeEvent.call(track, onValueChangedCallback);
// 			// TODO: unregisterOnEngineReload?
// 		};
// 	}, [track]);
//
// 	const setValWrapper = (val) => track.ChangeDiff(val); // TODO: memo
// 	return [track[valueKey] as TVal, setValWrapper];
// }