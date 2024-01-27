"use strict";
var __makeTemplateObject = (this && this.__makeTemplateObject) || function (cooked, raw) {
    if (Object.defineProperty) { Object.defineProperty(cooked, "raw", { value: raw }); } else { cooked.raw = raw; }
    return cooked;
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.BewilderActorList = void 0;
var Registry_1 = require("../../util/Registry");
var _array_1 = require("../../util/buckle/$array");
var _tyle_1 = require("../../util/$tyle");
var preact_1 = require("preact");
var Track_1 = require("../../util/Track");
function BewilderActorList(props) {
    var act = props.act;
    var actors = (0, Registry_1.useRegistry)(act.Actors);
    return ((0, preact_1.h)(W_ActorList, null, (0, _array_1.$map)(actors, function (actor) { return ((0, preact_1.h)(ActorRow, { key: actor.EntityId, actor: actor })); })));
}
exports.BewilderActorList = BewilderActorList;
var W_ActorList = (0, _tyle_1.$div)('ActorList')(templateObject_1 || (templateObject_1 = __makeTemplateObject(["\n  padding: 8px;\n"], ["\n  padding: 8px;\n"])));
function ActorRow(props) {
    var actor = props.actor;
    var agent = actor.Agent;
    var info = (0, Track_1.useTrack)(agent.Info);
    var status = (0, Track_1.useTrack)(agent.Status);
    var color = (0, Track_1.useTrack)(agent.Color);
    var score = (0, Track_1.useTrack)(agent.Score);
    return ((0, preact_1.h)(C_AgentRow, null,
        (0, preact_1.h)(V_Color, { style: { backgroundColor: color } }),
        (0, preact_1.h)(L_Name, { text: info.Nickname }),
        (0, preact_1.h)(L_Status, { text: status }),
        (0, preact_1.h)(L_Score, { text: "".concat(score), class: 'monospaced' })));
}
var C_AgentRow = (0, _tyle_1.$div)('C_AgentRow')(templateObject_2 || (templateObject_2 = __makeTemplateObject(["\n  flex-direction: row;\n  overflow: hidden;\n"], ["\n  flex-direction: row;\n  overflow: hidden;\n"])));
var V_Color = (0, _tyle_1.$div)('V_Color')(templateObject_3 || (templateObject_3 = __makeTemplateObject(["\n  background-color: rgb(255, 0, 183);\n  width: 18px;\n  margin-top: 4px;\n  margin-bottom: 4px;\n  margin-right: 2px;\n"], ["\n  background-color: rgb(255, 0, 183);\n  width: 18px;\n  margin-top: 4px;\n  margin-bottom: 4px;\n  margin-right: 2px;\n"])));
var L_Name = (0, _tyle_1.$label)('L_Name')(templateObject_4 || (templateObject_4 = __makeTemplateObject(["\n  overflow: hidden;\n  width: 40%;\n  color: #000;\n  font-size: 24px;\n"], ["\n  overflow: hidden;\n  width: 40%;\n  color: #000;\n  font-size: 24px;\n"])));
var L_Status = (0, _tyle_1.$label)('L_Status')(templateObject_5 || (templateObject_5 = __makeTemplateObject(["\n  font-size: 14px;\n  -unity-font-style: italic;\n  -unity-text-align: middle-center;\n  flex-shrink: 1;\n  flex-grow: 1;\n  overflow: hidden;\n"], ["\n  font-size: 14px;\n  -unity-font-style: italic;\n  -unity-text-align: middle-center;\n  flex-shrink: 1;\n  flex-grow: 1;\n  overflow: hidden;\n"])));
var L_Score = (0, _tyle_1.$label)('L_Score')(templateObject_6 || (templateObject_6 = __makeTemplateObject(["\n  width: 80px;\n  font-size: 18px;\n  -unity-font-style: bold;\n  color: #fff;\n  -unity-text-align: middle-center;\n  background-color: #000;\n"], ["\n  width: 80px;\n  font-size: 18px;\n  -unity-font-style: bold;\n  color: #fff;\n  -unity-text-align: middle-center;\n  background-color: #000;\n"])));
var templateObject_1, templateObject_2, templateObject_3, templateObject_4, templateObject_5, templateObject_6;
