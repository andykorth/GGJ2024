using Swoonity.CSharp;
using Swoonity.MHasher;
using UnityEngine;

namespace Bobsled.Defs
{
public abstract class Definition : ScriptableObject, IMHashable
{
	public string Name;

	public void LoadHash() => Hash = MHash.Hash(name, GetType());

	public MHash Hash;
	public MHash GetHash() => Hash;

	void OnValidate()
	{
		// if (Name == string.Empty) Name = name.SplitAfterFirst("~").GetStringBefore("~");
		if (Name == string.Empty) Name = name;
		LoadHash();
		WhenEditorValidates();
	}


	/// Called when changed in editor. Can be used for caching calculations.
	protected virtual void WhenEditorValidates() { }


	public override string ToString() => name;
}
}


/*

	Definition -> defDef
	Collection -> defCollection
	Library - holds Collections


	## Bobsled (eventually)
	Import: button in custom editor window
		- Pulls from airtable
		- Find existing Def of same type + name
			- Writes data
		- If no existing def found
			- Log message (telling us to create def)
			- *eventually creates def
		- Refreshes library

	### AirTable
	Table description can hold Bobsled instructions
		- Pulls everything in table
		- but only writes what is in Def
	
	### Type Converters	
	Bobsled has Type converters (similar to de/serializer)
		- Based on the Type of the variable on the definition
		- Def class can override with var name
		- x: def has Image var, takes string, converts to asset ref
		- x: def has Image var, but takes enum, converts to 1 of 3 icons
		
		AddDefiner<TDef, TDb>(string dbColumnName, Action<TDef, TDb> definer);
		
		
		
		
	### TODO
	See: _NOTES_
*/