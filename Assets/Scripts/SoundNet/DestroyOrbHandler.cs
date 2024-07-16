using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DestroyOrbHandler : MonoBehaviour
{
    [SerializeField] private float duration = 15f; //once duration is final change here since this script is not available in scene view
    private Vector3 mousePosition;
    private Vector3 worldPosition;
    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;
    private Gradient gradient;
    public bool hasPlayedOnce = false;


    
    private void Awake()
    {

        
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        edgeCollider = this.gameObject.GetComponent<EdgeCollider2D>();
    }

    private void Update()
    {
        
        mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        

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

    private void OnDisable()
    {
        NetRegeneratorHandler.instance.ActivateAfterDelay(this.gameObject);
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
        
        if(!lineRenderer.enabled || !edgeCollider.enabled)
        {
            Debug.Log("LineRenderer and EdgeCollider are disabled. Enabling them again.");
            lineRenderer.enabled = true;
            edgeCollider.enabled = true;
        }
    }
}
