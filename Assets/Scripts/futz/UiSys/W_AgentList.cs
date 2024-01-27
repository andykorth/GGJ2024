using System;
using Glui;

namespace UiSys
{
public class W_AgentList : GluWindow
{
	public LabEl L_Title = new();
	public C_AgentStatus.Pool Pool = new();
}

[Serializable]
public class C_AgentStatus : GluCEll
{
	public LabEl L_Name = new();
	public LabEl L_Status = new();
	public VEl V_Color = new();

	protected override bool CallLoad()
		=> Load(
			L_Name,
			L_Status,
			V_Color
		);


	[Serializable]
	public class Pool : GluCEllPool<C_AgentStatus> { }
}
}