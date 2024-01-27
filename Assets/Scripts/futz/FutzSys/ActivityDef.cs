using System.Collections.Generic;
using Idealist;
using Lumberjack;
using Swoonity.CSharp;
using Swoonity.Unity;
using UnityEngine;
using static UnityEngine.Debug;

namespace FutzSys
{
[CreateAssetMenu(menuName = "Futz/Activity DEF")]
public class ActivityDef : ScriptableObject
{
	[Tooltip("dev-facing name, used when alphabetizing activities")]
	[Btn(nameof(MakePacketFacts))]
	public string Idf;

	[Tooltip("player-facing name")]
	public string Name;
	public bool ShowAgentList = true;
	public bool ShowScore = true;

	public GameObject Fab_Activity;
	public GameObject Fab_Actor;

	public List<PacketFact> PacketFacts = new();
	public bool SystemOnly;
	public int IdOffset = 1;

	protected virtual void WhenEditorValidates() { }
	protected virtual void WhenRuntimeInitializes() { }

	void OnValidate()
	{
		MakePacketFacts(); // TEMP
		WhenEditorValidates();
	}

	public void MakePacketFacts()
	{
		if (!Fab_Activity) {
			LogWarning($"{this} missing Activity prefab");
			return; //>> missing Activity prefab
		}

		var activityType = Fab_Activity.GetComponent<ActivityBase>().GetType();

		// Log($"{activityType.Name} find packets  |  {Lg.Time}".LgGold(), this);

		PacketFacts =
			activityType
			   .ListFieldsWithInterface<IPacketFlow>()
			   .MapNew(
					(field, index) => {
						var packetType = field.FieldType.GetGenericTypes()[0];
						var fact = new PacketFact {
							Label = field.Name,
							PacketId = IdOffset + index,
							PacketName = packetType.Name,
							FullTypeName = packetType.AssemblyQualifiedName,
							ActivityDef = this,
							FieldInfos = packetType.ListAllFields().MapNew(
								static field => field.Name
								// static field => $"{field.FieldType.Name} {field.Name}"
							),
						};
						// Log($"    {fact} {{ {fact.FieldInfos.Join()} }}".LgGreen(), this);
						return fact;
					}
				);

		Log($"{activityType.Name} found {PacketFacts.Count} packets!".LgGold(), this);

		this.SetDirtyIfEditor();
	}

	public ActivityBase SpawnActivity()
	{
		var activity = Instantiate(Fab_Activity).GetComponent<ActivityBase>();
		activity.name = $"{Idf}  --ACTIVITY";
		return activity;
	}

	public override string ToString() => $"{GetType().Name}";
}
}