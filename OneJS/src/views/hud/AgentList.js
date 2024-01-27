"use strict";
var __makeTemplateObject = (this && this.__makeTemplateObject) || function (cooked, raw) {
    if (Object.defineProperty) { Object.defineProperty(cooked, "raw", { value: raw }); } else { cooked.raw = raw; }
    return cooked;
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.AgentRow = exports.AgentList = void 0;
var preact_1 = require("preact");
var Registry_1 = require("../../util/Registry");
var _array_1 = require("../../util/buckle/$array");
var Track_1 = require("../../util/Track");
var clips_1 = require("../../clips");
var _tyle_1 = require("../../util/$tyle");
var styled_1 = require("onejs/styled");
function AgentList() {
    var agents = (0, Registry_1.useRegistry)(clips_1.GameSys_.Agents);
    var showScore = (0, Track_1.useTrackToggle)(clips_1.CoreUi_.ShowScore)[0];
    return ((0, preact_1.h)(W_AgentList, { showScore: showScore },
        (0, preact_1.h)(L_Title, { text: 'Players' }),
        (0, preact_1.h)(_tyle_1.Col, null, (0, _array_1.$map)(agents, function (agent) { return ((0, preact_1.h)(AgentRow, { key: agent.EntityId, agent: agent, showScore: showScore })); }))));
}
exports.AgentList = AgentList;
var W_AgentList = (0, _tyle_1.$div)('W_AgentList')(templateObject_2 || (templateObject_2 = __makeTemplateObject(["\n  position: absolute;\n  left: 8px;\n  bottom: 8px;\n  background-color: rgb(32, 32, 32);\n  padding: 8px;\n  width: 220px;\n\n  ", "\n"], ["\n  position: absolute;\n  left: 8px;\n  bottom: 8px;\n  background-color: rgb(32, 32, 32);\n  padding: 8px;\n  width: 220px;\n\n  ", "\n"])), function (props) { return props.showScore && (0, styled_1.uss)(templateObject_1 || (templateObject_1 = __makeTemplateObject(["\n    width: 280px;\n  "], ["\n    width: 280px;\n  "]))); });
var L_Title = (0, _tyle_1.$label)('L_Title')(templateObject_3 || (templateObject_3 = __makeTemplateObject(["\n  color: rgb(212, 212, 212);\n  font-size: 24px;\n  -unity-font-style: bold;\n  -unity-text-align: middle-left;\n  padding: 0;\n  margin: 0;\n  white-space: normal;\n"], ["\n  color: rgb(212, 212, 212);\n  font-size: 24px;\n  -unity-font-style: bold;\n  -unity-text-align: middle-left;\n  padding: 0;\n  margin: 0;\n  white-space: normal;\n"])));
function AgentRow(props) {
    var agent = props.agent;
    var showScore = props.showScore;
    var info = (0, Track_1.useTrack)(agent.Info);
    var status = (0, Track_1.useTrack)(agent.Status);
    var color = (0, Track_1.useTrack)(agent.Color);
    var score = (0, Track_1.useTrack)(agent.Score);
    return ((0, preact_1.h)(C_AgentRow, null,
        showScore && ((0, preact_1.h)(L_Score, { text: "".concat(score), class: 'monospaced' })),
        (0, preact_1.h)(V_Color, { style: { backgroundColor: color } }),
        (0, preact_1.h)(L_Name, { text: info.Nickname }),
        (0, preact_1.h)(L_Status, { text: status })));
}
exports.AgentRow = AgentRow;
var C_AgentRow = (0, _tyle_1.$div)('C_AgentRow')(templateObject_4 || (templateObject_4 = __makeTemplateObject(["\n  flex-direction: row;\n  overflow: hidden;\n"], ["\n  flex-direction: row;\n  overflow: hidden;\n"])));
var L_Score = (0, _tyle_1.$label)('L_Score')(templateObject_5 || (templateObject_5 = __makeTemplateObject(["\n  width: 60px;\n  font-size: 18px;\n  color: rgb(255, 128, 2);\n  -unity-text-align: middle-center;\n"], ["\n  width: 60px;\n  font-size: 18px;\n  color: rgb(255, 128, 2);\n  -unity-text-align: middle-center;\n"])));
var V_Color = (0, _tyle_1.$div)('V_Color')(templateObject_6 || (templateObject_6 = __makeTemplateObject(["\n  background-color: rgb(255, 0, 183);\n  width: 18px;\n  margin-top: 4px;\n  margin-bottom: 4px;\n  margin-right: 2px;\n"], ["\n  background-color: rgb(255, 0, 183);\n  width: 18px;\n  margin-top: 4px;\n  margin-bottom: 4px;\n  margin-right: 2px;\n"])));
var L_Name = (0, _tyle_1.$label)('L_Name')(templateObject_7 || (templateObject_7 = __makeTemplateObject(["\n  overflow: hidden;\n  flex-shrink: 1;\n  flex-grow: 1;\n  color: rgb(255, 255, 255);\n"], ["\n  overflow: hidden;\n  flex-shrink: 1;\n  flex-grow: 1;\n  color: rgb(255, 255, 255);\n"])));
var L_Status = (0, _tyle_1.$label)('L_Status')(templateObject_8 || (templateObject_8 = __makeTemplateObject(["\n  font-size: 12px;\n  -unity-font-style: italic;\n  -unity-text-align: middle-right;\n  width: 20%;\n  flex-shrink: 1;\n  flex-grow: 1;\n  color: rgb(229, 229, 229);\n"], ["\n  font-size: 12px;\n  -unity-font-style: italic;\n  -unity-text-align: middle-right;\n  width: 20%;\n  flex-shrink: 1;\n  flex-grow: 1;\n  color: rgb(229, 229, 229);\n"])));
var templateObject_1, templateObject_2, templateObject_3, templateObject_4, templateObject_5, templateObject_6, templateObject_7, templateObject_8;
