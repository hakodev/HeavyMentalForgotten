using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ChargeHandler : MonoBehaviour
{
    private Vector3 mousePosition;
    private Vector3 worldPosition;
    private ParticleSystem lineCuttingParticleSystem;
    public float particleSystemDuration = 1f;
    public bool isDraggingReady = false;
    private TrailRenderer trailCutEffect;

    
    [Header("Charge Settings")] 
    [SerializeField] private float chargeThreshold = 4f;
    [SerializeField] private float maxLightIntensity = 4f;
    [SerializeField] private float chargeCircleRadius = 1f;
    public Collider2D[] lineRenderersColliders;
    
    private static float charge;
    public float chargeTester;
    private Light2D light2D;
    private AudioSource audioSource;

    private void Awake()
    {
        trailCutEffect = GameObject.Find("CuttingTrail").GetComponent<TrailRenderer>();
        trailCutEffect.enabled = false;

        GameObject mouseFollowerGameObject = GameObject.Find("MouseFollower");
        lineCuttingParticleSystem = GameObject.Find("LineCutEffect").GetComponent<ParticleSystem>();
        audioSource = mouseFollowerGameObject.GetComponent<AudioSource>();
        light2D = mouseFollowerGameObject.GetComponent<Light2D>();
        light2D.intensity = 0f;
        lineCuttingParticleSystem.Stop();

    }

    void Update()
    {
        trailCutEffect.enabled = true;
        mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        
        chargeTester = charge;
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

    private void OnDisable()
    {
        isDraggingReady = false;
    }

    private void OnEnable()
    {
        isDraggingReady = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(worldPosition, chargeCircleRadius);
    }
}
