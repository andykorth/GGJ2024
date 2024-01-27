using System;
using UnityEngine;

namespace Weasel
{
[Serializable]
public class WeaselTf : BaseWeasel
{
	[Header("Transform Config")]
	public Transform Tf;

	[Header("Position")]
	public bool DoPosition;
	public Vector3 ToPosition;
	public bool UseLocalPosition;

	[Header("Rotation")]
	public bool DoRotation;
	public Vector3 ToRotation;
	public bool UseLocalRotation;

	[Header("Scale")]
	public bool DoScale;
	public Vector3 ToScale;

	[Header("Misc / State")]
	public Vector3 FromPosition;
	public Quaternion FromRotation;
	public Vector3 FromScale;
	public Transform ToWorldTf;

	// public override void Initialize() { }

	public override bool CheckIsValid() => Tf;

	public override void SetFrom()
	{
		if (!Tf) {
			Stop(); // HACK
			return;
		}

		FromPosition = UseLocalPosition ? Tf.localPosition : Tf.position;
		FromRotation = UseLocalRotation ? Tf.localRotation : Tf.rotation;
		FromScale = Tf.localScale;
	}

	public void PlayWith(
		Vector3 toPosition = default,
		Vector3 toRotation = default,
		Vector3 toScale = default,
		Transform toWorldTf = null
	)
	{
		ToPosition = toPosition;
		ToRotation = toRotation;
		ToScale = toScale;
		ToWorldTf = toWorldTf;
		BeginPlaying();
	}

	public void Play() => BeginPlaying();
	public void PlayPosition(Vector3 toPosition) => PlayWith(toPosition: toPosition);
	public void PlayRotation(Vector3 toRotation) => PlayWith(toRotation: toRotation);
	public void PlayScale(Vector3 toScale) => PlayWith(toScale: toScale);
	public void PlayWorldTf(Transform toWorldTf) => PlayWith(toWorldTf: toWorldTf);

	public override void ApplyFractionValue(float frac)
	{
		if (!Tf) {
			Stop();
			return;
		}

		if (DoPosition) SetPosition(frac);
		if (DoRotation) SetRotation(frac);
		if (DoScale) SetScale(frac);
	}

	public void SetPosition(float frac)
	{
		if (ToWorldTf) ToPosition = ToWorldTf.position;

		var position = Vector3.Lerp(FromPosition, ToPosition, frac);

		if (UseLocalPosition) Tf.localPosition = position;
		else Tf.position = position;
	}


	public void SetRotation(float frac)
	{
		if (ToWorldTf) ToRotation = ToWorldTf.eulerAngles;

		var rotation = Quaternion.Lerp(FromRotation, Quaternion.Euler(ToRotation), frac);

		if (UseLocalRotation) Tf.localRotation = rotation;
		else Tf.rotation = rotation;
	}

	public void SetScale(float frac)
	{
		if (ToWorldTf) ToScale = ToWorldTf.localScale;

		var scale = Vector3.Lerp(FromScale, ToScale, frac);
		Tf.localScale = scale;
	}
}
}