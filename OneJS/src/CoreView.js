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
var clips_1 = require("./clips");
var _tyle_1 = require("./util/$tyle");
var CoreView = function () {
    (0, lg_1.lg)("<color=#E6FF00>----------- render CoreView</color>", _this);
    return ((0, preact_1.h)(CoreDiv, null,
        (0, preact_1.h)(ActivityView, null),
        (0, preact_1.h)(Hud_1.Hud, null)));
};
exports.CoreView = CoreView;
var CoreDiv = (0, _tyle_1.$div)('CoreDiv')(templateObject_1 || (templateObject_1 = __makeTemplateObject(["\n  width: 100%;\n  height: 100%;\n"], ["\n  width: 100%;\n  height: 100%;\n"])));
function ActivityView() {
    var activity = (0, Track_1.useTrack)(clips_1.GameSys_.CurrentActivity);
    if (!activity)
        return (0, preact_1.h)("div", null);
    var idf = activity.Idf;
    var View = ActivityConfig_1.ACTIVITY_VIEWS[idf];
    if (!View)
        return (0, preact_1.h)(MissingActivity, null,
            "missing ActivityView: ",
            idf);
    return ((0, preact_1.h)(ActivityDiv, null,
        (0, preact_1.h)(View, { act: activity })));
}
var ActivityDiv = (0, _tyle_1.$div)('ActivityDiv')(templateObject_2 || (templateObject_2 = __makeTemplateObject(["\n  flex: 1 1 auto;\n  margin: 10%;\n"], ["\n  flex: 1 1 auto;\n  margin: 10%;\n"])));
var MissingActivity = (0, _tyle_1.$div)('MissingActivity')(templateObject_3 || (templateObject_3 = __makeTemplateObject(["\n  flex: 1 1 auto;\n  margin: 10%;\n  font-size: 48px;\n  color: white;\n  background-color: red;\n  padding: 32px;\n"], ["\n  flex: 1 1 auto;\n  margin: 10%;\n  font-size: 48px;\n  color: white;\n  background-color: red;\n  padding: 32px;\n"])));
var templateObject_1, templateObject_2, templateObject_3;
