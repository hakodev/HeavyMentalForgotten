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
            if (collider.gameObject.CompareTag("SoundOrbDisconnected") || collider.gameObject.CompareTag("SoundOrbConnected"))
            {
                MemoryLayers memoryLayer;
                bool hovering;

                if (collider.gameObject.CompareTag("SoundOrbDisconnected"))
                {
                    var handler = collider.gameObject.GetComponent<DisconnectedSoundOrbHandler>();
                    memoryLayer = handler.MemoryLayer;
                    hovering = handler.isHovering;
                }
                else //SoundOrbConnected
                {
                    var handler = collider.gameObject.GetComponent<ConnectedSoundOrbHandler>();
                    memoryLayer = handler.MemoryLayer;
                    hovering = handler.isHovering;
                }
                
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
            
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, squareDimensions);
    }

}
