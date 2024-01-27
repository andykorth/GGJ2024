using System;
using System.Diagnostics.Contracts;

namespace Swoonity.Functional
{
public static class __Void
{
	public static readonly __ _ = new __();

	/// Equivalent to void or 'Unit' from some functional languages.
	/// <see cref="https://github.com/louthy/language-ext#void-isnt-a-real-type"/>
	[Serializable]
	public readonly struct __ : IEquatable<__>, IComparable<__>
	{
		public static readonly __ Default = new __();
		public static readonly __ Void = new __();

		[Pure] public override int GetHashCode() => 0;
		[Pure] public override bool Equals(object obj) => obj is __;
		[Pure] public override string ToString() => "__";
		[Pure] public bool Equals(__ other) => true;
		[Pure] public static bool operator ==(__ lhs, __ rhs) => true;
		[Pure] public static bool operator !=(__ lhs, __ rhs) => false;
		[Pure] public static bool operator >(__ lhs, __ rhs) => false;
		[Pure] public static bool operator >=(__ lhs, __ rhs) => true;
		[Pure] public static bool operator <(__ lhs, __ rhs) => false;
		[Pure] public static bool operator <=(__ lhs, __ rhs) => true;
		[Pure] public int CompareTo(__ other) => 0;
		[Pure] public static __ operator +(__ a, __ b) => Default;
		[Pure] public static implicit operator ValueTuple(__ _) => default;
		[Pure] public static implicit operator __(ValueTuple _) => default;


		public static Func<T1, __> Fun<T1>(Action<T1> method)
			=> (a) => {
				method(a);
				return Void;
			};

		public static Func<T1, T2, __> Fun<T1, T2>(Action<T1, T2> method)
			=> (a, b) => {
				method(a, b);
				return Void;
			};
	}
}


// using static Swoonity.Functional.__Void;
//
//
// public __ Test(Food food) {
// 	return food switch {
// 		Food.APPLE => PrintFruit(food),
// 		Food.BANANA => PrintFruit(food),
// 		Food.CARROT => PrintVeggie(food),
// 		_ => throw new ArgumentOutOfRangeException(nameof(food), food, null)
// 	};
// }
//
// public __ PrintFruit(Food food) {
// 	Debug.Log($"Fruit: {food}");
// 	return _;
// }
//
// public __ PrintVeggie(Food food) {
// 	Debug.Log($"Veggie: {food}");
// 	return _;
// }
//
//
// public enum Food {
// 	APPLE,
// 	BANANA,
// 	CARROT,
// }

public static class VoidO
{
	public static readonly V _ = new V();

	/// Equivalent to void or 'Unit' from some functional languages.
	/// <see cref="https://github.com/louthy/language-ext#void-isnt-a-real-type"/>
	[Serializable]
	public readonly struct V : IEquatable<V>, IComparable<V>
	{
		public static readonly V Default = new V();
		public static readonly V Void = new V();

		[Pure] public override int GetHashCode() => 0;
		[Pure] public override bool Equals(object obj) => obj is V;
		[Pure] public override string ToString() => "O";
		[Pure] public bool Equals(V other) => true;
		[Pure] public static bool operator ==(V lhs, V rhs) => true;
		[Pure] public static bool operator !=(V lhs, V rhs) => false;
		[Pure] public static bool operator >(V lhs, V rhs) => false;
		[Pure] public static bool operator >=(V lhs, V rhs) => true;
		[Pure] public static bool operator <(V lhs, V rhs) => false;
		[Pure] public static bool operator <=(V lhs, V rhs) => true;
		[Pure] public int CompareTo(V other) => 0;
		[Pure] public static V operator +(V a, V b) => Default;
		[Pure] public static implicit operator ValueTuple(V _) => default;
		[Pure] public static implicit operator V(ValueTuple _) => default;


		public static Func<T1, V> Fun<T1>(Action<T1> method)
			=> (a) => {
				method(a);
				return Void;
			};

		public static Func<T1, T2, V> Fun<T1, T2>(Action<T1, T2> method)
			=> (a, b) => {
				method(a, b);
				return Void;
			};
	}
}
}