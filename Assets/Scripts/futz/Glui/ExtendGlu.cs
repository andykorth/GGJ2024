using System;
using Swoonity.CSharp;
using UnityEngine.UIElements;
using Vel = UnityEngine.UIElements.VisualElement;

namespace Glui
{
public static class ExtendGlu
{
	/// outs & returns found element, or throws
	public static TEl Glu<TEl>(this Vel parent, out TEl el, string name) where TEl : Vel
	{
		el = parent.Q<TEl>(name);
		if (el != null) return el;

		throw new Exception($"{parent} missing {typeof(TEl).Name} {name.OrPh()}");
	}
}
}