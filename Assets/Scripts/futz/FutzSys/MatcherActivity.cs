using Regent.Core;
using Sz = System.SerializableAttribute;

namespace FutzSys
{
public class MatcherActivity : Activity<MatcherActor>
{
	public const int MATCHER_ID_START = 230;
	public const int PING_ID = 254;
	public const int PONG_ID = 255;

	// public override Registry<MatcherActor> GetRegistry() => null;
	
	//## PACKETS	
	public PkFlow<Pk_AgentKnockRequest> AgentKnockRequest = new(); //## Relay -> Host
	public PkFlow<Pk_AgentKnockResult> AgentKnockResult = new(); //## Host -> Matcher
	public PkFlow<Pk_AgentLeft> AgentLeft = new(); //## Relay -> Host
	public PkFlow<Pk_CloseRoom> CloseRoom = new(); // Host -> Matcher
	public _unused_on_host JoinRoomRequest = new(); // Agent -> Matcher
	public _unused_on_host JoinRoomResult = new(); // Relay -> Agent
	public _unused_on_host LeaveRoom = new(); // Agent -> Matcher
	public PkFlow<Pk_RegisterRoomRequest> RegisterRoomRequest = new(); //## Host -> Matcher
	public PkFlow<Pk_RegisterRoomResult> RegisterRoomResult = new(); //## Relay -> Host
	public _unused_on_host RoomClosed = new(); // Relay -> Agent
	public PkFlow<Pk_OpenAiRequest> OpenAiRequest = new();
	public PkFlow<Pk_OpenAiResult> OpenAiResult = new();
}

/// fake matcher actor, will not be created
public class MatcherActor : ActorBase { }

public enum RoomCreateResultEnum
{
	UNSET = 0,
	SUCCESS = 1,
	FAIL_TODO = 20,
}

public enum RoomJoinResultEnum
{
	UNSET = 0,
	SUCCESS = 1,
	FAIL_MISSING_ROOM = 10,
	FAIL_TODO = 20,
}

public enum RoomClosedReasonEnum
{
	UNSET = 0,
	HOST_DISCONNECTED = 1,
	CLOSED = 2,
	TODO = 20,
}

[Sz] public struct Pk_AgentKnockRequest : IPk
{
	public int GlobalId;
	public string Nickname;
	public string Uuid;
}

[Sz] public struct Pk_AgentKnockResult : IPk
{
	public int GlobalId;
	public RoomJoinResultEnum Result;
	public int SlotId;
}

[Sz] public struct Pk_AgentLeft : IPk
{
	public int Code;
	public int SlotId;
}

public struct Pk_CloseRoom : IPk
{
	public RoomClosedReasonEnum Reason;
}

[Sz] public struct Pk_RegisterRoomRequest : IPk
{
	// empty packet
}

[Sz] public struct Pk_RegisterRoomResult : IPk
{
	public RoomCreateResultEnum Result;
	public string RoomIdf;
}


[Sz] public struct Pk_OpenAiRequest : IPk
{
	public int ReqId;
	public string Prompt;
	// public string model;
	// public OpenAiMessage[] messages;
	// "model": "gpt-3.5-turbo",
	// "messages": [{"role": "user", "content": "Hello!"}]
}

[Sz] public struct Pk_OpenAiResult : IPk
{
	public int ReqId;
	public string Answer;
	// public string id;
	// public OpenAiChoice[] choices;
	// "id": "chatcmpl-123",
	// "object": "chat.completion",
	// "created": 1677652288,
	// "choices": [{
	//   "index": 0,
	//   "message": {
	//     "role": "assistant",
	//     "content": "\n\nHello there, how may I assist you today?",
	//   },
	//   "finish_reason": "stop"
	// }],
	// "usage": {
	//   "prompt_tokens": 9,
	//   "completion_tokens": 12,
	//   "total_tokens": 21
	// }
}

// [Sz] public struct OpenAiMessage
// {
// 	public string role;
// 	public string content;
// }
//
// [Sz] public struct OpenAiChoice
// {
// 	public int index;
// 	public OpenAiMessage message;
// }
}