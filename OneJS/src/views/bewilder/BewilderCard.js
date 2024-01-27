"use strict";
var __makeTemplateObject = (this && this.__makeTemplateObject) || function (cooked, raw) {
    if (Object.defineProperty) { Object.defineProperty(cooked, "raw", { value: raw }); } else { cooked.raw = raw; }
    return cooked;
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.BewilderCard = void 0;
var Track_1 = require("../../util/Track");
var preact_1 = require("preact");
var _tyle_1 = require("../../util/$tyle");
var styled_1 = require("onejs/styled");
var _array_1 = require("../../util/buckle/$array");
function BewilderCard(props) {
    var card = props.card;
    var act = props.act;
    var isRevealed = (0, Track_1.useTrack)(card.IsRevealed);
    var word = (0, Track_1.useTrack)(card.Word);
    var isCorrect = (0, Track_1.useTrack)(card.IsCorrect);
    return ((0, preact_1.h)(C_Cell, null,
        (0, preact_1.h)(C_Card, { isRevealed: isRevealed, isCorrect: isCorrect },
            (0, preact_1.h)(L_Word, { text: word }),
            (0, preact_1.h)(CardPickedList, { card: card, act: act }))));
}
exports.BewilderCard = BewilderCard;
var width = 100 / 3;
var height = 100 / 3;
var C_Cell = (0, _tyle_1.$div)('cell')(templateObject_1 || (templateObject_1 = __makeTemplateObject(["\n  width: ", "%;\n  height: ", "%;\n"], ["\n  width: ", "%;\n  height: ", "%;\n"])), width, height);
var C_Card = (0, _tyle_1.$div)('card')(templateObject_4 || (templateObject_4 = __makeTemplateObject(["\n  flex: 1 0 auto;\n  justify-content: center;\n  align-items: center;\n  background-color: #f1f1f1;\n  color: #000;\n  border-radius: 32px;\n  margin: 16px;\n\n  translate: 200px 1000px;\n  transition-property: all;\n  transition-delay: 0s;\n  transition-duration: .5s;\n  transition-timing-function: ease-in-out;\n\n  ", "\n  \n  ", "\n"], ["\n  flex: 1 0 auto;\n  justify-content: center;\n  align-items: center;\n  background-color: #f1f1f1;\n  color: #000;\n  border-radius: 32px;\n  margin: 16px;\n\n  translate: 200px 1000px;\n  transition-property: all;\n  transition-delay: 0s;\n  transition-duration: .5s;\n  transition-timing-function: ease-in-out;\n\n  ", "\n  \n  ", "\n"])), function (props) { return props.isRevealed && (0, styled_1.uss)(templateObject_2 || (templateObject_2 = __makeTemplateObject(["\n    translate: 0 0;\n  "], ["\n    translate: 0 0;\n  "]))); }, function (props) { return props.isCorrect && (0, styled_1.uss)(templateObject_3 || (templateObject_3 = __makeTemplateObject(["\n    background-color: #88f16a;\n  "], ["\n    background-color: #88f16a;\n  "]))); });
var L_Word = (0, _tyle_1.$label)('word')(templateObject_5 || (templateObject_5 = __makeTemplateObject(["\n  font-size: 48px;\n"], ["\n  font-size: 48px;\n"])));
function CardPickedList(props) {
    var card = props.card;
    var act = props.act;
    var actors = (0, Track_1.useTrackList)(act.Actors);
    var slotIds = (0, Track_1.useTrack)(card.PickedByActorSlotIds);
    var showCount = (0, Track_1.useTrack)(card.ShowCount);
    var showActors = [];
    for (var i = 0; i < showCount; i++) {
        var slotId = slotIds[i];
        var actor = actors[slotId];
        showActors.push(actor);
    }
    return ((0, preact_1.h)(_tyle_1.Row, null, (0, _array_1.$map)(showActors, function (actor, i) { return ((0, preact_1.h)(ActorPick, { key: actor.EntityId, actor: actor })); })));
}
function ActorPick(props) {
    var actor = props.actor;
    return ((0, preact_1.h)(C_ActorPick, null,
        (0, preact_1.h)(V_Color, { style: { backgroundColor: actor.Color } }),
        (0, preact_1.h)(L_PickedList, { text: actor.Nickname })));
}
var C_ActorPick = (0, _tyle_1.$div)('C_ActorPick')(templateObject_6 || (templateObject_6 = __makeTemplateObject(["\n  flex-direction: row;\n  flex-wrap: wrap;\n  margin-right: 4px;\n"], ["\n  flex-direction: row;\n  flex-wrap: wrap;\n  margin-right: 4px;\n"])));
var V_Color = (0, _tyle_1.$div)('Color')(templateObject_7 || (templateObject_7 = __makeTemplateObject(["\n  background-color: rgb(255, 0, 183);\n  width: 18px;\n  margin-top: 4px;\n  margin-bottom: 4px;\n  margin-right: 2px;\n"], ["\n  background-color: rgb(255, 0, 183);\n  width: 18px;\n  margin-top: 4px;\n  margin-bottom: 4px;\n  margin-right: 2px;\n"])));
var L_PickedList = (0, _tyle_1.$label)('L_PickedList')(templateObject_8 || (templateObject_8 = __makeTemplateObject(["\n  font-size: 18px;\n  color: #444444;\n"], ["\n  font-size: 18px;\n  color: #444444;\n"])));
var templateObject_1, templateObject_2, templateObject_3, templateObject_4, templateObject_5, templateObject_6, templateObject_7, templateObject_8;
