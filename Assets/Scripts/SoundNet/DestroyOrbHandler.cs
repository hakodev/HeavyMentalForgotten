using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOrbHandler : MonoBehaviour
{
    private bool isDragging = false;
    private ParticleSystem lineCuttingParticleSystem;
    private TrailRenderer trailCutEffect;
    public float particleSystemDuration = 1f;
    [SerializeField] private float duration = 15f; //once duration is final change here since this script is not available in scene view
    private Vector3 mousePosition;
    private Vector3 worldPosition;
    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;
    private Gradient gradient;
    public bool hasPlayedOnce = false;
    
    private void Awake()
    {
        lineCuttingParticleSystem = GameObject.Find("LineCutEffect").GetComponent<ParticleSystem>();
        trailCutEffect = GameObject.Find("CuttingTrail").GetComponent<TrailRenderer>();
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        edgeCollider = this.gameObject.GetComponent<EdgeCollider2D>();
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
            lineRenderer.enabled = false; //TODO: find a better way 
            edgeCollider.enabled = false;
        }
    }
    
    private IEnumerator PlayParticleEffect()
    {
        lineCuttingParticleSystem.Play();
        lineCuttingParticleSystem.transform.position = worldPosition;
        this.gameObject.SetActive(false);
        lineRenderer.enabled = false; //TODO: find a better way 
        edgeCollider.enabled = false;
        yield return new WaitForSeconds(particleSystemDuration);
        lineCuttingParticleSystem.Stop();
    }

    private void OnDisable()
    {
        NetRegeneratorHandler.instance.ActivateAfterDelay(this.gameObject);
        isDragging = false;
    }
    
    private IEnumerator Fadein(float duration)
    {
        Gradient gradient = lineRenderer.colorGradient;
        GradientAlphaKey[] alphaKeys = gradient.alphaKeys;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float newAlpha = t / duration;

            for (int i = 0; i < alphaKeys.Length; i++)
            {
                alphaKeys[i].alpha = newAlpha;
            }

            gradient.alphaKeys = alphaKeys;

            lineRenderer.colorGradient = gradient;

            yield return null;
        }

        for (int i = 0; i < alphaKeys.Length; i++)
        {
            alphaKeys[i].alpha = 1f;
        }
        gradient.alphaKeys = alphaKeys;
        lineRenderer.colorGradient = gradient;
    }

    private void OnEnable()
    {
        if(hasPlayedOnce)
        {
            StartCoroutine(Fadein(duration));
        }
        hasPlayedOnce = true;
        lineRenderer.enabled = true;
        edgeCollider.enabled = true;
        isDragging = false;
    }
}
