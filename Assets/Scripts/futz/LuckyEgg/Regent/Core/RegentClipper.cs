using System;
using System.Collections.Generic;
using Regent.Logging;
using Swoonity.Collections;
using Swoonity.MHasher;
using UnityEngine;
using static UnityEngine.Debug;

namespace Regent.Clips
{
[Serializable]
public class RegentClipper : MonoBehaviour
{
	public static RegentClipper I; // for now
	
	Dictionary<MHash, IClip> _hash__clip = new(32);
	Dictionary<Type, IClip> _type__clip = new(32);
	Dictionary<string, IClip> _string__clip = new(32);

	public void Add(IClip clip)
	{
		var hash = clip.GetHash();

		if (_hash__clip.Has(hash)) {
			clip = HandleDuplicate(clip);
		}

		_hash__clip[hash] = clip;
		_type__clip[clip.GetType()] = clip;
		_string__clip[clip.GetType().Name] = clip;
		clip.SetRef();

		if (RLog.Clip.On) Log($"RegenClipper.Add {clip}"._RLog(RLog.Clip));
	}

	public void Cut(IClip clip)
	{
		_hash__clip.Remove(clip.GetHash());
		_type__clip.Remove(clip.GetType());
		_string__clip.Remove(clip.GetType().Name);
	}

	// public virtual T Get<T>() => (T)_type__clip.Get(typeof(T));
	public virtual TClip Get<TClip>() where TClip : MonoBehaviour, IClip
	{
		var (has, clip) = StorageForClipper<TClip>.HasGet();
		if (has) return clip;

		(has, clip) = ((bool, TClip)) _type__clip.HasGet(typeof(TClip));
		if (has) {
			StorageForClipper<TClip>.Set(clip);
			return clip;
		}

		return default;
	}

	public virtual IClip Get(MHash hashId) => _hash__clip.Get(hashId);
	public virtual IClip Get(Type type) => _type__clip.Get(type);
	public virtual IClip Get(string typeName) => _string__clip.Get(typeName);


	public virtual IClip HandleDuplicate(IClip duplicateClip)
	{
		throw new NotImplementedException(
			$"Regent clipper {duplicateClip} has unhandled duplicate. " +
			$"Ensure there is only 1 of this type of Clip!"
		);
	}
}

public static class StorageForClipper<TClip> where TClip : MonoBehaviour, IClip
{
	static TClip _clip;
	// public static TClip Get() => _clip;
	public static (bool has, TClip clip) HasGet() => (!!_clip, _clip);
	public static void Set(TClip clip) => _clip = clip;
}

// Holds shared state. See: Blackboard pattern
public interface IClip
{
	public MHash GetHash();
	public void SetRef();
}
}