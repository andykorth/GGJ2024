"use strict";
var __makeTemplateObject = (this && this.__makeTemplateObject) || function (cooked, raw) {
    if (Object.defineProperty) { Object.defineProperty(cooked, "raw", { value: raw }); } else { cooked.raw = raw; }
    return cooked;
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.Hud = void 0;
var preact_1 = require("preact");
var RoomInfo_1 = require("./RoomInfo");
var AgentList_1 = require("./AgentList");
var Watermark_1 = require("./Watermark");
var UIElements_1 = require("UnityEngine/UIElements");
var Track_1 = require("../../util/Track");
var _tyle_1 = require("../../util/$tyle");
var FutzInterop_1 = require("../../FutzInterop");
function Hud() {
    var showAgentList = (0, Track_1.useTrackToggle)(FutzInterop_1.Clips.CoreUi_.ShowAgentList)[0];
    return ((0, preact_1.h)(S_Hud, { "picking-mode": UIElements_1.PickingMode.Ignore },
        (0, preact_1.h)(RoomInfo_1.RoomInfo, null),
        showAgentList && (0, preact_1.h)(AgentList_1.AgentList, null),
        (0, preact_1.h)(Watermark_1.Watermark, null)));
}
exports.Hud = Hud;
var S_Hud = (0, _tyle_1.$div)('hud')(templateObject_1 || (templateObject_1 = __makeTemplateObject(["\n  position: absolute;\n  top: 0;\n  right: 0;\n  bottom: 0;\n  left: 0;\n"], ["\n  position: absolute;\n  top: 0;\n  right: 0;\n  bottom: 0;\n  left: 0;\n"])));
var templateObject_1;
