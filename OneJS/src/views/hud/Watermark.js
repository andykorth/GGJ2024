"use strict";
var __makeTemplateObject = (this && this.__makeTemplateObject) || function (cooked, raw) {
    if (Object.defineProperty) { Object.defineProperty(cooked, "raw", { value: raw }); } else { cooked.raw = raw; }
    return cooked;
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.Watermark = void 0;
var preact_1 = require("preact");
var _tyle_1 = require("../../util/$tyle");
var version = {
    VersionName: 'TODO',
    AutoSet: false,
    Year: 'TODO',
    Month: 'TODO',
    Day: 'TODO',
    Hour: 'TODO',
    Minute: 'TODO',
    Version: 'TODO',
    NameAndVersion: 'GGJ2023',
};
function Watermark() {
    return ((0, preact_1.h)(W_Watermark, null,
        (0, preact_1.h)(L_Version, { text: version.NameAndVersion, class: 'monospaced' })));
}
exports.Watermark = Watermark;
var W_Watermark = (0, _tyle_1.$div)('Watermark')(templateObject_1 || (templateObject_1 = __makeTemplateObject(["\n\tposition: absolute;\n\tright: 8px;\n\tbottom: 8px;\n"], ["\n\tposition: absolute;\n\tright: 8px;\n\tbottom: 8px;\n"])));
var L_Version = (0, _tyle_1.$label)('L_Version')(templateObject_2 || (templateObject_2 = __makeTemplateObject(["\n\tfont-size: 14px;\n\t-unity-font-style: bold;\n\tcolor: rgba(255, 255, 255, 0.2);\n\t-unity-text-align: middle-center;\n\tpadding: 0;\n\tmargin: 0;\n"], ["\n\tfont-size: 14px;\n\t-unity-font-style: bold;\n\tcolor: rgba(255, 255, 255, 0.2);\n\t-unity-text-align: middle-center;\n\tpadding: 0;\n\tmargin: 0;\n"])));
var templateObject_1, templateObject_2;
