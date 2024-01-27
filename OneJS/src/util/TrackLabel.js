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
exports.TrackLabel = void 0;
var preact_1 = require("preact");
var _tyle_1 = require("./$tyle");
var hooks_1 = require("preact/hooks");
var valueKey = 'Current';
var addEventKey = 'add_EventValueChanged';
var removeEventKey = 'remove_EventValueChanged';
function TrackLabel(props) {
    var track = props.track, fnText = props.fnText;
    var ref = (0, hooks_1.useRef)();
    var setText = (0, hooks_1.useCallback)(function () {
        var el = ref.current.ve;
        var text = fnText(track[valueKey]);
        el.text = text;
        if (props.hideIfEmpty) {
            ref.current.style.display = !!text ? 'Flex' : 'None';
        }
    }, []);
    (0, hooks_1.useEffect)(function () {
        var addEvent = track[addEventKey];
        var removeEvent = track[removeEventKey];
        setText();
        addEvent.call(track, setText);
        onEngineReload(removeHandler);
        return function () {
            removeHandler();
            unregisterOnEngineReload(removeHandler);
        };
        function removeHandler() {
            removeEvent.call(track, setText);
        }
    }, [track]);
    return ((0, preact_1.h)("label", { ref: ref, style: __assign({ margin: props.mar || 0, padding: props.pad || 0, fontSize: props.fontSize || 18, unityFontStyleAndWeight: props.fontStyle }, props.style) }));
}
exports.TrackLabel = TrackLabel;
var L_Lbl = (0, _tyle_1.$label)('B_Lbl')(templateObject_1 || (templateObject_1 = __makeTemplateObject(["\n  color: #000000;\n"], ["\n  color: #000000;\n"])));
var templateObject_1;
