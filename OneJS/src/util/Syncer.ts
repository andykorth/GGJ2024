import {StateUpdater, useCallback, useEffect, useState} from 'preact/hooks';
import {uList} from './buckle/UnityTypeHelpers';
import {lg} from './lg';

export type ValidSyncerTypes =
	boolean
	| int
	| float
	| string
	| uList<ValidSyncerTypes>
	| any // ???
// TODO
	;

type GetSyncerType<TSyncer extends Syncer<ValidSyncerTypes>>
	= TSyncer extends Syncer<infer TVal> ? TVal : unknown;

export interface Syncer<TVal extends ValidSyncerTypes> {
	Current: TVal;
	Change(value: TVal): void;
	ChangeDiff(value: TVal): void;
}

export function useSyncer<
	TSyncer extends Syncer<any>,
	TVal extends GetSyncerType<TSyncer>,
>(
	syncer: TSyncer,
): TVal {
	const [val] = useInternalSyncerState(syncer);
	return val;
}

// TODO: set syncer value?
// export function useSyncerSet<
// 	TSyncer extends Syncer<any>,
// 	TVal extends GetSyncerType<TSyncer>,
// >(
// 	syncer: TSyncer,
// ): [TVal, StateUpdater<TVal>] {
// 	return useInternalSyncerState(syncer);
// }

const valueKey = 'Current';
const addEventKey = 'add_EventValueChanged';
const removeEventKey = 'remove_EventValueChanged';

// TODO: is the val useState actually needed?
export function useInternalSyncerState<
	TSyncer extends Syncer<any>,
	TVal extends GetSyncerType<TSyncer>,
>(
	syncer: TSyncer,
): [TVal, StateUpdater<TVal>] {
	const [val, setVal] = useState(syncer[valueKey] as TVal);
	
	const [, updateState] = useState({});
	const forceUpdate = useCallback(() => updateState({}), []);
	
	const addEvent = syncer[addEventKey] as Function;
	const removeEvent = syncer[removeEventKey] as Function;
	
	const onValueChangedCallback = function (nextVal) {
		setVal(nextVal);
		forceUpdate();
	};
	
	useEffect(() => {
		addEvent.call(syncer, onValueChangedCallback);
		onEngineReload(() => removeEvent.call(syncer, onValueChangedCallback));
		return () => removeEvent.call(syncer, onValueChangedCallback);
	}, []);
	
	// const setValWrapper = (val) => syncer.ChangeDiff(val);
	const setValWrapper = (val) => {throw new Error(`TODO: set syncer val`)};
	return [val, setValWrapper];
}