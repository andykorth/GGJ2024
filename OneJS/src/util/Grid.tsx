import {Dom} from 'OneJS/Dom';
import {h, render} from 'preact';
import {useRef, useEffect} from 'preact/hooks';
import {Mathf} from 'UnityEngine';
import {
	Length,
	LengthUnit,
	MeshGenerationContext,
	StyleFloat,
	StyleLength,
	TextElement,
} from 'UnityEngine/UIElements';
import {MeasureMode} from 'UnityEngine/UIElements/VisualElement';


export type P_Grid = {
	columns: number,
	// w/h
	aspectRatio?: number,
}


export function FixedGrid(props: P_Grid) {
	const ref = useRef<Dom>();
	
	useEffect(() => {
		ref.current.ve.style.opacity = new StyleFloat(0);
		ref.current.ve.generateVisualContent = onGenerateVisualContent;
		ref.current.ve.MarkDirtyRepaint();
	}, []);
	
	function onGenerateVisualContent(_: MeshGenerationContext) {
		setTimeout(resize);
	}
	
	function resize() {
		const columns = props.columns;
		const aspectRatio = props.aspectRatio || 1;
		
		const contentRect = ref.current.ve.contentRect;
		const gridWidth = contentRect.width;
		const cellWidth = gridWidth / columns;
		const cellHeight = cellWidth / aspectRatio;
		
		// TODO
	}
	
	return (
		<div
			style={{}}
		>
		
		</div>
	);
}


const AutoText = (props: { text?: string }) => {
	const ref = useRef<Dom>();
	let dirtyCount = 0;
	
	useEffect(() => {
		ref.current.ve.style.opacity = new StyleFloat(0);
		ref.current.ve.generateVisualContent = onGenerateVisualContent;
		ref.current.ve.MarkDirtyRepaint();
	}, []);
	
	function onGenerateVisualContent(mgc: MeshGenerationContext) {
		dirtyCount++;
		setTimeout(resize);
	}
	
	function resize() {
		let contentRect = ref.current.ve.contentRect;
		let textElement = (
			ref.current.ve.Children() as any
		)[0] as TextElement;
		
		let textSize = textElement.MeasureTextSize(
			props.text,
			99999,
			MeasureMode.AtMost,
			99999,
			MeasureMode.AtMost,
		);
		let fontSize = Mathf.Max(ref.current.ve.style.fontSize.value.value, 1);
		let heightDictatedFontSize = Mathf.Abs(contentRect.height);
		let widthDictatedFontSize = Mathf.Abs(contentRect.width / textSize.x) * fontSize;
		let newFontSize = Mathf.FloorToInt(Mathf.Min(
			heightDictatedFontSize,
			widthDictatedFontSize,
		));
		newFontSize = Mathf.Clamp(newFontSize, 1, 9999);
		
		ref.current.ve.style.fontSize = new StyleLength(new Length(newFontSize, LengthUnit.Pixel));
		if (dirtyCount > 1) {
			ref.current.ve.style.opacity = new StyleFloat(1);
		}
		else {
			ref.current.ve.MarkDirtyRepaint();
		}
	}
	
	return <div ref={ref} class='w-full h-full'>{props.text}</div>;
};