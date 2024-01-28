using Regent.Clips;
using Sonic;

namespace futz
{
	public class AudioSonicClip : ClipNative
	{
		public static AudioSonicClip I;
		public override void SetRef() => I = this;

		public SonicManager SonicManager;
	}

// public static class WabiAudioSonicHelpers
// {
// 	public static void PlayAt(this SonicSfx sfx, Vector3 pos)
// 	{
// 		AudioSonicClip.I.SonicManager.PlayAt(sfx, pos);
// 	}
//
// 	public static void PlayAt(this SonicSfx sfx, Transform tf) => PlayAt(sfx, tf.position);
//
// 	public static void PlayAt(this SonicSfx sfx, MonoBehaviour mb)
// 		=> PlayAt(sfx, mb.transform.position);
// }
}