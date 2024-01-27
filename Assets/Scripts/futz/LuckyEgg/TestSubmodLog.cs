using UnityEngine;

public class TestSubmodLog : MonoBehaviour
{
	public bool DoLog;

	void Start() => PrintLog();

	void Update()
	{
		if (!DoLog) return;

		DoLog = false;
		PrintLog();
	}

	void PrintLog()
	{
		Debug.Log($"333 test");
	}
}