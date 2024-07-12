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
    private bool isFlickeringCoroutineRunning;
    [SerializeField] private float flickerSpeed;
    
    [Header("Other Various Settings")]
    [SerializeField] private float lightIntensity;
    [SerializeField] private float innerRadius;
    [SerializeField] private float outerRadius;
    
    private void Awake()
    {
        light2d = GetComponent<Light2D>();
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        light2d.intensity = lightIntensity;
        light2d.pointLightInnerRadius = innerRadius;
        light2d.pointLightOuterRadius = outerRadius;
        
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
