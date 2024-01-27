using System;
using System.Linq;
using Swoonity.CSharp;
using UnityEngine;
using UnityEngine.UIElements;
using Vel = UnityEngine.UIElements.VisualElement;

namespace Swoonity.Unity
{
public static class ElementUtils
{
	// ReSharper disable once UnusedParameter.Global
	public static T Q<T>(this Vel el, string name, T refToType)
		where T : Vel
	{
		return el.Q<T>(name);
	}

	public static VisualTreeAsset OrLoad(this VisualTreeAsset asset, string path)
		=> asset ? asset : Resources.Load<VisualTreeAsset>(path);

	public static StyleSheet OrLoad(this StyleSheet asset, string path)
		=> asset ? asset : Resources.Load<StyleSheet>(path);


	/// use: NewButton().OnClick() etc.
	public static Vel NewButton_DEPRECATED(
		this Vel root,
		string text = "TODO",
		Action onClick = null,
		string tooltip = "",
		float height = 40
	)
	{
		var button = new Button {
			text = text,
			tooltip = tooltip,
			style = { height = height }
		};

		if (onClick != null)
			button.clickable.clicked += onClick;

		root.Add(button);

		return button;
	}


	/// shortcut for template.Instantiate().Children().FirstOrDefault();
	/// to get around Unity's stupid TemplateContainer
	public static Vel Spawn(this VisualTreeAsset template)
	{
		var templateContainer = template.Instantiate();
		return templateContainer.Children().FirstOrDefault();
	}

	public static Vel FirstChild(this Vel el)
	{
		if (el.childCount < 1) return null;
		return el.Children().FirstOrDefault();
	}

	public static void Show(this Vel el) => el.style.display = DisplayStyle.Flex;
	public static void Hide(this Vel el) => el.style.display = DisplayStyle.None;

	public static void SetShow(this Vel el, bool show)
		=> el.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;

	// public static Vel AddClass(this Vel el, string ussClass) {
	// 	if (!string.IsNullOrEmpty(ussClass)) el.AddToClassList(ussClass);
	// 	return el;
	// }


	public static Vel NewElement(this Vel parent, string className = "", string name = "")
	{
		var child = new Vel();
		if (className != "") child.AddToClassList(className);
		if (name != "") child.name = name;
		parent.Add(child);
		return child;
	}

	public static Vel NewRow(
		this Vel parent,
		Justify justifyContent = Justify.FlexStart,
		Align alignContent = Align.Auto,
		Wrap wrap = Wrap.NoWrap
	)
	{
		var child = new Vel();
		parent.Add(child);
		child.style.flexDirection = FlexDirection.Row;
		child.style.justifyContent = justifyContent;
		child.style.alignContent = alignContent;
		child.style.flexWrap = wrap;
		return child;
	}

	public static Label NewLabel(
		this Vel parent,
		string text = "",
		string className = "",
		string name = ""
	)
	{
		var label = new Label {
			text = text,
		};
		if (className != "") label.AddToClassList(className);
		if (name != "") label.name = name;
		parent.Add(label);
		return label;
	}

	public static Vel Spacer(this Vel parent, float size)
	{
		var child = new Vel {
			style = {
				height = size,
				width = size
			}
		};
		parent.Add(child);
		return child;
	}

	public static Vel SpacerInColumn(this Vel parent, float height = 8)
	{
		var child = new Vel { style = { height = height } };
		parent.Add(child);
		return child;
	}

	public static Vel SpacerInRow(this Vel parent, float width = 8)
	{
		var child = new Vel { style = { width = width } };
		parent.Add(child);
		return child;
	}

	public static Button NewButton(this Vel el, string text)
	{
		var button = new Button {
			text = text,
		};

		el.Add(button);
		return button;
	}

	public static Button NewButton(this Vel el, string text, Action onClick)
		=> el.NewButton(text).OnClick(onClick);

	public static Button NewButton(this Vel el, string text, Action onClick, string tip)
		=> el.NewButton(text).Tooltip(tip).OnClick(onClick);


	/// chainable
	public static T SetClass<T>(this T el, string className, bool shouldAdd = true) where T : Vel
	{
		if (className.Nil()) return el;

		if (shouldAdd) el.AddToClassList(className);
		else el.RemoveFromClassList(className);
		return el;
	}

