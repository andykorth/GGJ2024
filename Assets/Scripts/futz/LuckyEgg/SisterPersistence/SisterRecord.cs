using System;

namespace SisterPersistence
{
public delegate void PackRecord(SisterRecord record);
public delegate void UnpackRecord(SisterRecord record);

[Serializable]
public abstract class SisterRecord
{
	public PackRecord FnPack;
	public UnpackRecord FnUnpack;

	public abstract void Reset();

	public virtual void Pack()
	{
		if (FnPack == null) throw new Exception($"{this} missing FnPack or override");
		FnPack.Invoke(this);
	}

	public virtual void Unpack()
	{
		if (FnUnpack == null) throw new Exception($"{this} missing FnUnpack or override");
		FnUnpack.Invoke(this);
	}

	public void RegisterFn<T>(Action<T> pack, Action<T> unpack) where T : SisterRecord
	{
		FnPack = record => pack(record as T);
		FnUnpack = record => unpack(record as T);
	}
}
}