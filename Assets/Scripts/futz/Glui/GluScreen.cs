using System.Collections.Generic;
using Lumberjack;
using Swoonity.CSharp;
using Swoonity.Unity;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Debug;

namespace Glui
{
[RequireComponent(typeof(UIDocument))]
public class GluScreen : GluComponent
{
	[Header("UI Screen Config")]
	public GluStack Stack;
	public UIDocument ScreenDoc;
	public bool StartShown;

	[Btn(nameof(BtnFindWindows), height: 60)]
	public string CustomScreenClass;
	public List<GluWindow> Windows = new();

	[Header("Screen State")]
	public VisualElement ScreenRoot;
	public GluVis DesiredVis;
	public GluVis CurrentVis;
	public bool IsVisible => CurrentVis == GluVis.VISIBLE;

	public override void Preload(string reason)
	{
		Log($"PRELOAD ({reason}): {this}".LgYellow(), this);

		if (CustomScreenClass.Nil()) CustomScreenClass = name;
		TrySetParent(ref Stack);
		TrySet(ref ScreenDoc);

		foreach (var window in Windows) {
			window.TryPreload("Screen.Preload", forcePreload: true);
		}
	}

	public override void Load(string reason)
	{
		ScreenRoot = ScreenDoc.rootVisualElement;
		if (ScreenRoot == null) {
			Log($"LOAD ({reason}) failed: {this}, rootVisualElement not loaded".LgRed(), this);
			return; //>> rootVisualElement not loaded
		}

		ScreenRoot.SetClass(Stack.ScreenClass);
		ScreenRoot.SetClass(CustomScreenClass);

		Log($"LOAD ({reason}): {this}".LgGreen(), this);

		foreach (var window in Windows) {
			window.Load("Screen.Load");
		}
	}

	public void BtnFindWindows()
	{
		GetComponentsInChildren(Windows);
		ForceReload(nameof(BtnFindWindows));
	}

	public void SetVisible() => SetVis(GluVis.VISIBLE);
	public void SetHidden() => SetVis(GluVis.HIDDEN);
	public void ToggleVis()
		=> SetVis(DesiredVis != GluVis.VISIBLE ? GluVis.VISIBLE : GluVis.HIDDEN);

	public void SetVis(GluVis vis)
	{
		DesiredVis = vis;
		if (Stack) Stack.CheckVisibleScreens.Trigger();
	}

	public virtual void ApplyVisibilityStyle(bool isVisible)
		=> ScreenRoot.style.display = isVisible
			? DisplayStyle.Flex
			: DisplayStyle.None;
}
}