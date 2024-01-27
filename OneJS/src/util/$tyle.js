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
exports.Col = exports.Row = exports.Grow = exports.$radiobuttongroup = exports.$radiobutton = exports.$button = exports.$foldout = exports.$img = exports.$label = exports.$div = void 0;
var preact_1 = require("preact");
var generateComponentId_1 = require("onejs/styled/utils/generateComponentId");
var css_flatten_1 = require("css-flatten");
function _hashAndAddRuntimeUSS(style) {
    var compId = (0, generateComponentId_1.default)(style);
    style = (0, css_flatten_1.default)(".".concat(compId, " {").concat(style, "}"));
    document.addRuntimeUSS(style);
    return compId;
}
function _processTemplate(props, strings, values) {
    var style = values.reduce(function (result, expr, index) {
        var value = typeof expr === 'function' ? expr(props) : expr;
        if (typeof value === 'function')
            value = value(props);
        if (!value)
            value = '';
        return "".concat(result).concat(value).concat(strings[index + 1]);
    }, strings[0]);
    return style;
}
var $div = function (name) { return styling('div', name); };
exports.$div = $div;
var $label = function (name) { return styling('label', name); };
exports.$label = $label;
var $img = function (name) { return styling('image', name); };
exports.$img = $img;
var $foldout = function (name) { return styling('foldout', name); };
exports.$foldout = $foldout;
var $button = function (name) { return styling('button', name); };
exports.$button = $button;
var $radiobutton = function (name) { return styling('radiobutton', name); };
exports.$radiobutton = $radiobutton;
var $radiobuttongroup = function (name) { return styling('radiobuttongroup', name); };
exports.$radiobuttongroup = $radiobuttongroup;
function styling(JsxTag, name) {
    return function (strings) {
        var values = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            values[_i - 1] = arguments[_i];
        }
        return function (props) {
            var style = _processTemplate(props, strings, values);
            var compId = _hashAndAddRuntimeUSS(style);
            var className = props.class ? "".concat(compId, " ").concat(props.class) : compId;
            return (0, preact_1.h)(JsxTag, __assign({ name: name }, props, { class: className }));
        };
    };
}
exports.Grow = (0, exports.$div)('grow')(templateObject_1 || (templateObject_1 = __makeTemplateObject(["\n  flex: 1 0 auto;\n"], ["\n  flex: 1 0 auto;\n"])));
exports.Row = (0, exports.$div)('row')(templateObject_2 || (templateObject_2 = __makeTemplateObject(["\n  flex-direction: row;\n"], ["\n  flex-direction: row;\n"])));
exports.Col = (0, exports.$div)('col')(templateObject_3 || (templateObject_3 = __makeTemplateObject([""], [""])));
var templateObject_1, templateObject_2, templateObject_3;
