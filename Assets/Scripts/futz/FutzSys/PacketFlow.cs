using System;
using System.Collections.Generic;
using Lumberjack;
using Swoonity.Unity;
using static UnityEngine.Debug;

namespace FutzSys
{
/// reflection marker
public interface IPacketFlow
{
	void SetFact(PacketFact fact);
	PacketFact GetFact();
	void SetHost(IHost host);
	void Receive(Agent agent, MsgBuffer msgBuffer, int dataIndex);
	int PacketId { get; }
	int PendingCount { get; }
}

[Serializable]
public class PkFlow<TPacket> : IPacketFlow
{
	public PacketFact Fact;
	public int PacketId => Fact.PacketId;
	public IHost Host; // TEMP
	public Type PacketType = typeof(TPacket);
	public List<AgentPk> ReceivedList = new();
	public int PendingCount => ReceivedList.Count;

	// DUMMY
	public Func<MsgBuffer, int, TPacket> FnDeserialize =
		static (msgBuffer, dataIndex) => msgBuffer.GetJson<TPacket>(dataIndex);

	public void SetFact(PacketFact fact) => Fact = fact;
	public PacketFact GetFact() => Fact;
	public void SetHost(IHost host) => Host = host;

	// temp?
	public void SendMatcher(TPacket packet)
	{
		Log($"{Fact} SendMatcher".LgBlue());
		Host.Send(PacketId, 0, packet);
	}

	public void SendToAllAgents(TPacket packet)
	{
		Log($"{Fact} SendToAllAgents".LgBlue());
		Host.Send(PacketId, 0, packet);
	}

	public void SendToAgent(Agent agent, TPacket packet)
	{
		Log($"{Fact} SendToAgent {agent}".LgBlue());
		if (agent.SlotId == 0) throw new Err($"TODO: agent slotId is 0  {agent}", agent);
		Host.Send(PacketId, agent.SlotId, packet);
	}

	public void SendTo(ActorBase actor, TPacket packet) => SendToAgent(actor.Agent, packet);

	public void Receive(Agent agent, MsgBuffer msgBuffer, int dataIndex)
	{
		Log($"{Fact.Label} receive: {agent} --> {msgBuffer.Length} bytes".LgGreen());

		var packet = FnDeserialize(msgBuffer, dataIndex);
		ReceivedList.Add(
			new AgentPk {
				Agent = agent,
				Packet = packet,
			}
		);
	}

	public void Drain(Action<TPacket> fn)
	{
		foreach (var (_, packet) in ReceivedList) fn(packet);
		ReceivedList.Clear();
	}

	public void Drain(Action<Agent, TPacket> fn)
	{
		foreach (var (agent, packet) in ReceivedList) fn(agent, packet);
		ReceivedList.Clear();
	}

	public void Drain<TData>(TData data, Action<TData, Agent, TPacket> fn)
	{
		foreach (var (agent, packet) in ReceivedList) fn(data, agent, packet);
		ReceivedList.Clear();
	}

	public void ClearReceived() => ReceivedList.Clear();

	public override string ToString() => $"pkFlow {Fact}";

	[Serializable]
	public struct AgentPk
	{
		public Agent Agent;
		public TPacket Packet;

		public void Deconstruct(out Agent agent, out TPacket packet)
			=> (agent, packet) = (Agent, Packet);
	}
}
}