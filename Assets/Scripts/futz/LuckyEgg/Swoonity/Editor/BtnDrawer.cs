using System;
using Swoonity.CSharp;
using Swoonity.Unity;
using UnityEditor;
using UnityEngine;

namespace Swoonity.Editor
{
[CustomPropertyDrawer(typeof(BtnAttribute))]
public class BtnDrawer : PropertyDrawer
{
	const float BTN_SPACE = 4;

	public override void OnGUI(Rect baseRect, SerializedProperty property, GUIContent label)
	{
		var btn = attribute as BtnAttribute;
		if (btn == null) return;

		var propRect = new Rect(
			baseRect.x,
			baseRect.y + btn.Height + BTN_SPACE,
			baseRect.width,
			baseRect.height - btn.Height - BTN_SPACE
		);

		var btnCount = 1
		             + (btn.FuncName2.Any() ? 1 : 0)
		             + (btn.FuncName3.Any() ? 1 : 0);

		var rowWidth = baseRect.width;
		var btnWidth = rowWidth / btnCount;

		var btnRect = new Rect(
			baseRect.x,
			baseRect.y,
			btnWidth,
			btn.Height
		);

		DrawBtn(property, btn.FuncName1, btnRect);

		btnRect = btnRect.AddX(btnWidth);
		if (btnCount >= 2) DrawBtn(property, btn.FuncName2, btnRect);

		btnRect = btnRect.AddX(btnWidth);
		if (btnCount >= 3) DrawBtn(property, btn.FuncName3, btnRect);


		EditorGUI.PropertyField(propRect, property, label, true);
	}

	void DrawBtn(SerializedProperty property, string funcName, Rect rect)
	{
		if (!GUI.Button(rect, GetButtonLabel(funcName))) return; //>> btn not pressed

		var targetObj = property.serializedObject.targetObject;

		var fieldMethod = fieldInfo.FieldType.GetAnyMethod(funcName, false);
		if (fieldMethod != null) {
			// TODO: re-enable this
			// fieldMethod.Invoke(property.boxedValue, null);
			// EditorUtility.SetDirty(targetObj);
			return; //>> found method on field
		}

		var compMethod = targetObj.GetType().GetAnyMethod(funcName, false);
		if (compMethod != null) {
			compMethod.Invoke(targetObj, null);
			EditorUtility.SetDirty(targetObj);
			return; //>> found method on component
		}

		var propObjName = property.propertyPath.SplitFirst(".");
		var propObjField = targetObj.GetType().GetField(propObjName);
		if (propObjField != null) {
			var objInstance = propObjField.GetValue(targetObj);

			var objMethod = propObjField.FieldType.GetAnyMethod(funcName, false);
			if (objMethod != null) {
				objMethod.Invoke(objInstance, null);
				EditorUtility.SetDirty(targetObj);
				return; //>> found method on prop obj
			}
		}

		throw new Exception(
			$"Btn: missing method {funcName} on"
		  + $" {fieldInfo.FieldType}"
		  + $" OR {targetObj.GetType()}"
		  + $" OR {propObjName}"
		);
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		var btn = attribute as BtnAttribute;
		// return base.GetPropertyHeight(property, label) + btn.Height + BTN_SPACE;
		return EditorGUI.GetPropertyHeight(property, label) + btn.Height + BTN_SPACE;
	}

	static string GetButtonLabel(string funcName)
		=> funcName.Replace("btn", "", StringComparison.OrdinalIgnoreCase);
}
}