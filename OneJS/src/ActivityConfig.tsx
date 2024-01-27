import {GhostView} from './views/ghost/GhostView';
import {h} from 'preact';

export const ACTIVITY_VIEWS = {
	// ghost: BewilderView,
	// golf: GolfView,
	// stickler: SticklerView,
	ghost: GhostView,
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
// Register('ghost', BewilderView);


