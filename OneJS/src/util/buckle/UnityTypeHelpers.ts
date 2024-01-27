import {List} from 'System/Collections/Generic';

export class uList<TEl> extends List<TEl> {
	[Symbol.iterator]: Function;
}