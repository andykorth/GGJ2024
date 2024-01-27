using System;
using System.Collections.Generic;

namespace FutzSys
{
[Serializable]
public class PacketFact
{
	public string Label;
	public int PacketId; // specific to Activity
	public string PacketName;
	public string FullTypeName;
	public ActivityDef ActivityDef;
	public List<string> FieldInfos;

	public override string ToString() => $"{ActivityDef.Idf}.{PacketId}.{Label}";
}
}