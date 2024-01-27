"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.E_GhostActorStatusEnum = exports.E_GhostPhaseEnum = void 0;
var E_GhostPhaseEnum;
(function (E_GhostPhaseEnum) {
    E_GhostPhaseEnum[E_GhostPhaseEnum["UNINITIALIZED"] = 0] = "UNINITIALIZED";
    E_GhostPhaseEnum[E_GhostPhaseEnum["WAITING_TO_START"] = 1] = "WAITING_TO_START";
    E_GhostPhaseEnum[E_GhostPhaseEnum["ROUND_INTRO"] = 2] = "ROUND_INTRO";
    E_GhostPhaseEnum[E_GhostPhaseEnum["PLAYING_ROOM"] = 3] = "PLAYING_ROOM";
    E_GhostPhaseEnum[E_GhostPhaseEnum["ROOM_SUMMARY"] = 4] = "ROOM_SUMMARY";
    E_GhostPhaseEnum[E_GhostPhaseEnum["GAME_COMPLETE"] = 5] = "GAME_COMPLETE";
})(E_GhostPhaseEnum = exports.E_GhostPhaseEnum || (exports.E_GhostPhaseEnum = {}));
var E_GhostActorStatusEnum;
(function (E_GhostActorStatusEnum) {
    E_GhostActorStatusEnum[E_GhostActorStatusEnum["UNSET"] = 0] = "UNSET";
    E_GhostActorStatusEnum[E_GhostActorStatusEnum["READY"] = 1] = "READY";
})(E_GhostActorStatusEnum = exports.E_GhostActorStatusEnum || (exports.E_GhostActorStatusEnum = {}));
