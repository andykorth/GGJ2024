using UnityEngine;

namespace Swoonity.Unity
{
public class NoteAttribute : PropertyAttribute
{
	public string Text;
	public NoteMsgType MessageType;
	public float MarginTop;

	public NoteAttribute(
		string text,
		NoteMsgType type = NoteMsgType.Warning,
		float marginTop = 0
	)
	{
		Text = text;
		MessageType = type;
		MarginTop = marginTop;
	}
}

// TODO: Margin is broken

public class NoteSectionAttribute : NoteAttribute
{
	public NoteSectionAttribute(
		string text,
		float marginTop = 12f
	) : base(
		text,
		NoteMsgType.None,
		marginTop
	) { }
}

public class NoteInfoAttribute : NoteAttribute
{
	public NoteInfoAttribute(string text) : base(text, NoteMsgType.Info) { }
}

// public class NoteWarnAttribute : NoteAttribute {
// 	public NoteWarnAttribute(string text) : base(text, MessageType.Warning) {}
// }

public class NoteErrorAttribute : NoteAttribute
{
	public NoteErrorAttribute(string text) : base(text, NoteMsgType.Error) { }
}

public enum NoteMsgType
{
	None,
	Info,
	Warning,
	Error,
}
}