using Foundational;
using FutzSys;
// using InputSys;
using Lumberjack;
using Regent.Syncers;
using Regent.Workers;
using static UnityEngine.Debug;

namespace UiSys
{
public class CoreUiBaron : FutzBaron
{
	static CoreUiClip CoreUi_ => CoreUiClip.I;
	static GameSysClip GameSys_ => GameSysClip.I;
	// static InputClip Input_ => InputClip.I;


	[ClipInit]
	static void Init_CoreUi(CoreUiClip coreUi_)
	{
		coreUi_.ShowActivitySelect.SetInitial(EnumToggle.ON);
		coreUi_.ShowAgentList.SetInitial(EnumToggle.ON);
	}

	[React.Native(FRAME, nameof(GameSysClip.CurrentActivity))]
	static void React_CurrentActivity(GameSysClip gameSys_, ActivityBase activity)
	{
		CoreUi_.ShowAgentList.TriggerBool(activity.Def.ShowAgentList);
		CoreUi_.ShowScore.TriggerBool(activity.Def.ShowScore);
	}


	#region Input Events

	// Cursor.lockState = inCursorMode
	// 	? CursorLockMode.None
	// 	: CursorLockMode.Locked;

	// [Enabled.Native]
	// static void Init_Inputs(CoreUiClip coreUi_)
	// {
	// 	Input_.CORE.OptionsMenu.FnWentDown = () => coreUi_.OptionsMenu.Trigger();
	// 	Input_.CORE.ToggleHud.FnWentDown = () => coreUi_.ToggleHud.Trigger();
	// 	Input_.CORE.Cancel.FnWentDown = () => coreUi_.Cancel.Trigger();
	// 	Input_.DEBUG.Console.FnWentDown = () => coreUi_.Console.Trigger();
	// 	Input_.DEBUG.F1.FnWentDown = () => coreUi_.FKeys.Change(1);
	// 	Input_.DEBUG.F2.FnWentDown = () => coreUi_.FKeys.Change(2);
	// 	Input_.DEBUG.F3.FnWentDown = () => coreUi_.FKeys.Change(3);
	// 	Input_.DEBUG.F4.FnWentDown = () => coreUi_.FKeys.Change(4);
	// 	Input_.DEBUG.F5.FnWentDown = () => coreUi_.FKeys.Change(5);
	// 	Input_.DEBUG.F6.FnWentDown = () => coreUi_.FKeys.Change(6);
	// 	Input_.DEBUG.F7.FnWentDown = () => coreUi_.FKeys.Change(7);
	// 	Input_.DEBUG.F8.FnWentDown = () => coreUi_.FKeys.Change(8);
	// 	Input_.DEBUG.F9.FnWentDown = () => coreUi_.FKeys.Change(9);
	// 	Input_.DEBUG.F10.FnWentDown = () => coreUi_.FKeys.Change(10);
	// 	Input_.DEBUG.F11.FnWentDown = () => coreUi_.FKeys.Change(11);
	// 	Input_.DEBUG.F12.FnWentDown = () => coreUi_.FKeys.Change(12);
	// }

	[React.Native(INPUT, nameof(CoreUiClip.OptionsMenu))]
	static void React_OptionsMenu(CoreUiClip coreUi_) => Log($"OptionsMenu".LgTodo(), coreUi_);

	[React.Native(INPUT, nameof(CoreUiClip.ToggleHud))]
	static void React_ToggleHud(CoreUiClip coreUi_) => Log($"ToggleHud".LgTodo(), coreUi_);

	[React.Native(INPUT, nameof(CoreUiClip.Cancel))]
	static void React_Cancel(CoreUiClip coreUi_) => Log($"Cancel".LgTodo(), coreUi_);

	[React.Native(INPUT, nameof(CoreUiClip.Console))]
	static void React_Console(CoreUiClip coreUi_) => Log($"Console".LgTodo(), coreUi_);

	[React.Native(INPUT, nameof(CoreUiClip.FKeys))]
	static void React_FKeys(CoreUiClip coreUi_, int fKey) => Log($"F{fKey}".LgTodo(), coreUi_);

	#endregion
}
}