using FutzSys;
using Regent.Clips;
using Regent.Core;
using Regent.Syncers;
using UnityEngine;

namespace Foundational
{
public class GameSysClip : ClipNative
{
	public static GameSysClip I;
	public override void SetRef() => I = this;

	public FutzConfig FutzConfig;
	public FutzHost FutzHost;
	public float ReconnectIntervalMs = 2000;


	[Header("State")]
	public Track<string> RoomIdf = new();
	public Track<string> Status = new();
	public MatcherActivity MatcherActivity;
	public SystemActivity SystemActivity;
	public TrackChoice<ActivityDef> ActivityChoice = new();
	public Registry<Agent> Agents;
	public Track<ActivityBase> CurrentActivity = new(); // TODO: combine with host?

	protected override void WhenAwake()
	{
		GlobalMiscSystemInitializers.Initialize(this);
		Agents = GameSysBaron.Agents_; // TEMP
	}
}
}


// [Header("Spawning")]
// public TrackCued<ItemSpawnInfo> CuedSpawnItem = new();
// public TrackCued<Ka> CuedDespawnItem = new();
// /// sugar (easier IDE tracking)
// public void CueSpawn(ItemSpawnInfo info) => CuedSpawnItem.Cue(info);
// public void CueDespawn(Ka ka) => CuedDespawnItem.Cue(ka);