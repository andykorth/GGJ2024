using System;

namespace Swoonity.Unity
{
public static class FlagUtils
{
	public static bool HasFlag(this int flag, int other) => (flag & other) == other;
	public static bool MissingFlag(this int flag, int other) => (flag & other) == 0;
	public static bool AnyFlag(this int flag, int other) => (flag & other) != 0;

	public static int SetFlag(this int flag, int other) => flag | other;
	public static int UnsetFlag(this int flag, int other) => flag & ~other;
	public static int CutFlag(this int flag, int other) => flag & ~other;
	public static int ToggleFlag(this int flag, int other) => flag ^ other;

	/* for reference */
	// public static int FlagAnd(this int flag, int other) => flag & other;
	// public static int FlagOr(this int flag, int other) => flag | other;
	// public static int FlagNot(this int flag, int other) => flag & ~other;
	// public static int FlagXor(this int flag, int other) => flag ^ other;
	// public static int FlagShiftLeft(this int flag) => flag << 1;
	// public static int FlagShiftRight(this int flag) => flag >> 1;
	// public static int FlagInverse(this int flag) => ~flag;


	/// get binary string: 5 => "101"
	public static string BitString(this int val) => Convert.ToString(val, 2);
}

public static class Flag
{
	public static bool Has(int flag, int has) => (flag & has) == has;
	public static bool Missing(int flag, int missing) => (flag & missing) == 0;
}

/*


[Flags]
public enum FlagExample {
	NONE			=			0b_0000000000000000000000000000000,	  // 0
	ALL				=			0b_1111111111111111111111111111111,	  // ~0
	
	Example01			=			0b_______________________________1,   //  1 <<  0	dec:  1
	Example02			=			0b______________________________10,   //  1 <<  1	dec:  2
	Example03			=			0b_____________________________100,   //  1 <<  2	dec:  3
	Example04			=			0b____________________________1000,   //  1 <<  3	dec:  4
	Example05			=			0b___________________________10000,   //  1 <<  4	dec:  5
	Example06			=			0b__________________________100000,   //  1 <<  5	dec:  6
	Example07			=			0b_________________________1000000,   //  1 <<  6	dec:  7
	Example08			=			0b________________________10000000,   //  1 <<  7	dec:  8
	Example09			=			0b_______________________100000000,   //  1 <<  8	dec:  9
	Example10			=			0b______________________1000000000,   //  1 <<  9	dec: 10
	Example11			=			0b_____________________10000000000,   //  1 << 10	dec: 11
	Example12			=			0b____________________100000000000,   //  1 << 11	dec: 12
	Example13			=			0b___________________1000000000000,   //  1 << 12	dec: 13
	Example14			=			0b__________________10000000000000,   //  1 << 13	dec: 14
	Example15			=			0b_________________100000000000000,   //  1 << 14	dec: 15
	Example16			=			0b________________1000000000000000,   //  1 << 15	dec: 16
	Example17			=			0b_______________10000000000000000,   //  1 << 16	dec: 17
	Example18			=			0b______________100000000000000000,   //  1 << 17	dec: 18
	Example19			=			0b_____________1000000000000000000,   //  1 << 18	dec: 19
	Example20			=			0b____________10000000000000000000,   //  1 << 19	dec: 20
	Example21			=			0b___________100000000000000000000,   //  1 << 20	dec: 21
	Example22			=			0b__________1000000000000000000000,   //  1 << 21	dec: 22
	Example23			=			0b_________10000000000000000000000,   //  1 << 22	dec: 23
	Example24			=			0b________100000000000000000000000,   //  1 << 23	dec: 24
	Example25			=			0b_______1000000000000000000000000,   //  1 << 24	dec: 25
	Example26			=			0b______10000000000000000000000000,   //  1 << 25	dec: 26
	Example27			=			0b_____100000000000000000000000000,   //  1 << 26	dec: 27
	Example28			=			0b____1000000000000000000000000000,   //  1 << 27	dec: 28
	Example29			=			0b___10000000000000000000000000000,   //  1 << 28	dec: 29
	Example30			=			0b__100000000000000000000000000000,   //  1 << 29	dec: 30
	Example31			=			0b_1000000000000000000000000000000,   //  1 << 30	dec: 31
	
}



	// can't use signed bit?

*/
}