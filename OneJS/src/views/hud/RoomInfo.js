"use strict";
var __makeTemplateObject = (this && this.__makeTemplateObject) || function (cooked, raw) {
    if (Object.defineProperty) { Object.defineProperty(cooked, "raw", { value: raw }); } else { cooked.raw = raw; }
    return cooked;
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.RoomInfo = void 0;
var Track_1 = require("../../util/Track");
var preact_1 = require("preact");
var clips_1 = require("../../clips");
var _tyle_1 = require("../../util/$tyle");
function RoomInfo() {
    var roomIdf = (0, Track_1.useTrack)(clips_1.GameSys_.RoomIdf);
    var status = (0, Track_1.useTrack)(clips_1.GameSys_.Status);
    return ((0, preact_1.h)(W_RoomInfo, null,
        (0, preact_1.h)(L_RoomIdf, { text: roomIdf }),
        (0, preact_1.h)(L_Status, { text: status })));
}
exports.RoomInfo = RoomInfo;
var W_RoomInfo = (0, _tyle_1.$div)('W_RoomInfo')(templateObject_1 || (templateObject_1 = __makeTemplateObject(["\n  position: absolute;\n  left: 8px;\n  top: 8px;\n  background-color: rgb(118, 200, 110);\n  padding: 16px;\n"], ["\n  position: absolute;\n  left: 8px;\n  top: 8px;\n  background-color: rgb(118, 200, 110);\n  padding: 16px;\n"])));
var L_RoomIdf = (0, _tyle_1.$label)('L_RoomIdf')(templateObject_2 || (templateObject_2 = __makeTemplateObject(["\n  font-size: 64px;\n  -unity-font-style: bold;\n  -unity-text-outline-width: 1px;\n  -unity-text-outline-color: rgb(0, 0, 0);\n  color: rgb(255, 255, 255);\n  -unity-text-align: middle-center;\n  padding: 0;\n  margin: 0;\n"], ["\n  font-size: 64px;\n  -unity-font-style: bold;\n  -unity-text-outline-width: 1px;\n  -unity-text-outline-color: rgb(0, 0, 0);\n  color: rgb(255, 255, 255);\n  -unity-text-align: middle-center;\n  padding: 0;\n  margin: 0;\n"])));
var L_Status = (0, _tyle_1.$label)('L_Status')(templateObject_3 || (templateObject_3 = __makeTemplateObject(["\n  font-size: 22px;\n  color: rgb(135, 0, 0);\n  -unity-text-align: middle-center;\n"], ["\n  font-size: 22px;\n  color: rgb(135, 0, 0);\n  -unity-text-align: middle-center;\n"])));
var templateObject_1, templateObject_2, templateObject_3;
