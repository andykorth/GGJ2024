using Foundational;
using JetBrains.Annotations;
using Lumberjack;
using Regent.Clips;
using Regent.Coordinator;
using UiSys;
using UnityEngine;
using static UnityEngine.Debug;

namespace Wabi.Interop
{
// for WabiJS
	[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
	[CreateAssetMenu(menuName = "_DEF/Rare/" + nameof(FutzInterop))]
	public class FutzInterop : ScriptableObject
	{
		public GameSysClip GameSys_ => Get<GameSysClip>();
		public CoreUiClip CoreUi_ => Get<CoreUiClip>();

		// TEMP
		static TClip Get<TClip>() where TClip : MonoBehaviour, IClip
		{
			// Log($"Get {typeof(TClip).Name}".LgBlue());
			return RegentCoordinator.__Coordinator.Clipper.Get<TClip>();
		}
	}
}