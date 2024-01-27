"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.useInternalSyncerState = exports.useSyncer = void 0;
var hooks_1 = require("preact/hooks");
function useSyncer(syncer) {
    var val = useInternalSyncerState(syncer)[0];
    return val;
}
exports.useSyncer = useSyncer;
var valueKey = 'Current';
var addEventKey = 'add_EventValueChanged';
var removeEventKey = 'remove_EventValueChanged';
function useInternalSyncerState(syncer) {
    var _a = (0, hooks_1.useState)(syncer[valueKey]), val = _a[0], setVal = _a[1];
    var _b = (0, hooks_1.useState)({}), updateState = _b[1];
    var forceUpdate = (0, hooks_1.useCallback)(function () { return updateState({}); }, []);
    var addEvent = syncer[addEventKey];
    var removeEvent = syncer[removeEventKey];
    var onValueChangedCallback = function (nextVal) {
        setVal(nextVal);
        forceUpdate();
    };
    (0, hooks_1.useEffect)(function () {
        addEvent.call(syncer, onValueChangedCallback);
        onEngineReload(function () { return removeEvent.call(syncer, onValueChangedCallback); });
        return function () { return removeEvent.call(syncer, onValueChangedCallback); };
    }, []);
    var setValWrapper = function (val) { throw new Error("TODO: set syncer val"); };
    return [val, setValWrapper];
}
exports.useInternalSyncerState = useInternalSyncerState;
