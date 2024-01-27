import {h} from 'preact';
import {$button} from './$tyle';
import {Style} from 'preact/jsx';

export type P_Btn = {
	on: () => void;
	text?: string;
	disabled?: boolean;
	style?: Style;
	
	
	mar?: number | number[];
	pad?: number | number[];
	fontSize?: number | string;
}

export function Btn(props: P_Btn) {
	return (
		<B_Btn
			onClick={props.on}
			text={props.text}
			disabled={props.disabled}
			style={{
				margin: props.mar || 0,
				padding: props.pad || [8, 16, 8, 16],
				fontSize: props.fontSize || 24,
				...props.style,
			}}
		/>
	);
}

const B_Btn = $button('B_Btn')`
  background-color: #E7DCBA;
  color: #0a0a0a;
  -unity-font-style: bold;
  -unity-text-align: middle-center;

  box-shadow: 0 1px 5px 0 rgba(0, 0, 0, 0.2),
  0 2px 2px 0 rgba(0, 0, 0, 0.14),
  0 3px 1px -2px rgba(0, 0, 0, 0.12);

  &:hover {
    background-color: #c0b496;
  }

  &:active {
    background-color: #faf7ee;
  }
`;
