using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AudioTriggerPrefab : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject nextTriggerToActivate;
    [SerializeField] private bool connectedDisconnetedTrigger;
    public Light2D light2d;
    private AudioSource audioSource;
    private Collider2D[] colliders2D;

    [Header("SquareSettings")] 
    [SerializeField] private Vector3 squareDimensions;
    
    [Header("Audio Clips")]
    [SerializeField] private AudioClip audioClipBothOrbs;
    [SerializeField] private AudioClip audioClipConnectedOrb;
    
    [Header("Light settings")]
    [SerializeField] private float maxIntensity;
    [SerializeField] private float duration;
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    
    }

    private void Update()
    {
        Square();
    }

    private void Square()
    {
        Vector3 center = transform.position;
        Quaternion orientation = Quaternion.identity;

        colliders2D = Physics2D.OverlapBoxAll(center, squareDimensions, orientation.z);

        foreach (Collider2D collider in colliders2D)
        {                    

            
            // if (connectedDisconnetedTrigger)
            // {
                Debug.Log("Connected or disconnected trigger");
                if (collider.gameObject.CompareTag("SoundOrbDisconnected") || collider.gameObject.CompareTag("SoundOrbConnected"))
                {
                    MemoryLayers memoryLayer;
                    bool followMouse;

                    if (collider.gameObject.CompareTag("SoundOrbDisconnected"))
                    {
                        var handler = collider.gameObject.GetComponent<DisconnectedSoundOrbHandler>();
                        memoryLayer = handler.MemoryLayer;
                        followMouse = handler.followMouse;
                    }
                    else //SoundOrbConnected
                    {
                        var handler = collider.gameObject.GetComponent<ConnectedSoundOrbHandler>();
                        memoryLayer = handler.MemoryLayer;
                        followMouse = handler.followMouse;
                    }
                
                    if (audioSource != null && !audioSource.isPlaying && memoryLayer == MemoryLayers.C && followMouse)
                    {
                        audioSource.clip = audioClipBothOrbs;
                        audioSource.Play();
                        
                        if (nextTriggerToActivate != null)
                        {
                            nextTriggerToActivate.SetActive(true);
                        }
                        // this.enabled = false;
                        StartCoroutine(DisableScript(duration));
                    }
                    break;
                }
            // }

            if (collider.gameObject.CompareTag("SoundOrbConnected"))
            {
                MemoryLayers memoryLayer;
                bool followMouse;
                
                ConnectedSoundOrbHandler handler = collider.gameObject.GetComponent<ConnectedSoundOrbHandler>();
                light2d = collider.gameObject.transform.GetChild(0).GetComponent<Light2D>();
                memoryLayer = handler.MemoryLayer;
                followMouse = handler.followMouse;
                
                if (audioSource != null && !audioSource.isPlaying && memoryLayer == MemoryLayers.C && followMouse)
                {
                    StartCoroutine(ChangeLightIntensity(light2d, light2d.intensity, maxIntensity, duration));
                    audioSource.clip = audioClipConnectedOrb;
                    audioSource.Play();
                    if (nextTriggerToActivate != null)
                    {
                        nextTriggerToActivate.SetActive(true);
                    }
                }
            }
            
        }
    }
    
    private IEnumerator ChangeLightIntensity(Light2D light, float originalIntensity, float maxIntensity, float duration)
    {
        StartCoroutine(DisableScript(duration));
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
    
    private IEnumerator DisableScript(float time)
    {
        yield return new WaitForSeconds(time);
        this.enabled = false;

    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, squareDimensions);
    }

}
