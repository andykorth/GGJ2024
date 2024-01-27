using System;
using System.Collections.Generic;
using Swoonity.CSharp;
using UnityEngine.UIElements;
using static UnityEngine.Debug;

namespace Glui
{
public interface IElRef { }

[Serializable]
public abstract class GluElRef : IElRef
{
	public string Name;
	public bool IsLoaded; // TODO: don't serialize this? but show in editor?

	protected abstract bool TryLoad(VisualElement parent);

	public bool Load(VisualElement parent) => IsLoaded = TryLoad(parent);
	public bool Load(VisualElement parent, GluElRef example) => Load(parent, example.Name);
	public bool Load(VisualElement parent, Fact fact) => Load(parent, fact.Name);

	public bool Load(VisualElement parent, string name)
	{
		Name = name;
		return IsLoaded = TryLoad(parent);
	}

	public override string ToString() => Name.Or(GetType().Name);


	[Serializable]
	public class Fact
	{
		public string Name;
	}
}

[Serializable]
public class VEl : GluElRef
{
	public VisualElement El;

	protected override bool TryLoad(VisualElement parent)
		=> (El = parent.Q<VisualElement>(Name)) != null;
}

[Serializable]
public class LabEl : GluElRef
{
	public Label El;

	public string CurrentText;
	bool _wasEverSet;

	protected override bool TryLoad(VisualElement parent)
	{
		El = parent.Q<Label>(Name);
		if (El == null) return false; //>> couldn't load

		if (_wasEverSet) {
			El.text = CurrentText;
		}
		else {
			CurrentText = El.text;
		}

		return true; //>> loaded
	}

	public void Set(string text)
	{
		_wasEverSet = true;
		CurrentText = text;
		if (El != null) El.text = text;
	}
}

[Serializable]
public class RadioBtn : GluElRef
{
	public RadioButton El;

	protected override bool TryLoad(VisualElement parent)
		=> (El = parent.Q<RadioButton>(Name)) != null;
}

[Serializable]
public class RadioGrp : GluElRef
{
	public RadioButtonGroup El;
	public Action<int> FnWhenChosen;
	EventCallback<ChangeEvent<int>> _whenChosen;

	protected override bool TryLoad(VisualElement parent)
	{
		if (_whenChosen == null) {
			_whenChosen = evt => FnWhenChosen.Try(evt.newValue);
		}
		else {
			El?.UnregisterValueChangedCallback(_whenChosen);
		}

		El = parent.Q<RadioButtonGroup>(Name);
		if (El == null) return false; //>> couldn't load

		El.RegisterValueChangedCallback(_whenChosen);
		return true; //>> loaded
	}


	public void SetSelected(int index) => El.value = index;
	public void SetChoices(List<string> choices) => El.choices = choices;
}
}