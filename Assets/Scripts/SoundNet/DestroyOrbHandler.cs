using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DestroyOrbHandler : MonoBehaviour
{
    public bool isDraggingReady = false;
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

    [Header("Charge Settings")] 
    [SerializeField] private float chargeThreshold = 4f;
    [SerializeField] private float maxLightIntensity = 4f;
    [SerializeField] private float chargeCircleRadius = 1f;
    public Collider2D[] lineRenderersColliders;
    
    private static float charge;
    private Light2D light2D;
    private AudioSource audioSource;
    
    private void Awake()
    {
        GameObject mouseFollowerGameObject = GameObject.Find("MouseFollower");
        audioSource = mouseFollowerGameObject.GetComponent<AudioSource>();
        light2D = mouseFollowerGameObject.GetComponent<Light2D>();
        lineCuttingParticleSystem = GameObject.Find("LineCutEffect").GetComponent<ParticleSystem>();
        trailCutEffect = GameObject.Find("CuttingTrail").GetComponent<TrailRenderer>();
        
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        edgeCollider = this.gameObject.GetComponent<EdgeCollider2D>();
        trailCutEffect.enabled = false;
        lineCuttingParticleSystem.Stop();
        light2D.intensity = 0f;
    }

    private void Update()
    {
        trailCutEffect.enabled = true;
        mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        
        light2D.transform.position = worldPosition;
        
        if (isDraggingReady)
        {
            trailCutEffect.transform.position = worldPosition;
        }

        if (Input.GetMouseButtonUp(1) && charge >= chargeThreshold)
        {
            StartCoroutine(PlayParticleEffect());
            
            foreach (var collider in lineRenderersColliders)
            {
                if(lineRenderersColliders == null)
                {
                    Debug.Log("No colliders found");
                    return;
                }
                
                LineRenderer lineRenderer = collider.GetComponent<LineRenderer>();
                EdgeCollider2D edgeCollider = collider.GetComponent<EdgeCollider2D>();
                lineRenderer.gameObject.SetActive(false);
                if (lineRenderer != null)
                {
                    lineRenderer.enabled = false;
                }

                if (edgeCollider != null)
                {
                    edgeCollider.enabled = false;
                }
            }
        }
        
        if (Input.GetMouseButton(1))
        {
            Charger();
        }
        else
        {
            isDraggingReady = false;
            audioSource.Stop();
            light2D.intensity = 0f;
            charge = 0f;
        }
    }
    
    private IEnumerator PlayParticleEffect()
    {
        lineCuttingParticleSystem.Play();
        lineCuttingParticleSystem.transform.position = worldPosition;
        // this.gameObject.SetActive(false);
        // lineRenderer.enabled = false; //TODO: find a better way 
        // edgeCollider.enabled = false;
        yield return new WaitForSeconds(particleSystemDuration);
        lineCuttingParticleSystem.Stop();
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
    
    private void Charger()
    {
        charge += Time.deltaTime;
        light2D.intensity = Mathf.Clamp(charge / chargeThreshold, 0f, maxLightIntensity);
        
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        
        if (charge >= chargeThreshold)
        {
            isDraggingReady = true;
            int layerToIgnore1 = 3;
            int layerToIgnore2 = 7;
            LayerMask layerMask = ~((1 << layerToIgnore1) | (1 << layerToIgnore2));
            lineRenderersColliders = Physics2D.OverlapCircleAll(worldPosition, chargeCircleRadius, layerMask);
            //Stop the previous audio and play a new thing instead here
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(worldPosition, chargeCircleRadius);
    }

    private void OnDisable()
    {
        NetRegeneratorHandler.instance.ActivateAfterDelay(this.gameObject);
        isDraggingReady = false;
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
        isDraggingReady = false;
        
        if(!lineRenderer.enabled || !edgeCollider.enabled)
        {
            Debug.Log("LineRenderer and EdgeCollider are disabled. Enabling them again.");
            lineRenderer.enabled = true;
            edgeCollider.enabled = true;
        }
    }
}
