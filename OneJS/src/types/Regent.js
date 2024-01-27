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
exports.ClipNet = exports.ClipNative = exports.CogNet = exports.CogNative = exports.ICog = exports.Entity = void 0;
var UnityEngine_1 = require("UnityEngine");
var Entity = (function () {
    function Entity() {
    }
    return Entity;
}());
exports.Entity = Entity;
var ICog = (function (_super) {
    __extends(ICog, _super);
    function ICog() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return ICog;
}(UnityEngine_1.MonoBehaviour));
exports.ICog = ICog;
var CogNative = (function (_super) {
    __extends(CogNative, _super);
    function CogNative() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return CogNative;
}(ICog));
exports.CogNative = CogNative;
var CogNet = (function (_super) {
    __extends(CogNet, _super);
    function CogNet() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return CogNet;
}(ICog));
exports.CogNet = CogNet;
var ClipNative = (function (_super) {
    __extends(ClipNative, _super);
    function ClipNative() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return ClipNative;
}(CogNative));
exports.ClipNative = ClipNative;
var ClipNet = (function (_super) {
    __extends(ClipNet, _super);
    function ClipNet() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return ClipNet;
}(CogNet));
exports.ClipNet = ClipNet;
