using UnityEngine;
using UnityEngine.Audio;

namespace Sonic
{
[CreateAssetMenu(menuName = "Sonic SFX/Sonic Config")]
public class SonicConfig : ScriptableObject
{
	[Header("Sonic Manager config")]
	public bool AutoRegisterSugarFns = true;

	[Header("Pooled Emitter config")]
	[Tooltip("0 = full 2D, 1 = full 3D")]
	[Range(0f, 1f)]
	public float SpatialBlend2dTo3d = 1f;
	[Tooltip("how volume goes down with distance")]
	public AudioRolloffMode RolloffMode = AudioRolloffMode.Logarithmic;
	[Tooltip("<this = max volume")]
	public float RolloffStartDistance = 1f;
	[Tooltip(">this = 0 volume")]
	public float RolloffEndDistance = 500f;

	// TEMP
	public float LengthBuffer = .1f;

	[Header("Refs")]
	public AudioMixer Mixer;

	[Header("Logging")]
	public bool LogEvents;
}
}