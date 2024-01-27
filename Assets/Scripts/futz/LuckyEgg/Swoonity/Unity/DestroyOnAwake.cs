using UnityEngine;
using static UnityEngine.Debug;

namespace Swoonity.Unity
{
public class DestroyOnAwake : MonoBehaviour
{
	void Awake()
	{
		Log($"{GetType().Name} {gameObject}");
		Destroy(gameObject);
	}
}
}