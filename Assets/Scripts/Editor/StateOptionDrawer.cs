// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Lumberjack;
// using UnityEditor;
// using UnityEditor.UIElements;
// using UnityEngine;
// using UnityEngine.UIElements;
//
// [CustomPropertyDrawer(typeof(StateOption))]
// public class StateOptionDrawer : PropertyDrawer
// {
// 	// public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
// 	// {
// 	// 	// var stateOption = property.ref as StateOption;
// 	//
// 	//
// 	// 	var Label = property.FindPropertyRelative("Label");
// 	// 	var UseSprite = property.FindPropertyRelative("UseSprite");
// 	// 	var EnableObj = property.FindPropertyRelative("EnableObj");
// 	// 	var VerbToNext = property.FindPropertyRelative("VerbToNext");
// 	// 	var SfxInteract = property.FindPropertyRelative("SfxInteract");
// 	// 	var RotateAmount = property.FindPropertyRelative("RotateAmount");
// 	// 	var Scale = property.FindPropertyRelative("Scale");
// 	// 	var Wants = property.FindPropertyRelative("Wants");
// 	// 	var Hates = property.FindPropertyRelative("Hates");
// 	// 	var ExitWants = property.FindPropertyRelative("ExitWants");
// 	// 	var ExitHates = property.FindPropertyRelative("ExitHates");
// 	// }
//
// 	public override VisualElement CreatePropertyGUI(SerializedProperty property)
// 	{
// 		var root = new VisualElement();
//
// 		var labelProp = property.FindPropertyRelative("Label");
// 		var labelString = labelProp.stringValue;
// 		root.Add(new PropertyField(labelProp));
//
// 		root.Add(new PropertyField(property.FindPropertyRelative("UseSprite")));
// 		root.Add(new PropertyField(property.FindPropertyRelative("EnableObj")));
// 		root.Add(new PropertyField(property.FindPropertyRelative("VerbToNext")));
// 		root.Add(new PropertyField(property.FindPropertyRelative("SfxInteract")));
// 		root.Add(new PropertyField(property.FindPropertyRelative("RotateAmount")));
// 		root.Add(new PropertyField(property.FindPropertyRelative("Scale")));
//
// 		var wantsProp = property.FindPropertyRelative("Wants");
// 		var hatesProp = property.FindPropertyRelative("Hates");
// 		var exitWantsProp = property.FindPropertyRelative("ExitWants");
// 		var exitHatesProp = property.FindPropertyRelative("ExitHates");
//
// 		if (property.serializedObject.targetObjects.Length > 1)
// 		{
// 			// revert to default inspector when multi-editing
// 			root.Add(new PropertyField(wantsProp));
// 			root.Add(new PropertyField(hatesProp));
// 			root.Add(new PropertyField(exitWantsProp));
// 			root.Add(new PropertyField(exitHatesProp));
// 			return root;
// 		}
//
// 		root.Add(CreateListView($"if Ghost WANTS {labelString}, say:", wantsProp));
// 		root.Add(CreateListView($"if Ghost NOT want {labelString}, say:", hatesProp));
// 		root.Add(CreateListView($"if Exit WANTS {labelString}, say:", exitWantsProp));
// 		root.Add(CreateListView($"if Exit NOT want {labelString}, say:", exitHatesProp));
//
// 		return root;
// 	}
//
// 	VisualElement CreateListView(string listLabel, SerializedProperty listProp)
// 	{
// 		var listVel = new VisualElement() { style = { marginTop = 8 } };
// 		var length = listProp.arraySize;
//
// 		var header = new VisualElement { style = { flexDirection = FlexDirection.Row } };
// 		header.Add(new Label(listLabel));
// 		header.Add(
// 			new Button(
// 				() => listProp.InsertArrayElementAtIndex(length))
// 			{
// 				text = "+"
// 			});
// 		listVel.Add(header);
//
// 		for (var i = 0; i < length; i++)
// 		{
// 			var rowVel = new VisualElement
// 			{
// 				style =
// 				{
// 					flexDirection = FlexDirection.Row,
// 					justifyContent = Justify.FlexStart,
// 					alignContent = Align.Auto,
// 					flexWrap = Wrap.NoWrap
// 				}
// 			};
//
// 			var index = i;
//
// 			rowVel.Add(
// 				new Button(
// 					() => listProp.DeleteArrayElementAtIndex(index))
// 				{
// 					text = "X"
// 				}
// 			);
//
// 			var elementPropVel = new PropertyField(listProp.GetArrayElementAtIndex(i));
// 			rowVel.Add(elementPropVel);
// 			listVel.Add(rowVel);
// 		}
//
// 		return listVel;
// 	}
// }