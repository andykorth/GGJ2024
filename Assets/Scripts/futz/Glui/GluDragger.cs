using System;
using Lumberjack;
using Swoonity.CSharp;
using Swoonity.Unity;
using UnityEngine.UIElements;
using static UnityEngine.Debug;

namespace Glui
{
public class GluDragger : MouseManipulator
{
	public GluDragMethod DragMethod;
	public float ElStartX;
	public float ElStartY;
	public float MouseStartX;
	public float MouseStartY;
	public bool IsDragging;
	public float MaxX;
	public float MaxY;

	public GluDragger(GluDragMethod dragMethod)
	{
		if (dragMethod == GluDragMethod.NONE) {
			throw new Exception($"invalid GluDragger {dragMethod} {target}");
		}

		activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
		IsDragging = false;
		DragMethod = dragMethod;
	}

	protected override void RegisterCallbacksOnTarget()
	{
		target.RegisterCallback<MouseDownEvent>(OnMouseDown);
		target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
		target.RegisterCallback<MouseUpEvent>(OnMouseUp);
	}

	protected override void UnregisterCallbacksFromTarget()
	{
		target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
		target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
		target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
	}

	protected void OnMouseDown(MouseDownEvent evt)
	{
		if (IsDragging) {
			evt.StopImmediatePropagation();
			return; //>> already dragging
		}

		if (!CanStartManipulation(evt)) return; //>> can't start
		
		var resolvedStyle = target.resolvedStyle;
		var parentResolvedStyle = target.parent.resolvedStyle;
		MaxX = parentResolvedStyle.width - resolvedStyle.width;
		MaxY = parentResolvedStyle.height - resolvedStyle.height;

		ElStartX = target.layout.x;
		ElStartY = target.layout.y;

		target.style.left = ElStartX;
		target.style.top = ElStartY;
		target.style.bottom = StyleKeyword.Auto;
		target.style.right = StyleKeyword.Auto;

		(MouseStartX, MouseStartY) = evt.mousePosition;

		IsDragging = true;
		target.CaptureMouse();
		evt.StopPropagation();
		//>> start dragging
	}

	protected void OnMouseMove(MouseMoveEvent evt)
	{
		if (!IsDragging || !target.HasMouseCapture()) {
			return; //>> not dragging
		}

		evt.StopPropagation();

		var diffX = evt.mousePosition.x - MouseStartX;
		var diffY = evt.mousePosition.y - MouseStartY;

		var newLeft = ElStartX + diffX;
		var newTop = ElStartY + diffY;

		if (DragMethod == GluDragMethod.FREE) {
			target.style.left = newLeft;
			target.style.top = newTop;
			return; //>> free drag
		}
		
		//>> constrained

		target.style.left = newLeft.OrMinMax(0, MaxX);
		target.style.top = newTop.OrMinMax(0, MaxY);
		// Log($"{target.name} left {newLeft} / {MaxX}, top {newTop} {MaxY}".LgGold());
	}

	protected void OnMouseUp(MouseUpEvent evt)
	{
		var skipEvent = !IsDragging
		             || !target.HasMouseCapture()
		             || !CanStopManipulation(evt);

		if (skipEvent) return; //>> skip this event

		IsDragging = false;
		target.ReleaseMouse();
		evt.StopPropagation();
	}
}

public enum GluDragMethod
{
	NONE,
	FREE,
	CONSTRAINED,
}
}