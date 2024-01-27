using System;
using Regent.Barons;
using Regent.Catalog;
using Swoonity.Unity;
using UnityEngine;

namespace Regent.Core
{
public class AutoBaronCreator : MonoBehaviour
{
	public RegentCatalogDef Catalog;

	void Start()
	{
		var root = gameObject;

		foreach (var baronFact in Catalog.BaronFacts) {
			var baronGobj = root.NewChild(baronFact.Name);
			var baronType = Type.GetType(baronFact.TypeName);

			var baron = (Baron)baronGobj.AddComponent(baronType);
		}
	}
}
}