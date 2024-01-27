using FutzSys;
using Regent.Workers;
using UnityEngine;

namespace Foundational
{
public class AgentActorBaron : FutzBaron
{
	static GameSysClip GameSys_ => GameSysClip.I;


	static Transform _ActorRoot;

	protected override void WhenCreated()
		=> _ActorRoot = new GameObject("<><><> ACTORS <><><>").transform;

	[React.Native(UNSURE_WHAT_STAGE, nameof(GameSysClip.ActivityChoice))]
	static void React_LoadedActivityDef(
		GameSysClip gameSys,
		(ActivityDef activityDef, int index) value
	)
	{
		var host = GameSys_.FutzHost;

		foreach (var (_, agent) in host.AgentLup) {
			TryDespawnActor(agent);
			TrySpawnActor(agent);
		}
	}

	[Added.Native(CUE_SPAWN)]
	static void Added_Agent(Agent agent) => TrySpawnActor(agent);

	[Disabled.Native]
	static void Disabled_Agent(Agent agent) => TryDespawnActor(agent);


	static void TrySpawnActor(Agent agent)
	{
		var host = GameSys_.FutzHost;
		var activity = host.CurrentActivity;
		if (!activity) return; //>> no current activity

		var activityDef = activity.Def;
		if (!activityDef.Fab_Actor) return; //>> activity doesn't need actor

		var actor = Instantiate(activity.Def.Fab_Actor, _ActorRoot).GetComponent<ActorBase>();
		actor.name = $"actor {agent.SlotId} {agent.Nickname} ({activity.Def.Idf})";
		actor.Agent = agent;

		agent.CurrentActivity = activity;
		agent.CurrentActor = actor;
	}

	static void TryDespawnActor(Agent agent)
	{
		var actor = agent.CurrentActor;
		if (!actor) return; //>> agent didn't have actor

		// TEMP
		// TODO: reconnection etc.
		// TODO: pooling
		Destroy(actor.gameObject);
	}
}
}