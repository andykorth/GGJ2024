// @formatter:off
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using uObj = UnityEngine.Object;
using File = System.Runtime.CompilerServices.CallerFilePathAttribute;
using Line = System.Runtime.CompilerServices.CallerLineNumberAttribute;
using Caller = System.Runtime.CompilerServices.CallerMemberNameAttribute;
using MI = System.Runtime.CompilerServices.MethodImplAttribute;
/*


Recommended:



using static UnityEngine.Debug;


 */

namespace Lumberjack {
public static class Lumberjack {
	const System.Runtime.CompilerServices.MethodImplOptions AGGR = MethodImplOptions.AggressiveInlining;

	public static Func<string> FnGetGlobalPrefix = static () => "";

	#region New Hotness

	[MI(AGGR)] public static void LG(this string text, uObj obj = null, [File] string f = "", [Line] int l = -1, [Caller] string c = "", bool skipPre = false) => Debug.Log(L(Blue, text, f, l, c, skipPre), obj);


	[MI(AGGR)] public static void Lrg(this string tx, uObj obj = null, [File] string f = "", [Line] int l = -1, [Caller] string c = "", bool skipPre = false) => Debug.Log(L(Blue, tx, f, l, c, skipPre), obj);

	const string ActColor = "8EC4ED";
	const string OrangeColor = "FF861F";
	const string RedColor = "FF0400";
	const string TodoColor = "7A8CCC";
	[MI(AGGR)] public static void LgAct(this string tx, uObj obj = null, [Caller] string pre = "") => L2(ActColor, tx, obj, pre);
	[MI(AGGR)] public static void LgOrange0(this string tx, uObj obj = null, [Caller] string pre = "") => L2(OrangeColor, tx, obj, pre);
	[MI(AGGR)] public static void LgRed0(this string tx, uObj obj = null, [Caller] string pre = "") => L2(RedColor, tx, obj, pre);
	[MI(AGGR)] public static void LgTodo0(this string tx, uObj obj = null, [Caller] string pre = "") => L2(TodoColor, tx, obj, pre);

	[MI(AGGR)] static void L2(string color, string text, uObj obj, string pre) => Debug.Log($"{Lg.TimePrefix} {FnGetGlobalPrefix()}{pre}|  <color=#{color}>{text}</color>", obj);

	// TODO: with this we can do more ext methods for objects

	#endregion



	public static string LgBlue		(this string text, [File] string file = "", [Line] int line = -1, [Caller] string caller = "", bool skipPrefix = false) => L(Blue, text, file, line, caller, skipPrefix);
	public static string LgGreen	(this string text, [File] string file = "", [Line] int line = -1, [Caller] string caller = "", bool skipPrefix = false) => L(Green, text, file, line, caller, skipPrefix);
	public static string LgOrange	(this string text, [File] string file = "", [Line] int line = -1, [Caller] string caller = "", bool skipPrefix = false) => L(Orange, text, file, line, caller, skipPrefix);
	public static string LgRed		(this string text, [File] string file = "", [Line] int line = -1, [Caller] string caller = "", bool skipPrefix = false) => L(Red, text, file, line, caller, skipPrefix);
	public static string LgYellow	(this string text, [File] string file = "", [Line] int line = -1, [Caller] string caller = "", bool skipPrefix = false) => L(Yellow, text, file, line, caller, skipPrefix);
	public static string LgGold		(this string text, [File] string file = "", [Line] int line = -1, [Caller] string caller = "", bool skipPrefix = false) => L(Gold, text, file, line, caller, skipPrefix);
	public static string LgPink		(this string text, [File] string file = "", [Line] int line = -1, [Caller] string caller = "", bool skipPrefix = false) => L(Pink, text, file, line, caller, skipPrefix);

	static string L(Color color, string text, string file, int line, string caller, bool skipPrefix = false) {
		CheckLoad();

		if (skipPrefix) return text.Gen(ColorUtility.ToHtmlStringRGB(color));

		return text.Gen(
			ColorUtility.ToHtmlStringRGB(color),
			$"{Path.GetFileNameWithoutExtension(file)}:{line} {caller}"
			);
	}


