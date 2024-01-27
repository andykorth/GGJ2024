using System;
using UnityEngine;

namespace Weasel
{
[Serializable]
public abstract class BaseWeasel
{
	[Header("Basic Config")]
	public float Duration = 1f;
	public Easing Easing;
	[Tooltip("Perf optimization. Mark this if it will play/stop frequently")]
	public bool KeepActive;

	[Tooltip("Looping")]
	public LoopMode LoopMode;
	public int N;

	[Header("State")]
	public bool IsPlaying;
	public float Elapsed;
	public bool IsReversing;
	public int TimesLooped;
	public FnEasing FnEasing; // fraction => fraction

	public Action OnBegin;
	public Action OnLoopStart;
	public Action OnDone;

	protected void BeginPlaying()
	{
		IsPlaying = true;
		Elapsed = 0f;
		Duration = Duration > 0f ? Duration : .01f;
		TimesLooped = 0;
		IsReversing = false;
		FnEasing = Easing.GetFn(); // optimize (micro)
		SetFrom();

		OnBegin?.Invoke();
		OnLoopStart?.Invoke();
		WeaselManager.PlayWeasel(this);
	}

	public void Tick(float dt)
	{
		if (!CheckIsValid()) {
			IsPlaying = false;
			return;
		}

		Elapsed += dt;

		if (Elapsed < Duration) {
			//## continue
			var frac = Elapsed / Duration;
			SetRawFraction(frac);
			return;
		}

		//## done
		SetRawFraction(1f);
		CheckDoneOrLoop();
	}

	public void Stop(bool jumpToEnd = false)
	{
		if (jumpToEnd) SetRawFraction(1f);
		IsPlaying = false;
	}

	void DonePlaying()
	{
		IsPlaying = false;
		OnDone?.Invoke();
	}

	void StartLoop()
	{
		++TimesLooped;
		Elapsed = 0f;
		IsReversing = false;
		OnLoopStart?.Invoke();
	}

	void StartReverse()
	{
		Elapsed = 0f;
		IsReversing = true;
	}

	void CheckDoneOrLoop()
	{
		switch (LoopMode) {
			case LoopMode.NONE:
				DonePlaying();
				return;

			case LoopMode.PLAY_N_TIMES:
				if (TimesLooped < N) {
					StartLoop();
					return;
				}

				DonePlaying();
				return;

			case LoopMode.PLAY_FOREVER:
				StartLoop();
				return;

			case LoopMode.YOYO_ONCE:
				if (!IsReversing) {
					StartReverse();
					return;
				}

				DonePlaying();
				return;

			case LoopMode.YOYO_N_TIMES:
				if (!IsReversing) {
					StartReverse();
					return;
				}

				if (TimesLooped < N) {
					StartLoop();
					return;
				}

				DonePlaying();
				return;

			case LoopMode.YOYO_FOREVER:
				if (!IsReversing) {
					StartReverse();
					return;
				}

				StartLoop();
				return;

			default: throw new ArgumentOutOfRangeException();
		}
	}

	void SetRawFraction(float rawFrac)
	{
		var frac = IsReversing
			? FnEasing(1 - rawFrac)
			: FnEasing(rawFrac);
		ApplyFractionValue(frac);
	}

	public void JumpToStart() => ApplyFractionValue(0f);
	public void JumpToEnd() => ApplyFractionValue(1f);

	// public abstract void Initialize();
	public abstract bool CheckIsValid();
	public abstract void SetFrom();
	public abstract void ApplyFractionValue(float frac);
}

public enum LoopMode
{
	NONE,
	PLAY_N_TIMES,
	PLAY_FOREVER,
	YOYO_ONCE,
	YOYO_N_TIMES,
	YOYO_FOREVER,
}

//
//
//
//
// TODO: delete below
//
//
//
//

// public static class WeaselSomething {
// 	public static TVal GetVal<TVal>(
// 		FnEasing fnEasing,
// 		FnLerp<TVal> fnLerp,
// 		TVal original,
// 		TVal desired,
// 		float rawFrac
// 	) {
// 		var eased = fnEasing(rawFrac);
// 		var val = fnLerp(original, desired, eased);
// 		return val;
// 	}
// }
//
//
// public class WeaselTransform {
// 	public enum Props {
// 		position,
// 		localPosition,
//
// 		rotation,
// 		localRotation,
//
// 		scale,
// 	}
//
// 	public static void Something(Props prop) { }
//
// 	public static (Func<Transform, Vector3> fnGet, Action<Transform, Vector3> fnSet) Position = (
// 		static (tf) => tf.position,
// 		static (tf, val) => tf.position = val
// 	);
// }
//
// public abstract class BaseBlah {
// 	public abstract void DoThing();
// }
//
// public abstract class ObjBlah<TObj> : BaseBlah {
// 	public TObj Obj;
// 	public override void DoThing() => DoThing(Obj);
// 	public abstract void DoThing(TObj obj);
// }
//
// public abstract class ObjValBlah<TObj, TVal> : ObjBlah<TObj> {
// 	public TVal Val;
// 	public override void DoThing(TObj obj) => DoThing(obj, Val);
// 	public abstract void DoThing(TObj obj, TVal val);
// }

// [Serializable]
// public class WeaselTransformPosition : Weasel<Transform, Vector3> {
// 	public override void ApplyFraction(
// 		Transform tar,
// 		Vector3 original,
// 		Vector3 desired,
// 		float fraction
// 	) {
// 		tar.position = Vector3.Lerp(original, desired, fraction);
// 	}
// }
}