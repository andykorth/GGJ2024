using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickeryTorchLight : MonoBehaviour
{
    public Light2D lightToChange;

    public float minIntensity = 0.6f;
    public float intensityVariance = 0.3f;
    public float noiseSpeed = 3f;

    // Update is called once per frame
    void Update()
    {
        float noise = Mathf.PerlinNoise(0.3f, Time.time * noiseSpeed) * intensityVariance * 0.5f;
        noise += Mathf.PerlinNoise(0.35f, Time.time * noiseSpeed * 2f) * intensityVariance * 0.5f;

        lightToChange.intensity = noise + minIntensity;        
    }
}
