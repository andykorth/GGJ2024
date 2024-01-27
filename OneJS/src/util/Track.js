"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.useTrackEffect = exports.useTrackEvtFn = exports.useTrackBoolVisible = exports.useTrackRefFn = exports.useTrackFn = exports.useTrackToggle = exports.useTrackEvt = exports.useTrackChoice = exports.useTrackChoiceList = exports.useTrackList = exports.useTrack = void 0;
var hooks_1 = require("preact/hooks");
var lg_1 = require("./lg");
var valueKey = 'Current';
var addEventKey = 'add_EventValueChanged';
var removeEventKey = 'remove_EventValueChanged';
function useTrack(track) {
    var _this = this;
    var currentVal = track[valueKey];
    var _a = (0, hooks_1.useState)(1), v = _a[0], setV = _a[1];
    var onValueChangedCallback = (0, hooks_1.useCallback)(function () {
        setV(function (v) { return v + 1; });
    }, []);
    (0, hooks_1.useEffect)(function () {
        var addEvent = track[addEventKey];
        var removeEvent = track[removeEventKey];
        addEvent.call(track, onValueChangedCallback);
        onEngineReload(function () { return removeEvent.call(track, onValueChangedCallback); });
        if (currentVal != track[valueKey]) {
            (0, lg_1.lg)("".concat(track, " (useEffect) initial value changed (SHOULD BE RARE)"), _this);
            onValueChangedCallback();
        }
        return function () {
            removeEvent.call(track, onValueChangedCallback);
        };
    }, [track]);
    return track[valueKey];
}
exports.useTrack = useTrack;
function useTrackList(trackList) {
    var _a = (0, hooks_1.useState)(1), v = _a[0], setV = _a[1];
    var onValueChangedCallback = (0, hooks_1.useCallback)(function () {
        setV(function (v) { return v + 1; });
    }, []);
    (0, hooks_1.useEffect)(function () {
        var addEvent = trackList[addEventKey];
        var removeEvent = trackList[removeEventKey];
        addEvent.call(trackList, onValueChangedCallback);
        onEngineReload(function () { return removeEvent.call(trackList, onValueChangedCallback); });
        return function () {
            removeEvent.call(trackList, onValueChangedCallback);
        };
    }, [trackList]);
    return trackList[valueKey];
}
exports.useTrackList = useTrackList;
function useTrackChoiceList(track) {
    var _a = (0, hooks_1.useState)(track.CurrentIndex), index = _a[0], setIndex = _a[1];
    var _b = (0, hooks_1.useState)({}), updateState = _b[1];
    var forceUpdate = (0, hooks_1.useCallback)(function () { return updateState({}); }, []);
    var addEvent = track[addEventKey];
    var removeEvent = track[removeEventKey];
    var onValueChangedCallback = function (nextVal) {
        setIndex(nextVal);
        forceUpdate();
    };
    (0, hooks_1.useEffect)(function () {
        addEvent.call(track, onValueChangedCallback);
        onEngineReload(function () { return removeEvent.call(track, onValueChangedCallback); });
        return function () { return removeEvent.call(track, onValueChangedCallback); };
    }, []);
    var setIndexWrapper = function (i) { return track.ChangeDiff(i); };
    return [track.Choices, index, track.Current, setIndexWrapper];
}
exports.useTrackChoiceList = useTrackChoiceList;
function useTrackChoice(track) {
    var _a = useTrackChoiceList(track), val = _a[2];
    return val;
}
exports.useTrackChoice = useTrackChoice;
function useTrackEvt(track) {
    var _a = (0, hooks_1.useState)({}), updateState = _a[1];
    var forceUpdate = (0, hooks_1.useCallback)(function () { return updateState({}); }, []);
    var handler = function () {
        forceUpdate();
    };
    (0, hooks_1.useEffect)(function () {
        track.add_EventValueChanged(handler);
        onEngineReload(function () { return track.remove_EventValueChanged(handler); });
        return function () { return track.remove_EventValueChanged(handler); };
    }, []);
    return function () { return track.Trigger(); };
}
exports.useTrackEvt = useTrackEvt;
function useTrackToggle(track) {
    var _a = (0, hooks_1.useState)({}), updateState = _a[1];
    var forceUpdate = (0, hooks_1.useCallback)(function () { return updateState({}); }, []);
    var handler = function () {
        forceUpdate();
    };
    (0, hooks_1.useEffect)(function () {
        track.add_EventValueChanged(handler);
        onEngineReload(function () { return track.remove_EventValueChanged(handler); });
        return function () { return track.remove_EventValueChanged(handler); };
    }, []);
    return [track.IsOn, function () { return track.Toggle(); }];
}
exports.useTrackToggle = useTrackToggle;
function useTrackFn(track, fn) {
    var callFn = (0, hooks_1.useCallback)(function () { return fn(track[valueKey]); }, []);
    useTrackEffect(track, callFn);
}
exports.useTrackFn = useTrackFn;
function useTrackRefFn(track, fn) {
    var ref = (0, hooks_1.useRef)();
    var callFn = (0, hooks_1.useCallback)(function () { return fn(track[valueKey], ref.current); }, []);
    useTrackEffect(track, callFn);
    return ref;
}
exports.useTrackRefFn = useTrackRefFn;
function useTrackBoolVisible(track) {
    var ref = (0, hooks_1.useRef)();
    var callFn = (0, hooks_1.useCallback)(function () {
        ref.current.style.display = track[valueKey] ? 'Flex' : 'None';
    }, []);
    useTrackEffect(track, callFn);
    return ref;
}
exports.useTrackBoolVisible = useTrackBoolVisible;
function useTrackEvtFn(track, fn) {
    var ref = (0, hooks_1.useRef)();
    var callFn = (0, hooks_1.useCallback)(function () { return fn(ref.current); }, []);
    useTrackEffect(track, callFn);
    return ref;
}
exports.useTrackEvtFn = useTrackEvtFn;
function useTrackEffect(track, callFn) {
    (0, hooks_1.useEffect)(function () {
        var addEvent = track[addEventKey];
        var removeEvent = track[removeEventKey];
        callFn();
        addEvent.call(track, callFn);
        onEngineReload(removeHandler);
        return function () {
            removeHandler();
            unregisterOnEngineReload(removeHandler);
        };
        function removeHandler() {
            removeEvent.call(track, callFn);
        }
    }, [track]);
}
exports.useTrackEffect = useTrackEffect;
