using System;
using JetBrains.Annotations;
using Regent.Barons;
using Regent.Staging;
using Regent.Workers;

namespace Foundational {
public abstract class  FutzBaron : Baron {
	/// new entities MUST be instantiated on this stage
	public const string CUE_SPAWN =			stage.SPAWN + nameof(CUE_SPAWN);
		
	public const string INIT_CLIP =			stage.UPDATE + nameof(INIT_CLIP);
	public const string TIME_TICK =			stage.UPDATE + nameof(TIME_TICK);
	public const string TIME_EVT =			stage.UPDATE + nameof(TIME_EVT);
	public const string VERIFICATION =		stage.UPDATE + nameof(VERIFICATION);
	// public const string CUE_SPAWN =			stage.UPDATE + nameof(CUE_SPAWN);
	public const string POST_SPAWN =		stage.UPDATE + nameof(POST_SPAWN);
	public const string PROPAGATION =		stage.UPDATE + nameof(PROPAGATION);
	public const string SOCKET_OUT =		stage.UPDATE + nameof(SOCKET_OUT);
	public const string SOCKET_IN =			stage.UPDATE + nameof(SOCKET_IN);
	public const string SYS_PK =			stage.UPDATE + nameof(SYS_PK);
	public const string POLL_INPUT = 		stage.UPDATE + nameof(POLL_INPUT);
	public const string INPUT = 			stage.UPDATE + nameof(INPUT);
	public const string ACTIVITY_CHANGE = 	stage.UPDATE + nameof(ACTIVITY_CHANGE);
	public const string ACTIVITY_PK = 		stage.UPDATE + nameof(ACTIVITY_PK);
	public const string UNSURE_WHAT_STAGE = stage.UPDATE + nameof(UNSURE_WHAT_STAGE);
	public const string PRE_FRAME_1 = 		stage.UPDATE + nameof(PRE_FRAME_1);
	public const string PRE_FRAME_2 = 		stage.UPDATE + nameof(PRE_FRAME_2);
	public const string FRAME = 			stage.UPDATE + nameof(FRAME);
	public const string POST_FRAME_1 = 		stage.UPDATE + nameof(POST_FRAME_1);
	public const string POST_FRAME_2 = 		stage.UPDATE + nameof(POST_FRAME_2);
	// for when unsure but need order
	public const string TEMP_1 =			stage.UPDATE + nameof(TEMP_1);
	public const string TEMP_2 =			stage.UPDATE + nameof(TEMP_2);
	public const string TEMP_3 =			stage.UPDATE + nameof(TEMP_3);
	public const string TEMP_4 =			stage.UPDATE + nameof(TEMP_4);
	public const string TEMP_5 =			stage.UPDATE + nameof(TEMP_5);


	public const string PRE_CAMERA =		stage.LATE + nameof(PRE_CAMERA);
	public const string CAMERA =			stage.LATE + nameof(CAMERA);
	public const string MOVE_ATTACH =		stage.LATE + nameof(MOVE_ATTACH);
	public const string WEASELS =			stage.LATE + nameof(WEASELS);
	public const string LATE_UNSURE =		stage.LATE + nameof(LATE_UNSURE);
		
	public const string PRE_UI =			stage.LATE + nameof(PRE_UI);
	public const string WORLD_UI =			stage.LATE + nameof(WORLD_UI);
	public const string UI_STACK =			stage.LATE + nameof(UI_STACK);
	public const string UI_WINDOW =			stage.LATE + nameof(UI_WINDOW);
	public const string UI_REFRESH =		stage.LATE + nameof(UI_REFRESH);
	public const string UI_UNSURE_STAGE =	stage.LATE + nameof(UI_UNSURE_STAGE);
	public const string UI_CLICK =			stage.LATE + nameof(UI_CLICK);
		
	public const string LAST =				stage.LATE + nameof(LAST);
	public const string CUE_DESPAWN =		stage.LATE + nameof(CUE_DESPAWN);
	public const string POST_DESPAWN =		stage.LATE + nameof(POST_DESPAWN);
	// public const string CUE_SPAWN =			stage.LATE + nameof(CUE_SPAWN);


	public const string PRE_PHYSICS =		stage.FIXED + nameof(PRE_PHYSICS);
	public const string PHYSICS_1 =			stage.FIXED + nameof(PHYSICS_1);
	public const string PHYSICS_2 =			stage.FIXED + nameof(PHYSICS_2);
	public const string PHYSICS_3 =			stage.FIXED + nameof(PHYSICS_3);
	public const string PHYSICS_4 =			stage.FIXED + nameof(PHYSICS_4);
	public const string PHYSICS_5 =			stage.FIXED + nameof(PHYSICS_5);
	public const string POST_PHYSICS =		stage.FIXED + nameof(POST_PHYSICS);


	public static float ClockTime;
	public static float ClockDelta;

	public static T N<T>(T obj) where T : UnityEngine.Object
	{
		return obj // uses Unity implicit conversion to bool
			? obj
			: null; // actual C# null ref
	}
}

/// alias for [Added.Native(INIT_CLIP)]
[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Method)]
public class ClipInitAttribute : BaseAddedAttribute {
	public ClipInitAttribute() : base(FutzBaron.INIT_CLIP) { }
	public override RegentDomain GetDomain() => RegentDomain.NATIVE;
}

// /// alias for Added.xxx(POST_SPAWN)
// public static class Enabled {
// 	public const string DEFAULT_ADDED_STAGE = FutzBaron.POST_SPAWN;
// 	
// 	
// 	[MeansImplicitUse]
// 	[AttributeUsage(AttributeTargets.Method)]
// 	public class NativeAttribute : BaseAddedAttribute {
// 		public NativeAttribute() : base(DEFAULT_ADDED_STAGE) { }
// 		public override RegentDomain GetDomain() => RegentDomain.NATIVE;
// 	}
//
// 	[MeansImplicitUse]
// 	[AttributeUsage(AttributeTargets.Method)]
// 	public class ServerAttribute : BaseAddedAttribute {
// 		public ServerAttribute() : base(DEFAULT_ADDED_STAGE) { }
// 		public override RegentDomain GetDomain() => RegentDomain.SERVER;
// 	}
//
// 	[MeansImplicitUse]
// 	[AttributeUsage(AttributeTargets.Method)]
// 	public class ClientAttribute : BaseAddedAttribute {
// 		public ClientAttribute() : base(DEFAULT_ADDED_STAGE) { }
// 		public override RegentDomain GetDomain() => RegentDomain.CLIENT;
// 	}
//
// 	[MeansImplicitUse]
// 	[AttributeUsage(AttributeTargets.Method)]
// 	public class AuthorAttribute : BaseAddedAttribute {
// 		public AuthorAttribute() : base(DEFAULT_ADDED_STAGE) { }
// 		public override RegentDomain GetDomain() => RegentDomain.AUTHOR;
// 	}
//
// 	[MeansImplicitUse]
// 	[AttributeUsage(AttributeTargets.Method)]
// 	public class RemoteAttribute : BaseAddedAttribute {
// 		public RemoteAttribute() : base(DEFAULT_ADDED_STAGE) { }
// 		public override RegentDomain GetDomain() => RegentDomain.REMOTE;
// 	}
// }

}