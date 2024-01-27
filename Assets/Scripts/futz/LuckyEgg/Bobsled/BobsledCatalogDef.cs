using System;
using System.Collections.Generic;
using Bobsled.DefLoader;
using Bobsled.Defs;
using Swoonity.MHasher;
using Swoonity.Unity;
using UnityEngine;
using static UnityEngine.Debug;

namespace Bobsled.Catalog
{
public abstract class BobsledCatalogDef : ScriptableObject
{
	[Header("Bobsled Config")]
	[Tooltip("Reloads with editor OnValidate")]
	public bool AutoLoad = true;
	public bool ClickToTriggerAutoLoad = false;

	[Header("Defs")]
	public List<Definition> AllDefs;


	void OnValidate()
	{
		if (AutoLoad) BobsledCatalog.DevtimeInitialize(this);
		ClickToTriggerAutoLoad = false;
	}

	[ContextMenu("Force Load/Validate")]
	public void ForceLoadAndValidate()
	{
		BobsledCatalog.DevtimeInitialize(this);
	}

	/// called after Devtime Initialization
	public virtual void PostDevtimeInit() { }

	/// called after Runtime Initialization
	public virtual void PostRuntimeInit() { }
}

public static class BobsledCatalog
{
	public const string PRE = "<color=#58eb34>Bobsled 🛷  ";
	public const string SUF = "</color>";

	#region Dev time

	static Dictionary<MHash, Definition>
		_hashCollisionLup = new Dictionary<MHash, Definition>(64);


	public static void DevtimeInitialize(BobsledCatalogDef catalogDef)
	{
		var defs = BobsledDefLoader.FindAll();
		if (defs == null) return;

		catalogDef.AllDefs.Clear();
		_hashCollisionLup.Clear();

		foreach (var def in defs) {
			def.LoadHash();

			if (_hashCollisionLup.TryGetValue(def.Hash, out var collision)) {
				throw new Exception(
					$"Bobsled Hash collision: at {def.Hash}."
				  + $" Please rename {def.name}"
				  + $" OR {collision.name}"
				);
			}

			catalogDef.AllDefs.Add(def);
			_hashCollisionLup[def.Hash] = def;
			def.SetDirtyIfEditor();
		}

		catalogDef.PostDevtimeInit();
		catalogDef.SetDirtyIfEditor();

		Log(PRE + $"devtime initialized, found {catalogDef.AllDefs.Count} definitions" + SUF);
	}

	#endregion


	#region Run time

	public static Dictionary<MHash, Definition> DefinitionLup =
		new Dictionary<MHash, Definition>(64);


	public static void RuntimeInitialize(BobsledCatalogDef catalogDef)
	{
		DefinitionLup.Clear();

		foreach (var def in catalogDef.AllDefs) {
			DefinitionLup[def.Hash] = def;
		}

		catalogDef.PostRuntimeInit();

		// Log(PRE + $"runtime initialized" + SUF);
	}

	public static Definition GetDef(MHash hash)
		=> DefinitionLup.TryGetValue(hash, out var def) ? def : null;

	public static T GetDef<T>(MHash hash) where T : Definition
		=> DefinitionLup.TryGetValue(hash, out var def) ? (T)def : null;

	#endregion
}
}