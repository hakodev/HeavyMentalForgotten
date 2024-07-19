using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPlateEndAnimationHandler : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] List<AudioClip> audioClips = new List<AudioClip>();
    private bool hasGottenComponents = false;
    private bool hasPlayedEndAnimation = false;
    [SerializeField] MemoryPlateHandler[] memoryPlateHandler;
    private List<DisconnectedSoundOrbHandler> disconnectedSoundOrbHandlers = new List<DisconnectedSoundOrbHandler>();


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
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
        foreach (AudioClip clip in audioClips)
        {
            audioSource.clip = clip;
            audioSource.Play();
            yield return new WaitForSeconds(clip.length);
        }

        memoryPlateHandler[0].SelectNextLevel();
    }
}
