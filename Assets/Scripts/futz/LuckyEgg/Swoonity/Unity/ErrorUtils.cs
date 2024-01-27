using System;
using System.Runtime.CompilerServices;
using Swoonity.Collections;
using uObject = UnityEngine.Object;
using static UnityEngine.Debug;

namespace Swoonity.Unity
{
public static class ErrorUtils { }

public class Err : Exception
{
	public Err(string message) : base(message) { }

	public Err(string message, uObject obj) : base(message)
	{
		LogError(message, obj);
	}

	// alloc fine when throwing
	public Err(string prefix, params string[] messages) : base(prefix + messages.Join())
	{
		LogError(prefix + messages.Join());
	}

	// alloc fine when throwing
	public Err(uObject obj, string prefix, params string[] messages) : base(
		prefix + messages.Join()
	)
	{
		LogError(prefix + messages.Join(), obj);
	}
}

/// *should* never happen
public class ErrAbsurd : Err
{
	public ErrAbsurd(string message)
		: base($"Inconceivable! {message}") { }

	public ErrAbsurd(string message, uObject obj)
		: base($"Inconceivable! {message}", obj) { }
}

/// *should* never happen
public class ErrMiss : Err
{
	public ErrMiss(string name)
		: base($"Missing: {name}") { }

	public ErrMiss(string name, uObject obj)
		: base($"Missing: {name}", obj) { }
}

public class ErrTodo : Err
{
	public ErrTodo(string message = "", [CallerMemberName] string name = "")
		: base($"TODO: {name} {message}") { }
}
}