import {P_SticklerView} from './SticklerActivity';
import {$div} from '../../util/$tyle';
import {h} from 'preact';

export function SticklerView(props: P_SticklerView) {
	const act = props.act;
	
	return (
		<W_Stickler>
			stickler TODO
		</W_Stickler>
	);
}

const W_Stickler = $div('Stickler')`
  flex-direction: row;
  width: 100%;
  height: 100%;
`;