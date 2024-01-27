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
exports.Btn = void 0;
var preact_1 = require("preact");
var _tyle_1 = require("./$tyle");
function Btn(props) {
    return ((0, preact_1.h)(B_Btn, { onClick: props.on, text: props.text, disabled: props.disabled, style: __assign({ margin: props.mar || 0, padding: props.pad || [8, 16, 8, 16], fontSize: props.fontSize || 24 }, props.style) }));
}
exports.Btn = Btn;
var B_Btn = (0, _tyle_1.$button)('B_Btn')(templateObject_1 || (templateObject_1 = __makeTemplateObject(["\n  background-color: #E7DCBA;\n  color: #0a0a0a;\n  -unity-font-style: bold;\n  -unity-text-align: middle-center;\n\n  box-shadow: 0 1px 5px 0 rgba(0, 0, 0, 0.2),\n  0 2px 2px 0 rgba(0, 0, 0, 0.14),\n  0 3px 1px -2px rgba(0, 0, 0, 0.12);\n\n  &:hover {\n    background-color: #c0b496;\n  }\n\n  &:active {\n    background-color: #faf7ee;\n  }\n"], ["\n  background-color: #E7DCBA;\n  color: #0a0a0a;\n  -unity-font-style: bold;\n  -unity-text-align: middle-center;\n\n  box-shadow: 0 1px 5px 0 rgba(0, 0, 0, 0.2),\n  0 2px 2px 0 rgba(0, 0, 0, 0.14),\n  0 3px 1px -2px rgba(0, 0, 0, 0.12);\n\n  &:hover {\n    background-color: #c0b496;\n  }\n\n  &:active {\n    background-color: #faf7ee;\n  }\n"])));
var templateObject_1;
