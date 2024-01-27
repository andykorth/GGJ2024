using System;
using System.Collections.Generic;
using Regent.Catalog;
using Regent.SyncerFacts;
using Regent.Workers;
using Sz = System.SerializableAttribute;

namespace Regent.Syncers
{
/// worker signature value is tuple: (choice, index)
[Sz] public class TrackChoice<TChoice> : BaseSyncerNative
{
	public List<TChoice> Choices;

	public TChoice Current;
	public TChoice Former;

	public int CurrentIndex;
	public int FormerIndex;

	public ReactForwarder<(TChoice choice, int index)> ReactFwd;
	public event Action<int> EventValueChanged;

	public override void Initialize()
	{
		Info = RegentCatalogRuntime.GetSyncerInfo(Fact.HashId);
		SyncerFactMakers.CheckSyncerInfoInit<(TChoice choice, int index)>(Info);
		ReactFwd = (ReactForwarder<(TChoice choice, int index)>)Info.ReactFwd;
		// hi 👋 InvalidCastException? add the 2nd parameter to the worker signature

		CurrentIndex = -1;
		FormerIndex = -1;

		IsInitialized = true;
	}

	public void SetInitial(List<TChoice> choices, int index, bool triggerReaction = true)
	{
		if (!IsInitialized) Initialize();

		Choices = choices;

		var val = index >= 0 ? Choices[index] : default;

		Current = val;
		CurrentIndex = index;
		if (triggerReaction) ReactFwd.CueReaction(Entity, (Current, CurrentIndex));
	}

	public void Change(TChoice next) => Change(Choices.IndexOf(next));

	public void Change(int nextIndex)
	{
		if (!IsInitialized) Initialize();

		var next = nextIndex >= 0 ? Choices[nextIndex] : default;

		Former = Current;
		FormerIndex = CurrentIndex;
		Current = next;
		CurrentIndex = nextIndex;
		ReactFwd.CueReaction(Entity, (Current, CurrentIndex));
		EventValueChanged?.Invoke(nextIndex);
	}

	/// change if value is NOT EQUAL to current
	public void ChangeDiff(TChoice next) => ChangeDiff(Choices.IndexOf(next));

	/// change if value is NOT EQUAL to current
	public void ChangeDiff(int nextIndex)
	{
		if (!IsInitialized) Initialize();

		if (nextIndex == CurrentIndex) return;
		Change(nextIndex);
	}

	public void SetChoices(List<TChoice> choices, int newIndex = -1)
	{
		Choices = choices;
		Change(newIndex);
	}

	public override string ToString() => $"syncer<{Entity}.{typeof(TChoice).Name}>";
}
}