using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Sonic
{
[CreateAssetMenu(menuName = "Sonic SFX/Sfx")]
public class SonicSfx : ScriptableObject
{
	public AudioMixerGroup MixerGroup;
	public List<AudioClip> Clips = new();
	[Tooltip("pitch = rnd(1-this, 1+this)")]
	[Range(0, .5f)] public float PitchDeviation = 0f;
	[Range(0, 1f)] public float Volume = 1f;

	[Tooltip("plays forever (if POOLED_PLAY, will use MaxLength)")]
	public bool Loop;
	[Tooltip("overrides length if clip is longer (POOLED_PLAY only)")]
	public float MaxLength = 99;
	[Tooltip("waits this long before starting (seconds)")]
	public float Delay;
	[Tooltip("if > 0, sets clip time to random between Min and Max")]
	public float RandomStartTimeMin;
	[Tooltip("if > 0, sets clip time to random between Min and Max")]
	public float RandomStartTimeMax;

	AudioClip _previous;
	List<AudioClip> _bag = new();

	// public (AudioClip clip, float pitch, float volume) Next()
	// {
	// 	var clip = Clips.Count switch {
	// 		0 => null,
	// 		1 => Clips[0],
	// 		_ => GetFromBag(),
	// 	};
	// 	_previous = clip;
	// 	return (clip, FnGetPitch(this), Volume);
	// }

	// public AudioClip GetClip()
	// {
	// 	var clip = Clips.Count switch {
	// 		0 => null,
	// 		1 => Clips[0],
	// 		_ => GetFromBag(),
	// 	};
	// 	_previous = clip;
	// 	return clip;
	// }

	public AudioClip GetClip()
		=> _previous = Clips.Count switch {
			0 => null,
			1 => Clips[0],
			_ => GetFromBag(),
		};

	public float GetPitch() => FnGetPitch(this);
	public float GetVolume() => Volume; //TODO: deviation?

	public float GetStartTime()
		=> RandomStartTimeMax > 0
			? FnRandomRangeF(RandomStartTimeMin, RandomStartTimeMax)
			: 0;

	AudioClip GetFromBag()
	{
		if (_bag.Count > 0) return GrabLastInBag();

		FnRefillBag(this);

		var clip = GrabLastInBag();

		if (_previous != clip) return clip;

		// don't ever repeat same clip twice
		var other = GrabLastInBag();
		_bag.Add(clip);
		return other;
	}

	AudioClip GrabLastInBag()
	{
		var lastIndex = _bag.Count - 1;
		var clip = _bag[lastIndex];
		_bag.RemoveAt(lastIndex);
		return clip;
	}

	public override string ToString() => name;

	#region Static Functions (can replace)

	public static Func<int, int, int> FnRandomRangeI = static (min, max)
		=> UnityEngine.Random.Range(min, max);

	public static Func<float, float, float> FnRandomRangeF = static (min, max)
		=> UnityEngine.Random.Range(min, max);

	public static Action<SonicSfx> FnRefillBag = static sfx => {
		var count = sfx.Clips.Count;

		var runtimeClips = sfx._bag;
		runtimeClips.AddRange(sfx.Clips);

		for (var i = 0; i < count; ++i) {
			var swapIndex = FnRandomRangeI(0, count);
			(runtimeClips[i], runtimeClips[swapIndex]) = (runtimeClips[swapIndex], runtimeClips[i]);
		}
	};

	public static Func<SonicSfx, float> FnGetPitch = static sfx
		=> 1 + FnRandomRangeF(-sfx.PitchDeviation, sfx.PitchDeviation);

	#endregion
}
}