using Bobsled.Catalog;
using UnityEngine;

namespace Bobsled.Scene
{
public class BobsledSceneInitializer : MonoBehaviour
{
	public BobsledCatalogDef CatalogDef;

	void Awake()
	{
		BobsledCatalog.RuntimeInitialize(CatalogDef);
	}
}
}