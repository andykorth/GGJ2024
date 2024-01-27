using System;
using Cysharp.Threading.Tasks;
using Lumberjack;
using PoolBoyPooling;
using Regent.Cogs;
using Swoonity.Collections;
using static UnityEngine.Debug;

namespace FutzSys
{
public class FutzHost : CogNative, IHost
{
	public MatcherActivity MatcherActivity;
	public SystemActivity SystemActivity;
	public ActivityBase CurrentActivity;
	public ISocket Socket;
	public float PingIntervalSec = 5;

	public int NextSlotId; // TEMP
	public Lup<int, Agent> AgentLup = new();


	public async UniTask Connect(string url)
	{
		Log($"Connecting... {url}");

		Socket = new FutzWebSocketRelay();
		Socket.SetCallbacks(
			new SocketCallbacks() {
				OnOpen = () => OnOpen(),
				OnError = (err) => OnError(err),
				OnClose = (code, msg) => OnClose(code, msg),
				OnReceive = msg => OnReceived(msg),
			}
		);

		await Socket.Connect(url);
	}

	void OnOpen()
	{
		Log("Connection open!".LgRed());
		CancelInvoke(nameof(Ping));
		InvokeRepeating(nameof(Ping), PingIntervalSec, PingIntervalSec);
	}

	void OnError(Exception err)
	{
		Log($"error: {err.Message}".LgRed());
		if (this) CancelInvoke(nameof(Ping));
	}

	void OnClose(int code, string msg)
	{
		Log($"Connection closed: {code} - {msg}".LgTodo());
		if (this) CancelInvoke(nameof(Ping));
	}

	void OnReceived(MsgBuffer msgBuffer)
	{
		var packetId = msgBuffer.Bytes[0];
		var slotId = msgBuffer.Bytes[1];
		// var json = msgBuffer.GetString(2);

		if (packetId >= MatcherActivity.MATCHER_ID_START) {
			if (packetId == MatcherActivity.PONG_ID) {
				// Log($"PONG received");
				// TODO: it'd be nice & more standard if ping/pong was handled on Socket
				return; //>> PONG received
			}
			
			MatcherActivity.Receive(packetId, null, msgBuffer, 2);
			return; //>> MATCHER packet
		}

		var (isAgentValid, agent) = AgentLup.HasGet(slotId);
		if (!isAgentValid) {
			LogWarning($"missing agent: {slotId}, dropping pk {packetId}");
			return; //>> missing agent
		}

		if (packetId >= SystemActivity.SYSTEM_ID_START) {
			SystemActivity.Receive(packetId, agent, msgBuffer, 2);
			return; //>> SYSTEM packet
		}

		if (!CurrentActivity) {
			LogWarning($"no current activity, dropping pk {packetId}");
			return; //>> no current activity
		}
		
		CurrentActivity.Receive(packetId, agent, msgBuffer, 2);
	}

	public void Send(int packetId, int slotId, object packet)
	{
		var msgBuffer = Pooler.Take<MsgBuffer>();
		msgBuffer.Set(0, packetId);
		msgBuffer.Set(1, slotId);
		msgBuffer.FillJson(packet, 2);
		Socket.SendMsgBuffer(msgBuffer);
	}

	public void Ping()
	{
		var msgBuffer = Pooler.Take<MsgBuffer>();
		msgBuffer.Set(0, MatcherActivity.PING_ID);
		msgBuffer.Set(1, 0, true);
		Socket.SendMsgBuffer(msgBuffer);
	}

	async void OnApplicationQuit()
	{
		Log($"*********** Application QUIT ***********".LgRed());
		if (Socket == null) return;
		await Socket.Close(1000, "OnApplicationQuit");
	}
}

// TEMP TODO
public interface IHost
{
	public void Send(int packetId, int slotId, object packet);
}
}