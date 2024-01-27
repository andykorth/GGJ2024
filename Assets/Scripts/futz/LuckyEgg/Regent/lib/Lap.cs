using System.Collections;
using UnityEngine;

namespace LapUtil
{
/// TODO: actually going to use this or not?
/// 
/// int wrapper, representing a Frame number (Time.frameCount) 
public readonly struct Lap
{
	readonly int _value;

	public Lap(int value) => _value = value;

	public static implicit operator Lap(int value) => new Lap(value);
	public static implicit operator int(Lap lap) => lap._value;


	public static Lap Frame() => Time.frameCount;
	public static Lap This() => Time.frameCount;
	public static Lap Last() => Time.frameCount - 1;
	public static Lap Next() => Time.frameCount + 1;


	#region Frame Checking

	static Lap _completed;

	/// The last Lap (frame) that was completed.
	/// Generally this will be Time.frameCount - 1;
	public static Lap GetLastCompleted() => _completed;

	public static IEnumerator Timing()
	{
		_completed = Time.frameCount; // should be 0

		while (true) {
			yield return null;
			// should happen at the very end of frame?
			_completed = Time.frameCount;
		}
	}


	public bool IsCurrent() => _value == _completed + 1;

	public bool WasLast() => _value == _completed;
	// public bool Relevant() => _value == _completed;

	#endregion
}
}