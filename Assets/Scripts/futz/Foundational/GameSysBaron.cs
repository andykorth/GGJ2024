using System;
using Cysharp.Threading.Tasks;
using FutzSys;
using Lumberjack;
using Regent.Core;
using Regent.Workers;
using Swoonity.Unity;
using UnityEngine;
using static UnityEngine.Debug;

namespace Foundational
{
public class GameSysBaron : FutzBaron
{
	static GameSysClip GameSys_ => GameSysClip.I;


	[Registry] public static Registry<Agent> Agents_ = new();

	static Transform _AgentRoot;

	protected override void WhenCreated()
		=> _AgentRoot = new GameObject("=== AGENTS ===").transform;

	[ClipInit]
	static void Init_GameSys(GameSysClip gameSys)
	{
		Log($"Init Packets".LgBlue());
		var config = gameSys.FutzConfig;

		var system = Instantiate(config.SystemDef.Fab_Activity).GetComponent<SystemActivity>();
		system.RuntimeInitialize(gameSys.FutzHost, config.SystemDef);
		gameSys.SystemActivity = system;
		gameSys.FutzHost.SystemActivity = system;

		var matcher = Instantiate(config.MatcherDef.Fab_Activity).GetComponent<MatcherActivity>();
		matcher.RuntimeInitialize(gameSys.FutzHost, config.MatcherDef);
		gameSys.MatcherActivity = matcher;
		gameSys.FutzHost.MatcherActivity = matcher;


		// var activityNames = config.ActivityDefs.MapNew(static def => def.Name);
		// var activityIndex = config.ActivityDefs.IndexOf(config.AutoloadActivity);
		gameSys.ActivityChoice.SetChoices(config.ActivityDefs);


		StartRelayRoom(gameSys).Forget();
	}

	/// TEMP
	/// TODO: reconnect, etc.
	static async UniTaskVoid StartRelayRoom(GameSysClip gameSys)
	{
		var config = gameSys.FutzConfig;
		var host = gameSys.FutzHost;
		var matcherAct = gameSys.MatcherActivity;
		matcherAct.Host = host;

		var relayUrl = config.UseLanRelay ? config.RelayLan : config.RelayUrl;

#if !UNITY_EDITOR
		relayUrl = config.RelayUrl;
#endif

		gameSys.RoomIdf.Change("----");
		gameSys.Status.Change("Connecting");
		await host.Connect(relayUrl);

		Log($"connected".LgBlue());

		// await UniTask.Delay(1000);

		gameSys.Status.Change("Registering");
		Log($"sending register room request".LgBlue());

		matcherAct.RegisterRoomRequest.SendMatcher(new Pk_RegisterRoomRequest());
	}

	[Run.Native(SOCKET_OUT)]
	static void Run_Outgoing(FutzHost host) => host.Socket.ProcessOutgoing();

	[Run.Native(SOCKET_IN)]
	static void Run_Incoming(FutzHost host) => host.Socket.ProcessIncoming();

	[Run.Native(SYS_PK)]
	static void Run_MatcherPk(MatcherActivity matcher)
	{
		matcher.RegisterRoomResult.Drain(FnRegisterRoomResult);
		matcher.AgentLeft.Drain(FnAgentLeft);
	}

	[Run.Native(CUE_SPAWN)]
	static void Run_AgentKnockRequestPk(MatcherActivity matcher)
	{
		//## running this on CUE_SPAWN since it instantiates agents
		matcher.AgentKnockRequest.Drain(FnAgentKnockRequest);
	}

	[Run.Native(SYS_PK)]
	static void Run_SysPk(SystemActivity system)
	{
		system.AgentLog.Drain(FnAgentLog);
	}

	// DUMMY
	static Action<Pk_RegisterRoomResult> FnRegisterRoomResult = static pk => {
		var gameSys_ = GameSys_;
		var host = gameSys_.FutzHost;

		var result = pk.Result;
		var roomIdf = pk.RoomIdf;

		if (result != RoomCreateResultEnum.SUCCESS) {
			Log($"handle room fail {result}".LgTodo()); // TODO
			return; //>> failed to register room
		}

		host.NextSlotId = 1;
		host.AgentLup.Clear();
		gameSys_.RoomIdf.Change(roomIdf);
		gameSys_.Status.Change("In Room");

		var autoloadActivity = gameSys_.FutzConfig.AutoloadActivity;
		if (autoloadActivity) {
			gameSys_.ActivityChoice.Change(autoloadActivity);
		}
	};

