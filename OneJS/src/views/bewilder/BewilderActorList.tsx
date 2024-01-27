import {T_BewilderActivity, T_BewilderActor} from './BewilderActivity';
// import {useRegistry} from '../../util/Registry';
import {$map} from '../../util/buckle/$array';
import {$div, $label} from '../../util/$tyle';
import {h} from 'preact';
import {Agent} from '../../types/foundational';
import {useTrack, useTrackList} from '../../util/Track';


export function BewilderActorList(props) {
	const act: T_BewilderActivity = props.act;
	const actors = useTrackList(act.Actors);
	
	return (
		<W_ActorList>
			{$map(actors, actor => (
				<ActorRow
					key={actor.EntityId}
					actor={actor}
				/>
			))}
		</W_ActorList>
	);
}

const W_ActorList = $div('ActorList')`
  padding: 8px;
`;

function ActorRow(props: {
	actor: T_BewilderActor,
}) {
	const actor = props.actor;
	const agent = actor.Agent;
	
	const info = useTrack(agent.Info);
	const status = useTrack(agent.Status);
	const color = useTrack(agent.Color);
	const score = useTrack(agent.Score);
	
	// lg(`${agent} ${info} ${status} ${color}`, this);
	
	return (
		<C_AgentRow>
			<V_Color style={{backgroundColor: color}}/>
			<L_Name text={info.Nickname}/>
			<L_Status text={status}/>
			<L_Score
				text={`${score}`}
				class={'monospaced'}
			/>
		</C_AgentRow>
	);
}

const C_AgentRow = $div('C_AgentRow')`
  flex-direction: row;
  overflow: hidden;
`;

const V_Color = $div('V_Color')`
  background-color: rgb(255, 0, 183);
  width: 18px;
  margin-top: 4px;
  margin-bottom: 4px;
  margin-right: 2px;
`;

const L_Name = $label('L_Name')`
  overflow: hidden;
  width: 40%;
  color: #000;
  font-size: 24px;
`;

const L_Status = $label('L_Status')`
  font-size: 14px;
  -unity-font-style: italic;
  -unity-text-align: middle-center;
  flex-shrink: 1;
  flex-grow: 1;
  overflow: hidden;
`;

const L_Score = $label('L_Score')`
  width: 80px;
  font-size: 18px;
  -unity-font-style: bold;
  color: #fff;
  -unity-text-align: middle-center;
  background-color: #000;
`;