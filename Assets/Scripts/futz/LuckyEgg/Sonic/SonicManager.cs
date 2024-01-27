using System.Collections.Generic;
using UnityEngine;

namespace Sonic
{
public class SonicManager : MonoBehaviour
{
	public SonicConfig Config;
	public List<SonicEmitter> AvailableEmitters = new();

	public int Inc = 0;

	void Awake()
	{
		if (Config.AutoRegisterSugarFns) {
			SonicSugarFns.FnPlayAt = (sfx, position) => PlayAt(sfx, position);
			SonicSugarFns.FnPlayUi = sfx => PlayUi(sfx);
		}
	}

	/// automatically pooled, does null check
	public void PlayAt(SonicSfx sfx, Vector3 position)
	{
		if (!sfx) return;
		if (sfx.Clips.Count == 0) return;

		var emitter = TakeEmitter();

		emitter.Tform.position = position;
		emitter.On();
		emitter.Pooled_Play(this, sfx, Config.LengthBuffer);
	}
	
	/// automatically pooled, does null check
	public void PlayUi(SonicSfx sfx)
	{
		// Debug.LogWarning($"TODO: proper UI sound FX (Sonic)");
		PlayAt(sfx, Vector3.zero);
	}

	// void ApplySfx(SonicSfx sfx, SonicEmitter emitter)
	// {
	//
	// 	var source = emitter.Source;
	// 	var (clip, pitch, volume) = sfx.Next();
	// 	source.outputAudioMixerGroup = sfx.MixerGroup;
	// 	source.clip = clip;
	// 	source.pitch = pitch;
	// 	source.volume = volume;
	// }

	SonicEmitter TakeEmitter()
	{
		if (AvailableEmitters.Count == 0) return MakeNewEmitter();

		var last = AvailableEmitters.Count - 1;
		var takenEmitter = AvailableEmitters[last];
		AvailableEmitters.RemoveAt(last);

		return takenEmitter;
	}

	public void ReleaseEmitter(SonicEmitter emitter)
	{
		AvailableEmitters.Add(emitter);
		emitter.Off();
	}

	SonicEmitter MakeNewEmitter()
	{
		var instanceId = ++Inc;
		var emitter = new GameObject($"sfx {instanceId}").AddComponent<SonicEmitter>();
		emitter.Tform = emitter.transform;
		emitter.Tform.SetParent(transform, false);
		emitter.Off();

		var source = emitter.gameObject.AddComponent<AudioSource>();
		source.playOnAwake = false;
		source.spatialBlend = Config.SpatialBlend2dTo3d;
		source.rolloffMode = Config.RolloffMode;
		source.minDistance = Config.RolloffStartDistance;
		source.maxDistance = Config.RolloffEndDistance;
		// more init?
		emitter.Source = source;

		return emitter;
	}

	// static SonicManager _manager;
	//
	// static void RegisterSingleton(SonicManager newManager)
	// {
	// 	if (_manager) {
	// 		Destroy(newManager.gameObject);
	// 	}
	// 	else {
	// 		_manager = newManager;
	// 	}
	// }
	//
	// public static void Release(SonicEmitter emitter)
	// {
	// 	if (_manager) {
	// 		_manager.ReleaseEmitter(emitter);
	// 	}
	// 	else {
	// 		Destroy(emitter.gameObject);
	// 	}
	// }
}
}