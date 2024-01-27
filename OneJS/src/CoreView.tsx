import {h} from 'preact';
import {lg} from './util/lg';
import {Hud} from './views/hud/Hud';
import {useTrack} from './util/Track';
import {ACTIVITY_VIEWS} from './ActivityConfig';
import {GameSys_} from './clips';
import {$div} from './util/$tyle';


export const CoreView = () => {
	lg(`<color=#E6FF00>----------- render CoreView</color>`, this);
	
	
	return (
		<CoreDiv>
			<ActivityView/>
			<Hud/>
		</CoreDiv>
	);
};

const CoreDiv = $div('CoreDiv')`
  width: 100%;
  height: 100%;
`;

function ActivityView() {
	const activity = useTrack(GameSys_.CurrentActivity);
	if (!activity) return <div/>; //>> no activity loaded
	
	const idf = activity.Idf;
	const View = ACTIVITY_VIEWS[idf];
	
	if (!View) return <MissingActivity>missing ActivityView: {idf}</MissingActivity>;
	
	return (
		<ActivityDiv>
			<View act={activity}/>
		</ActivityDiv>
	);
}

const ActivityDiv = $div('ActivityDiv')`
  flex: 1 1 auto;
  margin: 10%;
`;

const MissingActivity = $div('MissingActivity')`
  flex: 1 1 auto;
  margin: 10%;
  font-size: 48px;
  color: white;
  background-color: red;
  padding: 32px;
`;