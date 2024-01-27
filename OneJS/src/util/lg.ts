interface Lumberjack {
	ExternalLog: (text: string, color: string, prefix: string) => {};
}


const lumberjack = require('Lumberjack') as Lumberjack;

const fallbackColor = 'c184ff';

// TODO: colors

export function lg(text: string, prefix: string | Function | object = '') {
	// log(text);
	const pre = typeof prefix === 'string'
		? `OneJS ${prefix}`
		: `OneJS ${prefix.constructor.name}`;
	
	lumberjack.ExternalLog(text, fallbackColor, pre);
}

export function lgRender(fn: Function) {
	lg(`render ${fn.constructor.name}`, fn);
}