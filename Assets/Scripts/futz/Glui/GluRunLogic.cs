using static UnityEngine.Debug;

namespace Glui
{
public static class GluRunLogic
{
	/// call this with a worker on a stage of your choice
	public static void InitializeStack(this GluStack stack)
	{
		foreach (var layer in stack.AllScreens) {
			layer.DesiredVis = layer.StartShown
				? GluVis.VISIBLE
				: GluVis.HIDDEN;
		}

		stack.CheckVisibleScreens.Trigger();
	}

	/// call this with a worker on a stage of your choice
	public static void RefreshVisibility(this GluStack stack)
	{
		foreach (var screen in stack.AllScreens) {
			var wantVisible = screen.DesiredVis;
			var currentVisible = screen.CurrentVis;

			if (wantVisible == currentVisible) continue; //>> already good

			if (wantVisible == GluVis.VISIBLE) {
				stack.ScreenStack.Add(screen);
				screen.CurrentVis = GluVis.VISIBLE;
				screen.ApplyVisibilityStyle(true);

				foreach (var window in screen.Windows) {
					//## refresh Window when shown
					window.TriggerRefresh();
				}
			}
			else {
				stack.ScreenStack.Remove(screen);
				screen.CurrentVis = GluVis.HIDDEN;
				screen.ApplyVisibilityStyle(false);
			}
		}
	}
}
}