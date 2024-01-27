using System.Collections.Generic;
using Regent.Annotation;
using Regent.Entities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace RegentEditor
{
// [CanEditMultipleObjects]
[CustomEditor(typeof(Entity))]
public class EntityEditor : Editor
{
	public const string UXML_PATH = "EntityEditor";
	const string LblName = nameof(LblName);
	const string LblIdent = nameof(LblIdent);
	const string BtnParent = nameof(BtnParent);
	const string BtnUp = nameof(BtnUp);


	public static VisualTreeAsset VisualAsset;
	static List<IAnnotation> _annotations = new();

	void OnSceneGUI()
	{
		var entity = target as Entity;
		if (!entity) return; //>> bad editor state;

		_annotations.Clear();
		entity.GetComponents(_annotations);
		foreach (var annotation in _annotations) {
			annotation.Annotate();
		}
	}


	public override VisualElement CreateInspectorGUI()
	{
		var root = new VisualElement();

		if (!VisualAsset) VisualAsset = Resources.Load<VisualTreeAsset>(UXML_PATH);
		VisualAsset.CloneTree(root);

		var entity = serializedObject.targetObject as Entity;
		if (!entity) return root; //>> bad editor state;


		var lblName = root.Q<Label>(LblName);
		var lblIdent = root.Q<Label>(LblIdent);
		var btnParent = root.Q<Button>(BtnParent);
		var btnUp = root.Q<Button>(BtnUp);

		lblName.text = GetNameString(entity);
		lblIdent.text = GetIdentString(entity);

		btnParent.clicked += () => SelectParentEntity(entity);
		btnParent.style.display = entity.ParentEntity ? DisplayStyle.Flex : DisplayStyle.None;

		btnUp.clicked += () => MoveUp(entity);
		btnUp.style.display = !entity.IsRegistered ? DisplayStyle.Flex : DisplayStyle.None;

		return root;
	}

	static string GetNameString(Entity entity)
	{
		return entity.IsRegistered
			? $"#{entity.EntityId}"
			: "--";
	}

	static string GetIdentString(Entity entity)
	{
		if (entity.IsNative) return $"native";

		if (!entity.IsRegistered) return $"net";

		var auth = entity.IsAuthor ? "author" : "remote";

		return entity.IsServer
			? entity.IsClient
				? $"HOST {auth}"
				: $"SERVER {auth}"
			: $"CLIENT {auth}";
	}

	static void SelectParentEntity(Entity entity)
	{
		if (entity.ParentEntity) Selection.activeObject = entity.ParentEntity;
	}

	static void MoveUp(Entity entity)
	{
		for (var i = 0; i < 10; i++) {
			UnityEditorInternal.ComponentUtility.MoveComponentUp(entity);
		}
	}
}
}