using System;
using System.Collections.Generic;
using UnityEngine;

namespace Weasel
{
// TODO: handle when obj is destroyed
public class WeaselManager : MonoBehaviour
{
	public static WeaselManager I;

	[Header("State")]
	public List<BaseWeasel> Weasels = new();

	void Awake()
	{
		// UnityLerps.DefineAll();

		if (!I) {
			I = this;
		}
		else if (I != this) {
			throw new Exception($"WeaselManager already exists TODO handle");
		}
	}


	public void TickAll(float dt)
	{
		var lastDex = Weasels.Count - 1;

		for (var i = lastDex; i >= 0; i--) {
			var weasel = Weasels[i];

			if (weasel.IsPlaying) {
				weasel.Tick(dt);
				continue; //>> playing
			}

			if (weasel.KeepActive) {
				continue; //>> not playing but keep active
			}

			//>> remove
			Weasels[i] = Weasels[lastDex]; // faster list removal
			Weasels.RemoveAt(lastDex);
			--lastDex;
		}
	}

	public static void PlayWeasel(BaseWeasel weasel)
	{
		if (!I) {
			I = new GameObject(nameof(WeaselManager))
			   .AddComponent<WeaselManager>();
		}

		I.Weasels.Add(weasel);
	}
}
}