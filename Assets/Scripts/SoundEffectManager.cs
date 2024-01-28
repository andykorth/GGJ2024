using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioBunch {
	public AudioClip[] clips;

	public void Play(){
		AudioSourceX.PlayClip (clips [Random.Range (0, clips.Length)]);
	}
}

public class SoundEffectManager : Singleton<SoundEffectManager>
{
	public void FadeOutMusic ()
	{
		this.AddTween(2.0f, (alpha) => musicSource.volume = (1f-alpha) );
	}

	public AudioSource musicSource;

	public AudioClip gameplayMusic;
	public AudioClip titleMusic;

	public void Start(){
		musicSource.loop = true;
		musicSource.clip = titleMusic;
		musicSource.Play ();
	}

	public void SwitchSongs(){
		musicSource.loop = true;
		musicSource.clip = (musicSource.clip == titleMusic) ? gameplayMusic : titleMusic;
		musicSource.Play ();
	}

}