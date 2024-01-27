namespace _Code.Swoonity.Editor
{
public static class EditorGuiUtils
{
	//		public static T GetPropertyDrawerTarget<T>(this SerializedProperty property, FieldInfo fieldInfo) where T : class {
	//			var obj = fieldInfo.GetValue(property.serializedObject.targetObject);
	//			if (obj == null) return null;
	//
	//			T actualObject = null;
	//			if (obj.GetType().IsArray) {
	//				var index = Convert.ToInt32(new string(property.propertyPath.Where(c => char.IsDigit(c)).ToArray()));
	//				actualObject = ((T[])obj)[index];
	//			} else {
	//				actualObject = obj as T;
	//			}
	//			return actualObject;
	//		}
}
}


//[CustomPropertyDrawer(typeof(MyDataClass))]
//public class MyDataClassDrawer : PropertyDrawer
//{
//	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//	{
//		if (GUI.Button(position, "Do Something"))
//		{
//			MyDataClass myDataClass = PropertyDrawerUtility.GetActualObjectForSerializedProperty<MyDataClass>(fieldInfo, property);
//			myDataClass.DoSomething();
//		}
//	}
//}