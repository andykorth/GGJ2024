import {h} from 'preact';
import {lg} from './util/lg';
import {Hud} from './views/hud/Hud';
import {useTrack} from './util/Track';
import {ACTIVITY_VIEWS} from './ActivityConfig';
import {$div} from './util/$tyle';
import {GhostView} from './views/ghost/GhostView';
import {Clips} from './FutzInterop';


export const CoreView = () => {
	lg(`<color=#E6FF00>----------- render CoreView</color>`, this);
	
	
	return (
		<CoreDiv>
			<GhostViewWrapperTemp/>
			{/*<ActivityView/>*/}
			<Hud/>
		</CoreDiv>
	);
};

const CoreDiv = $div('CoreDiv')`
	width: 100%;
	height: 100%;
`;

function GhostViewWrapperTemp() {
	const act = useTrack(Clips.GameSys_.GhostAct);
	
	if (!act) {
		lg(`no ghost act`);
		return <div/>;
	}
	
	
	lg(`found ghost act`);
	
	return (
		
		<GhostView act={act}/>
	)
}


function ActivityView() {
	lg(`ActivityView ${Clips.GameSys_}, ${Clips.GameSys_.CurrentActivity}`);
	
	// if (!GameSys_) {
	// 	lg(`no GameSys clip`);
	// 	return <div/>;
	// }
	
	if (!Clips.GameSys_.CurrentActivity) {
		lg(`no CurrentActivity`);
		return <div/>;
	}
	
	const activity = useTrack(Clips.GameSys_.CurrentActivity);
	lg(`ActivityView2`);
	if (!activity) return <div/>; //>> no activity loaded
	
	const idf = activity.Idf;
	const View = ACTIVITY_VIEWS[idf];
	
	lg(`ActivityView ${idf}`);
	
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