	// DUMMY
	static Action<Pk_AgentKnockRequest> FnAgentKnockRequest = static pk => {
		var gameSys_ = GameSys_;
		var config = gameSys_.FutzConfig;
		var host = gameSys_.FutzHost;
		var matcher = gameSys_.MatcherActivity;
		var system = gameSys_.SystemActivity;

		var globalId = pk.GlobalId;
		var nickname = pk.Nickname;
		var uuid = pk.Uuid;

		foreach (var (existingSlotId, existingAgent) in host.AgentLup) {
			var existingAgentInfo = existingAgent.Info.Current;

			if (uuid == existingAgentInfo.Uuid) {
				Log($"same uuid joined {uuid} as {existingAgent}".LgTodo());
			}

			if (nickname == existingAgentInfo.Nickname) {
				Log($"same nickname joined {nickname} as {existingAgent}".LgTodo());
			}
		}

		var slotId = host.NextSlotId;
		host.NextSlotId++;

		var agent = Instantiate(config.Fab_Agent, _AgentRoot);
		agent.name = $"agent {slotId} {nickname} | g{globalId}";
		Log($"spawned {agent}".LgPink(), agent);

		var info = new AgentInfo {
			SlotId = slotId,
			GlobalId = globalId,
			Nickname = nickname,
			Uuid = uuid,
		};
		agent.Info.Change(info);
		agent.Status.Change("is ready");
		// agent.Color.Change(config.AgentColors.Colors.GetRandom()); // DUMMY
		agent.Color.Change(config.AgentColors.GetByIndex(slotId - 1));
		host.AgentLup.Set(info.SlotId, agent);

		matcher.AgentKnockResult.SendMatcher(
			new Pk_AgentKnockResult {
				GlobalId = globalId,
				Result = RoomJoinResultEnum.SUCCESS,
				SlotId = slotId,
			}
		);

		var currentActivity = host.CurrentActivity;
		if (currentActivity != null) {
			system.ActivityChange.SendToAgent(
				agent,
				new Pk_ActivityChange {
					Idf = currentActivity.Idf
				}
			);
		}

		system.AgentColor.SendToAgent(
			agent,
			new Pk_AgentColor {
				Color = agent.Color.Current.ToHtml(true)
			}
		);
	};

	// DUMMY
	static Action<Pk_AgentLeft> FnAgentLeft = static pk => {
		var code = pk.Code;
		var slotId = pk.SlotId;

		Log($"agent left! slotId: {slotId}, code: {code}".LgTodo());

		var host = GameSys_.FutzHost;
		var agent = host.AgentLup.Get(slotId);
		if (!agent) {
			Log($"AgentLeft but can't find agent {slotId}".LgTodo());
		}

		host.AgentLup.Cut(slotId);

		Destroy(agent.gameObject);
	};

	static Action<Agent, Pk_AgentLog> FnAgentLog = static (agent, pk) => {
		var msg = pk.Msg;
		Log($"{agent.Nickname}  -->   <b>{msg}</b>".LgPink(skipPrefix: true));
	};

	[React.Native(ACTIVITY_CHANGE, nameof(GameSysClip.ActivityChoice))]
	static void React_LoadedActivityDef(
		GameSysClip gameSys,
		(ActivityDef activityDef, int index) value
	)
	{
		var (activityDef, index) = value;

		var system = gameSys.SystemActivity;
		var host = gameSys.FutzHost;

		foreach (var agent in Agents_.Value) {
			agent.Score.Change(0);
		}

		var currentActivity = host.CurrentActivity;
		if (currentActivity) {
			Log($"Unloading activity: {currentActivity}".LgOrange());
			Destroy(currentActivity.gameObject);
		}

		if (!activityDef) {
			// TODO: change activity to nothing
			Log($"change activity to nothing".LgTodo());
			return; //>> change activity to nothing
		}

		var activity = activityDef.SpawnActivity();
		activity.RuntimeInitialize(gameSys.FutzHost, activityDef);
		host.CurrentActivity = activity;

		gameSys.CurrentActivity.Change(activity);

		Log($"ACTIVITY is now: {activityDef.Idf}  ===================".LgGold());

		if (host.Socket.GetState() != SocketState.OPEN) return; //>> socket not open

		system.ActivityChange.SendToAllAgents(
			new Pk_ActivityChange {
				Idf = activityDef.Idf
			}
		);
	}
}
}