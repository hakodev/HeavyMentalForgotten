using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedSoundOrbHandler : MonoBehaviour
{
    [Header("Sound Orb Connected Stats")]
    [SerializeField] private AnimationCurve fadeCurve;
    [field: SerializeField] public MemoryLayers MemoryLayer { get; set; }
    [SerializeField] private Color connectedStartColor;
    [SerializeField] private Color connectedEndColor;
    public AudioClip connectedAudioClip;
    [SerializeField] private float ConnectedColorTransitionSpeed;
    [SerializeField] private float ConnectedVibrationIntensity;
    [SerializeField] private float vibrationSpeed;
    [SerializeField] private GameObject nonConnectedOrb;
    [SerializeField] private float delay;
    [SerializeField] private float fadeOutTime;
    private static GameObject[] connectedOrbs; 
    
    [Header("Bool Values")]
    private bool followMouse;
    private bool isHovering;
    public bool isOutsideCircle = false;
    
    [Header("Spawner")]
    public Collider2D[] colliders;
    private bool hasSpawned = false;
    private GameObject childObject;
    private GameObject nonConnectedOrbReference;
    private DisconnectedSoundOrbHandler disconnectOrbScript;
    private List<GameObject> lineRendererGameObject = new();
    
    [Header("References")]
    private SpriteRenderer spriteRenderer;
    
    [Header("Circle Behaviour Stats")]
    Vector2 circleCenter = new Vector2(0, 0);
    [SerializeField] private float circleRadius;
    private AudioSource audioSource;

    private void Awake()
    {
        // time = delay;
        spriteRenderer = GetComponent<SpriteRenderer>();
        connectedOrbs = GameObject.FindGameObjectsWithTag("SoundOrbConnected");
        childObject = this.gameObject.transform.GetChild(0).gameObject;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Circlecalculate();
        
        Connected();
        
        //Follow Mouse
        if (followMouse)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z;
            transform.position = mousePosition;
        }

        if(disconnectOrbScript != null)
        {
            if (disconnectOrbScript.isPlacedOnSnap)
            {
                List<GameObject> nonConnectedOrbsList = new List<GameObject>(connectedOrbs);

                nonConnectedOrbsList.Remove(this.gameObject);

                connectedOrbs = nonConnectedOrbsList.ToArray();
                
                foreach (var gameObject in lineRendererGameObject)
                {
                    Destroy(gameObject);
                }
                lineRendererGameObject.Clear();

                Destroy(this.gameObject);
            }
        }
        
        foreach (var collider in colliders)
        {
            if (collider is EdgeCollider2D)
            {
                if (!lineRendererGameObject.Contains(collider.gameObject))
                {
                    lineRendererGameObject.Add(collider.gameObject);
                }
            }
        }
    }

    private void LateUpdate()
    {
        Spawner();
    }

    private void OnMouseDown()
    {
        followMouse = true;

    }
    
    private void OnMouseUp()
    {
        followMouse = false;
    }

    private void OnMouseOver()
    {
        Debug.Log("Mouse is over orb");
        isHovering = true;
        
        foreach (GameObject orb in connectedOrbs)
        {
            AudioSource orbAudioSource = orb.GetComponent<AudioSource>();
            
            if (!isOutsideCircle)
            {
                StartCoroutine(Fadeinout(orb, connectedStartColor, connectedEndColor));

                ConnectedSoundOrbHandler orbHandler = orb.GetComponent<ConnectedSoundOrbHandler>();
                if (orbAudioSource != null && orbHandler != null)
                {
                    orbAudioSource.clip = orbHandler.connectedAudioClip;
                    if (!orbAudioSource.isPlaying)
                    {
                        orbAudioSource.Play();
                    }
                }
            }
        }
        
    }

    private void OnMouseExit()
    {
        isHovering = false;
        
        foreach (GameObject orb in connectedOrbs)
        {
            AudioSource orbAudioSource = orb.GetComponent<AudioSource>();
            if (orbAudioSource != null)
            {
                StartCoroutine(FadeOutVolume(orbAudioSource));
            }
        }
    }
    

    private void Connected()
    {
        for (int i = 0; i < connectedOrbs.Length; i++)
        {
            for (int j = i + 1; j < connectedOrbs.Length; j++)
            {
                Vector3 vibration = new Vector3(UnityEngine.Random.Range(-ConnectedVibrationIntensity, ConnectedVibrationIntensity),
                    UnityEngine.Random.Range(-ConnectedVibrationIntensity, ConnectedVibrationIntensity), 0) * vibrationSpeed * Time.deltaTime;

                connectedOrbs[i].transform.position += vibration;
                connectedOrbs[j].transform.position += vibration;
            }
        }
    }

    private IEnumerator Fadeinout(GameObject orb, Color firstColor, Color lastColor)
    {
        float curveEvaulate = 0;
        SpriteRenderer orbRenderer = orb.GetComponent<SpriteRenderer>();

        while (true)
        {
            if(!isHovering)
            {
                orbRenderer.color = firstColor;
                break;
            }
            
            float curveValue = fadeCurve.Evaluate(curveEvaulate);
            Color newColor = Color.Lerp(firstColor, lastColor, curveValue);
            orbRenderer.color = newColor;
            
            curveEvaulate += Time.deltaTime / ConnectedColorTransitionSpeed;
            if (curveEvaulate > 1)
            {
                curveEvaulate = 0;
            }

            yield return null;
        }
    }

    private IEnumerator EnableAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        if (!nonConnectedOrbReference.activeInHierarchy)
        {
            StopAllCoroutines();
            StartCoroutine(EnableAfterDelay());
        }
        
        spriteRenderer.enabled = true;
        childObject.SetActive(true);
    }

    private void Spawner()
    {
        if (nonConnectedOrbReference != null && !nonConnectedOrbReference.activeInHierarchy)
        {
            Debug.Log("coroutine running");
            StartCoroutine(EnableAfterDelay());
        }
        
        if (colliders.Length >= 2)
        {
            hasSpawned = false;
        }
        
        Vector2 center = this.gameObject.transform.position;
        float radius = 0.3f; //radius/diameter of the circle

        int layerToIgnore = 3;
        LayerMask layerMask = ~(1 << layerToIgnore); 
        colliders = Physics2D.OverlapCircleAll(center, radius, layerMask); 

        if (colliders.Length == 1 && !hasSpawned)
        {
            GameObject nonConnectedOrbSpawner = Instantiate(nonConnectedOrb, this.gameObject.transform.position, Quaternion.identity);
            nonConnectedOrbReference = nonConnectedOrbSpawner;
            disconnectOrbScript = nonConnectedOrbReference.GetComponent<DisconnectedSoundOrbHandler>();
            disconnectOrbScript.notConnectedAudio = connectedAudioClip;
            disconnectOrbScript.MemoryLayer = MemoryLayer;
            hasSpawned = true;
            // time = 4.1f;
            childObject.SetActive(false);
            StopAllCoroutines();
            spriteRenderer.enabled = false;
        }
    }
    
    private IEnumerator FadeOutVolume(AudioSource audioSource)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeOutTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
    
    private void Circlecalculate() 
    {
        // foreach (GameObject orb in connectedOrbs) 
        // {
            Vector2 orbPosition = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
            float distance = Vector2.Distance(circleCenter, orbPosition);
            ConnectedSoundOrbHandler connectedOrbReference = this.gameObject.GetComponent<ConnectedSoundOrbHandler>();

            if (distance > circleRadius) 
            {
                isOutsideCircle = true;
                
                if (isHovering)
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(connectedAudioClip);
                    }
                }
            }
            else
            {
                isOutsideCircle = false;
            }
        // }
    }

    void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(circleCenter, circleRadius);
    }
}
