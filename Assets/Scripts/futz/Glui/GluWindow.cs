using System.Collections.Generic;
using Idealist;
using Lumberjack;
using Regent.Syncers;
using Swoonity.CSharp;
using Swoonity.Unity;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Debug;

namespace Glui
{
public abstract class GluWindow : GluComponent
{
	[Header("UI Window Config")]
	[Btn(nameof(BtnCheckRefs))]
	public string WindowName;
	[ReadOnly] public string WindowUssClass;
	public GluDragMethod DragMethod;

	[Header("Window State")]
	public List<GluElRef> ElRefs = new();
	public List<GluCEllPoolBase> Pools = new();
	public GluScreen Screen;
	public VisualElement WindowEl;
	public GluDragger Dragger;

	public override void Preload(string reason)
	{
		var windowType = GetType();
		WindowUssClass = windowType.Name;
		if (WindowName.Nil()) WindowName = windowType.Name;

		TrySetParent(ref Screen);

		ElRefs.Clear();
		var elRefFields = windowType.ListFieldsWithBaseType<GluElRef>();

		foreach (var elRefField in elRefFields) {
			var elRef = elRefField.GetFieldValue<GluElRef>(this);

			if (elRef.Name.Nil()) {
				elRef.Name = GluDevLogic.MakeElRefName(elRefField.Name);
			}

			// facts.Add(new GluElRef.Fact {Name = elRef.Name});
			ElRefs.Add(elRef);
		}

		Log(
			ElRefs.JoinPrefix(
				$"PRELOAD ({reason}) {this} {ElRefs.Count} ElRefs:",
				static el => el.Name
			).LgYellow()
		);


		var poolFields = windowType.ListFieldsWithBaseType<GluCEllPoolBase>();

		foreach (var poolField in poolFields) {
			var pool = poolField.GetFieldValue<GluCEllPoolBase>(this);
			Pools.Add(pool);
			pool.Preload();
		}
	}

	/// called by parent Screen's OnEnable AND OnValidate (so it will be visible in editor)
	public override void Load(string reason)
	{
		if (!Screen || Screen.ScreenRoot == null) {
			Log($"LOAD ({reason}) failed: {this}, parent screen not loaded".LgRed(), this);
			return; //>> parent screen not loaded
		}

		var windowEl = Screen.ScreenRoot.Q(WindowName);

		if (windowEl == null) {
			Log($"LOAD ({reason}) failed: {this} missing {WindowName} element".LgRed(), this);
			return;
		}


		Log($"LOAD ({reason}): {this},  {ElRefs.Count} ElRefs".LgGreen(), this);

		if (ElRefs.Nil()) Log($"{WindowName} empty ElRefs".LgRed(), this);

		foreach (var elRef in ElRefs) {
			var isLoaded = elRef.Load(windowEl);
			if (!isLoaded) {
				Log($"    - {WindowName} missing: {elRef}".LgRed(), this);
			}
		}

		WindowEl = windowEl;
		WindowEl.SetClass(WindowUssClass);

		foreach (var pool in Pools) {
			pool.ParentEl = WindowEl;
			pool.RuntimeInitialize();
		}

		if (Dragger != null) GetDraggableElement().RemoveManipulator(Dragger);

		if (DragMethod != GluDragMethod.NONE) {
			Dragger = new GluDragger(DragMethod);
			GetDraggableElement().AddManipulator(Dragger);
		}

		AfterLoad();
	}

	public void BtnCheckRefs()
	{
		Screen.ForceReload(nameof(BtnCheckRefs));
	}

	public virtual void AfterLoad() { }

	// TODO: decide/choose a pattern
	public virtual void Refresh() { }
	public virtual TrackEvt GetRefresher() => null;

	public virtual void TriggerRefresh()
	{
		Refresh();
		GetRefresher()?.Trigger();
	}

	public virtual VisualElement GetDraggableElement() => WindowEl;


	public override string ToString() => WindowName.Or(name);
}
}