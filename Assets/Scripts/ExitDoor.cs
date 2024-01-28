using System;
using Foundational;
using futz.ActGhost;
using UnityEngine;


public class ExitDoor : MonoBehaviour
{
	public GameObject OpenObj;
	public GameObject ClosedObj;

	public GameObject BlockerObj;

	public void SetDoor(bool isOpen, bool isBlocked)
	{
		OpenObj.SetActive(isOpen);
		ClosedObj.SetActive(!isOpen);
		BlockerObj.SetActive(isBlocked);
	}
}
