using System;
using System.Collections.Generic;
using System.Linq;
using Idealist;
using Lumberjack;
using Swoonity.CSharp;
using Swoonity.Unity;
using UnityEngine;
using UnityEngine.UIElements;
using El = UnityEngine.UIElements.VisualElement;
using static UnityEngine.Debug;

namespace Glui
{
[Serializable]
public abstract class GluCEllPool<TCell> : GluCEllPoolBase
	where TCell : GluCEll, new()
{
	public TCell CellExample = new();
	public List<GluElRef.Fact> Facts = new();

	public List<TCell> Available = new();

	public override void Preload()
	{
		if (!CellAsset) {
			Log($"{this} missing required {nameof(CellAsset)}".LgRed());
			return; //>> missing CellAsset
		}

		var cellType = typeof(TCell);
		CellExample.CellName = GluDevLogic.MakeElRefName(CellAsset.name);

		Facts.Clear();
		var elRefFields = cellType.ListFieldsWithBaseType<GluElRef>();

		foreach (var elRefField in elRefFields) {
			var elRef = elRefField.GetFieldValue<GluElRef>(CellExample);

			if (elRef.Name.Nil()) {
				elRef.Name = GluDevLogic.MakeElRefName(elRefField.Name);
			}

			Facts.Add(new GluElRef.Fact { Name = elRef.Name });
		}


		Log(
			Facts.JoinPrefix(
				$"PRELOAD {this} {Facts.Count} ElRefs:",
				static f => f.Name
			).LgYellow()
		);
	}

	public override void RuntimeInitialize()
	{
		if (!Application.isPlaying) return; //>> not runtime
		
		if (HideExampleOnStart) {
			var cellName = CellExample.CellName;

			foreach (var child in ParentEl.Children()) {
				if (child.name == cellName) {
					child.Hide();
				}
			}
		}
	}

	TCell MakeNew()
	{
		// Log($"---------- MAKE NEW: {CellExample}".LgGreen());
		var cell = new TCell();
		var root = CellAsset.Instantiate()
		   .Children()
		   .FirstOrDefault(); // to get around Unity's stupid TemplateContainer 

		cell.LoadCell(CellAsset.name, root, Facts, () => Release(cell));

		if (!cell.IsLoaded) {
			Log($"   GluCell failed to load".LgRed());
		}

		return cell;
	}

	TCell TakeOrMake()
	{
		if (Available.Count > 0) {
			// Log($"{CellExample} taken from pool".LgGreen());
			return Available.GrabLast();
		}

		// Log($"{CellExample} created".LgGreen());
		return MakeNew();
	}

	/// takes from pool and auto adds to this.ParentEl
	public TCell TakeAdd()
	{
		var cell = TakeOrMake();
		ParentEl.Add(cell.Root);
		return cell;
	}

	/// takes from pool but adds to given element
	public TCell TakeButAddTo(El el)
	{
		var cell = TakeOrMake();
		el.Add(cell.Root);
		return cell;
	}

	public void Release(TCell cell)
	{
		Log($"{cell} released to pool ({CellExample}) ".LgGreen());
		if (cell == null) return; //>> null cell
		cell.BeforeRelease();
		cell.Root.RemoveFromHierarchy();
		Available.Add(cell);
		cell.AfterRelease();
	}

	/// releases each obj in list and clears list
	public void ReleaseAll(List<TCell> list)
	{
		foreach (var cell in list) {
			Release(cell);
		}

		list.Clear();
	}

	public void Premake(int count)
	{
		for (var i = 0; i < count; i++) {
			Available.Add(MakeNew());
		}
	}
}


[Serializable]
public abstract class GluCEllPoolBase
{
	[Header("Config")]
	public VisualTreeAsset CellAsset;
	public bool HideExampleOnStart = true;
	public El ParentEl;


	public abstract void Preload();

	public abstract void RuntimeInitialize();
	// public abstract CEll GetCellExample();
}
}