// using System;
// using Lumberjack;
// using Swoonity.Collections;
// using Swoonity.CSharp;
// using UnityEngine;
// using static UnityEngine.Debug;
//
//
// namespace FutzSys {
// /// TODO: eventually move away from JSON/strings
// [CreateAssetMenu(fileName = "__catalog", menuName = "Packet/System/Catalog")]
// public class PacketCatalog_DEPRECATED : ScriptableObject, IPacketCatalog_DEPRECATED {
//
// 	public const string DELIMITER = "/";
//
// 	public Lup<int, PacketFact_DEPRECATED> PacketFacts = new();
//
// 	void OnValidate() => FindAndRegisterAllPackets(); // TEMP
// 	// void Awake() => FindAndRegisterAllPackets(); 
//
// 	public void FindAndRegisterAllPackets() {
// 		PacketFacts.Clear();
//
// 		var id = FutzConst.CUSTOM_PACKET_START; // TODO: actually set this
//
// 		var registerPacketMethod = typeof(PacketCatalog_DEPRECATED).GetAnyMethod(nameof(RegisterPacket));
// 		var packetTypes = AppDomain.CurrentDomain.TypesWithInterface<IPacket_DEPRECATED>();
//
// 		foreach (var packetType in packetTypes) {
// 			var initializer = registerPacketMethod.MakeGenericMethod(packetType);
//
// 			id++;
// 			var fact = (PacketFact_DEPRECATED)initializer.Invoke(this, new object[] { id });
//
// 			PacketFacts.Set(fact.Id, fact);
//
// 			Log($"++Packet: {fact}".LgGreen());
// 		}
//
// 		Log($"{PacketFacts.Count} packets found!  ===============================".LgGreen());
// 	}
//
// 	public PacketFact_DEPRECATED RegisterPacket<TPacket>(int packetId) where TPacket : IPacket_DEPRECATED =>
// 		StorageForPacketCatalog<TPacket>.Initialize(packetId);
//
// 	public PacketFact_DEPRECATED GetPacketFact<TPacket>() where TPacket : IPacket_DEPRECATED {
// 		var fact = StorageForPacketCatalog<TPacket>.Fact;
// 		if (fact == null) LogError($"No packet for id: {typeof(TPacket)}");
// 		return fact;
// 	}
//
// 	public PacketFact_DEPRECATED GetPacketFact(int packetId) {
// 		var fact = PacketFacts.Get(packetId);
// 		if (fact == null) LogError($"No packet for id: {packetId}");
// 		return fact;
// 	}
//
// 	/// "packetId/agentKey/json.."
// 	public (string agentKey, PacketFact_DEPRECATED fact, IPacket_DEPRECATED packet) Read(MsgBuffer msgBuffer) {
// 		var (packetIdStr, agentKey, json) =
// 			msgBuffer
// 			   .GetString()
// 			   .SplitOn2(DELIMITER);
//
// 		var packetId = int.Parse(packetIdStr);
// 		var fact = GetPacketFact(packetId);
// 		var packet = fact.FnRead(json);
//
// 		Log($"{fact} (from {agentKey}): {json}".LogBlue());
// 		return (agentKey, fact, packet);
// 	}
//
// 	/// "packetId/agentIdf/json.."
// 	public string Write<TPacket>(IPacket_DEPRECATED packet, string agentIdf) where TPacket: IPacket_DEPRECATED {
// 		var fact = GetPacketFact<TPacket>();
// 		return (fact.IdStr, agentIdf, fact.FnWrite(packet)).Join(DELIMITER);
// 	}
//
// 	/// "packetId/agentIdf/json.."
// 	public string Write(PacketFact_DEPRECATED fact, IPacket_DEPRECATED packet, string agentIdf) =>
// 		(fact.IdStr, agentIdf, fact.FnWrite(packet)).Join(DELIMITER);
//
// }
//
//
// // ReSharper disable StaticMemberInGenericType
// public static class StorageForPacketCatalog<TPacket> where TPacket : IPacket_DEPRECATED {
//
// 	public static Func<string, IPacket_DEPRECATED> FnRead = static msg =>
// 		JsonUtility.FromJson<TPacket>(msg);
//
// 	public static Func<IPacket_DEPRECATED, string> FnWrite = static packet =>
// 		JsonUtility.ToJson(packet);
//
//
// 	public static PacketFact_DEPRECATED Fact;
//
// 	public static PacketFact_DEPRECATED Initialize(int id) {
// 		Fact = new PacketFact_DEPRECATED {
// 			Name = typeof(TPacket).Name,
// 			Id = id,
// 			IdStr = $"{id}",
// 			FnRead = FnRead,
// 			FnWrite = FnWrite,
// 		};
// 		return Fact;
// 	}
//
// }
// // ReSharper restore StaticMemberInGenericType
// }
//
//
// // public bool DoRefreshNow;
// // public List<BasePacket> Packets = new();
// // public BasePacket GetPacket(int packetId) {
// // 	if (packetId <= 0) return null;
// // 	return Packets[packetId];
// // }
// //
// // "123duderJSON
// // public const int PACKET_ID_SIZE = 3;
// // public const int AGENT_IDF_SIZE = 5;
// // public const int AGENT_IDF_START = PACKET_ID_SIZE;
// // public const int AGENT_IDF_END = PACKET_ID_SIZE + AGENT_IDF_SIZE;
// // public const int JSON_START = AGENT_IDF_END;
// // var packetId = int.Parse(message[..PACKET_ID_SIZE]);
// // var agentIdf = message[AGENT_IDF_START..AGENT_IDF_END];
// // var json = message[JSON_START..];
// // DoRefreshNow = false;
// // Packets.Clear();
// //
// // var packetAssets = AssetDatabase.FindAssets($"t:{nameof(BasePacket)}")
// // 	   .Select(
// // 			guid =>
// // 				AssetDatabase.LoadAssetAtPath<BasePacket>(
// // 					AssetDatabase.GUIDToAssetPath(guid)
// // 				)
// // 		)
// // 	   .OrderBy(p => p.name)
// // 	;
// //
// // for (var dex = 0; dex < FutzConst.CUSTOM_PACKET_START; dex++) {
// // 	Packets.Add(null);
// // }
// //
// // foreach (var packet in packetAssets) {
// // 	var systemId = packet.SystemPacketId();
// // 	if (systemId != null) {
// // 		if (systemId >= FutzConst.CUSTOM_PACKET_START) {
// // 			throw new Exception(
// // 				$"System packet ID must be below {FutzConst.CUSTOM_PACKET_START}"
// // 			);
// // 		}
// //
// // 		packet.PacketId = (int)systemId;
// // 		packet.PacketIdf = systemId.ToString();
// // 		Packets[packet.PacketId] = packet;
// // 		continue;
// // 	}
// //
// // 	var id = Packets.Count;
// // 	packet.PacketId = id;
// // 	packet.PacketIdf = id.ToString();
// // 	Packets.Add(packet);
// // }
// //
// // if (Packets.Count >= 100)
// // 	throw new Exception($"How have you not replaced serialization ;P");