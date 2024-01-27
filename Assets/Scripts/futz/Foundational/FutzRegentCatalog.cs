using System;
using Regent.Catalog;
using UnityEngine;

namespace Foundational {
	[CreateAssetMenu(fileName = "catalog", menuName = "_DEF/Rare/Futz Regent Catalog")]
	public class FutzRegentCatalog : RegentCatalogDef {

		public override Type GetStageSourceType() => typeof(FutzBaron);

	}
}