using System;
using Cysharp.Threading.Tasks;

namespace FutzSys
{
/// spending way too much time trying to make this system flexible
/// so just assume it's the relay implementation for now
public interface ISocket
{
	public void SetCallbacks(SocketCallbacks callbacks);
	public SocketState GetState();

	public UniTask Connect(string address);

	/// MsgBuffer will be DISPOSED after it is processed
	public void SendMsgBuffer(MsgBuffer msgBuffer);

	public void ProcessOutgoing();
	public void ProcessIncoming();

	public UniTask Close(int closeCode, string closeMessage);


	// public void SendSystemMessage(int systemMessageId, )
}

public struct SocketCallbacks
{
	public Action OnOpen;
	public Action<Exception> OnError;
	public Action<int, string> OnClose;

	/// MsgBuffer will be DISPOSED after this is called!
	public Action<MsgBuffer> OnReceive;
}

public enum SocketState
{
	UNSET,
	CONNECTING,
	OPEN,
	CLOSING,
	CLOSED,
}
}