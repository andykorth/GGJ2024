using Regent.Entities;
using Regent.SyncerFacts;
using Swoonity.MHasher;

namespace Regent.Syncers
{
public enum ClientAuthType
{
	Native, // non-networked (runs on everything)
	Control, // Full Control
	Respect, // Review After
	Propose, // Propose Change (server will change if PASS)
	Declare, // Declare value (value will not change)
	Monitor, // Read Only
}

public interface ISyncer
{
	public MHash GetHash();

	public Entity GetEntity();

	// public void __Initialize();
	public void __ValidateFromCog(Entity entity, SyncerFact fact);
}
}