using Regent.Entities;
using Swoonity.MHasher;

namespace Regent.Cogs
{
public interface ICog
{
	public void SetHashId(MHash hashId);
	public MHash GetHash();

	public Entity GetEntity();
}
}