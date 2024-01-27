using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Regent.Staging;
using Regent.WorkerFacts;
using Swoonity.Collections;
using Swoonity.CSharp;

namespace Regent.Workers
{
public abstract class BaseWorkerAttribute : Attribute
{
	public readonly string StagePh;

	public BaseWorkerAttribute(string stage) => StagePh = stage;

	public virtual string GetStagePlaceholder() => StagePh;
	public virtual RegentDomain GetDomain() => RegentDomain.UNSET;
	public virtual WorkerTrigger GetTrigger() => WorkerTrigger.UNSET;
	public virtual WorkerMemberType GetMemberType() => WorkerMemberType.UNSET;
	public virtual string GetSyncer() => "";
	public virtual float GetTimedSeconds() => 0;
	public abstract Type[] GetTypes(MemberInfo info);

	public virtual void ThrowIfMisconfigured(WorkerFact fact, Type[] requiredTypes) { }


	public static void ThrowWorkerMisconfigIf(
		WorkerFact fact,
		bool check,
		params string[] strings
	)
	{
		if (check) {
			throw new Exception(
				$"Worker {fact.WorkerTrigger} {fact.ParentTypeName}.{fact.Name}: {strings.Join(" | ")}"
			);
		}
	}

	public static void ThrowIfNonUnityComponent(WorkerFact fact, params Type[] requiredTypes)
		=> ThrowWorkerMisconfigIf(
			fact,
			requiredTypes.AnyIsNotSubclass<UnityEngine.Component>(),
			$"args must inherit from UnityEngine.Component  |  {requiredTypes.Join()}"
		);

	public static void ThrowIfBadTypesLength(
		WorkerFact fact,
		int min,
		int max,
		params Type[] requiredTypes
	)
		=> ThrowWorkerMisconfigIf(
			fact,
			!requiredTypes.Length.IsWithin(min, max),
			min == max
				? $"need {min} args, had {requiredTypes.Length}"
				: $"need {min}-{max} args, had {requiredTypes.Length}"
		);
}

public abstract class BaseMethodWorkerAttribute : BaseWorkerAttribute
{
	public BaseMethodWorkerAttribute(string stage) : base(stage) { }

	public override Type[] GetTypes(MemberInfo info) => ((MethodInfo)info).GetParameterTypes();
	public override WorkerMemberType GetMemberType() => WorkerMemberType.METHOD;
}

#region Registry

[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Field)]
public class RegistryAttribute : BaseWorkerAttribute
{
	public override RegentDomain GetDomain() => RegentDomain.NATIVE;
	public override WorkerTrigger GetTrigger() => WorkerTrigger.REGISTRY;
	public override WorkerMemberType GetMemberType() => WorkerMemberType.FIELD;
	public RegistryAttribute() : base(StageCreation.StageMarker.__SKIP_STAGE__) { }

	public override Type[] GetTypes(MemberInfo info)
		=> ((FieldInfo)info).FieldType.GenericTypeArguments;

	public override void ThrowIfMisconfigured(WorkerFact fact, Type[] requiredTypes)
	{
		ThrowIfBadTypesLength(fact, 1, 1, requiredTypes);
		ThrowIfNonUnityComponent(fact, requiredTypes);
	}
}

#endregion

#region Added

[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Method)]
public class BaseAddedAttribute : BaseMethodWorkerAttribute
{
	public override WorkerTrigger GetTrigger() => WorkerTrigger.ADDED;
	public BaseAddedAttribute(string stage) : base(stage) { }

	public override void ThrowIfMisconfigured(WorkerFact fact, Type[] requiredTypes)
	{
		ThrowIfBadTypesLength(fact, 1, 5, requiredTypes);
		ThrowIfNonUnityComponent(fact, requiredTypes);
	}
}

