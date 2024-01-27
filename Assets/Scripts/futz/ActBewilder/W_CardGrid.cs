using System;
using Glui;

namespace ActBewilder
{
public class W_CardGrid : GluWindow
{
	public C_Card.Pool Pool = new();
}

[Serializable]
public class C_Card : GluCEll
{
	public LabEl L_Word = new();


	protected override bool CallLoad()
	{
		return Load(L_Word);
	}

	[Serializable]
	public class Pool : GluCEllPool<C_Card> { }
}
}