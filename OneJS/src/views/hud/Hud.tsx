import {h} from 'preact';
import {RoomInfo} from './RoomInfo';
import {ActivitySelect} from './ActivitySelect';
import {AgentList} from './AgentList';
import {Watermark} from './Watermark';
import {PickingMode} from 'UnityEngine/UIElements';
import {useTrackToggle} from '../../util/Track';
import {$div} from '../../util/$tyle';
import {Clips} from '../../FutzInterop';

export function Hud() {
	const [showAgentList] = useTrackToggle(Clips.CoreUi_.ShowAgentList);
	
	return (
		<S_Hud picking-mode={PickingMode.Ignore}>
			<RoomInfo/>
			{showAgentList && <AgentList/>}
			<Watermark/>
		</S_Hud>
	);
}
// <ActivitySelect/>

const S_Hud = $div('hud')`
  position: absolute;
  top: 0;
  right: 0;
  bottom: 0;
  left: 0;
`;
//top: 0;
//right: 0;
//bottom: 0;
//left: 0;


// <ui:VisualElement
// 	name='W_ActivitySelect'
// 	style='position: absolute; top: 8px; right: 8px; background-color: rgb(219, 148, 173); padding-left: 16px; padding-right: 16px; padding-top: 16px; padding-bottom: 16px;'
// >
// 	<ui:Label text='Activity Select' name='L_Title' className='L_Title'/>
// 	<ui:RadioButtonGroup
// 		value='-1'
// 		name='G_Activities'
// 		choices='Moderately Fun Activity, This One Sucks'
// 		style='flex-direction: column;'
// 	/>
// </ui:VisualElement>;
// <ui:VisualElement name='W_AgentList' className='W_AgentList'>
// 	<ui:Label text='Players' name='L_Title' className='L_Title'/>
// 	<ui:Instance template='C_AgentStatus' name='C_AgentStatus'/>
// </ui:VisualElement>;
// <ui:VisualElement name='W_Watermark' style='position: absolute; right: 8px; bottom: 8px;'>
// 	<ui:Label
// 		text='Some Build Name 20??.0529.1619'
// 		name='L_Version'
// 		style='font-size: 14px; -unity-font-style: bold; color: rgba(255, 255, 255, 0.2); -unity-text-align: middle-center; padding-left: 0; padding-right: 0; padding-top: 0; padding-bottom: 0; margin-left: 0; margin-right: 0; margin-top: 0; margin-bottom: 0; -unity-font-definition: url(&apos;project://database/Assets/_art/UI/UI_Fonts/Roboto_Mono/RobotoMono-VariableFont_wght%20SDF.asset?fileID=11400000&amp;guid=5eee9c10281351944afa19b32e923af9&amp;type=2#RobotoMono-VariableFont_wght SDF&apos;);'
// 	/>
// </ui:VisualElement>;

// <ui:VisualElement name="C_AgentStatus" className="C_AgentStatus">
// 	<ui:VisualElement name="V_Color" className="V_Color"/>
// 	<ui:Label text="whatAl0ng@$$name12345678" name="L_Name" className="L_Name"/>
// 	<ui:Label text="is typing way too much" name="L_Status" className="L_Status"/>
// </ui:VisualElement>