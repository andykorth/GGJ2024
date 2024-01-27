using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Swoonity.Unity
{
public static class RendererUtils
{
	/// <summary>
	/// Set Emission Color for all materials on a renderer.
	/// </summary>
	public static void SetEmissionColor(this Renderer renderer, Color color)
	{
		for (int index = 0; index < renderer.materials.Length; index++) {
			renderer.materials[index].SetColor("_EmissionColor", color);
		}
	}

	/// <summary>
	/// Set Emission Color for all materials on an array of renderers.
	/// </summary>
	public static void SetEmissionColor(this Renderer[] renderers, Color color)
	{
		for (int index = 0; index < renderers.Length; index++) {
			renderers[index].SetEmissionColor(color);
		}
	}

	/// <summary>
	/// Set shared materials for an array of renderers.
	/// </summary>
	public static void SetSharedMaterials(this Renderer[] renderers, Material[] sharedMaterials)
	{
		for (int index = 0; index < renderers.Length; index++) {
			renderers[index].sharedMaterials = sharedMaterials;
		}
	}

	/// <summary>
	/// Enables EMISSION keyword on a renderer's shared materials
	/// </summary>
	public static void EnableEmission(this Renderer renderer)
	{
		for (int index = 0; index < renderer.sharedMaterials.Length; index++) {
			renderer.sharedMaterials[index].EnableKeyword("_EMISSION");
		}
	}

	public static void SetProbeProxy(this Renderer renderer, LightProbeProxyVolume proxy)
	{
		renderer.lightProbeUsage = LightProbeUsage.UseProxyVolume;
		renderer.lightProbeProxyVolumeOverride = proxy.gameObject;
	}

	public static void SetProbeProxy<T>(this List<T> list, LightProbeProxyVolume proxy)
		where T : Renderer
	{
		var proxyGobj = proxy.gameObject;
		foreach (var renderer in list) {
			renderer.lightProbeUsage = LightProbeUsage.UseProxyVolume;
			renderer.lightProbeProxyVolumeOverride = proxyGobj;
		}
	}

	public static Bounds GetBounds(this List<MeshRenderer> renderers)
	{
		if (renderers == null) return default;

		var numOfColliders = renderers.Count;
		if (numOfColliders == 1) return renderers[0].bounds;

		var startingBounds = renderers[0].bounds;
		var bounds = new Bounds(startingBounds.center, startingBounds.size);

		for (var dex = 1; dex < numOfColliders; dex++) {
			var coll = renderers[dex];
			bounds.Encapsulate(coll.bounds);
		}

		return bounds;
	}
}
}