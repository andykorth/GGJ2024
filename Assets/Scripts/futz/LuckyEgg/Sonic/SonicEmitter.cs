using UnityEngine;

namespace Sonic
{
public class SonicEmitter : MonoBehaviour
{
	public Transform Tform;
	public AudioSource Source;
	public SonicSfx Sfx;
	public bool PlayOnEnable;

	public void Play() => PlaySfx(Sfx);

	public void PlaySfx(SonicSfx sfx)
	{
		Sfx = sfx;
		Source.clip = sfx.GetClip();
		Source.outputAudioMixerGroup = sfx.MixerGroup;
		Source.pitch = sfx.GetPitch();
		Source.volume = sfx.GetVolume();
		Source.time = sfx.GetStartTime();
		Source.loop = sfx.Loop;

		if (sfx.Delay > 0) Source.PlayDelayed(sfx.Delay);
		else Source.Play();
	}

	void OnEnable()
	{
		if (PlayOnEnable) Play();
	}

	#region Pooling

	SonicManager _poolManager;

	public void Pooled_Play(SonicManager poolManager, SonicSfx sfx, float tempLengthBuffer = 0)
	{
		_poolManager = poolManager;
		PlaySfx(sfx);

		var length = sfx.Loop
			? sfx.MaxLength
			: Mathf.Min(Source.clip.length, sfx.MaxLength);

		Invoke(POOLED_ENDED, sfx.Delay + length + tempLengthBuffer);
	}

	const string POOLED_ENDED = nameof(Pooled_Ended);

	void Pooled_Ended() => _poolManager.ReleaseEmitter(this);

	#endregion

	#region Sugar

	public void On() => gameObject.SetActive(true);
	public void Off() => gameObject.SetActive(false);

	#endregion
}
}