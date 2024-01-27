"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.$bitsHasIndex = void 0;
function $bitsHasIndex(bits, index) {
    var bit = 1 << index;
    return (bits & bit) === bit;
}
exports.$bitsHasIndex = $bitsHasIndex;
