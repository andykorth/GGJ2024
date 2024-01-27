using System;
using System.Collections.Generic;
using Regent.Coordinator;
using Regent.BaronFacts;
using Regent.Clips;
using Regent.Staging;
using Regent.Workers;
using Swoonity.MHasher;
using UnityEngine;
using static UnityEngine.Debug;

namespace Regent.Barons
{
public abstract class Baron : MonoBehaviour, IMHashable
{
	public static IRegentCoordinator __Coordinator;
	public static RegentClipper __Clipper; // for now

	public BaronFact Fact;
	public MHash HashId;
	public MHash GetHash() => HashId;
	public List<BaseWorker> Workers = new();


	/// Called when changed in editor
	public virtual void WhenEditorValidates() { }

	// TODO: convert these to workers?
	protected virtual void WhenCreated() { }
	protected virtual void WhenNetChange(bool setClient, bool setServer) { }
	protected virtual void WhenDestroyed() { }


	void OnEnable() => __Coordinator.HandleBaronEnabled(this);
	void OnDisable() => __Coordinator.HandleBaronDisabled(this);

	void Start() => WhenCreated();
	void OnDestroy() => WhenDestroyed();

	void OnValidate()
	{
		// just for editor debug purposes (this will be overriden by the coordinator at runtime)
		Fact = BaronFactMakers.MakeBaronFact(GetType());

		WhenEditorValidates();
	}


	/// these are set by the Coordinator
	public static bool IsServer = false;
	public static bool IsClient = false;
	public static bool IsHost => IsServer && IsClient;

	public void __SetNetStatus(bool setServer, bool setClient)
		=> WhenNetChange(setClient, setServer);


	public static TClip Clip<TClip>() where TClip : MonoBehaviour, IClip => __Clipper.Get<TClip>();

	#region Stage Creation helpers

	public const string UpdateStage = StageCreation.StageMarker.Update;
	public const string LateStage = StageCreation.StageMarker.Late;
	public const string FixedStage = StageCreation.StageMarker.Fixed;
	public const string CustomStage = StageCreation.StageMarker.Custom;

	protected static class stage
	{
		public const string SPAWN = StageCreation.StageMarker.Spawn;
		public const string UPDATE = StageCreation.StageMarker.Update;
		public const string LATE = StageCreation.StageMarker.Late;
		public const string FIXED = StageCreation.StageMarker.Fixed;
		public const string CUSTOM = StageCreation.StageMarker.Custom;
	}

	// NEEDED for reflection
	public const string __SKIP_STAGE__ = StageCreation.StageMarker.__SKIP_STAGE__;

	#endregion

	public override string ToString() => $"{Fact.Name}";
}
}