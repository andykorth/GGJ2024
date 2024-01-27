using System;
using System.Collections.Generic;
using UnityEngine;
using uObject = UnityEngine.Object;

namespace Sonic
{
public abstract class BaseSonicSurfaceGroup : ScriptableObject
{
	public BaseEntry Fallback = new();

	protected Dictionary<string, BaseEntry> _lup = new();

	void OnValidate()
	{
		RenameEntries_OnValidate();
		RebuildLup(); // to catch changes without needing domain reload
	}

	protected abstract void RenameEntries_OnValidate();
	protected abstract void RebuildLup();

	public virtual (bool wasFound, BaseEntry entry) GetEntry(string key)
	{
		if (_lup.Count == 0) RebuildLup();

		var wasFound = _lup.TryGetValue(key, out var foundEntry);
		return (wasFound, wasFound ? foundEntry : Fallback);
	}

	public virtual (bool wasFound, BaseEntry entry) GetEntry(uObject uObj)
		=> uObj ? GetEntry(uObj.name) : (false, Fallback);

	public virtual SonicSfx GetSfx(string key, float force) => GetEntry(key).entry?.GetSfx(force);

	public virtual SonicSfx GetSfx(uObject uObj, float force)
		=> uObj ? GetEntry(uObj.name).entry?.GetSfx(force) : Fallback.GetSfx(force);

	[Serializable]
	public class BaseEntry
	{
		public string Key;

		public float LightThreshold = 1;
		public SonicSfx Light;

		public float MediumThreshold = 4;
		public SonicSfx Medium;

		public float SeriousThreshold = 10;
		public SonicSfx Serious;

		public SonicSfx GetSfx(float force)
		{
			if (force >= SeriousThreshold) return Serious;
			if (force >= MediumThreshold) return Medium;
			if (force >= LightThreshold) return Light;
			return null;
		}
	}
}
}