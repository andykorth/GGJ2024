import {uList} from './buckle/UnityTypeHelpers';
import {useCallback, useEffect, useState} from 'preact/hooks';
import {lg} from './lg';
import {$toLup, Lup} from './buckle/$array';


export interface Registry<TComp> {
	Count: number;
	Value: uList<TComp>;
	Id: number;
	
	add_EventValueChanged(fn: Function): void;
	
	remove_EventValueChanged(fn: Function): void;
}

type GetRegistryType<TRegistry extends Registry<any>>
	= TRegistry extends Registry<infer TComp> ? TComp : unknown;

export function useRegistry<
	TRegistry extends Registry<any>,
	TComp extends GetRegistryType<TRegistry>,
>(
	registry: TRegistry,
): uList<TComp> {
	// lg(`useRegistry ${registry.Id}`);
	
	const [, updateState] = useState({});
	const forceUpdate = useCallback(() => {
		updateState({});
		// lg(`forceUpdate ${registry.Id}`);
	}, []);
	
	const handler = function () {
		// lg(`handler ${registry.Id}`);
		forceUpdate();
	}
	
	useEffect(() => {
		// lg(`useEffect ${registry.Id}`);
		registry.add_EventValueChanged(handler);
		onEngineReload(() => registry.remove_EventValueChanged(handler));
		return () => registry.remove_EventValueChanged(handler);
	}, []);
	
	return registry.Value;
}


export function useRegistryLup<
	TRegistry extends Registry<any>,
	TComp extends GetRegistryType<TRegistry>,
>(
	registry: TRegistry,
	keyProp: string,
): Lup<TComp> {
	// lg(`useRegistry ${registry.Id}`);
	
	const [, updateState] = useState({});
	const forceUpdate = useCallback(() => {
		updateState({});
		// lg(`forceUpdate ${registry.Id}`);
	}, []);
	
	const handler = function () {
		// lg(`handler ${registry.Id}`);
		forceUpdate();
	}
	
	useEffect(() => {
		// lg(`useEffect ${registry.Id}`);
		registry.add_EventValueChanged(handler);
		onEngineReload(() => registry.remove_EventValueChanged(handler));
		return () => registry.remove_EventValueChanged(handler);
	}, []);
	
	return $toLup(registry.Value, keyProp); // OPTIMIZE
}