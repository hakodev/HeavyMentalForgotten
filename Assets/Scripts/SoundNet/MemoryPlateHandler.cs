using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPlateHandler : MonoBehaviour
{
    private GameObject snappedObject;
    [SerializeField] private float snapSpeed = 5f;
    private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (snappedObject != null)
        {
            snappedObject.transform.position = Vector3.Lerp(snappedObject.transform.position, transform.position, Time.deltaTime * snapSpeed);
            // snappedObject.GetComponent<CircleCollider2D>().enabled = false;
            snappedObject.GetComponent<DisconnectedSoundOrbHandler>().isPlacedOnSnap = true;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("SoundOrbDisconnected") && other.GetComponent<DisconnectedSoundOrbHandler>().isHovering)
        {
            snappedObject = other.gameObject;
            this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            spriteRenderer.material.color = Color.gray;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("SoundOrbDisconnected"))
        {
            //Change this to mouse position
            snappedObject = null;
            spriteRenderer.material.color = Color.black;
        }
    }
}
