using System.Collections.Generic;
using Idealist;
using Swoonity.Unity;
using UnityEngine;

namespace Foundational
{
[CreateAssetMenu(fileName = "colors~ ", menuName = "Def/Color Set")]
public class ColorSetDef : ScriptableObject
{
	[Tooltip("-1: random any, 0: first, 5: random of first 5")]
	public int RandomDefault = -1;

	[ColorUsage(false, false)]
	public List<Color> Colors = new();

	[Header("Color Randomizer")]
	// [Btn(nameof(Generate), nameof(AddThis), nameof(AddRnd))]
	[Btn(nameof(Generate), nameof(AddThis), nameof(AddAllDistinct))]
	public bool IgnoreThisBool;

	[ColorUsage(false, true)]
	public Color AddColor;

	public void Generate()
	{
		AddColor = AddColor.RandomColor();
	}

	public void AddThis()
	{
		Colors.Add(AddColor);
		this.SetDirtyIfEditor();
	}

	public void AddRnd()
	{
		Generate();
		AddThis();
	}

	public void AddAllDistinct()
	{
		Colors.Add("#e6194B".ToColor());
		Colors.Add("#3cb44b".ToColor());
		Colors.Add("#ffe119".ToColor());
		Colors.Add("#4363d8".ToColor());
		Colors.Add("#f58231".ToColor());
		Colors.Add("#911eb4".ToColor());
		Colors.Add("#42d4f4".ToColor());
		Colors.Add("#f032e6".ToColor());
		Colors.Add("#bfef45".ToColor());
		Colors.Add("#fabed4".ToColor());
		Colors.Add("#469990".ToColor());
		Colors.Add("#dcbeff".ToColor());
		Colors.Add("#9A6324".ToColor());
		Colors.Add("#fffac8".ToColor());
		Colors.Add("#800000".ToColor());
		Colors.Add("#aaffc3".ToColor());
		Colors.Add("#808000".ToColor());
		Colors.Add("#ffd8b1".ToColor());
		Colors.Add("#000075".ToColor());
		Colors.Add("#a9a9a9".ToColor());
		// Colors.Add("#ffffff".ToColor());
		// Colors.Add("#000000".ToColor());
	}

	public Color GetByIndex(int index)
		=> Colors.Count != 0
			? Colors[index % Colors.Count]
			: default;
}
}