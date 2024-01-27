using System;
using System.Collections.Generic;
using Regent.Barons;
using Regent.Catalog;
using Regent.Clips;
using Regent.Core;
using Regent.Entities;
using Regent.Logging;
using Regent.StageFacts;
using Regent.Staging;
using Regent.Workers;
using Swoonity.Collections;
using Swoonity.MHasher;
using Swoonity.Unity;
using UnityEngine;
using static UnityEngine.Debug;

// ReSharper disable InconsistentNaming

namespace Regent.Coordinator
{
/// TODO: Documentation, see: _Regent.md
public class RegentCoordinator : MonoBehaviour, IRegentCoordinator
{
	/// use sparingly
	public static RegentCoordinator __Coordinator;
	public static bool __Running;

	[Header("Config")]
	public RegentCatalogDef CatalogDef;
	public RegentClipper Clipper;

	[Header("Logs")]
	public RLog.LogConfig LogConfig;

	[Header("Debug")]
	public bool AddDebugRefs = false;
	public List<WorkerDebugRef> WorkerDebugRefs = new();


	public MHashLup<Baron> Barons = new(32);

	/// increments down
	int _nativeEntityId;
	public EntityLup Entities = new(64);
	public Queue<Entity> EntitiesToAdd = new(32);
	public Queue<int> EntitiesToCut = new(32);

	public WorkerLup Workers = new(64);
	public WorkerLup Workers_ADDED = new(64);
	public WorkerLup Workers_REMOVED = new(64);
	public WorkerLup Workers_ENABLED = new(64);
	public WorkerLup Workers_DISABLED = new(64);
	public WorkerLup Workers_RUN = new(64);
	public WorkerLup Workers_ALL = new(64);
	public WorkerLup Workers_VERIFY = new(64);
	public WorkerLup Workers_REACT = new(64);
	public WorkerLup Workers_REGISTRY = new(64);

	public MHashLup<StageState> Stages = new(64);
	public List<StageState> Stages_SPAWN = new(4);
	public List<StageState> Stages_UPDATE = new(32);
	public List<StageState> Stages_LATE = new(32);
	public List<StageState> Stages_FIXED = new(32);
	public List<StageState> Stages_CUSTOM = new(32);

	#region Lifecycle

	void OnValidate()
	{
		RLog.Config = LogConfig;
	}

	/// guaranteed to be first (hopefully...)
	void Awake()
	{
		RLog.Config = LogConfig;
		RLog.CurrentCycle = STR_COORDINATOR_PRE_INIT;
		RLog.CurrentStage = STR_NONE;

		if (!CatalogDef) throw Errors.MissingConfig(this, nameof(CatalogDef));
		if (!Clipper) throw Errors.MissingConfig(this, nameof(Clipper));

		__Coordinator = this;
		__Running = true;
		Baron.__Coordinator = this;
		Baron.__Clipper = Clipper;
		Entity.__Coordinator = this;
		RegentClipper.I = Clipper;

		RegentCatalogRuntime.RuntimeInitialize(CatalogDef);

		InitializeAllStages();

		if (AddDebugRefs) {
			InitializeDebugRefs(this);
		}

		if (RLog.Important.On) {
			var time = DateTime.Now.ToShortTimeString();
			// Log($"~~~~~~ ~~~~~~ ~~~~~~ ~~~~~~ ~~~~~~ ~~~~~~"._RLog(RLog.Important));
			Log($"{time} Initialized Regent Coordinator"._RLog(RLog.Important));
			// Log($"~~~~~~ ~~~~~~ ~~~~~~ ~~~~~~ ~~~~~~ ~~~~~~ ~~~~~~"._RLog(RLog.Important));
		}

		RLog.CurrentCycle = STR_COORDINATOR_INITIALIZED;
	}

	void OnApplicationQuit()
	{
		if (RLog.Important.On && Time.frameCount > 0) {
			Log($"OnApplicationQuit {name} xxxxxxxxxx"._RLog(RLog.Important));
		}

		__Coordinator = null;
		__Running = false;
	}

	void OnDisable()
	{
		if (RLog.Important.On && Time.frameCount > 0) {
			Log($"OnDisable {name} <<<<<<<<<<<<<<<"._RLog(RLog.Important));
		}

		__Coordinator = null;
		__Running = false;
	}