// TODO: avoid this? seems to break when object is quickly destroyed after being added
public static class Added
{
	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class NativeAttribute : BaseAddedAttribute
	{
		public NativeAttribute(string stage) : base(stage) { }
		public override RegentDomain GetDomain() => RegentDomain.NATIVE;
	}

	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class ServerAttribute : BaseAddedAttribute
	{
		public ServerAttribute(string stage) : base(stage) { }
		public override RegentDomain GetDomain() => RegentDomain.SERVER;
	}

	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class ClientAttribute : BaseAddedAttribute
	{
		public ClientAttribute(string stage) : base(stage) { }
		public override RegentDomain GetDomain() => RegentDomain.CLIENT;
	}

	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class AuthorAttribute : BaseAddedAttribute
	{
		public AuthorAttribute(string stage) : base(stage) { }
		public override RegentDomain GetDomain() => RegentDomain.AUTHOR;
	}

	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class RemoteAttribute : BaseAddedAttribute
	{
		public RemoteAttribute(string stage) : base(stage) { }
		public override RegentDomain GetDomain() => RegentDomain.REMOTE;
	}
}

#endregion

#region Removed

/// Different signature than Added. Will NOT check for Authority. TODO: make this better
[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Method)]
public class BaseRemovedAttribute : BaseMethodWorkerAttribute
{
	public readonly Type[] Types;
	public override Type[] GetTypes(MemberInfo info) => Types;
	public override WorkerTrigger GetTrigger() => WorkerTrigger.REMOVED;

	public BaseRemovedAttribute(string stage, Type type, params Type[] types) : base(stage)
	{
		Types = new[] { type }.Concat(types).ToArray();
	}
}

public static class Removed
{
	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class NativeAttribute : BaseRemovedAttribute
	{
		public NativeAttribute(string stage, Type type, params Type[] types) : base(
			stage,
			type,
			types
		) { }

		public override RegentDomain GetDomain() => RegentDomain.NATIVE;
	}

	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class ServerAttribute : BaseRemovedAttribute
	{
		public ServerAttribute(string stage, Type type, params Type[] types) : base(
			stage,
			type,
			types
		) { }

		public override RegentDomain GetDomain() => RegentDomain.SERVER;
	}

	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class ClientAttribute : BaseRemovedAttribute
	{
		public ClientAttribute(string stage, Type type, params Type[] types) : base(
			stage,
			type,
			types
		) { }

		public override RegentDomain GetDomain() => RegentDomain.CLIENT;
	}

	// public class AuthorAttribute : BaseRemovedAttribute {
	// public class RemoteAttribute : BaseRemovedAttribute {
}

#endregion

#region Enabled  (runs immediately, NOT staged)

[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Method)]
public class BaseEnabledAttribute : BaseMethodWorkerAttribute
{
	public override WorkerTrigger GetTrigger() => WorkerTrigger.ENABLED;
	public BaseEnabledAttribute() : base(StageCreation.StageMarker.__SKIP_STAGE__) { }

	public override void ThrowIfMisconfigured(WorkerFact fact, Type[] requiredTypes)
	{
		ThrowIfBadTypesLength(fact, 1, 5, requiredTypes);
		ThrowIfNonUnityComponent(fact, requiredTypes);
	}
}

///<summary>runs immediately (NOT staged)</summary>
public static class Enabled
{
	///	<inheritdoc cref="Enabled"/>
	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class NativeAttribute : BaseEnabledAttribute
	{
		public override RegentDomain GetDomain() => RegentDomain.NATIVE;
	}

	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class ServerAttribute : BaseEnabledAttribute
	{
		public override RegentDomain GetDomain() => RegentDomain.SERVER;
	}

	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class ClientAttribute : BaseEnabledAttribute
	{
		public override RegentDomain GetDomain() => RegentDomain.CLIENT;
	}

	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class AuthorAttribute : BaseEnabledAttribute
	{
		public override RegentDomain GetDomain() => RegentDomain.AUTHOR;
	}

	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class RemoteAttribute : BaseEnabledAttribute
	{
		public override RegentDomain GetDomain() => RegentDomain.REMOTE;
	}
}

#endregion

#region Disabled  (runs immediately, NOT staged)

[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Method)]
public class BaseDisabledAttribute : BaseMethodWorkerAttribute
{
	public override WorkerTrigger GetTrigger() => WorkerTrigger.DISABLED;
	public BaseDisabledAttribute() : base(StageCreation.StageMarker.__SKIP_STAGE__) { }

