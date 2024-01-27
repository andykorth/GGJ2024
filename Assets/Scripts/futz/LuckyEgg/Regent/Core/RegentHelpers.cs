using Regent.Coordinator;
using Regent.Entities;
using Regent.Logging;
using UnityEngine;
using static UnityEngine.Debug;

namespace Regent.Core
{
public static class RegentHelpers
{
	// TODO: change to HasGet
	/// for de/serialization
	public static Entity GetEntity(int entityId)
	{
		var entity = RegentCoordinator.__Coordinator.Entities.Get(entityId);

		if (entity == null) {
			if (RLog.Entity.Enabled)
				Log($"GetEntity can't find entity {entityId}"._RLog(RLog.Entity));
			return null;
		}

		return entity;
	}

	public static TComp GetEntityComp<TComp>(int entityId) where TComp : MonoBehaviour
	{
		if (entityId == 0) return null;

		var entity = GetEntity(entityId);
		return entity ? entity.Get<TComp>() : null;
	}
}
}