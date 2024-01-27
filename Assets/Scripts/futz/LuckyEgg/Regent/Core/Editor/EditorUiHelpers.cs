using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace RegentEditor
{
public static class EditorUiHelpers
{
	/// this breaks headers :(
	public static void FillDefaultInspector(VisualElement root, SerializedObject sobj)
	{
		var property = sobj.GetIterator();
		if (!property.NextVisible(true)) return;

		do {
			var field = new PropertyField(property);
			field.name = "PropertyField:" + property.propertyPath;

			var isScriptField = property.propertyPath == "m_Script"
			                 && sobj.targetObject != null;
			if (isScriptField) {
				field.SetEnabled(false);
			}

			root.Add(field);
		} while (property.NextVisible(false));
	}

	public static VisualElement Spacer(float height)
		=> new VisualElement { style = { height = height } };

	public static string CleanTypeName(string str)
		=> str
		   .Replace("`1", "")
		   .Replace("PPtr<$", "")
		   .Replace(">", "");
}
}