	public static T UssClass<T>(this T el, string ussName) where T : Vel
	{
		if (ussName.Nil()) return el;
		el.AddToClassList(ussName);
		return el;
	}

	public static T AddClass<T>(this T el, string className) where T : Vel
	{
		if (className.Nil()) return el;
		el.AddToClassList(className);
		return el;
	}

	public static T CutClass<T>(this T el, string className) where T : Vel
	{
		if (className.Nil()) return el;
		el.RemoveFromClassList(className);
		return el;
	}

	public static T Tooltip<T>(this T el, string tooltip) where T : Vel
	{
		el.tooltip = tooltip;
		return el;
	}

	public static T Label<T>(this T el, string label) where T : TextElement
	{
		el.text = label;
		return el;
	}

	public static Button OnClick(this Button btn, Action onClick)
	{
		btn.clicked += onClick;
		return btn;
	}

	public static T OnClick<T>(this T el, EventCallback<ClickEvent> onClick) where T : Vel
	{
		el.RegisterCallback(onClick);
		return el;
	}


	/// outs & returns found element
	public static T QQ<T>(this Vel parent, out T el, string name) where T : Vel
	{
		el = parent.Q<T>(name);
		if (el == null) {
			throw new Exception($"{parent} missing {typeof(T).Name} {name.OrPh()}");
		}

		return el;
	}

	// /// QQ but returns original parent (for chaining purposes)
	// public static Vel QQand<T>(this Vel parent, out T el, string name) where T : Vel {
	// 	el = parent.Q<T>(name);
	// 	if (el == null) {
	// 		throw new Exception($"{parent} missing {typeof(T).Name} {name.OrPh()}");
	// 	}
	//
	// 	return parent;
	// }

	public static void TryAdd(this Vel parent, Vel el)
	{
		if (el != null) parent.Add(el);
	}

	public static bool Has(this Vel el, string ussClass) => el.ClassListContains(ussClass);

	public static T SetColor<T>(this T el, StyleColor color) where T : Vel
	{
		el.style.color = color;
		return el;
	}

	/// el.style.backgroundColor = color
	public static T SetBgColor<T>(this T el, StyleColor color) where T : Vel
	{
		el.style.backgroundColor = color;
		return el;
	}

	public static T SetBgColor<T>(this T el, string color) where T : Vel
	{
		ColorUtility.TryParseHtmlString(color, out var colorValue);
		el.style.backgroundColor = colorValue;
		return el;
	}

	public static T SetBgBlack<T>(this T el, float alpha = 1f) where T : Vel
	{
		el.style.backgroundColor = new Color(0, 0, 0, alpha);
		return el;
	}

	/// el.style.backgroundImage = new StyleBackground(sprite)
	public static T SetBg<T>(this T el, Sprite sprite) where T : Vel
	{
		el.style.backgroundImage = new StyleBackground(sprite);
		return el;
	}

	/// el.style.backgroundImage = new StyleBackground(sprite ? sprite : or)
	public static T SetBg<T>(this T el, Sprite sprite, Sprite or) where T : Vel
	{
		el.style.backgroundImage = new StyleBackground(sprite ? sprite : or);
		return el;
	}

	/// el.style.backgroundImage = new StyleBackground(sprite)
	/// el.style.unityBackgroundImageTintColor = tint;
	public static T SetBg<T>(this T el, Sprite sprite, StyleColor tint) where T : Vel
	{
		el.style.backgroundImage = new StyleBackground(sprite);
		el.style.unityBackgroundImageTintColor = tint;
		return el;
	}

	/// el.style.unityBackgroundImageTintColor = tint;
	public static T SetBgTint<T>(this T el, StyleColor tint) where T : Vel
	{
		el.style.unityBackgroundImageTintColor = tint;
		return el;
	}

	public static T SetTextStyle<T>(
		this T el,
		int size = default,
		FontStyle style = FontStyle.Normal
	) where T : Vel
	{
		if (size > 0) el.style.fontSize = size;
		el.style.unityFontStyleAndWeight = style;
		return el;
	}

	public static T SetFlex<T>(this T el, float grow, float shrink, StyleLength basis) where T : Vel
	{
		el.style.flexGrow = grow;
		el.style.flexShrink = shrink;
		el.style.flexBasis = basis;
		return el;
	}

