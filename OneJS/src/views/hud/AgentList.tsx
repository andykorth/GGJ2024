import {h} from 'preact';
import {Agent} from '../../types/foundational';
// import {useRegistry} from '../../util/Registry';
import {$map} from '../../util/buckle/$array';
import {useTrack, useTrackList, useTrackToggle} from '../../util/Track';
import {lg} from '../../util/lg';
import {$div, $label, Col} from '../../util/$tyle';
import {uss} from 'onejs/styled';
import {Clips} from '../../FutzInterop';


export function AgentList() {
	const agents = useTrackList(Clips.GameSys_.Agents);
	const [showScore] = useTrackToggle(Clips.CoreUi_.ShowScore);
	
	// lg(`${gameSys.Agents.Count} agents, (reg id: ${gameSys.Agents.Id})`, this);
	
	return (
		<W_AgentList showScore={showScore}>
			<L_Title text={'Players'}/>
			
			<Col>
				{$map(agents, agent => (
					<AgentRow
						key={agent.EntityId}
						agent={agent}
						showScore={showScore}
					/>
				)) as any}
			</Col>
		
		</W_AgentList>
	);
}

const W_AgentList = $div<{
	showScore: boolean,
}>('W_AgentList')`
  position: absolute;
  left: 8px;
  bottom: 8px;
  background-color: rgb(32, 32, 32);
  padding: 8px;
  width: 220px;

  ${props => props.showScore && uss`
    width: 280px;
  `}
`;

const L_Title = $label('L_Title')`
  color: rgb(212, 212, 212);
  font-size: 24px;
  -unity-font-style: bold;
  -unity-text-align: middle-left;
  padding: 0;
  margin: 0;
  white-space: normal;
`;

export function AgentRow(props: {
	agent: Agent,
	showScore: boolean,
}) {
	const agent: Agent = props.agent;
	const showScore: boolean = props.showScore;
	
	const info = useTrack(agent.Info);
	const status = useTrack(agent.Status);
	const color = useTrack(agent.Color);
	const score = useTrack(agent.Score);
	
	// lg(`${agent} ${info} ${status} ${color}`, this);
	
	return (
		<C_AgentRow>
			{showScore && (
				<L_Score
					text={`${score}`}
					class={'monospaced'}
				/>
			)}
			<V_Color style={{backgroundColor: color}}/>
			<L_Name text={info.Nickname}/>
			<L_Status text={status}/>
		</C_AgentRow>
	);
}

const C_AgentRow = $div('C_AgentRow')`
  flex-direction: row;
  overflow: hidden;
`;

const L_Score = $label('L_Score')`
  width: 60px;
  font-size: 18px;
  color: rgb(255, 128, 2);
  -unity-text-align: middle-center;
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
  flex-shrink: 1;
  flex-grow: 1;
  color: rgb(255, 255, 255);
`;

const L_Status = $label('L_Status')`
  font-size: 12px;
  -unity-font-style: italic;
  -unity-text-align: middle-right;
  width: 20%;
  flex-shrink: 1;
  flex-grow: 1;
  color: rgb(229, 229, 229);
`;