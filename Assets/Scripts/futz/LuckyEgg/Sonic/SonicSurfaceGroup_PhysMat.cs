using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sonic
{
[CreateAssetMenu(menuName = "Sonic SFX/Surface Group - PhysMat")]
public class SonicSurfaceGroup_PhysMat : BaseSonicSurfaceGroup
{
	public List<Entry> Entries = new();

	protected override void RenameEntries_OnValidate()
	{
		foreach (var entry in Entries) {
			entry.Key = entry.PhysicMaterial ? entry.PhysicMaterial.name : "";
		}
	}

	protected override void RebuildLup()
	{
		_lup.Clear();
		foreach (var entry in Entries) {
			_lup.Add(entry.Key, entry);
		}
	}

	[Serializable]
	public class Entry : BaseEntry
	{
		public PhysicMaterial PhysicMaterial;
	}
}
}