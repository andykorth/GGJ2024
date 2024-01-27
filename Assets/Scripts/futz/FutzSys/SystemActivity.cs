using Regent.Core;
using Sz = System.SerializableAttribute;

namespace FutzSys
{
public class SystemActivity : Activity<SystemActor>
{
	public const int SYSTEM_ID_START = 200;

	// public override Registry<SystemActor> GetRegistry() => null;
	
	//## PACKETS	
	public PkFlow<Pk_ActivityChange> ActivityChange = new(); //## outgoing
	public PkFlow<Pk_AgentColor> AgentColor = new(); //## outgoing
	public PkFlow<Pk_AgentLog> AgentLog = new();
}

/// fake system actor, will not be created
public class SystemActor : ActorBase { }

[Sz] public struct Pk_ActivityChange : IPk
{
	public string Idf;
}

[Sz] public struct Pk_AgentColor : IPk
{
	public string Color;
}

[Sz] public struct Pk_AgentLog : IPk
{
	public string Msg;
}
}