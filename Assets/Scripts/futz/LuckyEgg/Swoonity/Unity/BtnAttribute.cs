using System;
using UnityEngine;

namespace Swoonity.Unity
{
/// run func when clicked then WILL mark object as Dirty
/// (label will remove "btn")
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class BtnAttribute : PropertyAttribute
{
	public readonly string FuncName1;
	public readonly string FuncName2;
	public readonly string FuncName3;
	public readonly int Height;

	public BtnAttribute(
		string funcName1,
		string funcName2 = "",
		string funcName3 = "",
		int height = 30
	)
	{
		FuncName1 = funcName1;
		FuncName2 = funcName2;
		FuncName3 = funcName3;
		Height = height;
	}
}
}