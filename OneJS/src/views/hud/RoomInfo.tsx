import {useTrack} from '../../util/Track';
import {h} from 'preact';
import {GameSys_} from '../../clips';
import {$div, $label} from '../../util/$tyle';

export function RoomInfo() {
	const roomIdf = useTrack(GameSys_.RoomIdf);
	const status = useTrack(GameSys_.Status);
	
	return (
		<W_RoomInfo>
			<L_RoomIdf text={roomIdf}/>
			<L_Status text={status}/>
		</W_RoomInfo>
	);
}

const W_RoomInfo = $div('W_RoomInfo')`
  position: absolute;
  left: 8px;
  top: 8px;
  background-color: rgb(118, 200, 110);
  padding: 16px;
`;

const L_RoomIdf = $label('L_RoomIdf')`
  font-size: 64px;
  -unity-font-style: bold;
  -unity-text-outline-width: 1px;
  -unity-text-outline-color: rgb(0, 0, 0);
  color: rgb(255, 255, 255);
  -unity-text-align: middle-center;
  padding: 0;
  margin: 0;
`;

const L_Status = $label('L_Status')`
  font-size: 22px;
  color: rgb(135, 0, 0);
  -unity-text-align: middle-center;
`;