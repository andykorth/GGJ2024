import {h} from 'preact';
import {$label} from './$tyle';
import {Style} from 'preact/jsx';
import {GetTrackType, Track} from './Track';
import {useCallback, useEffect, useRef} from 'preact/hooks';
import {Dom} from 'OneJS/Dom';
import {TextElement} from 'UnityEngine/UIElements';

const valueKey = 'Current';
const addEventKey = 'add_EventValueChanged';
const removeEventKey = 'remove_EventValueChanged';

export type P_TrackLabel<
	TTrack extends Track<any>,
	TVal extends GetTrackType<TTrack>,
> = {
	track: TTrack,
	fnText: (val: TVal) => string,
	style?: Style;
	
	hideIfEmpty?: boolean;
	mar?: number | number[];
	pad?: number | number[];
	fontSize?: number | string;
	fontStyle?: 'Normal' | 'Bold' | 'Italic' | 'BoldAndItalic';
}

export function TrackLabel<
	TTrack extends Track<any>,
	TVal extends GetTrackType<TTrack>,
>(props: P_TrackLabel<TTrack, TVal>) {
	const {
		track,
		fnText,
	} = props;
	
	const ref = useRef<Dom>();
	
	const setText = useCallback(() => {
		const el = ref.current.ve as TextElement;
		const text = fnText(track[valueKey]);
		el.text = text;
		
		if (props.hideIfEmpty) {
			ref.current.style.display = !!text ? 'Flex' : 'None';
		}
	}, []);
	
	useEffect(() => {
		const addEvent = track[addEventKey] as Function;
		const removeEvent = track[removeEventKey] as Function;
		
		setText();
		addEvent.call(track, setText);
		
		onEngineReload(removeHandler);
		
		return () => {
			removeHandler();
			unregisterOnEngineReload(removeHandler);
		};
		
		function removeHandler() {
			removeEvent.call(track, setText);
		}
		
	}, [track]);
	
	return (
		<label
			ref={ref}
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
