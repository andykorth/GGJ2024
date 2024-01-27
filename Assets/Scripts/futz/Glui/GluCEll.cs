using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Glui
{
[Serializable]
public abstract class GluCEll
{
	public string CellName;
	public bool IsLoaded;
	public VisualElement Root;
	List<GluElRef.Fact> _facts; // don't serialize (pool will hold facts instead)

	Action _fnRelease;
	public void Release() => _fnRelease();

	public virtual void BeforeRelease() { }
	public virtual void AfterRelease() { }

	public bool LoadCell(
		string cellName,
		VisualElement root,
		List<GluElRef.Fact> facts,
		Action fnRelease
	)
	{
		CellName = cellName;
		Root = root;
		_facts = facts;
		_fnRelease = fnRelease;
		return IsLoaded = CallLoad();
	}

	protected abstract bool CallLoad();

	#region Loaders

	protected bool Load() => true;

	protected bool Load(GluElRef elRef0) => elRef0.Load(Root, _facts[0]);

	protected bool Load(
		GluElRef elRef0,
		GluElRef elRef1
	)
		=> elRef0.Load(Root, _facts[0])
		&& elRef1.Load(Root, _facts[1]);

	protected bool Load(
		GluElRef elRef0,
		GluElRef elRef1,
		GluElRef elRef2
	)
		=> elRef0.Load(Root, _facts[0])
		&& elRef1.Load(Root, _facts[1])
		&& elRef2.Load(Root, _facts[2]);

	protected bool Load(
		GluElRef elRef0,
		GluElRef elRef1,
		GluElRef elRef2,
		GluElRef elRef3
	)
		=> elRef0.Load(Root, _facts[0])
		&& elRef1.Load(Root, _facts[1])
		&& elRef2.Load(Root, _facts[2])
		&& elRef3.Load(Root, _facts[3]);

	protected bool Load(
		GluElRef elRef0,
		GluElRef elRef1,
		GluElRef elRef2,
		GluElRef elRef3,
		GluElRef elRef4
	)
		=> elRef0.Load(Root, _facts[0])
		&& elRef1.Load(Root, _facts[1])
		&& elRef2.Load(Root, _facts[2])
		&& elRef3.Load(Root, _facts[3])
		&& elRef4.Load(Root, _facts[4]);

	protected bool Load(
		GluElRef elRef0,
		GluElRef elRef1,
		GluElRef elRef2,
		GluElRef elRef3,
		GluElRef elRef4,
		GluElRef elRef5
	)
		=> elRef0.Load(Root, _facts[0])
		&& elRef1.Load(Root, _facts[1])
		&& elRef2.Load(Root, _facts[2])
		&& elRef3.Load(Root, _facts[3])
		&& elRef4.Load(Root, _facts[4])
		&& elRef5.Load(Root, _facts[5]);

	protected bool Load(
		GluElRef elRef0,
		GluElRef elRef1,
		GluElRef elRef2,
		GluElRef elRef3,
		GluElRef elRef4,
		GluElRef elRef5,
		GluElRef elRef6
	)
		=> elRef0.Load(Root, _facts[0])
		&& elRef1.Load(Root, _facts[1])
		&& elRef2.Load(Root, _facts[2])
		&& elRef3.Load(Root, _facts[3])
		&& elRef4.Load(Root, _facts[4])
		&& elRef5.Load(Root, _facts[5])
		&& elRef6.Load(Root, _facts[6]);

	protected bool Load(
		GluElRef elRef0,
		GluElRef elRef1,
		GluElRef elRef2,
		GluElRef elRef3,
		GluElRef elRef4,
		GluElRef elRef5,
		GluElRef elRef6,
		GluElRef elRef7
	)
		=> elRef0.Load(Root, _facts[0])
		&& elRef1.Load(Root, _facts[1])
		&& elRef2.Load(Root, _facts[2])
		&& elRef3.Load(Root, _facts[3])
		&& elRef4.Load(Root, _facts[4])
		&& elRef5.Load(Root, _facts[5])
		&& elRef6.Load(Root, _facts[6])
		&& elRef7.Load(Root, _facts[7]);

	protected bool Load(
		GluElRef elRef0,
		GluElRef elRef1,
		GluElRef elRef2,
		GluElRef elRef3,
		GluElRef elRef4,
		GluElRef elRef5,
		GluElRef elRef6,
		GluElRef elRef7,
		GluElRef elRef8
	)
		=> elRef0.Load(Root, _facts[0])
		&& elRef1.Load(Root, _facts[1])
		&& elRef2.Load(Root, _facts[2])
		&& elRef3.Load(Root, _facts[3])
		&& elRef4.Load(Root, _facts[4])
		&& elRef5.Load(Root, _facts[5])
		&& elRef6.Load(Root, _facts[6])
		&& elRef7.Load(Root, _facts[7])
		&& elRef8.Load(Root, _facts[8]);

	protected bool Load(
		GluElRef elRef0,
		GluElRef elRef1,
		GluElRef elRef2,
		GluElRef elRef3,
		GluElRef elRef4,
		GluElRef elRef5,
		GluElRef elRef6,
		GluElRef elRef7,
		GluElRef elRef8,
		GluElRef elRef9
	)
		=> elRef0.Load(Root, _facts[0])
		&& elRef1.Load(Root, _facts[1])
		&& elRef2.Load(Root, _facts[2])
		&& elRef3.Load(Root, _facts[3])
		&& elRef4.Load(Root, _facts[4])
		&& elRef5.Load(Root, _facts[5])
		&& elRef6.Load(Root, _facts[6])
		&& elRef7.Load(Root, _facts[7])
		&& elRef8.Load(Root, _facts[8])
		&& elRef9.Load(Root, _facts[9]);

	#endregion


	public override string ToString() => CellName;
}
}