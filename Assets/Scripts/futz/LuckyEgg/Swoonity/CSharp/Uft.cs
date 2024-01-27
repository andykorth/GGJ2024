namespace Swoonity.CSharp
{
/// enum: UNSET, FALSE, TRUE
public enum Uft
{
	UNSET,
	FALSE,
	TRUE,
}

public static class UftUtils
{
	public static bool Is(this Uft uft) => uft == Uft.TRUE;
	public static bool Not(this Uft uft) => uft == Uft.FALSE;
	public static bool Unset(this Uft uft) => uft == Uft.UNSET;

	/// to Uft enum: UNSET, FALSE, TRUE
	public static Uft ToUft(this bool val) => val ? Uft.TRUE : Uft.FALSE;
}
}