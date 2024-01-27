using System;
using Regent.Cogs;
using Regent.Syncers;
using UnityEngine;

namespace FutzSys
{
public class Agent : CogNative
{
	[Header("State")]
	public Track<AgentInfo> Info = new();
	public Track<string> Status = new(); // TEMP
	public Track<Color> Color = new();
	public Track<int> Score = new();

	public ActivityBase CurrentActivity;
	public ActorBase CurrentActor;

	public int SlotId => Info.Current.SlotId;
	public string Nickname => Info.Current.Nickname;


	public override string ToString() => name;
}

[Serializable]
public struct AgentInfo
{
	public int SlotId; // id in room
	public int GlobalId; // id on relay (debug use only)
	public string Nickname;
	public string Uuid;
}
}


// public float LastReceivedAt;
// public float LastSentAt;
// public string Uuid; // private to client & host
// public string AgentKey; // private to client & host
// public string AgentIdf; // public
// public string Nonce; // registration only