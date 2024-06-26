using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOrbHandler : MonoBehaviour
{
    private bool isDragging = false;
    private ParticleSystem lineCuttingParticleSystem;
    private TrailRenderer trailCutEffect;
    private float particleSystemDuration = 1f;
    private Vector3 mousePosition;
    private Vector3 worldPosition;
    
    private void Awake()
    {
        lineCuttingParticleSystem = GameObject.Find("LineCutEffect").GetComponent<ParticleSystem>();
        trailCutEffect = GameObject.Find("CuttingTrail").GetComponent<TrailRenderer>();
        trailCutEffect.enabled = false;
        lineCuttingParticleSystem.Stop();
    }

    private void Update()
    {
        trailCutEffect.enabled = true;
        mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        
        if (isDragging)
        {
            trailCutEffect.transform.position = worldPosition;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;

        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    private void OnMouseOver()
    {
        if (isDragging)
        {
            StartCoroutine(PlayParticleEffect());
            this.gameObject.GetComponent<LineRenderer>().enabled = false; //TODO: find a better way 
            this.gameObject.GetComponent<EdgeCollider2D>().enabled = false;
        }
    }
    
    private IEnumerator PlayParticleEffect()
    {
        lineCuttingParticleSystem.Play();
        lineCuttingParticleSystem.transform.position = worldPosition;
        this.gameObject.SetActive(false);

        yield return new WaitForSeconds(particleSystemDuration);
        
        lineCuttingParticleSystem.Stop();
    }
}
