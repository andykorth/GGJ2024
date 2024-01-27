using UnityEngine;
using System.Collections;

/// <summary>
/// Contains additional functions that extend Unity's AudioSource class.
/// </summary>
public class AudioSourceX
{
	/// <summary>
	/// Immediately creates a GameObject to play the given AudioClip at the location of the given Transform.  (The GameObject will destroy itself at the end of the clip.)
	/// </summary>
	/// <param name="clip">The AudioClip to play.</param>
	/// <param name="transform">The Transform that will provide the location.</param>
	/// <param name="volume">Sets the AudioSource volume to this level.</param>
	/// <param name="maxLinearFalloffDistance">Sets the AudioSource minDistance to this value.</param>
	/// <param name="minLinearFalloffDistance">Sets the AudioSource maxDistance to this value.</param>
	/// <param name="pitch">Sets the AudioSource pitch to this vale.</param>
	/// <returns>An instance of the AudioSource created to play the AudioClip.</returns>
	public static AudioSource PlayClipAtTransform(AudioClip clip, Transform transform, float volume, float maxLinearFalloffDistance, float minLinearFalloffDistance, float pitch)
	{ 
		GameObject newClip = new GameObject(clip.name + " Instantiation");
		newClip.transform.position = transform.position;
		// newClip.transform.parent = transform;

		AudioSource audioSource = newClip.AddComponent<AudioSource>();
		audioSource.clip = clip;
		audioSource.volume = volume;
		audioSource.minDistance = minLinearFalloffDistance;
		audioSource.maxDistance = maxLinearFalloffDistance;
		audioSource.priority = 4;

		if (pitch != 1f)
			audioSource.pitch = pitch;

		audioSource.Play();

		Object.Destroy(newClip, clip.length + 0.2f);

		return audioSource;
	}
	
	/// <summary>
	/// Immediately creates a GameObject to play the given AudioClip.  (The GameObject will destroy itself at the end of the clip.
	/// </summary>
	/// <param name="clip">The AudioClip to play.</param>
	public static void PlayClip(AudioClip clip){
		if(clip == null)
			return;

		GameObject newClip = new GameObject(clip.name + " Instantiation");
		AudioSource audioSource = newClip.AddComponent<AudioSource>();
		audioSource.clip = clip;
		audioSource.Play();

		Object.Destroy(newClip, clip.length + 0.2f);
	}

	/// <summary>
	/// Immediately creates a GameObject to play the given AudioClip.  (The GameObject will destroy itself at the end of the clip.
	/// </summary>
	/// <param name="clip">The AudioClip to play.</param>
	/// <param name="pitchBend">Set the AudioSource pitch to this value.</param>
	public static void PlayClip(AudioClip clip, float pitchBend){
		GameObject newClip = new GameObject(clip.name + " Instantiation" + pitchBend);
		AudioSource asc = newClip.AddComponent<AudioSource>();
		asc.clip = clip;
		asc.pitch = pitchBend;
		
		asc.Play();
		Object.Destroy(newClip, clip.length + 0.2f);
	}

	/// <summary>
	/// Immediately creates a GameObject to play the given AudioClip at the given worldspace position.  (The GameObject will destroy itself at the end of the clip.
	/// </summary>
	/// <param name="clip">The AudioClip to play.</param>
	/// <param name="pos">A worldspace position to spawn the GameObject.</param>
	/// <param name="pitchBend">Set the AudioSource pitch to this value.</param>
	public static void PlayClipAt(AudioClip clip, Vector3 pos, float pitchBend){
		GameObject newClip = new GameObject(clip.name + " Instantiation" + pitchBend);
		newClip.transform.position = pos;

		AudioSource asc = newClip.AddComponent<AudioSource>();
		asc.clip = clip;
		asc.pitch = pitchBend;
		
		asc.Play();
		Object.Destroy(newClip, clip.length + 0.2f);
	}

	/// <summary>
	/// Immediately creates a GameObject to play the given AudioClip at the location of the given Transform.  (The GameObject will destroy itself at the end of the clip.)
	/// </summary>
	/// <param name="clip">The AudioClip to play.</param>
	/// <param name="transform">The Transform that will provide the location.</param>
	/// <param name="volume">Sets the AudioSource volume to this level.</param>
	/// <param name="maxLinearFalloffDistance">Sets the AudioSource minDistance to this value.</param>
	/// <param name="minLinearFalloffDistance">Sets the AudioSource maxDistance to this value.</param>
	/// <returns>An instance of the AudioSource created to play the AudioClip.</returns>
	public static AudioSource PlayClipAtTransform(AudioClip clip, Transform transform, float volume, float maxLinearFalloffDistance, float minLinearFalloffDistance)
	{
		return PlayClipAtTransform(clip, transform, volume, maxLinearFalloffDistance, minLinearFalloffDistance,  1f);
	}

	/// <summary>
	/// Immediately creates a GameObject to play the given AudioClip at the location of the given Transform.  (The GameObject will destroy itself at the end of the clip.)
	/// </summary>
	/// <param name="clip">The AudioClip to play.</param>
	/// <param name="transform">The Transform that will provide the location.</param>
	/// <param name="volume">Sets the AudioSource volume to this level.</param>
	/// <returns>An instance of the AudioSource created to play the AudioClip.</returns>
	public static AudioSource PlayClipAtTransform(AudioClip clip, Transform transform, float volume)
	{
		return PlayClipAtTransform(clip, transform, volume, 1f, 500f, 1f);
	}

	/// <summary>
	/// Immediately creates a GameObject to play the given AudioClip at the given worldspace position.  (The GameObject will destroy itself at the end of the clip.
	/// </summary>
	/// <param name="clip">The AudioClip to play.</param>
	/// <param name="position">The worldspace position to spawn the GameObject.</param>
	/// <param name="volume">Set the AudioSource volume to this value.</param>
	/// <param name="maxLinearFalloffDistance">Set the AudioSource maxDistance to this value.</param>
	/// <param name="minLinearFalloffDistance">Set the AudioSource minDistance to this value.</param>
	/// <param name="pitch">Set the AudioSource pitch to this value.</param>
	/// <returns></returns>
	public static AudioSource PlayClipAtPosition(AudioClip clip, Vector3 position, float volume, float maxLinearFalloffDistance, float minLinearFalloffDistance,  float pitch)
	{ 		
		GameObject newClip = new GameObject(clip.name + " Instantiation");
		newClip.transform.position = position;

		AudioSource asc = newClip.AddComponent<AudioSource>();
		asc.clip = clip;
		asc.volume = volume;		
		asc.maxDistance = maxLinearFalloffDistance;
		asc.minDistance = minLinearFalloffDistance;
		asc.rolloffMode = AudioRolloffMode.Linear;
		if (pitch != 1f)
			asc.pitch = pitch;
		asc.Play();
		Object.Destroy(newClip, clip.length + 0.2f);
		return asc;
	}
}