	public static string Log(this string text) => text.Gen(Color.yellow);
	public static string Log(this string text, Color color) => text.Gen(color);
	public static string Log(this string text, string color) => text.Gen(color);

	public static string LogRed(this string text, string prefix = "") => text.Gen(Red, prefix);
	public static string LogGreen(this string text, string prefix = "") => text.Gen(Color.green, prefix);
	public static string LogBlue(this string text, string prefix = "") => text.Gen(Blue, prefix);
	public static string LogWhite(this string text, string prefix = "") => text.Gen(Color.white, prefix);
	public static string LogBlack(this string text, string prefix = "") => text.Gen(Color.black, prefix);
	public static string LogCyan(this string text, string prefix = "") => text.Gen(Color.cyan, prefix);
	public static string LogMagenta(this string text, string prefix = "") => text.Gen(Color.magenta, prefix);
	public static string LogGrey(this string text, string prefix = "") => text.Gen(Color.grey, prefix);
	public static string LogOrange(this string text, string prefix = "") => text.Gen(Orange, prefix);
	public static string LogYellow(this string text, string prefix = "") => text.Gen(Yellow, prefix);
	public static string LogGold(this string text, string prefix = "") => text.Gen(Gold, prefix);

	public static Color Blue = new Color32(144, 197, 237, 255);
	public static Color Green = new Color32(0, 204, 32, 255);
	public static Color Orange = new Color32(255, 134, 0, 255);
	public static Color Red = new Color32(255, 72, 79, 255);
	public static Color Yellow = new Color32(255, 255, 0, 255);
	public static Color Gold = new Color32(255, 215, 0, 255);
	public static Color Pink = new Color32(255, 105, 180, 255);

	public static Color Rgb(byte r, byte g, byte b, byte a = 255) => new Color32(r, g, b, a);

	static readonly (Color, string) _server = (new Color(0.93f, 0.65f, 0.79f), "server");
	static readonly (Color, string) _client = (new Color(0.56f, 0.77f, 0.93f), "client");
	static readonly (Color, string) _remote = (new Color(0.62f, 0.93f, 0.71f), "remote");
	static readonly (Color, string) _author = (new Color(0.68f, 0.93f, 0.59f), "author");
	static readonly (Color, string) _native = (new Color(0.93f, 0.77f, 0.58f), "native");
	static readonly (Color, string) _test   = (new Color(0.93f, 0.77f, 0.58f), "|T|E|S|T");
	static readonly (Color, string) _todo   = (new Color(0.48f, 0.55f, 0.80f), "TODO");
	static readonly (Color, string) _act = (new Color(0.56f, 0.77f, 0.93f), "act");

	public static string LgNative(this string text, [Caller] string name = "") => text.Gen(_native, name);
	public static string LgServer(this string text, [Caller] string name = "") => text.Gen(_server, name);
	public static string LgClient(this string text, [Caller] string name = "") => text.Gen(_client, name);
	public static string LgAuthor(this string text, [Caller] string name = "") => text.Gen(_author, name);
	public static string LgRemote(this string text, [Caller] string name = "") => text.Gen(_remote, name);
	public static string LgTest(this string text, [Caller] string name = "") => text.Gen(_test, name);
	public static string LgTodo(this string text, [Caller] string name = "") => text.Gen(_todo, name);
	public static string LgAct_OLD(this string text, [Caller] string name = "") => text.Gen(_act, name);

	public static string LogNative(this string text) => text.Gen(_native);
	public static string LogServer(this string text) => text.Gen(_server);
	public static string LogClient(this string text) => text.Gen(_client);
	public static string LogAuthor(this string text) => text.Gen(_author);
	public static string LogRemote(this string text) => text.Gen(_remote);
	public static string LogTest(this string text) => text.Gen(_test);
	public static string LogTodo(this string text) => text.Gen(_todo);
	public static string LogAct(this string text) => text.Gen(_act);


