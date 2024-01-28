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
var GhostActivity_1 = require("./GhostActivity");
var GhostActorList_1 = require("./GhostActorList");
var TrackLabel_1 = require("../../util/TrackLabel");
function GhostView(props) {
    var act = props.act;
    var phase = (0, Track_1.useTrack)(act.Phase);
    if (phase == GhostActivity_1.E_GhostPhaseEnum.GAME_COMPLETE) {
        return (0, preact_1.h)(EndScreen, { act: act });
    }
    return ((0, preact_1.h)(W_Ghost, null,
        (0, preact_1.h)(InfoPanel, { act: act }),
        (0, preact_1.h)(Timer, { act: act })));
}
exports.GhostView = GhostView;
var W_Ghost = (0, _tyle_1.$div)('Ghost')(templateObject_1 || (templateObject_1 = __makeTemplateObject(["\n\twidth: 100%;\n\theight: 100%;\n"], ["\n\twidth: 100%;\n\theight: 100%;\n"])));
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
var L_PhaseDesc = (0, _tyle_1.$label)('L_PhaseDesc')(templateObject_4 || (templateObject_4 = __makeTemplateObject(["\n\tfont-size: 28px;\n\twhite-space: normal;\n\tmargin: 0 16px;\n"], ["\n\tfont-size: 28px;\n\twhite-space: normal;\n\tmargin: 0 16px;\n"])));
function Timer(props) {
    var act = props.act;
    return ((0, preact_1.h)(W_Timer, null,
        (0, preact_1.h)(TrackLabel_1.TrackLabel, { track: act.TimerString, fnText: function (s) { return s; }, fontSize: 64, mono: true })));
}
var W_Timer = (0, _tyle_1.$div)('W_Timer')(templateObject_5 || (templateObject_5 = __makeTemplateObject(["\n\tposition: absolute;\n\tright: 32px;\n\ttop: 32px;\n\t-unity-font-style: bold;\n\tcolor: rgb(255, 255, 255);\n\t-unity-text-align: middle-center;\n\tmargin: 0;\n\tbackground-color: rgb(0, 0, 0);\n\tpadding: 16px;\n"], ["\n\tposition: absolute;\n\tright: 32px;\n\ttop: 32px;\n\t-unity-font-style: bold;\n\tcolor: rgb(255, 255, 255);\n\t-unity-text-align: middle-center;\n\tmargin: 0;\n\tbackground-color: rgb(0, 0, 0);\n\tpadding: 16px;\n"])));
function EndScreen(props) {
    var act = props.act;
    var rescued = (0, Track_1.useTrack)(act.GhostsRescued);
    var successfullyEscaped = act.SuccessfullyEscaped;
    var text = successfullyEscaped
        ? "You escaped! You rescued ".concat(rescued, " ghosts.")
        : "You were ";
    return ((0, preact_1.h)(W_EndScreen, null,
        (0, preact_1.h)(L_End, { text: 'adsf' })));
}
var W_EndScreen = (0, _tyle_1.$div)('W_EndScreen')(templateObject_6 || (templateObject_6 = __makeTemplateObject(["\n\tposition: absolute;\n\tleft: 200px;\n\tright: 32px;\n\ttop: 32px;\n\tbottom: 32px;\n\tcolor: rgb(255, 255, 255);\n\t-unity-text-align: middle-center;\n\tmargin: 0;\n\tbackground-color: rgb(0, 0, 0);\n\tpadding: 16px;\n\tjustify-content: center;\n"], ["\n\tposition: absolute;\n\tleft: 200px;\n\tright: 32px;\n\ttop: 32px;\n\tbottom: 32px;\n\tcolor: rgb(255, 255, 255);\n\t-unity-text-align: middle-center;\n\tmargin: 0;\n\tbackground-color: rgb(0, 0, 0);\n\tpadding: 16px;\n\tjustify-content: center;\n"])));
var L_End = (0, _tyle_1.$label)('L_End = $label')(templateObject_7 || (templateObject_7 = __makeTemplateObject(["\n\tfont-size: 64px;\n\twhite-space: normal;\n"], ["\n\tfont-size: 64px;\n\twhite-space: normal;\n"])));
var templateObject_1, templateObject_2, templateObject_3, templateObject_4, templateObject_5, templateObject_6, templateObject_7;