	public override void ThrowIfMisconfigured(WorkerFact fact, Type[] requiredTypes)
	{
		ThrowIfBadTypesLength(fact, 1, 5, requiredTypes);
		ThrowIfNonUnityComponent(fact, requiredTypes);
	}
}

///<summary>runs immediately (NOT staged)</summary>
/// NOTE: if using clip or static ref, must null check
public static class Disabled
{
	///	<inheritdoc cref="Disabled"/>
	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class NativeAttribute : BaseDisabledAttribute
	{
		public override RegentDomain GetDomain() => RegentDomain.NATIVE;
	}

	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class ServerAttribute : BaseDisabledAttribute
	{
		public override RegentDomain GetDomain() => RegentDomain.SERVER;
	}

	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class ClientAttribute : BaseDisabledAttribute
	{
		public override RegentDomain GetDomain() => RegentDomain.CLIENT;
	}

	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class AuthorAttribute : BaseDisabledAttribute
	{
		public override RegentDomain GetDomain() => RegentDomain.AUTHOR;
	}

	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class RemoteAttribute : BaseDisabledAttribute
	{
		public override RegentDomain GetDomain() => RegentDomain.REMOTE;
	}
}

#endregion

#region Run

[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Method)]
public class BaseRunAttribute : BaseMethodWorkerAttribute
{
	public override WorkerTrigger GetTrigger() => WorkerTrigger.RUN;
	public BaseRunAttribute(string stage) : base(stage) { }

	public override void ThrowIfMisconfigured(WorkerFact fact, Type[] requiredTypes)
	{
		ThrowIfBadTypesLength(fact, 1, 5, requiredTypes);
		ThrowIfNonUnityComponent(fact, requiredTypes);
	}
}

public static class Run
{
	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class NativeAttribute : BaseRunAttribute
	{
		public NativeAttribute(string stage) : base(stage) { }
		public override RegentDomain GetDomain() => RegentDomain.NATIVE;
	}

	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class ServerAttribute : BaseRunAttribute
	{
		public ServerAttribute(string stage) : base(stage) { }
		public override RegentDomain GetDomain() => RegentDomain.SERVER;
	}

	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class ClientAttribute : BaseRunAttribute
	{
		public ClientAttribute(string stage) : base(stage) { }
		public override RegentDomain GetDomain() => RegentDomain.CLIENT;
	}

	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class AuthorAttribute : BaseRunAttribute
	{
		public AuthorAttribute(string stage) : base(stage) { }
		public override RegentDomain GetDomain() => RegentDomain.AUTHOR;
	}

	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class RemoteAttribute : BaseRunAttribute
	{
		public RemoteAttribute(string stage) : base(stage) { }
		public override RegentDomain GetDomain() => RegentDomain.REMOTE;
	}
}

#endregion

#region Timed

[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Method)]
public class BaseTimedAttribute : BaseMethodWorkerAttribute
{
	public float Seconds;

	public override WorkerTrigger GetTrigger() => WorkerTrigger.TIMED;

	public BaseTimedAttribute(string stage, float seconds) : base(stage)
	{
		Seconds = seconds;
	}

	public override float GetTimedSeconds() => Seconds;

	public override void ThrowIfMisconfigured(WorkerFact fact, Type[] requiredTypes)
	{
		ThrowIfBadTypesLength(fact, 1, 5, requiredTypes);
		ThrowIfNonUnityComponent(fact, requiredTypes);
	}
}

public static class Timed
{
	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class NativeAttribute : BaseTimedAttribute
	{
		public NativeAttribute(string stage, float seconds) : base(stage, seconds) { }
		public override RegentDomain GetDomain() => RegentDomain.NATIVE;
	}

	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class ServerAttribute : BaseTimedAttribute
	{
		public ServerAttribute(string stage, float seconds) : base(stage, seconds) { }
		public override RegentDomain GetDomain() => RegentDomain.SERVER;
	}

	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class ClientAttribute : BaseTimedAttribute
	{
		public ClientAttribute(string stage, float seconds) : base(stage, seconds) { }
		public override RegentDomain GetDomain() => RegentDomain.CLIENT;
	}

	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class AuthorAttribute : BaseTimedAttribute
	{
		public AuthorAttribute(string stage, float seconds) : base(stage, seconds) { }
		public override RegentDomain GetDomain() => RegentDomain.AUTHOR;
	}

	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class RemoteAttribute : BaseTimedAttribute
	{
		public RemoteAttribute(string stage, float seconds) : base(stage, seconds) { }
		public override RegentDomain GetDomain() => RegentDomain.REMOTE;
	}
}

