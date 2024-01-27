"use strict";
var __makeTemplateObject = (this && this.__makeTemplateObject) || function (cooked, raw) {
    if (Object.defineProperty) { Object.defineProperty(cooked, "raw", { value: raw }); } else { cooked.raw = raw; }
    return cooked;
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.GhostView = void 0;
var preact_1 = require("preact");
var Track_1 = require("../../util/Track");
var _tyle_1 = require("../../util/$tyle");
var GhostActorList_1 = require("./GhostActorList");
function GhostView(props) {
    var act = props.act;
    return ((0, preact_1.h)(W_Ghost, null,
        (0, preact_1.h)(InfoPanel, { act: act })));
}
exports.GhostView = GhostView;
var W_Ghost = (0, _tyle_1.$div)('Ghost')(templateObject_1 || (templateObject_1 = __makeTemplateObject(["\n  width: 100%;\n  height: 100%;\n"], ["\n  width: 100%;\n  height: 100%;\n"])));
function InfoPanel(props) {
    var act = props.act;
    var phaseTitle = (0, Track_1.useTrack)(act.PhaseTitle);
    var phaseDesc = (0, Track_1.useTrack)(act.PhaseDesc);
    return ((0, preact_1.h)(W_InfoPanel, null,
        (0, preact_1.h)(L_PhaseTitle, { text: phaseTitle }),
        (0, preact_1.h)(L_PhaseDesc, { text: phaseDesc }),
        (0, preact_1.h)(_tyle_1.Grow, null),
        (0, preact_1.h)(GhostActorList_1.GhostActorList, { act: act })));
}
var W_InfoPanel = (0, _tyle_1.$div)('W_InfoPanel')(templateObject_2 || (templateObject_2 = __makeTemplateObject(["\n\tposition: absolute;\n\tleft: 0;\n\tright: 0;\n\ttop: 0;\n\tbottom: 0;\n\tpadding: 8px;\n"], ["\n\tposition: absolute;\n\tleft: 0;\n\tright: 0;\n\ttop: 0;\n\tbottom: 0;\n\tpadding: 8px;\n"])));
var L_PhaseTitle = (0, _tyle_1.$label)('L_PhaseTitle')(templateObject_3 || (templateObject_3 = __makeTemplateObject(["\n\tfont-size: 24px;\n\twhite-space: normal;\n\tmargin-left: 400px;\n"], ["\n\tfont-size: 24px;\n\twhite-space: normal;\n\tmargin-left: 400px;\n"])));
var L_PhaseDesc = (0, _tyle_1.$label)('L_PhaseDesc')(templateObject_4 || (templateObject_4 = __makeTemplateObject(["\n  font-size: 28px;\n  white-space: normal;\n  margin: 0 16px;\n"], ["\n  font-size: 28px;\n  white-space: normal;\n  margin: 0 16px;\n"])));
var W_CardGrid = (0, _tyle_1.$div)('grid')(templateObject_5 || (templateObject_5 = __makeTemplateObject(["\n  flex: 1 1 auto;\n  flex-direction: row;\n  flex-wrap: wrap;\n  background-color: #cbc9a1;\n"], ["\n  flex: 1 1 auto;\n  flex-direction: row;\n  flex-wrap: wrap;\n  background-color: #cbc9a1;\n"])));
var templateObject_1, templateObject_2, templateObject_3, templateObject_4, templateObject_5;
