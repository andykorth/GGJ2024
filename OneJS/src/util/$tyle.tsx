import {Fragment, h} from 'preact';
import {JSXInternal} from 'preact/jsx';
import generateComponentId from 'onejs/styled/utils/generateComponentId';
import flatten from 'css-flatten';
import {lg} from './lg';
import {Sprite} from 'UnityEngine';
import {ChangeEvent} from 'UnityEngine/UIElements';


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

export const $div = <TProps = void>(name?: string, className?: string) => styling<
	TProps extends void ? Vel : Vel & TProps
>('div', name, className);

export const $screen = <TProps = void>(name?: string) => styling<
	TProps extends void ? Vel : Vel & TProps
>('div', name, 'full-screen');

export type T_Label = {
	text: string,
	enableRichText?: boolean,
} & Vel;
/**
 -unity-font-style: italic;
 -unity-font-style: bold;
 -unity-text-align: middle-right;
 */
export const $label = <TProps = void>(name?: string, className?: string) => styling<
	TProps extends void ? T_Label : T_Label & TProps
>('label', name, className);


export type T_Img = {
	sprite: Sprite,
} & Vel;
export const $img = <TProps = void>(name?: string, className?: string) => styling<
	TProps extends void ? T_Img : T_Img & TProps
>('image', name, className);


export type T_Foldout = {
	text?: string,
	value?: boolean,
} & Vel
export const $foldout = <TProps = void>(name?: string, className?: string) => styling<
	TProps extends void ? T_Foldout : T_Foldout & TProps
>('foldout', name, className);


export type T_Button = {
	text?: string,
	onClick: () => void,
	disabled?: boolean,
} & Vel;
/** TODO: default button styles (font-size, etc.) */
export const $button = <TProps = void>(name?: string, className?: string) => styling<
	TProps extends void ? T_Button : T_Button & TProps
>('button', name, className);


export type T_RadioButton = {
	text?: string,
	value?: boolean,
} & Vel
export const $radiobutton = <TProps = void>(name?: string, className?: string) => styling<
	TProps extends void ? T_RadioButton : T_RadioButton & TProps
>('radiobutton', name, className);


export type T_RadioButtonGroup = {
	// choices?: string[],
} & Vel;
export const $radiobuttongroup = <TProps = void>(name?: string, className?: string) => styling<
	TProps extends void ? T_RadioButtonGroup : T_RadioButtonGroup & TProps
>('radiobuttongroup', name, className);

export type T_TextField = {
	value: string,
	onValueChanged: (e: ChangeEvent<string>) => void
	
	label?: string,
	multiline?: boolean,
} & Vel;
export const $field = <TProps = void>(name?: string, className?: string) => styling<
	TProps extends void ? T_TextField : T_TextField & TProps
>('textfield', name, className);


function styling<TProps extends Vel>(
	JsxTag: string,
	name?: string,
	className?: string
) {
	return (strings: TemplateStringsArray, ...values) =>
		(props: TProps) => {
			const style = _processTemplate(props, strings, values);
			const compId = _hashAndAddRuntimeUSS(style);
			// const className = props.class ? `${compId} ${props.class}` : compId;
			
			const finalClass = `${compId} ${props.class || ''} ${className || ''}`;
			
			// lg(`render ${JsxTag} ${name} ${className}`, this);
			return <JsxTag
				name={name}
				{...props}
				class={finalClass}
			/>;
		};
}


export const Grow = $div('grow')`
  flex: 1 0 auto;
`;

export const Row = $div('row')`
  flex-direction: row;
`;

export const Col = $div('col')``;

export const Frag = () => <Fragment/>;


// export const $Grow = $div('grow')`
//   flex: 1 0 auto;
// `;
//
// export const $Row = $div('row')`
//   flex-direction: row;
// `;
//
// export const $Col = $div('col')``;