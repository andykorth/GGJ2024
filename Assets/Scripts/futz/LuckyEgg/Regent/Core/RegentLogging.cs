using System;
using UnityEngine;
using Caller = System.Runtime.CompilerServices.CallerMemberNameAttribute;

namespace Regent.Logging
{
public enum RegentLogs
{
	Important,
	Stage,
	Baron,
	Worker,
	Clip,
	Cog,
	Entity,
	Hashing,
	Misc,
}

public static class RLog
{
	public static string CurrentCycle = "?";
	public static string CurrentStage = "???";

	public static LogConfig Config = new();

	public static LogEntry Important => Config.LogImportant;
	public static LogEntry Stage => Config.LogStage;
	public static LogEntry Baron => Config.LogBaron;
	public static LogEntry Worker => Config.LogWorker;
	public static LogEntry Clip => Config.LogClip;
	public static LogEntry Cog => Config.LogCog;
	public static LogEntry Entity => Config.LogEntity;
	public static LogEntry Syncer => Config.LogSyncer;
	public static LogEntry Hashing => Config.LogHashing;
	public static LogEntry Misc => Config.LogMisc;

	public static string _RLog(this string text, LogEntry entry) => entry.Gen(text);

	public static string _RLg(this string text, LogEntry entry, [Caller] string name = "")
		=> entry.Gen(text, name);


	// TODO: move to definition SO
	[Serializable]
	public class LogConfig
	{
		public LogEntry LogImportant = new(new Color(237, 209, 0));
		public LogEntry LogStage = new(new Color(142, 212, 237));
		public LogEntry LogBaron = new(new Color(194, 135, 237));
		public LogEntry LogWorker = new(new Color(237, 164, 62));
		public LogEntry LogClip = new(new Color(237, 139, 191));
		public LogEntry LogCog = new(new Color(237, 213, 149));
		public LogEntry LogEntity = new(new Color(196, 237, 168));
		public LogEntry LogSyncer = new(new Color(196, 122, 168));
		public LogEntry LogHashing = new(new Color(237, 72, 0));
		public LogEntry LogMisc = new(new Color(237, 72, 0));
	}

	[Serializable]
	public class LogEntry
	{
		public bool Enabled = true;
		public Color Color;

		public LogEntry() : this(Color.black) { }
		public LogEntry(Color color) => Color = color;

		string Hex() => ColorUtility.ToHtmlStringRGB(Color);

		public string Gen(string text, string from = "")
			=> $"{Time.frameCount} {CurrentCycle}.{CurrentStage}|{from}  <color=#{Hex()}>{text}</color>";

		public bool On => Enabled;
	}
}
}