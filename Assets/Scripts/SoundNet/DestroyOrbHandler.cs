using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOrbHandler : MonoBehaviour
{
    private bool isDragging = false;
    private ParticleSystem lineCuttingParticleSystem;

    private void Awake()
    {
        lineCuttingParticleSystem = GameObject.Find("LineCutEffect").GetComponent<ParticleSystem>();
        lineCuttingParticleSystem.Stop();
    }

    private void Update()
    {
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
        }
    }
    
    private IEnumerator PlayParticleEffect()
    {
        lineCuttingParticleSystem.Play();
        
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        lineCuttingParticleSystem.transform.position = worldPosition;
        
        yield return new WaitForSeconds(1f);
        
        this.gameObject.SetActive(false);
        lineCuttingParticleSystem.Stop();
    }
}
