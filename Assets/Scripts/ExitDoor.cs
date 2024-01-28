using System;
using Foundational;
using futz.ActGhost;
using UnityEngine;


public class ExitDoor : MonoBehaviour
{
	public GameObject OpenObj;
	public GameObject ClosedObj;


	public void Start()
	{
		SetDoor(false);
	}

	public void SetDoor(bool isOpen)
	{
		OpenObj.SetActive(isOpen);
		ClosedObj.SetActive(!isOpen);
	}
}
