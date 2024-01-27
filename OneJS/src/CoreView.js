"use strict";
var __makeTemplateObject = (this && this.__makeTemplateObject) || function (cooked, raw) {
    if (Object.defineProperty) { Object.defineProperty(cooked, "raw", { value: raw }); } else { cooked.raw = raw; }
    return cooked;
};
var _this = this;
Object.defineProperty(exports, "__esModule", { value: true });
exports.CoreView = void 0;
var preact_1 = require("preact");
var lg_1 = require("./util/lg");
var Hud_1 = require("./views/hud/Hud");
var Track_1 = require("./util/Track");
var ActivityConfig_1 = require("./ActivityConfig");
var _tyle_1 = require("./util/$tyle");
var GhostView_1 = require("./views/ghost/GhostView");
var FutzInterop_1 = require("./FutzInterop");
var CoreView = function () {
    (0, lg_1.lg)("<color=#E6FF00>----------- render CoreView</color>", _this);
    return ((0, preact_1.h)(CoreDiv, null,
        (0, preact_1.h)(GhostViewWrapperTemp, null),
        (0, preact_1.h)(Hud_1.Hud, null)));
};
exports.CoreView = CoreView;
var CoreDiv = (0, _tyle_1.$div)('CoreDiv')(templateObject_1 || (templateObject_1 = __makeTemplateObject(["\n\twidth: 100%;\n\theight: 100%;\n"], ["\n\twidth: 100%;\n\theight: 100%;\n"])));
function GhostViewWrapperTemp() {
    var act = (0, Track_1.useTrack)(FutzInterop_1.Clips.GameSys_.GhostAct);
    if (!act) {
        (0, lg_1.lg)("no ghost act");
        return (0, preact_1.h)("div", null);
    }
    (0, lg_1.lg)("found ghost act");
    return ((0, preact_1.h)(GhostView_1.GhostView, { act: act }));
}
function ActivityView() {
    (0, lg_1.lg)("ActivityView ".concat(FutzInterop_1.Clips.GameSys_, ", ").concat(FutzInterop_1.Clips.GameSys_.CurrentActivity));
    if (!FutzInterop_1.Clips.GameSys_.CurrentActivity) {
        (0, lg_1.lg)("no CurrentActivity");
        return (0, preact_1.h)("div", null);
    }
    var activity = (0, Track_1.useTrack)(FutzInterop_1.Clips.GameSys_.CurrentActivity);
    (0, lg_1.lg)("ActivityView2");
    if (!activity)
        return (0, preact_1.h)("div", null);
    var idf = activity.Idf;
    var View = ActivityConfig_1.ACTIVITY_VIEWS[idf];
    (0, lg_1.lg)("ActivityView ".concat(idf));
    if (!View)
        return (0, preact_1.h)(MissingActivity, null,
            "missing ActivityView: ",
            idf);
    return ((0, preact_1.h)(ActivityDiv, null,
        (0, preact_1.h)(View, { act: activity })));
}
var ActivityDiv = (0, _tyle_1.$div)('ActivityDiv')(templateObject_2 || (templateObject_2 = __makeTemplateObject(["\n\tflex: 1 1 auto;\n\tmargin: 10%;\n"], ["\n\tflex: 1 1 auto;\n\tmargin: 10%;\n"])));
var MissingActivity = (0, _tyle_1.$div)('MissingActivity')(templateObject_3 || (templateObject_3 = __makeTemplateObject(["\n\tflex: 1 1 auto;\n\tmargin: 10%;\n\tfont-size: 48px;\n\tcolor: white;\n\tbackground-color: red;\n\tpadding: 32px;\n"], ["\n\tflex: 1 1 auto;\n\tmargin: 10%;\n\tfont-size: 48px;\n\tcolor: white;\n\tbackground-color: red;\n\tpadding: 32px;\n"])));
var templateObject_1, templateObject_2, templateObject_3;
