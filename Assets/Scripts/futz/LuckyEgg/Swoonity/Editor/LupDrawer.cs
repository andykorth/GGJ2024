// using Swoonity.Collections;
// using UnityEditor;
// using UnityEditorInternal;
// using UnityEngine;
//
// namespace Swoonity.Editor {
//
// 	[CustomPropertyDrawer(typeof(Lup<,>))]
// 	public class LupDrawer : PropertyDrawer {
//
// 		// ReorderableList _kvpRList;
//
// 		public override void OnGUI(Rect pos, SerializedProperty propLup, GUIContent label) {
// 			var propKvpList = propLup.FindPropertyRelative("KvpList");
//
// 			// if (_kvpRList == null) {
// 			// 	_kvpRList = new ReorderableList(propKvpList.serializedObject, propKvpList, true, false, true, true);
// 			// }
//
// 			var fieldName = ObjectNames.NicifyVariableName(fieldInfo.Name);
// 			// EditorGUI.PropertyField(pos, propKvpList, new GUIContent(fieldName), true);
// 			EditorGUI.PropertyField(pos, propKvpList, new GUIContent("asdfasdf"));
// 			// EditorGUI.LabelField(pos, fieldName);
// 			// _kvpRList.DoList(pos);
// 			
// 		}
//
// 		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
// 			var propKvpList = property.FindPropertyRelative("KvpList");
// 			return EditorGUI.GetPropertyHeight(propKvpList);
// 		}
// 	}
//
// }