	static string Gen(this string text, (Color color, string prefix) colorPrefix)
		=> text.Gen(ColorUtility.ToHtmlStringRGB(colorPrefix.color), colorPrefix.prefix);

	static string Gen(this string text, (Color color, string prefix) colorPrefix, string prefix)
		=> text.Gen(ColorUtility.ToHtmlStringRGB(colorPrefix.color), $"{colorPrefix.prefix}|{prefix}");

	static string Gen(this string text, Color color, string prefix = "") =>
		text.Gen(ColorUtility.ToHtmlStringRGB(color), prefix);


	static string Gen(this string text, string color, string prefix = "") =>
		$"{Lg.TimePrefix} {FnGetGlobalPrefix()}{prefix}|  <color=#{color}>{text}</color>";

	/// used by external plugin, such as OneJS
	public static void ExternalLog(string text, string color, string prefix) =>
		Debug.Log(text.Gen(color, prefix));


	const string VALUE_COLOR = "fc9ae9";

	public static bool Log(this bool val, string prefix = "bool", string suffix = "") {
		Debug.Log($"{Time.frameCount}| {prefix}: {val} {suffix}".Gen(VALUE_COLOR));
		return val;
	}

	public static string LogThis(this string val, string prefix = "string", string suffix = "") {
		Debug.Log($"{Time.frameCount}| {prefix}: {val} {suffix}".Gen(VALUE_COLOR));
		return val;

	}

	public static float Log(this float val, string prefix = "float", string suffix = "") {
		Debug.Log($"{Time.frameCount}| {prefix}: {val} {suffix}".Gen(VALUE_COLOR));
		return val;
	}

	public static Vector2 Log(this Vector2 val, string prefix = "vector2", string suffix = "") {
		Debug.Log($"{Time.frameCount}| {prefix}: {val} {suffix}".Gen(VALUE_COLOR));
		return val;
	}

	public static Vector3 Log(this Vector3 val, string prefix = "vector3", string suffix = "") {
		Debug.Log($"{Time.frameCount}| {prefix}: {val} {suffix}".Gen(VALUE_COLOR));
		return val;
	}

	public static Transform Log(this Transform val, string prefix = "transform", string suffix = "") {
		Debug.Log($"{Time.frameCount}| {prefix}: {val} {suffix}".Gen(VALUE_COLOR), val);
		return val;
	}

	const int NUM_OF_LINE_CHAR = 150;
	const int NUM_OF_APPEND_LINE_CHAR = 150;

	public static string LineOf(this string str, int count = NUM_OF_LINE_CHAR)
		=> new StringBuilder(str.Length * count).Insert(0, str, count).ToString();

	public static string AppendLineOf(this string to, string of, int count = NUM_OF_APPEND_LINE_CHAR)
		=> $"{to} {of.LineOf(count)}";


#if UNITY_EDITOR
	[UnityEditor.Callbacks.DidReloadScripts(-9001)]
	static void OnScriptsReload() => CheckLoad();
#endif
	
	
	static bool _hasAnnouncedLoad;
	
	public static void CheckLoad() {
		if (_hasAnnouncedLoad) return; //>> already announced
		_hasAnnouncedLoad = true;
		
		const string color = "E6FF00";
		const string line = "<><><><><><><><><><><><><><><><><><><><><><><><><><><><><>";
		var load = $"       FULL RELOAD:   {DateTime.Now.ToLongTimeString()}";
		
		// Debug.Log($"<b><color=#{color}>{line}</color></b>");
		Debug.Log($"<b><color=#{color}>{load}</color></b>");
		// Debug.Log($"<b><color=#{color}>{line}</color></b>");
	}
	
}

public static class Lg {
	/// 1:29 PM
	public static string Time => DateTime.Now.ToShortTimeString();
	public static string TimePrefix => Application.isPlaying 
		? UnityEngine.Time.frameCount.ToString() 
		: DateTime.Now.ToLongTimeString();
	
	
	
}
}