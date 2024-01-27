"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.lgRender = exports.lg = void 0;
var lumberjack = require('Lumberjack');
var fallbackColor = 'c184ff';
function lg(text, prefix) {
    if (prefix === void 0) { prefix = ''; }
    var pre = typeof prefix === 'string'
        ? "OneJS ".concat(prefix)
        : "OneJS ".concat(prefix.constructor.name);
    lumberjack.ExternalLog(text, fallbackColor, pre);
}
exports.lg = lg;
function lgRender(fn) {
    lg("render ".concat(fn.constructor.name), fn);
}
exports.lgRender = lgRender;
