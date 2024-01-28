using Foundational;
using futz.ActGhost;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		var player = other.gameObject.GetComponent<Player>();
		if (player == null) return;

		var act = GameSysClip.I.GhostAct.Current;
		GhostLogic.EndSuccessful(act);
	}
}