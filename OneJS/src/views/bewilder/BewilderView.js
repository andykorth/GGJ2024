"use strict";
var __makeTemplateObject = (this && this.__makeTemplateObject) || function (cooked, raw) {
    if (Object.defineProperty) { Object.defineProperty(cooked, "raw", { value: raw }); } else { cooked.raw = raw; }
    return cooked;
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.BewilderView = void 0;
var preact_1 = require("preact");
var Registry_1 = require("../../util/Registry");
var Track_1 = require("../../util/Track");
var _array_1 = require("../../util/buckle/$array");
var _tyle_1 = require("../../util/$tyle");
var BewilderActivity_1 = require("./BewilderActivity");
var lg_1 = require("../../util/lg");
var BewilderActorList_1 = require("./BewilderActorList");
var BewilderCard_1 = require("./BewilderCard");
function BewilderView(props) {
    var act = props.act;
    return ((0, preact_1.h)(W_Bewilder, null,
        (0, preact_1.h)(InfoPanel, { act: act }),
        (0, preact_1.h)(CardGrid, { act: act })));
}
exports.BewilderView = BewilderView;
var W_Bewilder = (0, _tyle_1.$div)('Bewilder')(templateObject_1 || (templateObject_1 = __makeTemplateObject(["\n  flex-direction: row;\n  width: 100%;\n  height: 100%;\n"], ["\n  flex-direction: row;\n  width: 100%;\n  height: 100%;\n"])));
function InfoPanel(props) {
    var act = props.act;
    var phaseTitle = (0, Track_1.useTrack)(act.PhaseTitle);
    var phaseDesc = (0, Track_1.useTrack)(act.PhaseDesc);
    return ((0, preact_1.h)(W_InfoPanel, null,
        (0, preact_1.h)(L_PhaseTitle, { text: phaseTitle }),
        (0, preact_1.h)(L_PhaseDesc, { text: phaseDesc }),
        (0, preact_1.h)(_tyle_1.Grow, null),
        (0, preact_1.h)(BewilderActorList_1.BewilderActorList, { act: act })));
}
var W_InfoPanel = (0, _tyle_1.$div)('W_InfoPanel')(templateObject_2 || (templateObject_2 = __makeTemplateObject(["\n  width: 30%;\n  background-color: #c1efbb;\n  padding: 8px;\n"], ["\n  width: 30%;\n  background-color: #c1efbb;\n  padding: 8px;\n"])));
var L_PhaseTitle = (0, _tyle_1.$label)('L_PhaseTitle')(templateObject_3 || (templateObject_3 = __makeTemplateObject(["\n  font-size: 48px;\n  white-space: normal;\n  -unity-text-align: middle-center;\n"], ["\n  font-size: 48px;\n  white-space: normal;\n  -unity-text-align: middle-center;\n"])));
var L_PhaseDesc = (0, _tyle_1.$label)('L_PhaseDesc')(templateObject_4 || (templateObject_4 = __makeTemplateObject(["\n  font-size: 28px;\n  white-space: normal;\n  margin: 0 16px;\n"], ["\n  font-size: 28px;\n  white-space: normal;\n  margin: 0 16px;\n"])));
function CardGrid(props) {
    var act = props.act;
    var cards = (0, Registry_1.useRegistry)(act.Cards);
    return ((0, preact_1.h)(W_CardGrid, null, (0, _array_1.$map)(cards, function (card) { return ((0, preact_1.h)(BewilderCard_1.BewilderCard, { key: card.EntityId, card: card, act: act })); })));
}
var W_CardGrid = (0, _tyle_1.$div)('grid')(templateObject_5 || (templateObject_5 = __makeTemplateObject(["\n  flex: 1 1 auto;\n  flex-direction: row;\n  flex-wrap: wrap;\n  background-color: #cbc9a1;\n"], ["\n  flex: 1 1 auto;\n  flex-direction: row;\n  flex-wrap: wrap;\n  background-color: #cbc9a1;\n"])));
function Clues(props) {
    var act = props.act;
    var actors = (0, Registry_1.useRegistry)(act.Actors);
    var evtForceNextRound = (0, Track_1.useTrackEvt)(act.ForceNextRound);
    return ((0, preact_1.h)(W_Clues, null,
        (0, preact_1.h)(_tyle_1.Col, null, (0, _array_1.$map)(actors, function (actor) { return ((0, preact_1.h)(ActorClue, { key: actor.EntityId, actor: actor })); })),
        (0, preact_1.h)("div", { style: { flexGrow: 1 } }),
        (0, preact_1.h)(B_NextRound, { text: 'Force Next Round', onClick: evtForceNextRound })));
}
var W_Clues = (0, _tyle_1.$div)('Clues')(templateObject_6 || (templateObject_6 = __makeTemplateObject(["\n  min-width: 30%;\n  background-color: #c1efbb;\n  padding: 8px;\n"], ["\n  min-width: 30%;\n  background-color: #c1efbb;\n  padding: 8px;\n"])));
var B_NextRound = (0, _tyle_1.$button)('Next Round')(templateObject_7 || (templateObject_7 = __makeTemplateObject([""], [""])));
function ActorClue(props) {
    var actor = props.actor;
    var status = (0, Track_1.useTrack)(actor.Status);
    var clue = (0, Track_1.useTrack)(actor.Clue);
    var thing = BewilderActivity_1.E_BewilderActorStatusEnum.SUBMITTED_CLUE;
    (0, lg_1.lg)("status ".concat(typeof status, ": ").concat(status), this);
    (0, lg_1.lg)("thing ".concat(typeof thing, ": ").concat(thing), this);
    return ((0, preact_1.h)(C_ActorClue, null,
        (0, preact_1.h)(L_ActorName, { text: actor.Nickname }),
        (0, preact_1.h)(L_ActorClue, { text: clue || '...' })));
}
var C_ActorClue = (0, _tyle_1.$div)('C_ActorClue')(templateObject_8 || (templateObject_8 = __makeTemplateObject(["\n  flex-direction: row;\n"], ["\n  flex-direction: row;\n"])));
var L_ActorName = (0, _tyle_1.$label)('L_ActorName')(templateObject_9 || (templateObject_9 = __makeTemplateObject(["\n  font-size: 18px;\n  width: 50%;\n"], ["\n  font-size: 18px;\n  width: 50%;\n"])));
var L_ActorClue = (0, _tyle_1.$label)('L_ActorClue')(templateObject_10 || (templateObject_10 = __makeTemplateObject(["\n  font-size: 18px;\n  -unity-font-style: italic;\n  width: 50%;\n"], ["\n  font-size: 18px;\n  -unity-font-style: italic;\n  width: 50%;\n"])));
var templateObject_1, templateObject_2, templateObject_3, templateObject_4, templateObject_5, templateObject_6, templateObject_7, templateObject_8, templateObject_9, templateObject_10;
