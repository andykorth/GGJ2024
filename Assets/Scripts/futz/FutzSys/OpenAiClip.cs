using Regent.Clips;
using Swoonity.Unity;
using UnityEngine;

namespace FutzSys
{
public class OpenAiClip : ClipNative
{
	public static OpenAiClip I;
	public override void SetRef() => I = this;

	[TextArea(20, 20)]
	public string Prompt;

	[Btn(nameof(Send))]
	public bool IgnoreThis;
	
	[TextArea(20, 20)]
	public string Answer;

	public void Send()
	{
		OpenAiBaron.Send(Prompt); // TEMP HACK
	}
}
}