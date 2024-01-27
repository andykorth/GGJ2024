"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
exports.FutzInterop = exports.Clips = void 0;
var UnityEngine_1 = require("UnityEngine");
exports.Clips = require('FutzInterop');
var FutzInterop = (function (_super) {
    __extends(FutzInterop, _super);
    function FutzInterop() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return FutzInterop;
}(UnityEngine_1.ScriptableObject));
exports.FutzInterop = FutzInterop;
