using Regent.Clips;
using Regent.Syncers;
using UnityEngine;

namespace UiSys
{
public class CoreUiClip : ClipNative
{
	public static CoreUiClip I;
	public override void SetRef() => I = this;

	[Header("Hud")]
	public TrackToggle ShowActivitySelect = new();
	public TrackToggle ShowAgentList = new();
	public TrackToggle ShowScore = new();

	[Header("Buttons")]
	public TrackEvt OptionsMenu = new();
	public TrackEvt ToggleHud = new();
	public TrackEvt Cancel = new();
	public TrackEvt Console = new();
	public Track<int> FKeys = new();

}
}