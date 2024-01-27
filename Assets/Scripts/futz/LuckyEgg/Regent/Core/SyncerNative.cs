using System;
using System.Collections.Generic;
using Regent.Catalog;
using Regent.Entities;
using Regent.SyncerFacts;
using Regent.Workers;
using Swoonity.MHasher;
using UnityEngine;
using Sz = System.SerializableAttribute;

namespace Regent.Syncers
{
// [Sz] public class TrackColor : Track<Color> {
// 	/// TODO: decide if I like this:
// 	public static implicit operator Color(TrackColor track) => track.Current;
// }

[Sz] public class TrackV2 : Track<Vector2>
{
	// public override bool IsEqual(Vector2 a, Vector2 b) => a.Approx(b);
}

[Sz] public class TrackV3 : Track<Vector3>
{
	// public override bool IsEqual(Vector3 a, Vector3 b) => a.Approx(b);
}

[Sz] public class Track<TVal> : BaseSyncerNative
{
	public TVal Current;
	public TVal Former;

	public ReactForwarder<TVal> ReactFwd;
	public event Action<TVal> EventValueChanged;

	/// the last change: (Former, Current)
	public (TVal former, TVal current) LastShift => (Former, Current);

	public override void Initialize()
	{
		Info = RegentCatalogRuntime.GetSyncerInfo(Fact.HashId);
		SyncerFactMakers.CheckSyncerInfoInit<TVal>(Info);
		ReactFwd = (ReactForwarder<TVal>)Info.ReactFwd;
		// hi 👋 InvalidCastException? add the 2nd parameter to the worker signature

		IsInitialized = true;
	}

	public void SetInitial(TVal val, bool triggerReaction = true)
	{
		if (!IsInitialized) Initialize();

		Current = val;
		if (triggerReaction) ReactFwd.CueReaction(Entity, val);
	}

	public void Change(TVal next)
	{
		if (!IsInitialized) Initialize();

		Former = Current;
		Current = next;
		ReactFwd.CueReaction(Entity, next);
		EventValueChanged?.Invoke(next);
	}

	public void Change(TVal next, bool doDiffCheck)
	{
		if (doDiffCheck) ChangeDiff(next);
		else Change(next);
	}

	/// change if value is NOT EQUAL to current
	public void ChangeDiff(TVal next)
	{
		if (!IsInitialized) Initialize();

		if (IsEqual(next, Current)) return;
		Change(next);
	}

	/// change value without triggering reactions
	public void ChangeSilent(TVal next)
	{
		if (!IsInitialized) Initialize();

		Former = Current;
		Current = next;
	}

	/// only change if doChange is true
	public void ChangeIf(bool doChange, TVal next)
	{
		if (!doChange) return;
		Change(next);
	}

	/// only change if doChange is true
	public void ChangeIf((bool doChange, TVal next) val)
	{
		if (!val.doChange) return;
		Change(val.next);
	}

	/// alias for Change
	public void Trigger(TVal next) => Change(next);

	/// alias for ChangeDiff
	public void TriggerDiff(TVal next) => ChangeDiff(next);

	/// trigger "change" with current value
	public void Retrigger() => ReactFwd.CueReaction(Entity, Current);

	public virtual bool IsEqual(TVal a, TVal b) => EqualityComparer<TVal>.Default.Equals(a, b);

	public override string ToString() => $"syncer<{Entity}.{typeof(TVal).Name}>";
}

public enum EnumToggle
{
	UNSET,
	OFF,
	ON,
}

/// for button clicks, etc.
[Sz] public class TrackToggle : Track<EnumToggle>
{
	public bool IsOn => Current == EnumToggle.ON;
	public bool IsOff => Current == EnumToggle.OFF;
	public bool IsUnset => Current == EnumToggle.UNSET;

	/// won't trigger if already triggered this frame
	public void TriggerOn() => ChangeDiff(EnumToggle.ON);

	/// won't trigger if already triggered this frame
	public void TriggerOff() => ChangeDiff(EnumToggle.OFF);

	public void TurnOnIfUnset()
	{
		if (IsUnset) ChangeDiff(EnumToggle.ON);
	}

	public void TurnOffIfUnset()
	{
		if (IsUnset) ChangeDiff(EnumToggle.OFF);
	}

	public void TriggerBool(bool setOn) => ChangeDiff(setOn ? EnumToggle.ON : EnumToggle.OFF);

	public void Toggle() => TriggerBool(!IsOn);

	/// NO! Use TriggerOn() or TriggerOff() instead!
	public new void Change(EnumToggle USE_TRIGGER_INSTEAD)
		=> throw new Exception("Use TriggerOn() or TriggerOff() instead");


	/// IsUnset ? useIfUnset : IsOn
	public bool IfUnset(bool useIfUnset) => IsUnset ? useIfUnset : IsOn;
}

/// for button clicks, etc.
/// <remarks>only triggers ONCE per frame</remarks>
[Sz] public class TrackEvt : Track<int>
{
	public void Trigger()
	{
		base.Change(Time.frameCount);
	}

	/// NO! Use NativeEvt.Trigger() instead!
	public new void Change(int USE_TRIGGER_INSTEAD)
		=> throw new Exception("Use NativeEvt.Trigger() instead");

	/// NO! Use NativeEvt.Trigger() instead!
	public new void ChangeSilent(int USE_TRIGGER_INSTEAD)
		=> throw new Exception("Use NativeEvt.Trigger() instead");
}


/// <summary>only sends React event ONCE a frame (but gathers multiple changes)</summary>
/// <remarks>use Add, Remove, RemoveAt, Clear, OR edit list then Dirty()</remarks>
[Sz] public class TrackList<TVal> : BaseSyncerNative
{
	public List<TVal> Current = new();
	public int Count => Current.Count;

	public int FrameChanged;

	public ReactForwarder<TrackList<TVal>> ReactFwd;
	public event Action<List<TVal>> EventValueChanged;

	// public void Add(TVal val) => Dirty().Add(val);
	// public void Remove(TVal val) => Dirty().Remove(val);
	// public void RemoveAt(int index) => Dirty().RemoveAt(index);
	// public void Clear() => Dirty().Clear();

	public void Add(TVal val)
	{
		Current.Add(val);
		Dirty();
	}

	public void AddRange(List<TVal> list)
	{
		foreach (var val in list) {
			Current.Add(val);
		}

		Dirty();
	}

	/// Clear then AddRange
	public void SetRange(List<TVal> list)
	{
		Current.Clear();
		AddRange(list);
	}

	/// warning: will search entire list first
	public void AddDistinct(TVal val)
	{
		if (Current.Contains(val)) return; //>> already contains

		Current.Add(val);
		Dirty();
	}

	public void AddIf(List<TVal> list, Func<TVal, bool> staticFnIf)
	{
		foreach (var val in list) {
			if (staticFnIf(val)) Current.Add(val);
		}

		Dirty();
	}

	public void Remove(TVal val)
	{
		Current.Remove(val);
		Dirty();
	}

	public void RemoveAt(int index)
	{
		Current.RemoveAt(index);
		Dirty();
	}

	public void Clear()
	{
		Current.Clear();
		Dirty();
	}

	public List<TVal> Dirty()
	{
		if (!IsInitialized) Initialize();

		var frame = Time.frameCount;
		if (frame != FrameChanged) {
			FrameChanged = frame;
			ReactFwd.CueReaction(Entity, this);
		}

		EventValueChanged?.Invoke(Current);

		return Current;
	}

	public override void Initialize()
	{
		Info = RegentCatalogRuntime.GetSyncerInfo(Fact.HashId);
		SyncerFactMakers.CheckSyncerInfoInit<TrackList<TVal>>(Info);
		ReactFwd = (ReactForwarder<TrackList<TVal>>)Info.ReactFwd;

		IsInitialized = true;
	}
}


/// <remarks>queued values</remarks>
/// TODO: I doubt this actually works with Buffer & ReactWorker.EntityValsToUpdate / dirty
[Sz] public class TrackCued<TVal> : BaseSyncerNative
{
	public List<TVal> ListQueue = new();
	public int Count => ListQueue.Count;

	public int FrameChanged;
	public bool IsDraining;
	public List<TVal> Buffer = new();

	public ReactForwarder<TrackCued<TVal>> ReactFwd;

	public List<TVal> Dirty(bool silent = false)
	{
		if (!IsInitialized) Initialize();
		if (silent) return ListQueue;

		var frame = Time.frameCount;
		if (frame != FrameChanged) {
			FrameChanged = frame;
			ReactFwd.CueReaction(Entity, this);
		}

		return ListQueue;
	}

	/// CAN be used while draining
	public void Cue(TVal val)
	{
		if (IsDraining) {
			Buffer.Add(val);
			return; //>> send to buffer
		}

		ListQueue.Add(val);
		Dirty();
	}

	public void Clear(bool silent = false) => Dirty(silent).Clear();

	public void Drain(Action<TVal> fn)
	{
		IsDraining = true;
		foreach (var val in ListQueue) fn(val);
		IsDraining = false;
		ListQueue.Clear();
		CheckBuffer();
	}

	public void Drain<TData>(TData data, Action<TData, TVal> fn)
	{
		IsDraining = true;
		foreach (var val in ListQueue) fn(data, val);
		IsDraining = false;
		ListQueue.Clear();
		CheckBuffer();
	}

	void CheckBuffer()
	{
		if (Buffer.Count == 0) return; //>> empty buffer

		foreach (var val in Buffer) ListQueue.Add(val);
		Buffer.Clear();
		Dirty();
	}

	public override void Initialize()
	{
		Info = RegentCatalogRuntime.GetSyncerInfo(Fact.HashId);
		SyncerFactMakers.CheckSyncerInfoInit<TrackCued<TVal>>(Info);
		ReactFwd = (ReactForwarder<TrackCued<TVal>>)Info.ReactFwd;

		IsInitialized = true;
	}
}

[Sz] public abstract class BaseSyncerNative : ISyncer, IMHashable
{
	public MHash GetHash() => Fact.HashId; // TODO: make devtime?

	public Entity Entity;
	public SyncerFact Fact;

	public bool IsInitialized;
	public SyncerInfo Info;

	public Entity GetEntity() => Entity;

	public void __ValidateFromCog(Entity entity, SyncerFact fact)
	{
		Entity = entity;
		Fact = fact;
	}

	public abstract void Initialize();
}

public static class ExtTrack
{
	// // maybe? maybe not?
	// /// track.Change(val)
	// public static void Push<TVal>(this TVal val, Track<TVal> track) => track.Change(val);
	//
	// public static void ChangeAll<TVal>(this List<Track<TVal>> tracks, TVal val)
	// {
	// 	foreach (var track in tracks) {
	// 		track.Change(val);
	// 	}
	// }

	/// alias: track.Change(track.Current + num)
	public static void Increment(this Track<int> track, int num = 1)
		=> track.Change(track.Current + num);
}
}