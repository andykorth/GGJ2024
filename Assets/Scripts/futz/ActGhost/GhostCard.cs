using Regent.Cogs;
using Regent.Syncers;

namespace futz.ActGhost
{
	// TODO: delete
public class GhostCard : CogNative
{
	public int CardId;
	public Track<bool> IsRevealed = new();
	public Track<string> Word = new();
	// TODO: emojis? icons? images?
	public bool PendingIsCorrect;
	public Track<bool> IsCorrect = new();
	public TrackList<int> PickedByActorSlotIds = new();
	public Track<int> ShowCount = new();

	public override string ToString() => $"card{CardId}({Word.Current})";
}
}