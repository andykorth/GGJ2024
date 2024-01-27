using Regent.Cogs;

namespace Regent.Clips
{
/// A non-networked clip or "clipboard" for holding shared state. See: Blackboard pattern
public abstract class ClipNative : CogNative, IClip
{
	public abstract void SetRef();

	protected override void InternalWhenAwake()
	{
		base.InternalWhenAwake();
		Regent.Entities.Entity.__Coordinator.HandleClipCreated(this);
	}

	protected override void InternalWhenDestroyed()
	{
		base.InternalWhenDestroyed();
		Regent.Entities.Entity.__Coordinator.HandleClipDestroyed(this);
	}
}
}