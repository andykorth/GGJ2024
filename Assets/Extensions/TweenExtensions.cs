using System.Collections;
using UnityEngine;

/// <summary>
/// Contains methods that extend Unity's MonoBehaviour to add common tweening functions.
/// </summary>
public static class TweenExtensions {

	/// <summary>
	/// An IEnumerator function suitable for yielding in a coroutine.  This will execute the given function each game frame for a given duration.
	/// </summary>
	/// <param name="duration">The duration of the yield.</param>
	/// <param name="f">The function to execute each game frame.</param>
	/// <param name="wait">A pre-emptive YieldInstruction that does not count towards duration.</param>
	/// <returns>An IEnumerator suitable for yielding a coroutine.</returns>
	private static IEnumerator TweenHelper(float duration, System.Action<float> f, YieldInstruction wait){
		if(wait != null) yield return wait;
		
		float startTime = Time.time;
		for(float elapsed = 0.0f; elapsed < duration; elapsed = Time.time - startTime){
			f(elapsed / duration);
			yield return null;
		}
		
		f(1.0f);
	}
	
	/// <summary>
	/// Add and begin executing a tween coroutine on this MonoBehaviour.
	/// </summary>
	/// <param name="obj">The MonoBehaviour that will launch the tween coroutine.</param>
	/// <param name="duration">The duration of the tween.</param>
	/// <param name="f">The function to execute each game frame.</param>
	/// <returns>An instance of a Coroutine that is executing the tween.</returns>
	public static Coroutine AddTween(this MonoBehaviour obj, float duration, System.Action<float> f){
		return obj.StartCoroutine(TweenHelper(duration, f, null));
	}

	/// <summary>
	/// Add a tween coroutine on this MonoBehaviour, but delay execution for the given number of seconds.
	/// </summary>
	/// <param name="obj">The MonoBehaviour that will launch the tween coroutine.</param>
	/// <param name="duration">The duration of the tween.</param>
	/// <param name="f">The function to execute each game frame.</param>
	/// <param name="wait">The number of seconds to delay execution of the tween.</param>
	/// <returns>An instance of a Coroutine that is executing the tween.</returns>
	public static Coroutine AddTween(this MonoBehaviour obj, float duration, System.Action<float> f, float wait){
		return obj.StartCoroutine(TweenHelper(duration, f, new WaitForSeconds(wait)));
	}

	/// <summary>
	/// Add a tween corouting on this MonoBehaviour, but dealy execution until the given Coroutine has completed.
	/// </summary>
	/// <param name="obj">The MonoBehaviour that will launch the tween coroutine.</param>
	/// <param name="duration">The duration of the tween.</param>
	/// <param name="f">The function to execute each game frame.</param>
	/// <param name="wait">The pre-emptive Coroutine that will delay execution.</param>
	/// <returns>An instance of a Coroutine that is executing the tween.</returns>
	public static Coroutine AddTween(this MonoBehaviour obj, float duration, System.Action<float> f, Coroutine wait){
		return obj.StartCoroutine(TweenHelper(duration, f, wait));
	}

	/// <summary>
	/// An IEnumerator function suitable for yielding in a coroutine.  This will wait for the given YieldInstruction to complete before executing the given callback.
	/// </summary>
	/// <param name="wait">The YieldInstruction that will delay execution.</param>
	/// <param name="f">The callback to execute when the YieldInstruction is completed.</param>
	/// <returns>An IEnumerator suitable for yielding a coroutine.</returns>
	private static IEnumerator DelayedHelper(YieldInstruction wait, System.Action f){
		yield return wait;
		f();
	}

	/// <summary>
	/// An IEnumerator function suitable for yielding in a coroutine.  This will wait for the given IEnumerator to complete before executing the given callback.
	/// </summary>
	/// <param name="wait">The IEnumerator that will delay execution.</param>
	/// <param name="f">The callback to execute when the IEnumerator is completed.</param>
	/// <returns>An IEnumerator suitable for yielding a coroutine.</returns>
	private static IEnumerator DelayedHelper(IEnumerator wait, System.Action f){
		yield return wait;
		f();
	}

	/// <summary>
	/// Delay the execution of the given callback for the given number of seconds.
	/// </summary>
	/// <param name="obj">The MonoBehaviour that will launch the tween coroutine.</param>
	/// <param name="wait">The number of seconds to delay execution.</param>
	/// <param name="f">The callback to fire after the delay has completed.</param>
	/// <returns>An instance of a Coroutine that is executing the tween.</returns>
	public static Coroutine AddDelayed(this MonoBehaviour obj, float wait, System.Action f){
		return obj.StartCoroutine(DelayedHelper(new WaitForSeconds(wait), f));
	}

	/// <summary>
	/// Delay the execution of the given callback for the given number of seconds.
	/// </summary>
	/// <param name="obj">The MonoBehaviour that will launch the tween coroutine.</param>
	/// <param name="wait">The for which to delay execution.</param>
	/// <param name="f">The callback to fire after the delay has completed.</param>
	/// <returns>An instance of a Coroutine that is executing the tween.</returns>
	public static Coroutine AddDelayed(this MonoBehaviour obj, IEnumerator wait, System.Action f){
		return obj.StartCoroutine(DelayedHelper(wait, f));
	}

	/// <summary>
	/// Delay the execution of the given callback until the given Coroutine completes.
	/// </summary>
	/// <param name="obj">The MonoBehaviour that will launch the tween coroutine.</param>
	/// <param name="wait">The pre-emptive Coroutine that will delay execution.</param>
	/// <param name="f">The callback that will fire after the given Coroutine completes.</param>
	/// <returns>An instance of a Coroutine that is executing the tween.</returns>
	public static Coroutine AddDelayed(this MonoBehaviour obj, Coroutine wait, System.Action f){
		return obj.StartCoroutine(DelayedHelper(wait, f));
	}

	/// <summary>
	/// Continuously repeat the given callback at roughly the given interval.
	/// </summary>
	/// <param name="interval">The interval, in seconds, between executions of the callback.</param>
	/// <param name="f">The callback that will fire at each interval.</param>
	/// <returns>An IEnumerator suitable for yielding a coroutine.</returns>
	private static IEnumerator RepeatHelper(float interval, System.Action f){
		var wait = new WaitForSeconds(interval);

		while(true){
			yield return wait;
			f();
		}
	}

	/// <summary>
	/// Continuously repeat the given callback at roughly the given interval.
	/// </summary>
	/// <param name="obj">The MonoBehaviour that will launch the tween coroutine.</param>
	/// <param name="interval">The interval, in seconds, between executions of the callback.</param>
	/// <param name="f">The callback that will fire at each interval.</param>
	/// <returns>An instance of a Coroutine that is executing the tween.</returns>
	public static Coroutine AddRepeating(this MonoBehaviour obj, float interval, System.Action f){
		return obj.StartCoroutine(RepeatHelper(interval, f));
	}
}
