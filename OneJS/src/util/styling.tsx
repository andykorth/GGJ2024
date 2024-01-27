// import {h} from 'preact';
// import {JSXInternal} from 'preact/jsx';
// import generateComponentId from 'onejs/styled/utils/generateComponentId';
// import flatten from 'css-flatten';
//
//
// /*
// Settings -> Languages & Frameworks -> JavaScript -> Styled Components
//
// .idea\misc.xml
//
//  <option value="sty.div" />
//
// module:preact/jsx.JSXInternal.IntrinsicElements
// ScriptLib/definitions/jsx.d.ts
//
//
// attrs = sets props
//
//
// */
//
// /* copied from:  ScriptLib/onejs/styled/index.tsx */
// function _hashAndAddRuntimeUSS(style: string) {
// 	const compId = generateComponentId(style);
// 	style = flatten(`.${compId} {${style}}`);
// 	document.addRuntimeUSS(style);
// 	return compId;
// }
//
// function _processTemplate(props, strings: TemplateStringsArray, values: any[]) {
// 	const style = values.reduce((result, expr, index) => {
// 		let value = typeof expr === 'function' ? expr(props) : expr;
//
// 		if (typeof value === 'function') value = value(props);
// 		if (!value) value = '';
//
// 		return `${result}${value}${strings[index + 1]}`;
// 	}, strings[0]);
// 	return style as string;
// }
//
// function styling(Tag: string | ((props?) => h.JSX.Element)) {
// 	const tag = function (strings: TemplateStringsArray, ...values) {
// 		return (props: any) => {
// 			const style = _processTemplate(props, strings, values);
// 			const compId = _hashAndAddRuntimeUSS(style);
// 			const className = props.class ? `${compId} ${props.class}` : compId;
//
// 			return <Tag {...props} class={className}></Tag>;
// 		};
// 	};
//
// 	tag.attrs = (func: (props: any) => ({})) => {
// 		return function (strings: TemplateStringsArray, ...values) {
// 			return (props) => {
// 				const defaultProps = func(props);
// 				const condensedProps = Object.assign({}, defaultProps, props);
// 				const style = _processTemplate(condensedProps, strings, values);
// 				const compId = _hashAndAddRuntimeUSS(style);
// 				const className = props.class ? `${compId} ${props.class}` : compId;
//
// 				return <Tag {...condensedProps} class={className}></Tag>;
// 			};
// 		};
// 	};
//
// 	tag.Name = (name: string) => {
// 		return function (strings: TemplateStringsArray, ...values) {
// 			return (props) => {
// 				const style = _processTemplate(props, strings, values);
// 				const compId = _hashAndAddRuntimeUSS(style);
// 				const className = props.class ? `${compId} ${props.class}` : compId;
//
// 				return <Tag name={name} {...props} class={className}></Tag>;
// 			};
// 		};
// 	};
//
// 	return tag;
// };
//
//
// type Vel = JSXInternal.VisualElement;
//
// type StyEl_OLD<TEl extends Vel> = (
// 	strings: TemplateStringsArray,
// 	...values: any[]
// ) => (props: TEl) => h.JSX.Element;
//
// type StyEl<TEl extends Vel> = {
// 	(strings: TemplateStringsArray, ...values: any[]): (props: TEl) => h.JSX.Element,
// 	// attrs
// 	// Name: (name: string) => (
// 	// 	strings: TemplateStringsArray,
// 	// 	...values: any[]
// 	// ) => (props: TEl) => h.JSX.Element,
// 	Name: FnName<TEl>,
// }
//
// type FnNameProps<TProps = void> = TProps extends void
// 	? Vel
// 	: Vel & TProps;
//
// type FnName<TEl> = (name: string) => (
// 	strings: TemplateStringsArray,
// 	...values: any[]
// ) => (props: TEl) => h.JSX.Element;
//
// type T_Label = {
// 	text: string,
// 	enableRichText?: boolean,
// } & Vel;
//
// type T_Button = {
// 	text: string,
// 	onClick: () => void,
// } & Vel;
//
// type T_RadioButtonGroup = {
// 	// choices?: string[],
// } & Vel;
//
// type T_RadioButton = {
// 	text?: string,
// 	value?: boolean,
// } & Vel
//
// type T_Foldout = {
// 	text?: string,
// 	value?: boolean,
// } & Vel
//
// // TODO: other element props?
//
// class Styler {
// 	div: StyEl<Vel> = styling('div');
// 	box: StyEl<Vel> = styling('box');
// 	textelement: StyEl<Vel> = styling('textelement');
// 	label: StyEl<T_Label> = styling('label');
// 	image: StyEl<Vel> = styling('image');
// 	foldout: StyEl<T_Foldout> = styling('foldout');
// 	template: StyEl<Vel> = styling('template');
// 	instance: StyEl<Vel> = styling('instance');
// 	templatecontainer: StyEl<Vel> = styling('templatecontainer');
// 	button: StyEl<T_Button> = styling('button');
// 	radiobutton: StyEl<T_RadioButton> = styling('radiobutton');
// 	radiobuttongroup: StyEl<T_RadioButtonGroup> = styling('radiobuttongroup');
// 	repeatbutton: StyEl<Vel> = styling('repeatbutton');
// 	toggle: StyEl<Vel> = styling('toggle');
// 	scroller: StyEl<Vel> = styling('scroller');
// 	slider: StyEl<Vel> = styling('slider');
// 	sliderint: StyEl<Vel> = styling('sliderint');
// 	minmaxslider: StyEl<Vel> = styling('minmaxslider');
// 	enumfield: StyEl<Vel> = styling('enumfield');
// 	progressbar: StyEl<Vel> = styling('progressbar');
// 	textfield: StyEl<Vel> = styling('textfield');
// 	integerfield: StyEl<Vel> = styling('integerfield');
// 	floatfield: StyEl<Vel> = styling('floatfield');
// 	vector2field: StyEl<Vel> = styling('vector2field');
// 	vector2intfield: StyEl<Vel> = styling('vector2intfield');
// 	vector3field: StyEl<Vel> = styling('vector3field');
// 	vector3intfield: StyEl<Vel> = styling('vector3intfield');
// 	vector4field: StyEl<Vel> = styling('vector4field');
// 	rectfield: StyEl<Vel> = styling('rectfield');
// 	rectintfield: StyEl<Vel> = styling('rectintfield');
// 	boundsfield: StyEl<Vel> = styling('boundsfield');
// 	boundsintfield: StyEl<Vel> = styling('boundsintfield');
// 	listview: StyEl<Vel> = styling('listview');
// 	scrollview: StyEl<Vel> = styling('scrollview');
// 	treeview: StyEl<Vel> = styling('treeview');
// 	popupwindow: StyEl<Vel> = styling('popupwindow');
// 	dropdownfield: StyEl<Vel> = styling('dropdownfield');
// 	gradientrect: StyEl<Vel> = styling('gradientrect');
// 	flipbook: StyEl<Vel> = styling('flipbook');
// 	simplelistview: StyEl<Vel> = styling('simplelistview');
// }
//
// export const sty = new Styler();
//
//
//
// export const Grow = sty.div.Name('grow')`
//   flex: 1 0 auto;
// `;
//
// export const Row = sty.div.Name('row')`
//   flex-direction: row;
// `;
//
// export const Col = sty.div.Name('col')``;
//
//
//
// /*
//
//             div: VisualElement
//             box: Box
//             textelement: TextElement
//             label: Label
//             image: Image
//             foldout: Foldout
//             template: Template
//             instance: Instance
//             templatecontainer: TemplateContainer
//             button: Button
//             radiobutton: RadioButton
//             radiobuttongroup: RadioButtonGroup
//             repeatbutton: RepeatButton
//             toggle: Toggle
//             scroller: Scroller
//             slider: Slider
//             sliderint: SliderInt
//             minmaxslider: MinMaxSlider
//             enumfield: EnumField
//             progressbar: ProgressBar
//             textfield: TextField
//             integerfield: IntegerField
//             floatfield: FloatField
//             vector2field: Vector2Field
//             vector2intfield: Vector2IntField
//             vector3field: Vector3Field
//             vector3intfield: Vector3IntField
//             vector4field: Vector4Field
//             rectfield: RectField
//             rectintfield: RectIntField
//             boundsfield: BoundsField
//             boundsintfield: BoundsIntField
//             listview: ListView
//             scrollview: ScrollView
//             treeview: TreeView
//             popupwindow: PopupWindow
//             dropdownfield: DropdownField
// 			gradientrect: GradientRect
// 			flipbook: Flipbook
// 			simplelistview: SimpleListView
//  */
// // type Els = JSXInternal.IntrinsicElements;
//
// // labelMono: StyEl<T_Label> = styled('label')
// // 	.attrs(() => ({style: {unityFontDefinition: Monospaced}}));
// // lbl = (className?: string) => {
// // 	const fn = (
// // 		strings: TemplateStringsArray,
// // 		...values: any[]
// // 	) => {
// // 		const Jsx = styled('label')``;
// // 	};
// // };