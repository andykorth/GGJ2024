using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Debug;

namespace Swoonity.Unity
{
public class EnableUnparentChildOnAwake : MonoBehaviour
{
	public List<Transform> Children = new();
	public bool ThenDestroySelf = true;

	void OnValidate()
	{
		Children.Clear();
		foreach (Transform child in transform) {
			Children.Add(child);
		}
	}

	void Awake()
	{
		foreach (var child in Children) {
			child.gameObject.SetActive(true);
			child.parent = transform.parent;
		}

		if (ThenDestroySelf) {
			Log($"destroy self {GetType().Name} {gameObject}");
			Destroy(gameObject);
		}
	}
}
}