	#endregion

	#region Stage management

	void InitializeAllStages()
	{
		foreach (var stage in RegentCatalogRuntime.StageStates) {
			Stages.Set(stage);
			GetStageCycleList(stage.Fact.Cycle).Add(stage);
		}
	}

	List<StageState> GetStageCycleList(UnityCycle cycle)
		=> cycle switch {
			UnityCycle.SPAWN => Stages_SPAWN,
			UnityCycle.UPDATE => Stages_UPDATE,
			UnityCycle.LATE => Stages_LATE,
			UnityCycle.FIXED => Stages_FIXED,
			UnityCycle.CUSTOM => Stages_CUSTOM,
			UnityCycle.UNSET => throw new Exception($"bad stage cycle"),
			_ => throw new ArgumentOutOfRangeException()
		};

	#endregion

	#region Baron management

	void AddBaron(Baron baron)
	{
		baron.HashId = MHash.Hash(baron.GetType());
		baron.Fact = RegentCatalogRuntime.GetBaronFact(baron.HashId);
		Barons.Set(baron);

		if (RLog.Baron.On) Log($"++BARON {baron}"._RLog(RLog.Baron), baron);

		var workerParent = baron.transform; // TODO

		foreach (var workerFact in baron.Fact.WorkerFacts) {
			var worker = WorkerCreation.MakeWorker(baron, workerFact);
			worker.SceneGobj = workerParent.NewChild(workerFact.Name).gameObject;

			var workerRef = worker.SceneGobj.AddComponent<WorkerRef>();
			workerRef.Fact = workerFact;
			workerRef.Worker = worker;

			baron.Workers.Add(worker);

			AddWorker(worker);

			if (AddDebugRefs) DebugAddWorker(this, workerRef);
		}

		baron.__SetNetStatus(_isServer, _isClient);
	}

	void CutBaron(Baron baron)
	{
		if (Time.frameCount > 0) {
			LogWarning($"TODO: CutBaron {baron}");
		}
	}

	#endregion

	#region Entity management

	void AddQueuedEntities()
	{
		var addedCountThisFrame = EntitiesToAdd.Count;
		for (var i = 0; i < addedCountThisFrame; i++) {
			AddEntity(EntitiesToAdd.Dequeue());
		}
	}

	void CutQueuedEntities()
	{
		var cutCountThisFrame = EntitiesToCut.Count;
		for (var i = 0; i < cutCountThisFrame; i++) {
			CutEntity(EntitiesToCut.Dequeue());
		}
	}

	void AddEntity(Entity entity)
	{
		if (!entity) {
			if (RLog.Entity.On) Log($"+/-ENTITY was registered but is now null"._RLog(RLog.Entity));
			return; //>> null entity 
		}

		if (RLog.Entity.On) Log($"++ENTITY {entity}"._RLog(RLog.Entity), entity);

		Workers_ADDED.Workers_AddIfRelevant(entity);
		Workers_REMOVED.Workers_AddIfRelevant(entity);
		Workers_RUN.Workers_AddIfRelevant(entity);
		Workers_ALL.Workers_AddIfRelevant(entity);
	}

	void CutEntity(int entityId)
	{
		if (RLog.Entity.On) Log($"--ENTITY {entityId}#"._RLog(RLog.Entity));

		Entities.Cut(entityId);
		Workers_ADDED.Workers_CutIfRelevant(entityId);
		Workers_REMOVED.Workers_CutIfRelevant(entityId);
		Workers_RUN.Workers_CutIfRelevant(entityId);
		Workers_ALL.Workers_CutIfRelevant(entityId);
	}

	#endregion

	#region Worker management

	void AddWorker(BaseWorker worker)
	{
		if (RLog.Worker.On) Log($"++WORKER {worker}"._RLog(RLog.Worker), worker.SceneGobj);

		var fact = worker.Fact;

		Stages.Get(fact.StageHash)
		   .AddWorker(worker);

		Workers.Set(worker);
		GetWorkerLupByTrigger(fact.WorkerTrigger)
		   .Set(worker);

		var needToCheckExistingEntities = fact.WorkerTrigger
			is WorkerTrigger.ENABLED
			or WorkerTrigger.REGISTRY;

		if (needToCheckExistingEntities) {
			// Log($"AddWorker: {worker}, checking existing entities"._RLog(RLog.Worker));
			foreach (var (_, entity) in Entities) {
				worker.AddIfRelevant(entity);
			}
		}
	}

