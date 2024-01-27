
// public class SocketThreadManager : MonoBehaviour {
//
// 	void Awake() {
// 		_instance = this;
// 	}
//
// 	static SocketThreadManager _instance;
// 	static SynchronizationContext _synchronizationContext;
//
// 	public static void Run(IEnumerator waitForUpdate) {
// 		_synchronizationContext = SynchronizationContext.Current;
// 		_synchronizationContext.Post(_ => _instance.StartCoroutine(waitForUpdate), null);
// 	}
//
// }



// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Net.WebSockets;
// using System.Text;
// using System.Threading;
// using System.Threading.Tasks;
//
// // TODO/rewrite all this
// namespace FutzSys.TempSocketThreader {
// public class TempSocket {
//
// 	readonly Uri _uri;
// 	readonly SocketHandlers _handlers;
// 	readonly object _lock = new();
// 	readonly List<ArraySegment<byte>> _sendBytesQueue = new();
// 	readonly List<ArraySegment<byte>> _sendTextQueue = new();
//
// 	ClientWebSocket _socket;
// 	CancellationTokenSource _cancelSource;
// 	CancellationToken _cancelToken;
// 	bool _isSending;
//
//
// 	public TempSocket(
// 		string url,
// 		SocketHandlers handlers
// 	) {
// 		_uri = new Uri(url);
// 		_handlers = handlers;
//
// 		var protocol = _uri.Scheme;
// 		if (!protocol.Equals("ws") && !protocol.Equals("wss"))
// 			throw new ArgumentException("Unsupported protocol: " + protocol);
// 	}
//
// 	public void CancelConnection() {
// 		_cancelSource?.Cancel();
// 	}
//
// 	public async Task Connect() {
// 		try {
// 			_cancelSource = new CancellationTokenSource();
// 			_cancelToken = _cancelSource.Token;
// 			_socket = new ClientWebSocket();
//
// 			// if request headers needed: _socket.Options.SetRequestHeader(key, value);
// 			// if subprotocols needed: _socket.Options.AddSubProtocol(subprotocol);
// 			// https://developer.mozilla.org/en-US/docs/Web/API/WebSockets_API/Writing_WebSocket_servers#subprotocols
//
//
// 			await _socket.ConnectAsync(_uri, _cancelToken);
// 			_handlers.OnOpen.Invoke();
//
// 			await Receive();
// 		}
// 		catch (Exception ex) {
// 			_handlers.OnError.Invoke(ex.Message);
// 			_handlers.OnClose.Invoke(TempWebSocketCloseCode.Abnormal);
// 		}
// 		finally {
// 			if (_socket != null) {
// 				_cancelSource.Cancel();
// 				_socket.Dispose();
// 			}
// 		}
// 	}
//
// 	public WebSocketState State =>
// 		_socket.State switch {
// 			System.Net.WebSockets.WebSocketState.Connecting => WebSocketState.Connecting,
// 			System.Net.WebSockets.WebSocketState.Open => WebSocketState.Open,
// 			System.Net.WebSockets.WebSocketState.CloseSent => WebSocketState.Closing,
// 			System.Net.WebSockets.WebSocketState.CloseReceived => WebSocketState.Closing,
// 			System.Net.WebSockets.WebSocketState.Closed => WebSocketState.Closed,
// 			_ => WebSocketState.Closed
// 		};
//
// 	public Task Send(byte[] bytes) {
// 		// return m_Socket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
// 		return SendMessage(
// 			_sendBytesQueue,
// 			WebSocketMessageType.Binary,
// 			new ArraySegment<byte>(bytes)
// 		);
// 	}
//
// 	public Task SendText(string message) {
// 		var encoded = Encoding.UTF8.GetBytes(message);
//
// 		// m_Socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
// 		return SendMessage(
// 			_sendTextQueue,
// 			WebSocketMessageType.Text,
// 			new ArraySegment<byte>(encoded, 0, encoded.Length)
// 		);
// 	}
//
// 	async Task SendMessage(
// 		List<ArraySegment<byte>> queue,
// 		WebSocketMessageType messageType,
// 		ArraySegment<byte> buffer
// 	) {
// 		// Return control to the calling method immediately.
// 		// await Task.Yield ();
//
// 		// Make sure we have data.
// 		if (buffer.Count == 0) {
// 			return;
// 		}
//
// 		// The state of the connection is contained in the context Items dictionary.
// 		bool sending;
//
// 		lock (_lock) {
// 			sending = _isSending;
//
// 			// If not, we are now.
// 			if (!_isSending) {
// 				_isSending = true;
// 			}
// 		}
//
// 		if (!sending) {
// 			// Lock with a timeout, just in case.
// 			if (!Monitor.TryEnter(_socket, 1000)) {
// 				// If we couldn't obtain exclusive access to the socket in one second, something is wrong.
// 				await _socket.CloseAsync(
// 					WebSocketCloseStatus.InternalServerError,
// 					string.Empty,
// 					_cancelToken
// 				);
// 				return;
// 			}
//
// 			try {
// 				// Send the message synchronously.
// 				var t = _socket.SendAsync(buffer, messageType, true, _cancelToken);
// 				t.Wait(_cancelToken);
// 			}
// 			finally {
// 				Monitor.Exit(_socket);
// 			}
//
// 			// Note that we've finished sending.
// 			lock (_lock) {
// 				_isSending = false;
// 			}
//
// 			// Handle any queued messages.
// 			await HandleQueue(queue, messageType);
// 		}
// 		else {
// 			// Add the message to the queue.
// 			lock (_lock) {
// 				queue.Add(buffer);
// 			}
// 		}
// 	}
//
// 	async Task HandleQueue(List<ArraySegment<byte>> queue, WebSocketMessageType messageType) {
// 		var buffer = new ArraySegment<byte>();
// 		lock (_lock) {
// 			// Check for an item in the queue.
// 			if (queue.Count > 0) {
// 				// Pull it off the top.
// 				buffer = queue[0];
// 				queue.RemoveAt(0);
// 			}
// 		}
//
// 		// Send that message.
// 		if (buffer.Count > 0) {
// 			await SendMessage(queue, messageType, buffer);
// 		}
// 	}
//
// 	Mutex _messageListMutex = new();
// 	List<byte[]> _messageList = new();
//
// 	// simple dispatcher for queued messages.
// 	public void DispatchMessageQueue() {
// 		// lock mutex, copy queue content and clear the queue.
// 		_messageListMutex.WaitOne();
// 		var messageListCopy = new List<byte[]>();
// 		messageListCopy.AddRange(_messageList);
// 		_messageList.Clear();
// 		// release mutex to allow the websocket to add new messages
// 		_messageListMutex.ReleaseMutex();
//
// 		foreach (var bytes in messageListCopy) {
// 			_handlers.OnMessage.Invoke(bytes);
// 		}
// 	}
//
// 	public async Task Receive() {
// 		var closeCode = TempWebSocketCloseCode.Abnormal;
// 		await new WaitForBackgroundThread();
//
// 		var buffer = new ArraySegment<byte>(new byte[8192]);
// 		try {
// 			while (_socket.State == System.Net.WebSockets.WebSocketState.Open) {
// 				WebSocketReceiveResult result = null;
//
// 				using (var ms = new MemoryStream()) {
// 					do {
// 						result = await _socket.ReceiveAsync(buffer, _cancelToken);
// 						ms.Write(buffer.Array, buffer.Offset, result.Count);
// 					} while (!result.EndOfMessage);
//
// 					ms.Seek(0, SeekOrigin.Begin);
//
// 					if (result.MessageType == WebSocketMessageType.Text) {
// 						_messageListMutex.WaitOne();
// 						_messageList.Add(ms.ToArray());
// 						_messageListMutex.ReleaseMutex();
//
// 						//using (var reader = new StreamReader(ms, Encoding.UTF8))
// 						//{
// 						//	string message = reader.ReadToEnd();
// 						//	OnMessage?.Invoke(this, new MessageEventArgs(message));
// 						//}
// 					}
// 					else if (result.MessageType == WebSocketMessageType.Binary) {
// 						_messageListMutex.WaitOne();
// 						_messageList.Add(ms.ToArray());
// 						_messageListMutex.ReleaseMutex();
// 					}
// 					else if (result.MessageType == WebSocketMessageType.Close) {
// 						await Close();
// 						var closeStatus = (int)result.CloseStatus;
// 						closeCode = Enum.IsDefined(typeof(TempWebSocketCloseCode), closeStatus)
// 							? (TempWebSocketCloseCode)closeStatus
// 							: TempWebSocketCloseCode.Undefined;
//
// 						break;
// 					}
// 				}
// 			}
// 		}
// 		catch (Exception) {
// 			_cancelSource.Cancel();
// 		}
// 		finally {
// 			await new WaitForUpdate();
// 			_handlers.OnClose.Invoke(closeCode);
// 		}
// 	}
//
// 	public async Task Close() {
// 		if (State == WebSocketState.Open) {
// 			await _socket.CloseAsync(
// 				WebSocketCloseStatus.NormalClosure,
// 				string.Empty,
// 				_cancelToken
// 			);
// 		}
// 	}
//
// }


