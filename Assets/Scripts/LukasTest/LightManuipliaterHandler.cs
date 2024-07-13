using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManuipliaterHandler : MonoBehaviour
{
    [Header("References")]
    private Light2D light2d;

    [Header("Flicker Settings")] 
    [SerializeField] private bool isFlickering;
    [SerializeField] private bool isRandomFlickering;
    [SerializeField] private float flickerSpeed;
    
    [Header("Random Flicker Settings")]
    [SerializeField] private float minRangeFlickering;
    [SerializeField] private float maxRangeFlickering;
    [SerializeField] private float minRangeIntensityFlicker;
    [SerializeField] private float maxRangeIntensityFlicker;
    
    private bool isFlickeringCoroutineRunning;
    
    private void Awake()
    {
        light2d = GetComponent<Light2D>();
    }

    void Update()
    {
        if (isFlickering && !isFlickeringCoroutineRunning)
        {
            StartCoroutine(FlickerLight());
        }
    }
    
    IEnumerator FlickerLight()
    {
        isFlickeringCoroutineRunning = true;

        float originalIntensity = light2d.intensity;
        float elapsedTime = 0f;

        if (isRandomFlickering)
        {
            float randomFlickerSpeed = Random.Range(minRangeFlickering, maxRangeFlickering);
            float randomIntensity = Random.Range(minRangeIntensityFlicker, maxRangeIntensityFlicker);

            originalIntensity = randomIntensity;
            flickerSpeed = randomFlickerSpeed;
        }
        
        while (elapsedTime < flickerSpeed)
        {
            float newIntensity = Mathf.Lerp(originalIntensity, 0f, elapsedTime / flickerSpeed);
            light2d.intensity = newIntensity;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        light2d.intensity = 0f;

        elapsedTime = 0f;

        while (elapsedTime < flickerSpeed)
        {
            float newIntensity = Mathf.Lerp(0f, originalIntensity, elapsedTime / flickerSpeed);
            light2d.intensity = newIntensity;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        light2d.intensity = originalIntensity;

        isFlickeringCoroutineRunning = false;
    }
}
