import {T_BewilderActivity, T_BewilderActor, T_BewilderCard} from './BewilderActivity';
import {useTrack} from '../../util/Track';
import {h} from 'preact';
import {$div, $label, Row} from '../../util/$tyle';
import {uss} from 'onejs/styled';
import {useRegistryLup} from '../../util/Registry';
import {$map} from '../../util/buckle/$array';

export function BewilderCard(props) {
	const card: T_BewilderCard = props.card;
	const act: T_BewilderActivity = props.act;
	
	const isRevealed = useTrack(card.IsRevealed);
	const word = useTrack(card.Word);
	const isCorrect = useTrack(card.IsCorrect);
	
	return (
		<C_Cell>
			<C_Card
				isRevealed={isRevealed}
				isCorrect={isCorrect}
			>
				<L_Word text={word}/>
				<CardPickedList card={card} act={act}/>
			</C_Card>
		</C_Cell>
	);
}

const width = 100 / 3;
const height = 100 / 3;

const C_Cell = $div('cell')`
  width: ${width}%;
  height: ${height}%;
`;

const C_Card = $div<{
	isRevealed: boolean,
	isCorrect: boolean,
}>('card')`
  flex: 1 0 auto;
  justify-content: center;
  align-items: center;
  background-color: #f1f1f1;
  color: #000;
  border-radius: 32px;
  margin: 16px;

  translate: 200px 1000px;
  transition-property: all;
  transition-delay: 0s;
  transition-duration: .5s;
  transition-timing-function: ease-in-out;

  ${props => props.isRevealed && uss`
    translate: 0 0;
  `}
  
  ${props => props.isCorrect && uss`
    background-color: #88f16a;
  `}
`;

const L_Word = $label('word')`
  font-size: 48px;
`;

function CardPickedList(props) {
	const card: T_BewilderCard = props.card;
	const act: T_BewilderActivity = props.act;
	const actorLup = useRegistryLup(act.Actors, 'SlotId')
	
	const slotIds = useTrack(card.PickedByActorSlotIds);
	const showCount = useTrack(card.ShowCount);
	
	// OPTIMIZE
	const showActors = [];
	
	for (let i = 0; i < showCount; i++) {
		const slotId = slotIds[i];
		const actor = actorLup[slotId];
		showActors.push(actor);
	}
	
	return (
		<Row>
			{$map(showActors, (actor, i) => (
				<ActorPick
					key={actor.EntityId}
					actor={actor}
				/>
			))}
		</Row>
	)
}

function ActorPick(props) {
	const actor: T_BewilderActor = props.actor;
	
	return (
		<C_ActorPick>
			<V_Color style={{backgroundColor: actor.Color}}/>
			<L_PickedList text={actor.Nickname}/>
		</C_ActorPick>
	)
}

const C_ActorPick = $div('C_ActorPick')`
  flex-direction: row;
  flex-wrap: wrap;
  margin-right: 4px;
`

const V_Color = $div('Color')`
  background-color: rgb(255, 0, 183);
  width: 18px;
  margin-top: 4px;
  margin-bottom: 4px;
  margin-right: 2px;
`;

const L_PickedList = $label('L_PickedList')`
  font-size: 18px;
  color: #444444;
`
