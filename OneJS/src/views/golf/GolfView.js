"use strict";
var __makeTemplateObject = (this && this.__makeTemplateObject) || function (cooked, raw) {
    if (Object.defineProperty) { Object.defineProperty(cooked, "raw", { value: raw }); } else { cooked.raw = raw; }
    return cooked;
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.GolfView = void 0;
var _tyle_1 = require("../../util/$tyle");
var preact_1 = require("preact");
function GolfView(props) {
    var act = props.act;
    return ((0, preact_1.h)(W_Golf, null));
}
exports.GolfView = GolfView;
var W_Golf = (0, _tyle_1.$div)('Golf')(templateObject_1 || (templateObject_1 = __makeTemplateObject(["\n\n"], ["\n\n"])));
var templateObject_1;
