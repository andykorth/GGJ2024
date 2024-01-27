using Regent.Cogs;
using UnityEngine;

namespace FutzSys
{
public abstract class ActorBase : CogNative
{
	public Agent Agent;
	public string Nickname => Agent.Nickname;
	public int SlotId => Agent.SlotId;
	public Color Color => Agent.Color.Current;

	public override string ToString() => name;
}
}