using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MemoryPlateEndAnimationHandler : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] List<AudioClip> audioClips = new List<AudioClip>();
    private bool hasGottenComponents = false;
    private bool hasPlayedEndAnimation = false;
    [SerializeField] MemoryPlateHandler[] memoryPlateHandler;
    private List<DisconnectedSoundOrbHandler> disconnectedSoundOrbHandlers = new List<DisconnectedSoundOrbHandler>();
    public List<Light2D> lights = new List<Light2D>();

    [Header("Glow Effect")]
    [SerializeField] private float maxIntensity;
    [SerializeField] private Light2D light2D;
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        foreach (var memoryPlate in memoryPlateHandler)
        {
            if (memoryPlate.isSnapped)
            {
                Light2D light = memoryPlate.snappedObject.GetComponentInChildren<Light2D>();
                if (light != null && !lights.Contains(light))
                {
                    lights.Add(light);
                }
            }
        }
        
        if(memoryPlateHandler.Length == MemoryPlateHandler.filledPlates.Count)
        {
            PlayEndAnimation();
        }
    }
    
    private void PlayEndAnimation()
    {
        if (hasPlayedEndAnimation)
        {
            return;
        }

        if (memoryPlateHandler != null)
        {
            if (!hasGottenComponents)
            {
                foreach (MemoryPlateHandler handler in memoryPlateHandler)
                {
                    foreach (GameObject filledPlate in MemoryPlateHandler.filledPlates)
                    {
                        DisconnectedSoundOrbHandler disconnectedSoundOrbHandler = filledPlate.GetComponent<DisconnectedSoundOrbHandler>();
                        disconnectedSoundOrbHandlers.Add(disconnectedSoundOrbHandler);
                    }
                }
                hasGottenComponents = true;
            }

            foreach (DisconnectedSoundOrbHandler disconnectedSoundOrbHandler in disconnectedSoundOrbHandlers)
            {
                if(!audioClips.Contains(disconnectedSoundOrbHandler.notConnectedAudio))
                {
                    audioClips.Add(disconnectedSoundOrbHandler.notConnectedAudio);
                }
            }
        }

        hasPlayedEndAnimation = true;
        
        StartCoroutine(PlayAudioClips());

    }
    
    private IEnumerator PlayAudioClips()
    {
        int arrayElement = 0;
        foreach (AudioClip clip in audioClips)
        {
            if (arrayElement < lights.Count)
            {
                Light2D light = lights[arrayElement];
                StartCoroutine(ChangeLightIntensity(light, 0.8f, maxIntensity, clip.length));

                audioSource.clip = clip;
                audioSource.Play();
                yield return new WaitForSeconds(clip.length);
                arrayElement++;
            }
        }

        memoryPlateHandler[0].SelectNextLevel();
    }
    
    private IEnumerator ChangeLightIntensity(Light2D light, float originalIntensity, float maxIntensity, float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration / 2)
        {
            light.intensity = Mathf.Lerp(originalIntensity, maxIntensity, elapsedTime / (duration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        while (elapsedTime < duration)
        {
            light.intensity = Mathf.Lerp(maxIntensity, originalIntensity, (elapsedTime - duration / 2) / (duration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        light.intensity = originalIntensity;
    }
}
