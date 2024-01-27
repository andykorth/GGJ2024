import {ScriptableObject} from 'UnityEngine';
import {CoreUiClip, GameSysClip} from './types/foundational';

export const Clips = require('FutzInterop') as FutzInterop;

export class FutzInterop extends ScriptableObject {
	GameSys_: GameSysClip;
	CoreUi_: CoreUiClip;
}