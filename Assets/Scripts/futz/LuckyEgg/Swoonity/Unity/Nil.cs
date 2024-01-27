using UnityEngine;
using uObj = UnityEngine.Object;

namespace Swoonity.Unity
{
/// TODO: in progress
/// <code>
/// Unity info
///  
///  var gob = new GameObject();   (or any UnityEngine.Object)
///  Destroy(gob);
///    - this kills the native C++ object
///    - but NOT the C# object wrapper
///    - the wrapper object still exists and is not actually null!
///    - using this as a "lifetime check" can cause issues
///    - (specifically in the editor)
///  
///  FALSE:  object.ReferenceEquals(gob, null)
///  TRUE:   gob == null    unity overrides == operator
///  TRUE:   gob == false   unity implicit conversion to bool
///  TRUE:   if (!gob)      ^
///   
///  ?. null-conditional operator
///  gob?.name    might throw MissingReferenceException
///   
/// Enter NIL
///   
///  nil == destroyedUnityObject
///  nil == null
/// </code> 
public static class Nil
{
	/// <seealso cref="Nil"/>
	public static bool Any(uObj o1) => !o1;

	/// <seealso cref="Nil"/>
	public static bool Any(uObj o1, uObj o2) => !o1 || !o2;

	/// <seealso cref="Nil"/>
	public static bool Any(uObj o1, uObj o2, uObj o3) => !o1 || !o2 || !o3;

	/// <seealso cref="Nil"/>
	public static bool Any(uObj o1, uObj o2, uObj o3, uObj o4) => !o1 || !o2 || !o3 || !o4;

	/// <seealso cref="Nil"/>
	public static bool Any(uObj o1, uObj o2, uObj o3, uObj o4, uObj o5)
		=> !o1 || !o2 || !o3 || !o4 || !o5;

	/// <seealso cref="Nil"/>
	public static bool Any(uObj o1, uObj o2, uObj o3, uObj o4, uObj o5, uObj o6)
		=> !o1 || !o2 || !o3 || !o4 || !o5 || !o6;

	/// <seealso cref="Nil"/>
	public static bool Any(uObj o1, uObj o2, uObj o3, uObj o4, uObj o5, uObj o6, uObj o7)
		=> !o1 || !o2 || !o3 || !o4 || !o5 || !o6 || !o7;


	/// <seealso cref="Nil"/>
	public static bool None(uObj o1) => o1;

	/// <seealso cref="Nil"/>
	public static bool None(uObj o1, uObj o2) => o1 && o2;

	/// <seealso cref="Nil"/>
	public static bool None(uObj o1, uObj o2, uObj o3) => o1 && o2 && o3;

	/// <seealso cref="Nil"/>
	public static bool None(uObj o1, uObj o2, uObj o3, uObj o4) => o1 && o2 && o3 && o4;

	/// <seealso cref="Nil"/>
	public static bool None(uObj o1, uObj o2, uObj o3, uObj o4, uObj o5)
		=> o1 && o2 && o3 && o4 && o5;

	/// <seealso cref="Nil"/>
	public static bool None(uObj o1, uObj o2, uObj o3, uObj o4, uObj o5, uObj o6)
		=> o1 && o2 && o3 && o4 && o5 && o6;

	/// <seealso cref="Nil"/>
	public static bool None(uObj o1, uObj o2, uObj o3, uObj o4, uObj o5, uObj o6, uObj o7)
		=> o1 && o2 && o3 && o4 && o5 && o6 && o7;


	/// Returns C# null ref if Unity considers obj to be null (Unity overrides '==' operator).
	/// Use this before a null-conditional operator ?. or null-coalescing operator '??'
	/// <seealso cref="Nil"/>
	public static T _<T>(this T obj) where T : UnityEngine.Object
	{
		return obj // uses Unity implicit conversion to bool
			? obj
			: null; // actual C# null ref
	}
}


/// TODO: test!
/// <seealso cref="Nil"/>
public abstract class NilBase
{
	[HideInInspector] public bool WasCreated;

	// public NilBase(bool DO_NOT_USE_NEW_USE_CREATE_INSTEAD) {}

	public static T NilCreate<T>() where T : NilBase, new()
	{
		var nil = new T();
		nil.WasCreated = true;
		return nil;
	}

	public static implicit operator bool(NilBase nil) => nil is { WasCreated: true };
	// !object.ReferenceEquals(nil, null) && nil.WasCreated;
}

public class Test1 : NilBase
{
	public static Test1 Create() => NilCreate<Test1>();
}
}