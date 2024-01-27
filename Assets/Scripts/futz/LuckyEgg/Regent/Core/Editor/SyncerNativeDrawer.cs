using System;
using Regent.Syncers;
using UnityEditor;
using UnityEngine;

namespace RegentEditor
{
[CustomPropertyDrawer(typeof(Track<>))]
public class SyncerNativeDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty propSyncer, GUIContent label)
	{
		if (propSyncer == null) return; // I guess?

		var propCurrent = propSyncer.FindPropertyRelative("Current");

		if (propCurrent == null) {
			throw new Exception($"SyncerNative {propSyncer.name} must be serializable");
		}

		// var valueTypeStr = CleanTypeName(propCurrent.type);
		/* if this throws a null ref, make sure that Native<T> T is serializable */

		// var name = $"{propSyncer.name} Native<{valueTypeStr}>";
		var name = $"{propSyncer.name} NATIVE";


		if (!propSyncer.isExpanded) {
			propSyncer.isExpanded = EditorGUI.Foldout(
				position,
				propSyncer.isExpanded,
				new GUIContent(name)
			);
			return;
		}

		var pastIndent = EditorGUI.indentLevel;
		var pastGuiEnabled = GUI.enabled;
		EditorGUI.indentLevel = 0;
		GUI.enabled = false;

		EditorGUI.PropertyField(position, propCurrent, new GUIContent(name), true);

		EditorGUI.indentLevel = pastIndent;
		GUI.enabled = pastGuiEnabled;
	}

	public override float GetPropertyHeight(SerializedProperty propSyncer, GUIContent label)
	{
		var propCurrent = propSyncer.FindPropertyRelative("Current");
		return EditorGUI.GetPropertyHeight(propCurrent, true);
	}

	public static string CleanTypeName(string str)
		=> str
		   .Replace("`1", "")
		   .Replace("PPtr<$", "")
		   .Replace(">", "");
}
}