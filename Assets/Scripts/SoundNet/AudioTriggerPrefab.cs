using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTriggerPrefab : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject nextTriggerToActivate;
    private AudioSource audioSource;
    private Collider2D[] colliders2D;

    [Header("SquareSettings")] 
    [SerializeField] private Vector3 squareDimensions;
    
    
    [Header("Audio Clips")]
    [SerializeField] private AudioClip audioClip;
    
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
            if (collider.gameObject.CompareTag("SoundOrbDisconnected"))
            {
                MemoryLayers memoryLayer = collider.gameObject.GetComponent<DisconnectedSoundOrbHandler>().MemoryLayer;
                bool hovering = collider.gameObject.GetComponent<DisconnectedSoundOrbHandler>().isHovering;
                
                if (audioSource != null && !audioSource.isPlaying && memoryLayer == MemoryLayers.C && hovering)
                {
                    audioSource.clip = audioClip;
                    audioSource.Play();
                    //Add code to activate next trigger here
                    if (nextTriggerToActivate != null)
                    {
                        nextTriggerToActivate.SetActive(true);
                    }
                    this.enabled = false;
                }
                break;
            }
            
            if (collider.gameObject.CompareTag("SoundOrbConnected"))
            {
                Debug.Log("SoundOrbConnected");
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, squareDimensions);
    }

}
