using Regent.Entities;
using UnityEngine;
using uObject = UnityEngine.Object;
using Quat = UnityEngine.Quaternion;
using Mb = UnityEngine.MonoBehaviour;

namespace Foundational
{
public static class Spawner
{
	/// instantiates a prefab (that has an Entity component)
	public static Entity CreateEntityFab(GameObject fab, Vector3 pos = default, Quat rot = default)
		=> uObject.Instantiate(fab, pos, rot).GetComponent<Entity>();
}

public static class Spawner<T> where T : Mb
{
	public static T Create(T compOnFab, Vector3 pos = default, Quat rot = default)
		=> Create(compOnFab.gameObject, pos, rot);

	public static T Create(GameObject fab, Vector3 pos = default, Quat rot = default)
		=> Spawner.CreateEntityFab(fab, pos, rot)
		   .Get<T>();
}
}