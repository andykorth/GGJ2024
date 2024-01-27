using Regent.Barons;
using Regent.Clips;
using Regent.Entities;

namespace Regent.Coordinator
{
public interface IRegentCoordinator
{
	public void HandleBaronEnabled(Baron baron);
	public void HandleBaronDisabled(Baron baron);

	public void HandleEntityCreated(Entity entity);
	public void HandleEntityDestroyed(Entity entity);

	public void HandleClipCreated(IClip clip);
	public void HandleClipDestroyed(IClip clip);

	public void OnStartClient();
	public void OnCloseClient();
	public void OnStartServer();
	public void OnCloseServer();
}
}