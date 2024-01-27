using UnityEngine;

namespace Swoonity.Unity
{
public class EnableChildrenOnAwake : MonoBehaviour
{
	void Awake()
	{
		var tf = transform;
		foreach (Transform child in tf) {
			child.gameObject.SetActive(true);
		}
	}
}
}