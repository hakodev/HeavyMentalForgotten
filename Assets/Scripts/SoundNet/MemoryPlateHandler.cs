using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPlateHandler : MonoBehaviour
{
    private GameObject snappedObject;
    [SerializeField] private float snapSpeed = 5f;
    private SpriteRenderer spriteRenderer;
    private float time = 1.5f;
    private bool isSnapped = false;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (snappedObject != null)
        {
            snappedObject.transform.position = Vector3.Lerp(snappedObject.transform.position, transform.position, Time.deltaTime * snapSpeed);
            time -= Time.deltaTime;
            
            if(time <= 0f)
            {
                Debug.Log("Snapped");
                snappedObject.GetComponent<DisconnectedSoundOrbHandler>().enabled = false;
                snappedObject.GetComponent<CircleCollider2D>().enabled = false;
                this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
                isSnapped = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isSnapped)
        {
            return;
        }
        
        DisconnectedSoundOrbHandler disconnectedSoundOrbHandler = other.GetComponent<DisconnectedSoundOrbHandler>();

        if (other.gameObject.CompareTag("SoundOrbDisconnected") && disconnectedSoundOrbHandler.isHovering)
        {
            snappedObject = other.gameObject;
            spriteRenderer.material.color = Color.gray;
            disconnectedSoundOrbHandler.isPlacedOnSnap = true;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (isSnapped)
        {
            return;
        }
        
        DisconnectedSoundOrbHandler disconnectedSoundOrbHandler = other.GetComponent<DisconnectedSoundOrbHandler>();
        if (other.gameObject.CompareTag("SoundOrbDisconnected"))
        {
            time = 3f;
            Debug.Log("Exited");
            snappedObject = null;
            disconnectedSoundOrbHandler.isPlacedOnSnap = false;
            spriteRenderer.material.color = Color.black;
        }
    }
}
