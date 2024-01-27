import {h} from 'preact';
import {useTrackChoiceList, useTrackToggle} from '../../util/Track';
import {$map} from '../../util/buckle/$array';
import {lg} from '../../util/lg';
import {$button, $div, $label, $radiobutton, $radiobuttongroup, Row} from '../../util/$tyle';
import {Clips} from '../../FutzInterop';


export function ActivitySelect() {
	const [show, toggleShow] = useTrackToggle(Clips.CoreUi_.ShowActivitySelect);
	
	const [
		defs,
		currentIndex,
		currentDef,
		setIndex,
	] = useTrackChoiceList(Clips.GameSys_.ActivityChoice);
	
	// const show = false;
	// const toggleShow = () => {};
	//
	// const defs = [];
	// const currentIndex = 0;
	// const setIndex = (i) => ({});
	
	const inner = show ? (
		<G_Activities>
			{$map(defs, (def, i) => (
				<R_Act
					key={def.Idf}
					text={def.Name}
					onClick={() => setIndex(i)}
					value={i === currentIndex}
				/>
			))}
		</G_Activities>
	) : <div/>;
	
	lg(`render `, this);
	
	return (
		<W_ActivitySelect>
			<Row>
				<L_Title text={'Activity Select'}/>
				<B_Show
					text={show ? '-' : '+'}
					onClick={() => toggleShow()}
				/>
			</Row>
			{inner}
		</W_ActivitySelect>
	);
}

const W_ActivitySelect = $div('W_ActivitySelect')`
  position: absolute;
  top: 8px;
  right: 8px;
  background-color: rgb(219, 148, 173);
  padding: 8px 16px;
`;

const L_Title = $label('L_Title')``;

const B_Show = $button('B_Show')`
  background-color: unset;
  font-size: 24px;
  padding: 0;
  border: none;
  outline: none;
`;

const G_Activities = $radiobuttongroup('G_Activities')`
  flex-direction: column;
`;

const R_Act = $radiobutton('R_Act')``;