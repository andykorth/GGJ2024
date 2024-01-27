import {h} from 'preact';
import {JSXInternal} from 'preact/jsx';
import generateComponentId from 'onejs/styled/utils/generateComponentId';
import flatten from 'css-flatten';
import {lg} from './lg';
import {Sprite} from 'UnityEngine';


/*
Settings -> Languages & Frameworks -> JavaScript -> Styled Components

.idea\misc.xml

 <option value="sty.div" />

module:preact/jsx.JSXInternal.IntrinsicElements
ScriptLib/definitions/jsx.d.ts

 copied from:  ScriptLib/onejs/styled/index.tsx

*/

function _hashAndAddRuntimeUSS(style: string) {
	const compId = generateComponentId(style);
	style = flatten(`.${compId} {${style}}`);
	document.addRuntimeUSS(style);
	return compId;
}

function _processTemplate(props, strings: TemplateStringsArray, values: any[]) {
	const style = values.reduce((result, expr, index) => {
		let value = typeof expr === 'function' ? expr(props) : expr;
		
		if (typeof value === 'function') value = value(props);
		if (!value) value = '';
		
		return `${result}${value}${strings[index + 1]}`;
	}, strings[0]);
	return style as string;
}


type Vel = JSXInternal.VisualElement;

type T_Label = {
	text: string,
	enableRichText?: boolean,
} & Vel;

type T_Img = {
	sprite: Sprite,
} & Vel;

type T_Button = {
	text?: string,
	onClick: () => void,
	disabled?: boolean,
} & Vel;

type T_RadioButtonGroup = {
	// choices?: string[],
} & Vel;

type T_RadioButton = {
	text?: string,
	value?: boolean,
} & Vel

type T_Foldout = {
	text?: string,
	value?: boolean,
} & Vel

// TODO: other element props?

export const $div = <TProps = void>(name?: string) => styling<
	TProps extends void ? Vel : Vel & TProps
>('div', name);

/**
 -unity-font-style: italic;
 -unity-font-style: bold;
 -unity-text-align: middle-right;
 */
export const $label = <TProps = void>(name?: string) => styling<
	TProps extends void ? T_Label : T_Label & TProps
>('label', name);

export const $img = <TProps = void>(name?: string) => styling<
	TProps extends void ? T_Img : T_Img & TProps
>('image', name);

export const $foldout = <TProps = void>(name?: string) => styling<
	TProps extends void ? T_Foldout : T_Foldout & TProps
>('foldout', name);

export const $button = <TProps = void>(name?: string) => styling<
	TProps extends void ? T_Button : T_Button & TProps
>('button', name);

export const $radiobutton = <TProps = void>(name?: string) => styling<
	TProps extends void ? T_RadioButton : T_RadioButton & TProps
>('radiobutton', name);

export const $radiobuttongroup = <TProps = void>(name?: string) => styling<
	TProps extends void ? T_RadioButtonGroup : T_RadioButtonGroup & TProps
>('radiobuttongroup', name);

function styling<TProps extends Vel>(JsxTag: string, name?: string) {
	return (strings: TemplateStringsArray, ...values) =>
		(props: TProps) => {
			const style = _processTemplate(props, strings, values);
			const compId = _hashAndAddRuntimeUSS(style);
			const className = props.class ? `${compId} ${props.class}` : compId;
			
			// lg(`render ${JsxTag} ${name} ${className}`, this);
			return <JsxTag name={name} {...props} class={className}></JsxTag>;
		};
}



export const Grow = $div('grow')`
  flex: 1 0 auto;
`;

export const Row = $div('row')`
  flex-direction: row;
`;

export const Col = $div('col')``;


// export const $Grow = $div('grow')`
//   flex: 1 0 auto;
// `;
//
// export const $Row = $div('row')`
//   flex-direction: row;
// `;
//
// export const $Col = $div('col')``;