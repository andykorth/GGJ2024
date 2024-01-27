using UnityEngine;

namespace Swoonity.Unity
{
public class DontDestroyOnLoad : MonoBehaviour
{
	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}
}
}