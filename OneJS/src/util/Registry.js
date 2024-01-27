"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.useRegistryLup = exports.useRegistry = void 0;
var hooks_1 = require("preact/hooks");
var _array_1 = require("./buckle/$array");
function useRegistry(registry) {
    var _a = (0, hooks_1.useState)({}), updateState = _a[1];
    var forceUpdate = (0, hooks_1.useCallback)(function () {
        updateState({});
    }, []);
    var handler = function () {
        forceUpdate();
    };
    (0, hooks_1.useEffect)(function () {
        registry.add_EventValueChanged(handler);
        onEngineReload(function () { return registry.remove_EventValueChanged(handler); });
        return function () { return registry.remove_EventValueChanged(handler); };
    }, []);
    return registry.Value;
}
exports.useRegistry = useRegistry;
function useRegistryLup(registry, keyProp) {
    var _a = (0, hooks_1.useState)({}), updateState = _a[1];
    var forceUpdate = (0, hooks_1.useCallback)(function () {
        updateState({});
    }, []);
    var handler = function () {
        forceUpdate();
    };
    (0, hooks_1.useEffect)(function () {
        registry.add_EventValueChanged(handler);
        onEngineReload(function () { return registry.remove_EventValueChanged(handler); });
        return function () { return registry.remove_EventValueChanged(handler); };
    }, []);
    return (0, _array_1.$toLup)(registry.Value, keyProp);
}
exports.useRegistryLup = useRegistryLup;