	void CutWorker(BaseWorker worker)
	{
		if (RLog.Worker.On) Log($"--WORKER {worker}"._RLog(RLog.Worker));
		// Stages.Get(worker.Fact.StageHash).CutWorker(worker);
		throw new NotImplementedException($"TODO"); // TODO
	}

	WorkerLup GetWorkerLupByTrigger(WorkerTrigger trigger)
	{
		return trigger switch {
			WorkerTrigger.ADDED => Workers_ADDED,
			WorkerTrigger.REMOVED => Workers_REMOVED,
			WorkerTrigger.ENABLED => Workers_ENABLED,
			WorkerTrigger.DISABLED => Workers_DISABLED,
			WorkerTrigger.RUN => Workers_RUN,
			WorkerTrigger.VERIFY => Workers_VERIFY,
			WorkerTrigger.REACT => Workers_REACT,
			WorkerTrigger.ALL => Workers_ALL,
			WorkerTrigger.REGISTRY => Workers_REGISTRY,

			WorkerTrigger.UNSET => throw new Exception($"UNSET is not valid trigger"),
			_ => throw new ArgumentOutOfRangeException(nameof(trigger), trigger, null)
		};
	}

	#endregion

	#region Cycles

	void Update()
	{
		RLog.CurrentCycle = STR_SPAWN;

		foreach (var stageState in Stages_SPAWN) {
			RLog.CurrentStage = stageState.Name;
			stageState.Workers_TryExecuteAll(_isServer, _isClient);
		}

		AddQueuedEntities();


		RLog.CurrentCycle = STR_UPDATE;

		foreach (var stageState in Stages_UPDATE) {
			RLog.CurrentStage = stageState.Name;
			stageState.Workers_TryExecuteAll(_isServer, _isClient);
			CutQueuedEntities();
		}

		RLog.CurrentCycle = STR_WAS_UPDATE;
		RLog.CurrentStage = STR_NONE;
	}

	void LateUpdate()
	{
		RLog.CurrentCycle = STR_LATE;

		foreach (var stageState in Stages_LATE) {
			RLog.CurrentStage = stageState.Name;
			stageState.Workers_TryExecuteAll(_isServer, _isClient);
			CutQueuedEntities();
		}

		RLog.CurrentCycle = STR_WAS_LATE;
		RLog.CurrentStage = STR_NONE;
	}

	void FixedUpdate()
	{
		RLog.CurrentCycle = STR_FIXED;

		foreach (var stageState in Stages_FIXED) {
			RLog.CurrentStage = stageState.Name;
			stageState.Workers_TryExecuteAll(_isServer, _isClient);
			CutQueuedEntities();
		}

		RLog.CurrentCycle = STR_WAS_FIXED;
		RLog.CurrentStage = STR_NONE;
	}

	// TODO: custom cycle?

	#endregion

	#region Networking

	bool _isServer = false;
	bool _isClient = false;

	public void OnStartClient() => SetNetStatus(_isServer, true);
	public void OnCloseClient() => SetNetStatus(_isServer, false);
	public void OnStartServer() => SetNetStatus(true, _isClient);
	public void OnCloseServer() => SetNetStatus(false, _isClient);

	void SetNetStatus(bool setServer, bool setClient)
	{
		_isClient = setClient;
		_isServer = setServer;
		foreach (var (_, baron) in Barons) {
			baron.__SetNetStatus(_isServer, _isClient);
		}

		Baron.IsClient = _isClient;
		Baron.IsServer = _isServer;
	}

	#endregion

	#region Debug

