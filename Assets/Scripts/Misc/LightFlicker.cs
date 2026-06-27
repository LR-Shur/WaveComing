using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    private Light _light;

    public float min = 1.2f;
    public float max = 2.0f;


    private void Awake()
    {
        _light = GetComponent<Light>();
    }


    private void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time, Time.time * 5.0f);
        _light.intensity = Mathf.Lerp(min, max, noise);
    }
}
