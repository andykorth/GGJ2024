using System.Collections.Generic;
using Foundational;
using UnityEngine;

namespace FutzSys
{
[CreateAssetMenu(menuName = "Futz/" + nameof(FutzConfig))]
public class FutzConfig : ScriptableObject
{

	[Header("Relay")]
	public string RelayUrl = "";
	public bool UseLanRelay;
	public string RelayLan = "";

	[Header("Agents")]
	public Agent Fab_Agent;
	public ColorSetDef AgentColors;

	[Header("Activities")]
	public ActivityDef MatcherDef;
	public ActivityDef SystemDef;
	public List<ActivityDef> ActivityDefs = new();
	public ActivityDef AutoloadActivity;
}
}