// public class WaitForUpdate : CustomYieldInstruction {
//
// 	public override bool keepWaiting => false;
//
// 	public MainThreadAwaiter GetAwaiter() {
// 		var awaiter = new MainThreadAwaiter();
// 		SocketThreadManager.Run(CoroutineWrapper(this, awaiter));
// 		return awaiter;
// 	}
//
// 	public class MainThreadAwaiter : INotifyCompletion {
//
// 		Action _continuation;
//
// 		public bool IsCompleted { get; set; }
//
// 		public void GetResult() { }
//
// 		public void Complete() {
// 			IsCompleted = true;
// 			_continuation?.Invoke();
// 		}
//
// 		void INotifyCompletion.OnCompleted(Action continuation) {
// 			this._continuation = continuation;
// 		}
//
// 	}
//
// 	public static IEnumerator CoroutineWrapper(IEnumerator theWorker, MainThreadAwaiter awaiter) {
// 		yield return theWorker;
// 		awaiter.Complete();
// 	}
//
// }

// public static class WebSocketHelpers {

// 	public static WebSocketException GetErrorMessageFromCode(int errorCode, Exception inner) =>
// 		errorCode switch {
// 			-1 => new WebSocketUnexpectedException("WebSocket instance not found.", inner),
// 			-2 => new WebSocketInvalidStateException(
// 				"WebSocket is already connected or in connecting state.",
// 				inner
// 			),
// 			-3 => new WebSocketInvalidStateException("WebSocket is not connected.", inner),
// 			-4 => new WebSocketInvalidStateException("WebSocket is already closing.", inner),
// 			-5 => new WebSocketInvalidStateException("WebSocket is already closed.", inner),
// 			-6 => new WebSocketInvalidStateException("WebSocket is not in open state.", inner),
// 			-7 => new WebSocketInvalidArgumentException(
// 				"Cannot close WebSocket. An invalid code was specified or reason is too long.",
// 				inner
// 			),
// 			_ => new WebSocketUnexpectedException("Unknown error.", inner)
// 		};
// }
//
// public class WebSocketException : Exception {
//
// 	public WebSocketException() { }
// 	public WebSocketException(string message) : base(message) { }
// 	public WebSocketException(string message, Exception inner) : base(message, inner) { }
//
// }
//
// public class WebSocketUnexpectedException : WebSocketException {
//
// 	public WebSocketUnexpectedException() { }
// 	public WebSocketUnexpectedException(string message) : base(message) { }
//
// 	public WebSocketUnexpectedException(string message, Exception inner) :
// 		base(message, inner) { }
//
// }
//
// public class WebSocketInvalidArgumentException : WebSocketException {
//
// 	public WebSocketInvalidArgumentException() { }
// 	public WebSocketInvalidArgumentException(string message) : base(message) { }
//
// 	public WebSocketInvalidArgumentException(string message, Exception inner) : base(
// 		message,
// 		inner
// 	) { }
//
// }
//
// public class WebSocketInvalidStateException : WebSocketException {
//
// 	public WebSocketInvalidStateException() { }
// 	public WebSocketInvalidStateException(string message) : base(message) { }
//
// 	public WebSocketInvalidStateException(string message, Exception inner) : base(
// 		message,
// 		inner
// 	) { }
//
//
// }
//
// public class WaitForBackgroundThread {
//
// 	public ConfiguredTaskAwaitable.ConfiguredTaskAwaiter GetAwaiter() {
// 		return Task.Run(() => { }).ConfigureAwait(false).GetAwaiter();
// 	}
//
// }
// }