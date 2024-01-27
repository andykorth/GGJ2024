"use strict";
var __makeTemplateObject = (this && this.__makeTemplateObject) || function (cooked, raw) {
    if (Object.defineProperty) { Object.defineProperty(cooked, "raw", { value: raw }); } else { cooked.raw = raw; }
    return cooked;
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.ActivitySelect = void 0;
var preact_1 = require("preact");
var Track_1 = require("../../util/Track");
var _array_1 = require("../../util/buckle/$array");
var clips_1 = require("../../clips");
var lg_1 = require("../../util/lg");
var _tyle_1 = require("../../util/$tyle");
function ActivitySelect() {
    var _a = (0, Track_1.useTrackToggle)(clips_1.CoreUi_.ShowActivitySelect), show = _a[0], toggleShow = _a[1];
    var _b = (0, Track_1.useTrackChoiceList)(clips_1.GameSys_.ActivityChoice), defs = _b[0], currentIndex = _b[1], currentDef = _b[2], setIndex = _b[3];
    var inner = show ? ((0, preact_1.h)(G_Activities, null, (0, _array_1.$map)(defs, function (def, i) { return ((0, preact_1.h)(R_Act, { key: def.Idf, text: def.Name, onClick: function () { return setIndex(i); }, value: i === currentIndex })); }))) : (0, preact_1.h)("div", null);
    (0, lg_1.lg)("render ", this);
    return ((0, preact_1.h)(W_ActivitySelect, null,
        (0, preact_1.h)(_tyle_1.Row, null,
            (0, preact_1.h)(L_Title, { text: 'Activity Select' }),
            (0, preact_1.h)(B_Show, { text: show ? '-' : '+', onClick: function () { return toggleShow(); } })),
        inner));
}
exports.ActivitySelect = ActivitySelect;
var W_ActivitySelect = (0, _tyle_1.$div)('W_ActivitySelect')(templateObject_1 || (templateObject_1 = __makeTemplateObject(["\n  position: absolute;\n  top: 8px;\n  right: 8px;\n  background-color: rgb(219, 148, 173);\n  padding: 8px 16px;\n"], ["\n  position: absolute;\n  top: 8px;\n  right: 8px;\n  background-color: rgb(219, 148, 173);\n  padding: 8px 16px;\n"])));
var L_Title = (0, _tyle_1.$label)('L_Title')(templateObject_2 || (templateObject_2 = __makeTemplateObject([""], [""])));
var B_Show = (0, _tyle_1.$button)('B_Show')(templateObject_3 || (templateObject_3 = __makeTemplateObject(["\n  background-color: unset;\n  font-size: 24px;\n  padding: 0;\n  border: none;\n  outline: none;\n"], ["\n  background-color: unset;\n  font-size: 24px;\n  padding: 0;\n  border: none;\n  outline: none;\n"])));
var G_Activities = (0, _tyle_1.$radiobuttongroup)('G_Activities')(templateObject_4 || (templateObject_4 = __makeTemplateObject(["\n  flex-direction: column;\n"], ["\n  flex-direction: column;\n"])));
var R_Act = (0, _tyle_1.$radiobutton)('R_Act')(templateObject_5 || (templateObject_5 = __makeTemplateObject([""], [""])));
var templateObject_1, templateObject_2, templateObject_3, templateObject_4, templateObject_5;
