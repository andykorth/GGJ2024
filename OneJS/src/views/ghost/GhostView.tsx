import {h} from 'preact';
import {useTrack} from '../../util/Track';
import {$div, $label, Grow} from '../../util/$tyle';
import {E_GhostPhaseEnum, P_GhostView} from './GhostActivity';
// import {lg, lgRender} from '../../util/lg';
import {GhostActorList} from './GhostActorList';
import {TrackLabel} from '../../util/TrackLabel';

export function GhostView(props: P_GhostView) {
	const act = props.act;
	// lgRender(this);
	
	const phase = useTrack(act.Phase);
	
	// return <EndScreen act={act}/>;
	if (phase == E_GhostPhaseEnum.GAME_COMPLETE) {
		return <EndScreen act={act}/>;
	}
	
	return (
		<W_Ghost>
			<InfoPanel act={act}/>
			{/*<CardGrid act={act}/>*/}
			<Timer act={act}/>
		</W_Ghost>
	);
}

const W_Ghost = $div('Ghost')`
	width: 100%;
	height: 100%;
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
	position: absolute;
	left: 0;
	right: 0;
	top: 0;
	bottom: 0;
	padding: 8px;
`;

const L_PhaseTitle = $label('L_PhaseTitle')`
	font-size: 24px;
	white-space: normal;
	margin-left: 400px;
`;

const L_PhaseDesc = $label('L_PhaseDesc')`
	font-size: 28px;
	white-space: normal;
	margin: 0 16px;
`;

function Timer(props: P_GhostView) {
	const act = props.act;
	
	
	return (
		<W_Timer>
			<TrackLabel
				track={act.TimerString}
				fnText={s => s}
				fontSize={64}
				mono
			/>
		</W_Timer>
	);
}

const W_Timer = $div('W_Timer')`
	position: absolute;
	right: 32px;
	top: 32px;
	-unity-font-style: bold;
	color: rgb(255, 255, 255);
	-unity-text-align: middle-center;
	margin: 0;
	background-color: rgb(0, 0, 0);
	padding: 16px;
`;


function EndScreen(props: P_GhostView) {
	const act = props.act;
	
	const rescued = useTrack(act.GhostsRescued);
	
	const successfullyEscaped = act.SuccessfullyEscaped;
	
	const text = successfullyEscaped
		? `You escaped! You rescued ${rescued} ghosts.`
		: `You (and ${rescued} ghosts) were consumed by the darkness.`;
	
	
	return (
		<W_EndScreen>
			<L_End
				text={text}
			/>
		</W_EndScreen>
	);
}

const W_EndScreen = $div('W_EndScreen')`
	position: absolute;
	left: 200px;
	right: 32px;
	top: 32px;
	bottom: 32px;
	color: rgb(255, 255, 255);
	-unity-text-align: middle-center;
	margin: 0;
	background-color: rgb(0, 0, 0);
	padding: 16px;
	justify-content: center;
`;

const L_End = $label('L_End = $label')`
	font-size: 64px;
	white-space: normal;
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

// const W_CardGrid = $div('grid')`
// 	flex: 1 1 auto;
// 	flex-direction: row;
// 	flex-wrap: wrap;
// 	background-color: #cbc9a1;
// `;


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