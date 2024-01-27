"use strict";
var __makeTemplateObject = (this && this.__makeTemplateObject) || function (cooked, raw) {
    if (Object.defineProperty) { Object.defineProperty(cooked, "raw", { value: raw }); } else { cooked.raw = raw; }
    return cooked;
};
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.Lbl = void 0;
var preact_1 = require("preact");
var _tyle_1 = require("./$tyle");
function Lbl(props) {
    var fontStyle = '';
    return ((0, preact_1.h)(L_Lbl, { text: props.text, style: __assign({ margin: props.mar || 0, padding: props.pad || 0, fontSize: props.fontSize || 18, unityFontStyleAndWeight: props.fontStyle }, props.style) }));
}
exports.Lbl = Lbl;
var L_Lbl = (0, _tyle_1.$label)('B_Lbl')(templateObject_1 || (templateObject_1 = __makeTemplateObject(["\n  color: #000000;\n"], ["\n  color: #000000;\n"])));
var templateObject_1;
