"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.$toLup = exports.$map = exports.$isIterable = void 0;
function $isIterable(x) {
    return !!(x === null || x === void 0 ? void 0 : x[Symbol.iterator]);
}
exports.$isIterable = $isIterable;
function $map(x, fn) {
    if (!(x === null || x === void 0 ? void 0 : x[Symbol.iterator]))
        throw new Error("not iterable: ".concat(x));
    var arr = [];
    var i = 0;
    for (var _i = 0, x_1 = x; _i < x_1.length; _i++) {
        var el = x_1[_i];
        arr.push(fn(el, i));
        i++;
    }
    return arr;
}
exports.$map = $map;
function $toLup(x, keyProp) {
    if (!(x === null || x === void 0 ? void 0 : x[Symbol.iterator]))
        throw new Error("not iterable: ".concat(x));
    var lup = {};
    for (var _i = 0, x_2 = x; _i < x_2.length; _i++) {
        var el = x_2[_i];
        var key = el[keyProp];
        lup[key] = el;
    }
    return lup;
}
exports.$toLup = $toLup;
