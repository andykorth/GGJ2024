using System;
using System.Collections.Generic;
using Lumberjack;
using Regent.Cogs;
using Regent.Core;
using Regent.Syncers;
using Swoonity.Collections;
using Swoonity.CSharp;
using Swoonity.Unity;
using UnityEngine;
using static UnityEngine.Debug;

namespace FutzSys
{
public abstract class ActivityBase : CogNative
{
	[Header("Runtime")]
	public ActivityDef Def;
	public IHost Host; // TEMP
	public string Idf => Def.Idf;

	public Dictionary<int, IPacketFlow> Flows = new();

	public void RuntimeInitialize(IHost host, ActivityDef def)
	{
		Host = host;
		Def = def;
		var activityType = GetType();

		// TODO: facts could be serialized
		foreach (var packetFact in def.PacketFacts) {
			var flow = activityType
			   .GetFieldInstance<IPacketFlow>(packetFact.Label, this);
			flow.SetFact(packetFact);
			flow.SetHost(host);
			Flows[packetFact.PacketId] = flow;
		}

		// InitCustomSerialization();
		// InitRegistry();
	}

	public void Receive(int packetId, Agent agent, MsgBuffer msgBuffer, int dataIndex)
	{
		var (isValidPacketId, flow) = Flows.HasGet(packetId);
		if (!isValidPacketId) {
			Log($"missing packetId {packetId} | {this}".LgTodo(), this);
			return; // TODO: bad packetId
		}

		flow.Receive(agent, msgBuffer, dataIndex);
	}

	// public virtual void InitCustomSerialization() { }
	// public virtual void InitRegistry() { }

	protected override void WhenDestroyed()
	{
		base.WhenDestroyed();
		//## log missing packet handler/drain warnings
		foreach (var (packetId, flow) in Flows) {
			if (flow.PendingCount == 0) continue;
			LogWarning($"{flow} had {flow.PendingCount} unprocessed packets");
		}
	}
}

public abstract class Activity<TActor> : ActivityBase
	where TActor : ActorBase
{
	// public override void InitRegistry()
	// {
	// 	Actors = GetRegistry(); // TEMP
	// }



    [Btn(nameof(PrintActors))]
    // public Registry<TActor> Actors;
    public TrackList<TActor> Actors = new();
	// public abstract Registry<TActor> GetRegistry();
	public void PrintActors() => Log($"{Actors.Count} actors: {Actors.Current.Join()}".LgRed());


	// ReSharper disable once InconsistentNaming
	// needs to stay in generic class for reflection purposes
	public struct _packet_placeholder : IPk { }

	// ReSharper disable once InconsistentNaming
	// needs to stay in generic class for reflection purposes
	public class _unused_on_host : PkFlow<_packet_placeholder> { }
	
	
	/// slotId
	public class Pk<TPacket> : PkFlow<TPacket>
	{

		/// (for short circuiting) if false: clears received
		public Pk<TPacket> OnlyIf(bool condition)
		{
			if (!condition) ClearReceived();
			return this;
		}

		/// (for short circuiting) if true: clears received
		public Pk<TPacket> SkipIf(bool condition)
		{
			if (condition) ClearReceived();
			return this;
		}
		
		public void DrainAsActor(Action<TActor, TPacket> fn)
		{
			if (ReceivedList.IsEmpty()) return; //>> no packets
			
			foreach (var (agent, packet) in ReceivedList) {
				var actor = agent.CurrentActor as TActor;
				if (!actor) {
					Log($"(wrong actor type) dropped packet for {agent} {packet}".LgTodo(), agent);
					continue; // TODO: handle 
				}

				fn(actor, packet);
			}

			ReceivedList.Clear();
		}

		public void DrainAsActor<TData>(TData data, Action<TData, TActor, TPacket> fn)
		{
			if (ReceivedList.IsEmpty()) return; //>> no packets
			
			foreach (var (agent, packet) in ReceivedList) {
				var actor = agent.CurrentActor as TActor;
				if (!actor) {
					Log($"(wrong actor type) dropped packet for {agent} {packet}".LgTodo(), agent);
					continue; // TODO: handle 
				}

				fn(data, actor, packet);
			}

			ReceivedList.Clear();
		}
	}
}
}