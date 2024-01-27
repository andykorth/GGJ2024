import {MonoBehaviour, Transform} from 'UnityEngine';

export class Entity {
	EntityId: int;
}

export class ICog extends MonoBehaviour {}

export class CogNative extends ICog {
	Tform: Transform;
	Entity: Entity;
	EntityId: int;
}

export class CogNet extends ICog {
	Tform: Transform;
	Entity: Entity;
	EntityId: int;
}

export class ClipNative extends CogNative {}
export class ClipNet extends CogNet {}