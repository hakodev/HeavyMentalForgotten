using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPlateHandler : MonoBehaviour
{
    private GameObject snappedObject;
    [SerializeField] private float snapSpeed = 5f;

    private void Update()
    {
        if (snappedObject != null)
        {
            snappedObject.transform.position = Vector3.Lerp(snappedObject.transform.position, transform.position, Time.deltaTime * snapSpeed);
            snappedObject.GetComponent<CircleCollider2D>().enabled = false;
            snappedObject.GetComponent<DisconnectedSoundOrbHandler>().isPlacedOnSnap = true;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("SoundOrbDisconnected") && other.GetComponent<DisconnectedSoundOrbHandler>().isHovering)
        {
            snappedObject = other.gameObject;
            this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
    }
    
}
