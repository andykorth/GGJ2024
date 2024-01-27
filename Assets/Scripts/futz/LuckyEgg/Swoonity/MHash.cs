using System;
using System.Collections.Generic;

namespace Swoonity.MHasher
{
/// deterministic conversion of type/string/etc. to uint (implicit) with relatively small chance of collision
[Serializable]
public struct MHash : IEquatable<MHash>
{
	public uint Value;

	public MHash(uint value) => Value = value;
	public MHash(string str) => Value = MakeHashValue(str);

	public bool IsValid => Value != 0;
	public bool IsInvalid => Value == 0;

	public static implicit operator MHash(uint hash) => new(hash);
	public static implicit operator uint(MHash mhash) => mhash.Value;

	public bool Equals(MHash other) => Value == other.Value;
	public override bool Equals(object obj) => obj is MHash other && Equals(other);
	public override int GetHashCode() => (int)Value;

	public override string ToString() => Value.ToString();

	public static uint MakeHashValue(string str)
	{
		{
			unchecked {
				uint hash = 23;
				foreach (var ch in str) {
					hash = hash * 31 + ch;
				}

				return hash;
			}
		}
	}

	public static MHash Hash(Type type) => Hash(type.FullName);
	public static MHash Hash(Type type, string name) => Hash($"{type.FullName}_{name}");
	public static MHash Hash(string name, Type type) => Hash($"{name}_{type.FullName}");
	public static MHash Hash(string str) => MakeHashValue(str);
	public static MHash Hash(string a, string b) => MakeHashValue($"{a}_{b}");
	public static MHash Hash(string a, string b, string c) => MakeHashValue($"{a}_{b}_{c}");

	public static MHash Hash(string a, string b, string c, string d)
		=> MakeHashValue($"{a}_{b}_{c}_{d}");


	public static MHash Get<T>() => TypeHash<T>.MHash;
}

public interface IMHashable
{
	public MHash GetHash();
}

public static class TypeHash<T>
{
	public static MHash MHash = MHash.Hash(typeof(T));
}

public static class MHashCollisionCheck
{
	public static List<TVal> CheckMHashCollisions<TVal>(
		this List<TVal> listToCheck,
		List<TVal> collisions = null,
		Dictionary<MHash, TVal> dict = null,
		bool clearCollisionListFirst = true,
		bool clearDictionaryFirst = true
	) where TVal : IMHashable
	{
		if (collisions == null) {
			collisions = new List<TVal>();
		}
		else if (clearCollisionListFirst) {
			collisions.Clear();
		}

		if (dict == null) {
			dict = new Dictionary<MHash, TVal>();
		}
		else if (clearDictionaryFirst) {
			dict.Clear();
		}

		foreach (var val in listToCheck) {
			var mhash = val.GetHash();
			if (dict.ContainsKey(mhash)) {
				collisions.Add(val);
			}
			else {
				dict[mhash] = val;
			}
		}

		return collisions;
	}
}
}


// // TODO use?
// [Serializable]
// public class HashTypes {
//
// 	public override string ToString() => $"HashTypes<{TargetTypes.Join()}>";
//
// 	public bool IsInitialized;
// 	public Type[] TargetTypes;
// 	public readonly Dictionary<uint, Type> HashId__Type = new Dictionary<uint, Type>();
//
// 	public HashTypes(Type targetType) => TargetTypes = new[] {targetType};
// 	public HashTypes(Type[] targetTypes) => TargetTypes = targetTypes;
//
// 	public void Check(Type type) {
// 		if (!IsInitialized) {
// 			Initialize();
// 			return;
// 		}
//
// 		var hash = MHash.Hash(type);
// 		var hasKey = HashId__Type.Has(hash);
//
// 		// Log($"{this} CHECK {{{hash}: {type}}}, hasKey: {hasKey}".LogGrey());
//
// 		if (!hasKey) {
// 			Initialize();
// 		}
// 	}
//
// 	public void Initialize() {
// 		HashId__Type.Clear();
//
// 		foreach (var type in TargetTypes) {
// 			LoadWithReflection(type);
// 		}
//
// 		IsInitialized = true;
// 		// Log($"{this} initialized {HashId__Type.Count} | {HashId__Type.Join()}".LogGreen());
// 		// Log($"{this} initialized {HashId__Type.Count} hashes".LogGreen());
// 	}
//
// 	public void LoadWithReflection(Type targetType) {
// 		var types = targetType.IsInterface
// 			? AppDomain.CurrentDomain.TypesWithInterface(targetType)
// 			: AppDomain.CurrentDomain.SubtypesOf(targetType);
//
//
// 		foreach (var type in types) {
// 			HashId__Type.AddOrThrow(
// 				MHash.Hash(type),
// 				type,
// 				DuplicateException
// 			);
// 		}
// 	}
//
// 	public static Exception DuplicateException(uint hash, Type adding, Type existing) =>
// 		new Exception(
// 			$"Hash collision at {hash}." +
// 			$" Please rename {adding.FullName}" +
// 			$" OR {existing.FullName}"
// 		);
//
// }


// // TODO: use for bit packing?
// // OPTIMIZE network: hashId (4 bytes) -> smallId (1 byte)
// // could generate a temporary lookup ID from registered hashes
// // sort hash list by its ID (uint)
// // then assign index byte 1..255 (0 for invalid)
// // would be deterministic
// // ?? isn't this similar to sorting by name and taking that index?
// public class ExampleLookupTODO<TBase> {
//
// 	Dictionary<uint, TBase> _hash__item = new Dictionary<uint, TBase>();
//
// 	Dictionary<byte, (uint, TBase)> _tinyId__hashItem = new Dictionary<byte, (uint, TBase)>();
//
// 	public uint Add<TItem>(TItem item) where TItem : TBase {
// 		var hash = typeof(TItem).GetSimpleHash();
// 		if (_hash__item.Has(hash)) throw new Exception($"Collision TODO");
//
// 		_hash__item[hash] = item;
//
// 		return hash;
// 	}
//
// 	public void GenerateTinyIds() {
// 		_tinyId__hashItem.Clear();
//
// 		if (_hash__item.Count >= 255) // 0 is reserved for invalid
// 			throw new Exception($"max 255 TODO");
//
// 		var hashes = _hash__item.ToList();
// 		hashes.Sort((a, b) => a.Key.CompareTo(b.Key));
//
// 		byte tinyId = 1; // 0 is invalid
// 		foreach (var (hash, item) in hashes) {
// 			_tinyId__hashItem[tinyId] = (hash, item);
// 			tinyId += 1;
// 		}
// 	}
//
//
// 	public void LoadWithReflection() {
// 		// TODO
// 		var itemTypes = AppDomain.CurrentDomain.SubtypesOf<TBase>();
// 	}
//
// }