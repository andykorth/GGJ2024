using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Debug;

namespace KeeperOfTime
{
/// TODO: OPTIMIZE later (depending on expected # of timers)
/// could change timers to array of structs (data-oriented design)
public class TimeKeeper : MonoBehaviour
{
	/// seconds
	public float Current;
	public float Scale = 1;

	/// Difference between: most recent tick's time and preceding tick's time
	public float Delta;

	/// manual adjustment (such as from console command)
	public float Adjustment;

	/// get Current time and Delta time
	public (float time, float dt) TimeDt => (Current, Delta);

	public void Tick(float deltaRaw)
	{
		var deltaScaled = deltaRaw * Scale;

		var timeMs = Current + deltaScaled + Adjustment;
		Delta = timeMs - Current;
		Current = timeMs;
		Adjustment = 0;
		CheckTimers();
	}

	/// manual adjustment, will be applied next tick
	public void AddAdjustment(float timeMs) => Adjustment += timeMs;

	public void SetScale(float scale) => Scale = scale;
	public void Pause() => Scale = 0;
	public void ResetScale() => Scale = 1;

	public void ResetTime()
	{
		Current = 0;
		Scale = 1;
		Delta = 0;
		Adjustment = 0;
	}


	#region Timers

	[Header("Timers")]
	public List<Timer> Timers = new();

	public void CheckTimers()
	{
		for (var dex = Timers.Count - 1; dex >= 0; dex--) {
			var timer = Timers[dex];

			timer.Check(Current);

			if (timer.NeedsToBeRemoved) {
				timer.Keeper = null;
				Timers.RemoveAt(dex);
			}
		}
	}

	public Timer CreateTimer(MonoBehaviour owner, Action onTrigger)
	{
		var timer = new Timer();
		RegisterTimer(timer, owner, onTrigger);
		return timer;
	}

	public void RegisterTimer(Timer timer, MonoBehaviour owner, Action onTrigger)
	{
		// if (timer.Keeper) throw new Exception($"Already linked {owner}");
		if (timer.Keeper) {
			LogWarning($"Already linked {owner}");
			return;
		}
		timer.Keeper = this;
		timer.Owner = owner;
		timer.OnTrigger = onTrigger;
		Timers.Add(timer);
		if (Timers.Count > 50) Debug.LogWarning($"TODO: Optimize TimeKeeper");
	}


	public void RemoveTimer(Timer timer) => timer?.Release();

	#endregion

	[Serializable]
	public class Timer
	{
		public bool Waiting;
		public float Until;
		public float Duration;
		public float LastTriggered;

		public TimeKeeper Keeper;
		public MonoBehaviour Owner;
		public Action OnTrigger;
		public bool NeedsToBeRemoved;
		public bool IsLoop;
		/// adjustments can trigger multiple events and duration is always added to previous
		public bool SimMultiLoops;

		// OPTIMIZE: could tell Keeper when started

		public void WaitFor(float duration)
		{
			Duration = EnsureNotZero(duration);
			Until = Keeper.Current + Duration;
			Waiting = true;
		}

		public void WaitLoop(float duration, bool simMultiLoops = true)
		{
			IsLoop = true;
			SimMultiLoops = simMultiLoops;
			WaitFor(duration);
		}

		public void Wait(float duration, bool loop, bool simMultiLoops = true)
		{
			if (loop) WaitLoop(duration, simMultiLoops);
			else WaitFor(duration);
		}

		public void Cancel() => Waiting = false;


		public void Check(float currentTime)
		{
			if (!Owner) NeedsToBeRemoved = true;
			if (NeedsToBeRemoved) return;
			if (!Waiting) return;
			if (currentTime < Until) return;

			// TRIGGER

			OnTrigger.Invoke();
			LastTriggered = currentTime;

			if (!IsLoop) {
				Waiting = false;
				return;
			}

			if (!SimMultiLoops) {
				Until = currentTime + Duration;
				return;
			}

			Until += Duration;
			Check(currentTime);
		}

		public void Release() => NeedsToBeRemoved = true;


		public static float EnsureNotZero(float val, float newValIfZero = 0.0001f)
			=> Mathf.Approximately(val, 0) ? newValIfZero : val;
	}
}
}