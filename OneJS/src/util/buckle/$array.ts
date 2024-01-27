import {uList} from './UnityTypeHelpers';

export function $isIterable(x: any): boolean {
	return !!x?.[Symbol.iterator];
}


export function $map<TEl, TR>(
	x: TEl[] | uList<TEl>,
	fn: (el: TEl, i: number) => TR,
): TR[] {
	if (!x?.[Symbol.iterator]) throw new Error(`not iterable: ${x}`);
	
	const arr: TR[] = [];
	
	let i = 0;
	
	// @ts-ignore
	for (const el of x) {
		arr.push(fn(el, i));
		i++;
	}
	return arr;
}


export type Lup<TVal> = {
	[key: string|number]: TVal
}

export function $toLup<TEl>(
	x: TEl[] | uList<TEl>,
	keyProp: string,
): Lup<TEl> {
	if (!x?.[Symbol.iterator]) throw new Error(`not iterable: ${x}`);
	
	const lup: Lup<TEl> = {};
	
	// @ts-ignore
	for (const el of x) {
		const key = el[keyProp];
		lup[key] = el;
	}
	return lup;
}