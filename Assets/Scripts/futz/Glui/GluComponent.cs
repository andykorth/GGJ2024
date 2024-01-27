using Lumberjack;
using Regent.Cogs;
using UnityEngine;
using static UnityEngine.Debug;

namespace Glui
{
// making this a cog for now?
[ExecuteInEditMode]
public abstract class GluComponent : CogNative
{
	// protected override void InternalWhenEditorValidates()
	// {
	// 	base.InternalWhenEditorValidates();
	// 	TryPreload("OnValidate", forcePreload: true);
	// 	Load("OnValidate");
	// }

	public void OnEnable()
	{
		Log($"{this} OnEnable".LgBlue(), this);
		TryPreload("OnEnable", forcePreload: InEditMode);
		Load("OnEnable");
	}


	bool _isPreloaded;

	/// first checks (non-serialized) bool _isPreloaded
	public void TryPreload(string reason, bool forcePreload = false)
	{
		if (_isPreloaded && !forcePreload) return; //>> already preloaded
		Preload(reason);
		_isPreloaded = true;
	}

	/// anything that can be done WITHOUT the root VisualElement reference
	public abstract void Preload(string reason);

	/// anything that REQUIRES the root VisualElement reference
	public abstract void Load(string reason);


	public void ForceReload(string reason)
	{
		TryPreload(reason, true);
		Load(reason);
	}

	public override string ToString() => name;
}

/// visibility enum
public enum GluVis
{
	UNSET,
	VISIBLE,
	HIDDEN,
}
}