#endregion

#region Verify

[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Method)]
public class BaseVerifyAttribute : BaseMethodWorkerAttribute
{
	public readonly string Syncer;
	public override string GetSyncer() => Syncer;
	public override WorkerTrigger GetTrigger() => WorkerTrigger.VERIFY;

	public BaseVerifyAttribute(string stage, string syncer) : base(stage)
	{
		Syncer = syncer;
	}

	public override void ThrowIfMisconfigured(WorkerFact fact, Type[] requiredTypes)
	{
		ThrowIfBadTypesLength(fact, 2, 3, requiredTypes);
		ThrowIfNonUnityComponent(fact, requiredTypes[0]);
	}
}

/// <code>static sbyte Verify_Name(Cog syncerCog, Val val, Entity optionalSource)</code>
public static class Verify
{
	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class ServerAttribute : BaseVerifyAttribute
	{
		public ServerAttribute(string stage, string syncer) : base(stage, syncer) { }
		public override RegentDomain GetDomain() => RegentDomain.SERVER;
	}
}

#endregion

#region React

[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Method)]
public class BaseReactAttribute : BaseMethodWorkerAttribute
{
	public readonly string Syncer;
	public override string GetSyncer() => Syncer;

	public override WorkerTrigger GetTrigger() => WorkerTrigger.REACT;

	public BaseReactAttribute(string stage, string syncer) : base(stage)
	{
		Syncer = syncer;
	}

	public override void ThrowIfMisconfigured(WorkerFact fact, Type[] requiredTypes)
	{
		ThrowIfBadTypesLength(fact, 1, 5, requiredTypes);
		ThrowIfNonUnityComponent(fact, requiredTypes[0]);
		if (requiredTypes.Length >= 3) ThrowIfNonUnityComponent(fact, requiredTypes[2]);
		if (requiredTypes.Length >= 4) ThrowIfNonUnityComponent(fact, requiredTypes[3]);
		if (requiredTypes.Length >= 5) ThrowIfNonUnityComponent(fact, requiredTypes[4]);
	}
}

/// <summary>React.NativeEtc(STAGE, nameof(Cog.Syncer))</summary>
/// <list type="bullet">
/// 	<listheader><b>Signatures</b></listheader>
/// <item>(Cog)</item>
/// <item>(Cog, newValue)</item>
/// <item>(Cog, newValue, get`T1`)</item>
/// <item>(Cog, newValue, get`T1`, get`T2`)</item>
/// <item>(Cog, newValue, get`T1`, get`T2`, get`T3`)</item>
/// </list>
public static class React
{
	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class NativeAttribute : BaseReactAttribute
	{
		public NativeAttribute(string stage, string syncer) : base(stage, syncer) { }
		public override RegentDomain GetDomain() => RegentDomain.NATIVE;
	}

	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class ServerAttribute : BaseReactAttribute
	{
		public ServerAttribute(string stage, string syncer) : base(stage, syncer) { }
		public override RegentDomain GetDomain() => RegentDomain.SERVER;
	}

	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class ClientAttribute : BaseReactAttribute
	{
		public ClientAttribute(string stage, string syncer) : base(stage, syncer) { }
		public override RegentDomain GetDomain() => RegentDomain.CLIENT;
	}

	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class AuthorAttribute : BaseReactAttribute
	{
		public AuthorAttribute(string stage, string syncer) : base(stage, syncer) { }
		public override RegentDomain GetDomain() => RegentDomain.AUTHOR;
	}

	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Method)]
	public class RemoteAttribute : BaseReactAttribute
	{
		public RemoteAttribute(string stage, string syncer) : base(stage, syncer) { }
		public override RegentDomain GetDomain() => RegentDomain.REMOTE;
	}
}

#endregion
}