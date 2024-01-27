"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.E_BewilderActorStatusEnum = exports.E_BewilderPhaseEnum = void 0;
var E_BewilderPhaseEnum;
(function (E_BewilderPhaseEnum) {
    E_BewilderPhaseEnum[E_BewilderPhaseEnum["UNINITIALIZED"] = 0] = "UNINITIALIZED";
    E_BewilderPhaseEnum[E_BewilderPhaseEnum["WAITING_TO_START"] = 1] = "WAITING_TO_START";
    E_BewilderPhaseEnum[E_BewilderPhaseEnum["ROUND_INTRO"] = 2] = "ROUND_INTRO";
    E_BewilderPhaseEnum[E_BewilderPhaseEnum["WRITING_CLUES"] = 3] = "WRITING_CLUES";
    E_BewilderPhaseEnum[E_BewilderPhaseEnum["GUESSING"] = 4] = "GUESSING";
    E_BewilderPhaseEnum[E_BewilderPhaseEnum["ROUND_SUMMARY"] = 5] = "ROUND_SUMMARY";
    E_BewilderPhaseEnum[E_BewilderPhaseEnum["GAME_COMPLETE"] = 6] = "GAME_COMPLETE";
})(E_BewilderPhaseEnum = exports.E_BewilderPhaseEnum || (exports.E_BewilderPhaseEnum = {}));
var E_BewilderActorStatusEnum;
(function (E_BewilderActorStatusEnum) {
    E_BewilderActorStatusEnum[E_BewilderActorStatusEnum["UNSET"] = 0] = "UNSET";
    E_BewilderActorStatusEnum[E_BewilderActorStatusEnum["OBSERVING"] = 1] = "OBSERVING";
    E_BewilderActorStatusEnum[E_BewilderActorStatusEnum["READY"] = 2] = "READY";
    E_BewilderActorStatusEnum[E_BewilderActorStatusEnum["WRITING_CLUE"] = 3] = "WRITING_CLUE";
    E_BewilderActorStatusEnum[E_BewilderActorStatusEnum["SUBMITTED_CLUE"] = 4] = "SUBMITTED_CLUE";
    E_BewilderActorStatusEnum[E_BewilderActorStatusEnum["SUBMITTED_GUESS"] = 5] = "SUBMITTED_GUESS";
})(E_BewilderActorStatusEnum = exports.E_BewilderActorStatusEnum || (exports.E_BewilderActorStatusEnum = {}));
