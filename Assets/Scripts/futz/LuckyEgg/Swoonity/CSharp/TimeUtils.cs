using System;

namespace Swoonity.CSharp
{
public static class TimeUtils
{
	/// 4:01 PM
	public static string Time(this DateTime dt) => dt.ToShortTimeString();
}
}