using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lumberjack;
using PoolBoyPooling;
using UnityEngine;
using static UnityEngine.Debug;

namespace FutzSys
{
/// TODO: put this on a different thread?
/// TODO: add fragmented messages using result.EndOfMessage
public class FutzWebSocketRelay : ISocket
{
	public string Address;
	public SocketState State;
	public bool DoLogs;

	Uri _uri;
	ClientWebSocket _ws;
	CancellationTokenSource _cancelSource;
	CancellationToken _cancelToken => _cancelSource.Token;
	Queue<MsgBuffer> _inbox = new(64);
	Queue<MsgBuffer> _outbox = new(64);
	bool _isSending;

	Action _fnOnOpen = static () => { };
	Action<MsgBuffer> _fnOnReceive = static _ => { };
	Action<Exception> _fnOnError = static _ => { };
	Action<int, string> _fnOnClose = static (_, _) => { };

	public void SetCallbacks(SocketCallbacks callbacks)
	{
		_fnOnOpen = callbacks.OnOpen ?? (static () => { });
		_fnOnReceive = callbacks.OnReceive ?? (static _ => { });
		_fnOnError = callbacks.OnError ?? (static _ => { });
		_fnOnClose = callbacks.OnClose ?? (static (_, _) => { });
	}

	public SocketState GetState() => State;

	#region CONNECTION

	public async UniTask Connect(string address)
	{
		State = SocketState.CONNECTING;

		Address = address;
		_uri = new Uri(Address);
		_ws = new ClientWebSocket();
		_cancelSource = new CancellationTokenSource();

		await _ws.ConnectAsync(_uri, _cancelToken); //>> AWAIT connection

		State = SocketState.OPEN;
		_fnOnOpen();

		BeginReceiveLoop().Forget();
	}

	public async UniTask Close(int closeCode, string closeMessage)
	{
		if (_ws.State != WebSocketState.Open && _ws.State != WebSocketState.Connecting) {
			return; //>> not open
		}

		State = SocketState.CLOSING;
		Log($"close socket (from {_ws.State}) {closeCode}: {closeMessage}".LgTodo());

		_cancelSource?.Cancel();
		_cancelSource = new CancellationTokenSource();

		var doClose =
			_ws is {
				State: WebSocketState.Open
				or WebSocketState.CloseReceived
				or WebSocketState.CloseSent
			};

		if (doClose) {
			await _ws.CloseAsync(
				(WebSocketCloseStatus)closeCode,
				closeMessage,
				_cancelToken
			); //>> AWAIT closing
		}

		State = SocketState.CLOSED;
		_ws?.Dispose();
		_fnOnClose(closeCode, closeMessage);
	}

	#endregion

	#region OUTGOING

	public void SendMsgBuffer(MsgBuffer msgBuffer)
	{
		if (State != SocketState.OPEN) throw new Exception($"socket not connected");
		_outbox.Enqueue(msgBuffer);
	}

	public void ProcessOutgoing()
	{
		if (State != SocketState.OPEN) return; //>> not open
		
		BeginSending().Forget();
	}

	/// run sending loop (skip if already running) until complete 
	async UniTaskVoid BeginSending()
	{
		if (_outbox.Count == 0) return; //>> nothing to send
		if (_isSending) return; //>> already processing
		_isSending = true;
		await Sending();
		_isSending = false;
	}

	async UniTask Sending()
	{
		var outCount = _outbox.Count;
		if (DoLogs) Log($"Send {outCount} packets".LgGold());
		
		for (var i = 0; i < outCount; i++) {
			var msgBuffer = _outbox.Dequeue();
			var segment = new ArraySegment<byte>(msgBuffer.Bytes, 0, msgBuffer.Length);

			await _ws.SendAsync(
				segment,
				WebSocketMessageType.Binary,
				true,
				_cancelToken
			); //>> AWAIT send message

			Pooler.Release(msgBuffer);
		}
	}

	#endregion

	#region INCOMING

	public void ProcessIncoming()
	{
		var inCount = _inbox.Count;
		if (inCount == 0) return; //>> nothing to receive
		if (State != SocketState.OPEN) return; //>> not open

		if (DoLogs) Log($"Receive {inCount} packets".LgGold());
		
		for (var i = 0; i < inCount; i++) {
			var msgBuffer = _inbox.Dequeue();
			_fnOnReceive(msgBuffer);
			Pooler.Release(msgBuffer);
		}
	}

	/// start an "await" loop that will run until connection closes
	async UniTaskVoid BeginReceiveLoop()
	{
		try {
			if (DoLogs) Log($"begin receiving loop".LgGold());
			while (State == SocketState.OPEN) {
				await Receiving();
			}

			if (DoLogs) Log($"end receiving loop".LgGold());
		}
		catch (Exception err) {
			// Log($"Receiving error: {err.Message}".LgRed());
			_fnOnError(err);
			await Close((int)WebSocketCloseStatus.ProtocolError, err.Message);
		}
	}

	async UniTask Receiving()
	{
		if (!Application.isPlaying) throw new Exception("application not playing");
		
		var msgBuffer = Pooler.Take<MsgBuffer>();
		var segment = new ArraySegment<byte>(msgBuffer.Bytes);

		var result = await _ws.ReceiveAsync(
			segment,
			_cancelToken
		); //>> AWAIT receive message

		if (DoLogs) Log($"received message {result.Count} bytes".LgYellow());
		
		if (!result.EndOfMessage) {
			throw new Exception($"TODO: result.EndOfMessage == false");
		}
		
		if (result.MessageType == WebSocketMessageType.Close) {
			Log($"handle CLOSE message received".LgTodo());
			await Close((int)result.CloseStatus, result.CloseStatusDescription);
			return; //>> CLOSING
		}

		msgBuffer.Length = result.Count;
		_inbox.Enqueue(msgBuffer);
		//>> received and queued
	}

	#endregion
}

public enum WebSocketCloseCode
{
	CLOSE_NORMAL = 1000,
	CLOSE_GOING_AWAY = 1001,
	CLOSE_PROTOCOL_ERROR = 1002,
	CLOSE_UNSUPPORTED = 1003,
	CLOSED_NO_STATUS = 1005,
	CLOSE_ABNORMAL = 1006,
	UNSUPPORTED_PAYLOAD = 1007,
	POLICY_VIOLATION = 1008,
	CLOSE_TOO_LARGE = 1009,
	MANDATORY_EXTENSION = 1010,
	SERVER_ERROR = 1011,
	SERVICE_RESTART = 1012,
	TRY_AGAIN_LATER = 1013,
	BAD_GATEWAY = 1014,
	TLS_HANDSHAKE_FAIL = 1015,
}

// var scheme = _uri.Scheme;
// if (scheme != "ws" || scheme != "wss") {
// 	throw new ProtocolViolationException($"{scheme} is not supported");
// }
}