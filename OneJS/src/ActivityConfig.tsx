import {BewilderView} from './views/bewilder/BewilderView';
import {h} from 'preact';
// import {GolfView} from './views/golf/GolfView';
// import {SticklerView} from './views/stickler/SticklerView';

export const ACTIVITY_VIEWS = {
	bewilder: BewilderView,
	// golf: GolfView,
	// stickler: SticklerView,
};

export type T_ActivityView = (props: P_ActivityView) => h.JSX.Element;
export type P_ActivityView = {}

// const Register = (idf: string, view: T_ActivityView) => ACTIVITIES[idf] = view;
//
// export function GetActivityView(idf: string): T_ActivityView {
// 	const view = ACTIVITIES[idf];
// 	if (!view) return () => <MissingViewDiv>missing ActivityView: {idf}</MissingViewDiv>;
// 	return view;
// }
//
// const MissingViewDiv = sty.div`
//   font-size: 48px;
//   color: white;
//   background-color: red;
//   padding: 16px;
// `;
//
//
//
// /* v v v v           ADD BELOW          v v v v  */
//
// Register('bewilder', BewilderView);


