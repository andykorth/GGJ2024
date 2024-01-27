import {h} from 'preact';
import {useTrack} from '../../util/Track';
import {$button, $div, $label, Col, Grow} from '../../util/$tyle';
import {P_GhostView} from './GhostActivity';
// import {lg, lgRender} from '../../util/lg';
import {GhostActorList} from './GhostActorList';

export function GhostView(props: P_GhostView) {
	const act = props.act;
	// lgRender(this);
	
	return (
		<W_Ghost>
			<InfoPanel act={act}/>
			{/*<CardGrid act={act}/>*/}
		</W_Ghost>
	);
}

const W_Ghost = $div('Ghost')`
  flex-direction: row;
  width: 100%;
  height: 40%;
`;

function InfoPanel(props: P_GhostView) {
	const act = props.act;
	const phaseTitle = useTrack(act.PhaseTitle);
	const phaseDesc = useTrack(act.PhaseDesc);
	// lgRender(this);
	
	return (
		<W_InfoPanel>
			<L_PhaseTitle text={phaseTitle}/>
			<L_PhaseDesc text={phaseDesc}/>
			<Grow/>
			<GhostActorList act={act}/>
		</W_InfoPanel>
	);
}

const W_InfoPanel = $div('W_InfoPanel')`
  width: 30%;
  background-color: #c1efbb;
  padding: 8px;
`;

const L_PhaseTitle = $label('L_PhaseTitle')`
  font-size: 24px;
  white-space: normal;
  -unity-text-align: middle-center;
`;

const L_PhaseDesc = $label('L_PhaseDesc')`
  font-size: 28px;
  white-space: normal;
  margin: 0 16px;
`;


// function CardGrid(props: P_GhostView) {
// 	const act = props.act;
// 	const cards = useTrackList(act.Cards);
//	
// 	return (
// 		<W_CardGrid>
// 			{$map(cards, card => (
// 				<GhostCard
// 					key={card.EntityId}
// 					card={card}
// 					act={act}
// 				/>
// 			))}
// 		</W_CardGrid>
// 	);
// }

const W_CardGrid = $div('grid')`
  flex: 1 1 auto;
  flex-direction: row;
  flex-wrap: wrap;
  background-color: #cbc9a1;
`;


// &:hover {
//   background-color: #ff0000;
// }

// function Clues(props: P_GhostView) {
// 	const act = props.act;
// 	const actors = useTrackList(act.Actors);
// 	const evtForceNextRound = useTrackEvt(act.ForceNextRound);
//	
// 	return (
// 		<W_Clues>
// 			<Col>
// 				{$map(actors, actor => (
// 					<ActorClue
// 						key={actor.EntityId}
// 						actor={actor}
// 					/>
// 				))}
// 			</Col>
//			
// 			<div style={{flexGrow: 1}}/>
//			
// 			<B_NextRound
// 				text={'Force Next Round'}
// 				onClick={evtForceNextRound}
// 			/>
// 		</W_Clues>
// 	);
// }
//
// const W_Clues = $div('Clues')`
//   min-width: 30%;
//   background-color: #c1efbb;
//   padding: 8px;
// `;
//
// const B_NextRound = $button('Next Round')``;
//
// function ActorClue(props) {
// 	const actor: T_GhostActor = props.actor;
// 	const status = useTrack(actor.Status);
// 	const clue = useTrack(actor.Clue);
//
// 	const thing = E_GhostActorStatusEnum.SUBMITTED_CLUE;
// 	lg(`status ${typeof status}: ${status}`, this);
// 	lg(`thing ${typeof thing}: ${thing}`, this);
//
//
// 	return (
// 		<C_ActorClue>
// 			<L_ActorName text={actor.Nickname}/>
// 			<L_ActorClue text={clue || '...'}/>
// 		</C_ActorClue>
// 	);
// }
//
// const C_ActorClue = $div('C_ActorClue')`
//   flex-direction: row;
// `;
//
// const L_ActorName = $label('L_ActorName')`
//   font-size: 18px;
//   width: 50%;
// `;
//
// const L_ActorClue = $label('L_ActorClue')`
//   font-size: 18px;
//   -unity-font-style: italic;
//   width: 50%;
// `;