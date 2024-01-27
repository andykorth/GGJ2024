"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.FixedGrid = void 0;
var preact_1 = require("preact");
var hooks_1 = require("preact/hooks");
var UnityEngine_1 = require("UnityEngine");
var UIElements_1 = require("UnityEngine/UIElements");
var VisualElement_1 = require("UnityEngine/UIElements/VisualElement");
function FixedGrid(props) {
    var ref = (0, hooks_1.useRef)();
    (0, hooks_1.useEffect)(function () {
        ref.current.ve.style.opacity = new UIElements_1.StyleFloat(0);
        ref.current.ve.generateVisualContent = onGenerateVisualContent;
        ref.current.ve.MarkDirtyRepaint();
    }, []);
    function onGenerateVisualContent(_) {
        setTimeout(resize);
    }
    function resize() {
        var columns = props.columns;
        var aspectRatio = props.aspectRatio || 1;
        var contentRect = ref.current.ve.contentRect;
        var gridWidth = contentRect.width;
        var cellWidth = gridWidth / columns;
        var cellHeight = cellWidth / aspectRatio;
    }
    return ((0, preact_1.h)("div", { style: {} }));
}
exports.FixedGrid = FixedGrid;
var AutoText = function (props) {
    var ref = (0, hooks_1.useRef)();
    var dirtyCount = 0;
    (0, hooks_1.useEffect)(function () {
        ref.current.ve.style.opacity = new UIElements_1.StyleFloat(0);
        ref.current.ve.generateVisualContent = onGenerateVisualContent;
        ref.current.ve.MarkDirtyRepaint();
    }, []);
    function onGenerateVisualContent(mgc) {
        dirtyCount++;
        setTimeout(resize);
    }
    function resize() {
        var contentRect = ref.current.ve.contentRect;
        var textElement = ref.current.ve.Children()[0];
        var textSize = textElement.MeasureTextSize(props.text, 99999, VisualElement_1.MeasureMode.AtMost, 99999, VisualElement_1.MeasureMode.AtMost);
        var fontSize = UnityEngine_1.Mathf.Max(ref.current.ve.style.fontSize.value.value, 1);
        var heightDictatedFontSize = UnityEngine_1.Mathf.Abs(contentRect.height);
        var widthDictatedFontSize = UnityEngine_1.Mathf.Abs(contentRect.width / textSize.x) * fontSize;
        var newFontSize = UnityEngine_1.Mathf.FloorToInt(UnityEngine_1.Mathf.Min(heightDictatedFontSize, widthDictatedFontSize));
        newFontSize = UnityEngine_1.Mathf.Clamp(newFontSize, 1, 9999);
        ref.current.ve.style.fontSize = new UIElements_1.StyleLength(new UIElements_1.Length(newFontSize, UIElements_1.LengthUnit.Pixel));
        if (dirtyCount > 1) {
            ref.current.ve.style.opacity = new UIElements_1.StyleFloat(1);
        }
        else {
            ref.current.ve.MarkDirtyRepaint();
        }
    }
    return (0, preact_1.h)("div", { ref: ref, class: 'w-full h-full' }, props.text);
};
