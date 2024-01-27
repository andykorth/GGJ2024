using Sz = System.SerializableAttribute;

// ReSharper disable ParameterHidesMember
// ReSharper disable InconsistentNaming

namespace FutzSys
{
/*

	F float
	I int
	S string

*/

// TODO: clean up this file

public interface IPk { }


[Sz] public struct Pk_Waiting : IPk
{
	public string Msg;
}

[Sz] public struct Pk_Float : IPk
{
	public float A;
}

[Sz] public struct Pk_Float2 : IPk
{
	public float A;
	public float B;

	public void Deconstruct(out float A, out float B) => (A, B) = (this.A, this.B);
}

[Sz] public struct Pk_Vector2 : IPk
{
	public float X;
	public float Y;

	public void Deconstruct(out float X, out float Y) => (X, Y) = (this.X, this.Y);
}

[Sz] public struct Pk_Int : IPk
{
	public int A;
}

[Sz] public struct Pk_Int2 : IPk
{
	public int A;
	public int B;
	public void Deconstruct(out int A, out int B) => (A, B) = (this.A, this.B);
}

[Sz] public struct Pk_Str : IPk
{
	public string A;
}

[Sz] public struct Pk_Str2 : IPk
{
	public string A;
	public string B;
	public void Deconstruct(out string A, out string B) => (A, B) = (this.A, this.B);
}

[Sz] public struct Pk_IntStr : IPk
{
	public int A;
	public string B;
	public void Deconstruct(out int A, out string B) => (A, B) = (this.A, this.B);
}

[Sz] public struct Pk_StrInt : IPk
{
	public string A;
	public int B;
	public void Deconstruct(out string A, out int B) => (A, B) = (this.A, this.B);
}

[Sz] public struct Pk_StrInt2 : IPk
{
	public string A;
	public int B;
	public int C;

	public void Deconstruct(out string A, out int B, out int C)
		=> (A, B, C) = (this.A, this.B, this.C);
}


[Sz] public struct Pk_TextEntry : IPk
{
	public string A;
}

[Sz] public struct Pk_TextIdEntry : IPk
{
	public int Id;
	public string Text;
	public void Deconstruct(out int Id, out string Text) => (Id, Text) = (this.Id, this.Text);
}
}

// public interface ITest<T1, T2>
// {
// 	// T1 A { get; set; }
// 	// T2 B { get; set; }
// 	public void Deconstruct(out T1 A, out T2 B);
// }
//
// [Sz] public struct TestIntInt : ITest<int, int>
// {
// 	public int A;
// 	public int B;
// 	public void Deconstruct(out int A, out int B) => (A, B) = (this.A, this.B);
// }
//
// [Sz] public struct TestIntStr : ITest<int, string>
// {
// 	public int A;
// 	public string B;
// 	public void Deconstruct(out int A, out string B) => (A, B) = (this.A, this.B);
// }