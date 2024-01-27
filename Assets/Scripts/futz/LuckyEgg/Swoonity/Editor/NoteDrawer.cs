using System;
using Swoonity.CSharp;
using Swoonity.Unity;
using UnityEditor;
using UnityEngine;

namespace Swoonity.Editor
{
[CustomPropertyDrawer(typeof(NoteAttribute), true)]
public class NoteDrawer : DecoratorDrawer
{
	const float MIN_HEIGHT = 36;

	float _height = MIN_HEIGHT;

	public override float GetHeight()
	{
		var note = attribute as NoteAttribute;
		if (note == null) return base.GetHeight();

		return Mathf.Max(_height, MIN_HEIGHT) + note.MarginTop;
	}

	public override void OnGUI(Rect position)
	{
		var note = attribute as NoteAttribute;
		if (note == null) return;

		var messageType = GetMessageType(note.MessageType);

		if (note.MarginTop.IsZero()) {
			EditorGUI.HelpBox(position, note.Text, messageType);
			return;
		}


		EditorGUILayout.BeginVertical();

		EditorGUILayout.Space(note.MarginTop);

		var boxPos = position
		   .WithHeight(position.height - note.MarginTop)
		   .WithY(position.y + note.MarginTop);

		EditorGUI.HelpBox(boxPos, note.Text, messageType);

		EditorGUILayout.EndVertical();

		var style = GUI.skin.GetStyle("helpbox");
		_height = style.CalcHeight(
			new GUIContent(note.Text),
			EditorGUIUtility.currentViewWidth
		);

		// EditorGUILayout.PropertyField(property, label);
	}

	public static MessageType GetMessageType(NoteMsgType noteMsgType)
		=> noteMsgType switch {
			NoteMsgType.None => MessageType.None,
			NoteMsgType.Info => MessageType.Info,
			NoteMsgType.Warning => MessageType.Warning,
			NoteMsgType.Error => MessageType.Error,
			_ => throw new ArgumentOutOfRangeException(nameof(noteMsgType), noteMsgType, null)
		};
	//
	// public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
	// 	var note = attribute as NoteAttribute;
	// 	if (note == null) return base.GetPropertyHeight(property, label);
	// 	
	// 	var style = GUI.skin.GetStyle("helpbox");
	//
	// 	var noteHeight = style.CalcHeight(
	// 		new GUIContent(note.Text),
	// 		EditorGUIUtility.currentViewWidth
	// 	);
	//
	// 	return noteHeight 
	// 	       + base.GetPropertyHeight(property, label);
	// }
	//
	// public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
	// 	var note = attribute as NoteAttribute;
	// 	if (note == null) return;
	// 	
	// 	EditorGUILayout.HelpBox(note.Text, note.MessageType);
	// 	// EditorGUILayout.PropertyField(property, label);
	// }
}
}