	static void InitializeDebugRefs(RegentCoordinator coord)
	{
		var catalog = coord.CatalogDef;
		var allStagesTf = coord.transform.NewChild("STAGES debug");

		void MakeStageDebug(string name, List<StageState> stageStats)
		{
			var stageTf = allStagesTf.NewChild(name);

			foreach (var stageState in stageStats) {
				var debugTf = stageTf.NewChild(stageState.Name);
				var debugRef = debugTf.AddComponent<StageDebugRef>();
				stageState.DebugRef = debugRef;
				debugRef.NativeWorkerRefs = debugTf.NewChild(STR_NATIVE);
				debugRef.ServerWorkerRefs = debugTf.NewChild(STR_SERVER);
				debugRef.ClientWorkerRefs = debugTf.NewChild(STR_CLIENT);
				debugRef.AuthorWorkerRefs = debugTf.NewChild(STR_AUTHOR);
				debugRef.RemoteWorkerRefs = debugTf.NewChild(STR_REMOTE);
			}
		}

		MakeStageDebug(STR_SPAWN, coord.Stages_SPAWN);
		MakeStageDebug(STR_UPDATE, coord.Stages_UPDATE);
		MakeStageDebug(STR_LATE, coord.Stages_LATE);
		MakeStageDebug(STR_FIXED, coord.Stages_FIXED);
		MakeStageDebug(STR_CUSTOM, coord.Stages_CUSTOM);
	}

	static void DebugAddWorker(RegentCoordinator coord, WorkerRef workerRef)
	{
		var workerFact = workerRef.Fact;

		var stageState = coord.Stages.Get(workerFact.StageHash);
		var stageDebugRef = stageState.DebugRef;
		var tf = stageDebugRef.GetTransformByDomain(workerFact.Domain);

		var debugName =
			$"{workerFact.Name} ({workerFact.RequiredTypeNamesShort.Join()}) {workerFact.Domain}"; // TODO: arg types
		var debugRef = tf.NewChild(debugName).AddComponent<WorkerDebugRef>();
		debugRef.WorkerRef = workerRef;
		debugRef.Fact = workerFact;
		debugRef.Worker = workerRef.Worker;
	}

	const string STR_COORDINATOR_PRE_INIT = "pre-init";
	const string STR_COORDINATOR_INITIALIZED = "init";
	const string STR_NONE = "_";
	const string STR_SPAWN = "spawn";
	const string STR_UPDATE = "update";
	const string STR_LATE = "late";
	const string STR_FIXED = "fixed";
	const string STR_CUSTOM = "custom";
	const string STR_WAS_SPAWN = "-post-spawn";
	const string STR_WAS_UPDATE = "-post-update";
	const string STR_WAS_LATE = "-post-late";
	const string STR_WAS_FIXED = "-post-fixed";
	const string STR_WAS_CUSTOM = "-post-custom";
	const string STR_NATIVE = "native";
	const string STR_SERVER = "server";
	const string STR_CLIENT = "client";
	const string STR_AUTHOR = "author";
	const string STR_REMOTE = "remote";

	public static class Errors
	{
		public static Exception MissingConfig(RegentCoordinator coord, string config)
			=> new($"{coord} missing config {config}");
	}

	#endregion

	#region public Handlers

	public void HandleBaronEnabled(Baron baron) => AddBaron(baron);
	public void HandleBaronDisabled(Baron baron) => CutBaron(baron);
	public void HandleClipCreated(IClip clip) => Clipper.Add(clip);
	public void HandleClipDestroyed(IClip clip) => Clipper.Cut(clip);

	public void HandleEntityCreated(Entity entity)
	{
		if (entity.EntityId <= 0) { // if networked, this will be >=1 set by liaison 
			var newEntityId = --_nativeEntityId;

			if (entity.EntityId < 0 && RLog.Entity.On) {
				// entity was previously spawned (pooling)
				Log($"entityId {entity.EntityId} is now {newEntityId}"._RLg(RLog.Entity));
			}

			entity.EntityId = newEntityId;
		}

		Entities.Set(entity);
		EntitiesToAdd.Enqueue(entity);
		Workers_REGISTRY.Workers_AddIfRelevant(entity);
		Workers_ENABLED.Workers_AddIfRelevant(entity);

		if (RLog.Entity.On) Log($"ENTITY created: {entity}"._RLg(RLog.Entity));
	}

	public void HandleEntityDestroyed(Entity entity)
	{
		if (!__Running) return; //>> app quit

		EntitiesToCut.Enqueue(entity.EntityId);
		Workers_REGISTRY.Workers_HandleDestroyedIfRelevant(entity);
		Workers_DISABLED.Workers_HandleDestroyedIfRelevant(entity);

		if (RLog.Entity.On)
			Log($"ENTITY destroyed: {entity} (iid {entity.InstanceId})"._RLg(RLog.Entity));
	}

	#endregion
}
}