using Cysharp.Threading.Tasks;

namespace Regent.Syncers.Auth
{
public static class SyncerAuthResults
{
	/*  OPEN	   11 to 127   */

	public const sbyte PARTIAL_SUCCESS = 9;

	public const sbyte CONTROL = 5; // auth not checked
	public const sbyte PASS = 1;
	/*  RESERVED	1 to 10    */

	public const sbyte UNSET = 0; // something went wrong with system (counts as failure)

	/*  RESERVED   -1 to -10    */
	public const sbyte FAIL = -1;
	public const sbyte INVALID = -2;
	public const sbyte MISSING = -3;
	public const sbyte OUT_OF_DATE = -4;
	public const sbyte NOT_ALLOWED = -5;

	/*  OPEN	  -11 to -128   */
}

public static class SyncerAuthUtils
{
	public static bool Passed(this sbyte val) => val >= 1;
	public static bool Failed(this sbyte val) => val <= 0;
	public static UniTask<sbyte> Result(this sbyte val) => UniTask.FromResult(val);
}
}