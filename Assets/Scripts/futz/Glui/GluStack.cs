using System.Collections.Generic;
using Lumberjack;
using Regent.Cogs;
using Regent.Syncers;
using Swoonity.Unity;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Debug;

namespace Glui
{
[RequireComponent(typeof(UIDocument))]
public class GluStack : GluComponent
{
	[Header("Refs")]
	public UIDocument RootDoc;

	[Header("Config")]
	public string StackClass = "GLU_STACK";
	public string ClickThroughClass = "CLICK_THRU";

	[Btn(nameof(BtnFindScreens), height: 60)]
	public string ScreenClass = "GLU_SCREEN";
	public List<GluScreen> AllScreens = new();

	[Header("State")]
	public VisualElement CoreRoot;
	public TrackEvt CheckVisibleScreens = new();
	public List<GluScreen> ScreenStack = new();

	public override void Preload(string reason)
	{
		Log($"PRELOAD ({reason}): {this}".LgYellow(), this);
		
		TrySet(ref RootDoc);
	}

	public override void Load(string reason)
	{
		CoreRoot = RootDoc.rootVisualElement;

		if (CoreRoot == null) {
			Log($"LOAD failed: {this}, rootVisualElement not loaded".LgRed(), this);
			return; //>> rootVisualElement not loaded
		}
		
		CoreRoot.SetClass(StackClass);
		
		Log($"LOAD ({reason}): {this}".LgGreen(), this);
	}

	public void BtnFindScreens()
	{
		GetComponentsInChildren(AllScreens);
		ForceReload(nameof(BtnFindScreens));
	}
}
}