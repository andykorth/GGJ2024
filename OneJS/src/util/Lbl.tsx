import {h} from 'preact';
import {$label} from './$tyle';
import {Style} from 'preact/jsx';

export type P_Lbl = {
	text: string;
	style?: Style;
	
	
	mar?: number | number[];
	pad?: number | number[];
	fontSize?: number | string;
	fontStyle?: 'Normal' | 'Bold' | 'Italic' | 'BoldAndItalic';
}

export function Lbl(props: P_Lbl) {
	let fontStyle = '';
	
	return (
		<L_Lbl
			text={props.text}
			style={{
				margin: props.mar || 0,
				padding: props.pad || 0,
				fontSize: props.fontSize || 18,
				unityFontStyleAndWeight: props.fontStyle,
				...props.style,
			}}
		/>
	);
}

const L_Lbl = $label('B_Lbl')`
  color: #000000;
`;
// -unity-font-style: bold;
// -unity-text-align: middle-center;