	public static T SetFlex<T>(this T el, float grow, float shrink) where T : Vel
	{
		el.style.flexGrow = grow;
		el.style.flexShrink = shrink;
		el.style.flexBasis = StyleKeyword.Auto;
		return el;
	}

	public static T SetRow<T>(
		this T el,
		Justify justifyContent = Justify.FlexStart,
		Align alignContent = Align.Auto,
		Wrap wrap = Wrap.NoWrap
	) where T : Vel
	{
		el.style.flexDirection = FlexDirection.Row;
		el.style.justifyContent = justifyContent;
		el.style.alignContent = alignContent;
		el.style.flexWrap = wrap;
		return el;
	}

	public static T SetColumn<T>(this T el) where T : Vel
	{
		el.style.flexDirection = FlexDirection.Column;
		return el;
	}

	public static T SetJustify<T>(this T el, Justify justify) where T : Vel
	{
		el.style.justifyContent = justify;
		return el;
	}

	public static T SetAlign<T>(this T el, Align align) where T : Vel
	{
		el.style.alignItems = align;
		return el;
	}

	public static T Name<T>(this T el, string name) where T : Vel
	{
		el.name = name;
		return el;
	}

	public static T PadTop<T>(this T el, StyleLength val) where T : Vel
	{
		el.style.paddingTop = val;
		return el;
	}

	public static T PadBottom<T>(this T el, StyleLength val) where T : Vel
	{
		el.style.paddingBottom = val;
		return el;
	}

	public static T PadLeft<T>(this T el, StyleLength val) where T : Vel
	{
		el.style.paddingLeft = val;
		return el;
	}

	public static T PadRight<T>(this T el, StyleLength val) where T : Vel
	{
		el.style.paddingRight = val;
		return el;
	}

	public static T Pad<T>(this T el, StyleLength all) where T : Vel
	{
		el.style.paddingTop = all;
		el.style.paddingBottom = all;
		el.style.paddingRight = all;
		el.style.paddingLeft = all;
		return el;
	}

	public static T Pad<T>(this T el, StyleLength topBottom, StyleLength leftRight) where T : Vel
	{
		el.style.paddingTop = topBottom;
		el.style.paddingBottom = topBottom;
		el.style.paddingRight = leftRight;
		el.style.paddingLeft = leftRight;
		return el;
	}

	public static T Pad<T>(
		this T el,
		StyleLength top,
		StyleLength right,
		StyleLength bottom,
		StyleLength left
	) where T : Vel
	{
		el.style.paddingTop = top;
		el.style.paddingBottom = bottom;
		el.style.paddingRight = right;
		el.style.paddingLeft = left;
		return el;
	}

	public static T MarTop<T>(this T el, StyleLength val) where T : Vel
	{
		el.style.marginTop = val;
		return el;
	}

	public static T MarBottom<T>(this T el, StyleLength val) where T : Vel
	{
		el.style.marginBottom = val;
		return el;
	}

	public static T MarLeft<T>(this T el, StyleLength val) where T : Vel
	{
		el.style.marginLeft = val;
		return el;
	}

	public static T MarRight<T>(this T el, StyleLength val) where T : Vel
	{
		el.style.marginRight = val;
		return el;
	}

	public static T Mar<T>(this T el, StyleLength all) where T : Vel
	{
		el.style.marginTop = all;
		el.style.marginBottom = all;
		el.style.marginRight = all;
		el.style.marginLeft = all;
		return el;
	}

	public static T Mar<T>(this T el, StyleLength topBottom, StyleLength leftRight) where T : Vel
	{
		el.style.marginTop = topBottom;
		el.style.marginBottom = topBottom;
		el.style.marginRight = leftRight;
		el.style.marginLeft = leftRight;
		return el;
	}

	public static T Mar<T>(
		this T el,
		StyleLength top,
		StyleLength right,
		StyleLength bottom,
		StyleLength left
	) where T : Vel
	{
		el.style.marginTop = top;
		el.style.marginBottom = bottom;
		el.style.marginRight = right;
		el.style.marginLeft = left;
		return el;
	}

	public static T Width<T>(this T el, StyleLength val) where T : Vel
	{
		el.style.width = val;
		return el;
	}

	public static T Height<T>(this T el, StyleLength val) where T : Vel
	{
		el.style.height = val;
		return el;
	}
}
}