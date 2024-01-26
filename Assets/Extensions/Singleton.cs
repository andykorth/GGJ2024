using UnityEngine;

/// <summary>
/// A class that inherits from MonoBehaviour and provides the basic members and methods to fulfill a singleton pattern.
/// </summary>
/// <typeparam name="T">The type of the instance.</typeparam>
public class Singleton <T> : MonoBehaviour where T:Object  {

	protected static T _instance;

	/// <summary>
	/// Return an instance of the singleton.
	/// (Some people might prefer this longer name.)
	/// </summary>
	public static T instance {
		get {
			 return i;
		}
	}

	/// <summary>
	/// Return an instance of the singleton.
	/// </summary>
	public static T i {
		get {
			if (_instance == null) {
				throw new System.Exception ("Missing instance on singleton type: " + typeof (T).FullName );
			}
			return _instance;
		}
	}

	/// <summary>
	/// Unity event that assigns the singleton instance.
	/// </summary>
	protected void Awake(){
		_instance = this as T;
	}

	/// <summary>
	/// Check if an instance of the singleton exists.
	/// </summary>
	/// <returns>True if an instance exists.  False if it does not.</returns>
	public static bool Exists(){
		return _instance != null;
	}
}
