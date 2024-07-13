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
    [SerializeField] private float particleSysFollowSpeed;
    
    [Header("Bool Values")]
    public bool followMouse;
    public bool isHovering;
    public bool isOutsideCircle = false;
    
    [Header("Spawner")]
    public Collider2D[] colliders;
    private bool hasSpawned = false;
    private GameObject childObject;
    private GameObject nonConnectedOrbReference;
    private DisconnectedSoundOrbHandler disconnectOrbScript;
    private List<GameObject> lineRendererGameObject = new();
    
    [Header("References")]
    [SerializeField] private ParticleSystem particleSystemm;
    [SerializeField] private ParticleSystem mouseParticleSystem;
    [SerializeField] private GameObject mouseFollower;
    private SpriteRenderer spriteRenderer;
    public List<LineRenderer> lineRenderers = new List<LineRenderer>();
    private AudioSource audioSource;

    [Header("Circle Behaviour Stats")]
    [SerializeField] private Color outsideHoverColor;
    [SerializeField] private AnimationCurve mouseSensivityCurve;
    [SerializeField] private float xCircleRadius;
    [SerializeField] private float yCircleRadius;
    [SerializeField] private float circleRadius;
    [SerializeField] private float vibrationAdd;
    private Vector3 lastMousePosition;
    private float mouseSensivity;
    private Vector2 circleCenter;
    private Coroutine increaseMousesen;
    private Vector3 lengthOfLR;
    public bool isOutsideOfCircle = false;
    public bool isInsideTheCircle = true;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        connectedOrbs = GameObject.FindGameObjectsWithTag("SoundOrbConnected");
        childObject = this.gameObject.transform.GetChild(0).gameObject;
        audioSource = GetComponent<AudioSource>();
        particleSystemm = this.gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        mouseParticleSystem = this.gameObject.transform.GetChild(2).GetComponent<ParticleSystem>();
        
        particleSystemm.gameObject.SetActive(false);
        mouseParticleSystem.gameObject.SetActive(false);
    }

    void Update()
    {
        circleCenter = new Vector2(xCircleRadius, yCircleRadius);
        
        Circlecalculate();
        
        Connected();

        if (isOutsideOfCircle && isHovering)
        {
            spriteRenderer.material.color = outsideHoverColor;
        }
        else
        {
            spriteRenderer.material.color = connectedStartColor;
        }
        
        //Follow Mouse
        if (followMouse)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z;
            
            Vector3 direction = (mousePosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            particleSystemm.transform.rotation = Quaternion.Slerp(particleSystemm.transform.rotation, targetRotation, particleSysFollowSpeed);
            particleSystemm.gameObject.SetActive(true);
            
            mouseParticleSystem.transform.position = mouseFollower.transform.position;
            Vector3 mouseParticle = (transform.position - mousePosition).normalized;
            Quaternion mouseRotation = Quaternion.LookRotation(mouseParticle);
            mouseParticleSystem.transform.rotation = mouseRotation;
            mouseParticleSystem.gameObject.SetActive(true);
            
            Vector3 screenBottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, transform.position.z));
            Vector3 screenTopRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, transform.position.z));

            float buffer = 1f;
            screenBottomLeft += new Vector3(buffer, buffer, 0);
            screenTopRight -= new Vector3(buffer, buffer, 0);

            mousePosition.x = Mathf.Clamp(mousePosition.x, screenBottomLeft.x, screenTopRight.x);
            mousePosition.y = Mathf.Clamp(mousePosition.y, screenBottomLeft.y, screenTopRight.y);

            
            if (mousePosition != lastMousePosition)
            {
                transform.position = Vector3.Lerp(transform.position, mousePosition, mouseSensivity * Time.deltaTime);
            }

            lastMousePosition = mousePosition;
        }
        else
        {
            particleSystemm.gameObject.SetActive(false);
            mouseParticleSystem.gameObject.SetActive(false);
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

        // Linerendererstrecher();
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
        
        foreach (GameObject orb in connectedOrbs)
        {
            AudioSource orbAudioSource = orb.GetComponent<AudioSource>();
            if (orbAudioSource != null)
            {
                audioSource.volume = 1f;
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
        Vector3 vibration = new Vector3(UnityEngine.Random.Range(-ConnectedVibrationIntensity, ConnectedVibrationIntensity),
            UnityEngine.Random.Range(-ConnectedVibrationIntensity, ConnectedVibrationIntensity), 0) * vibrationSpeed * Time.deltaTime;

        this.gameObject.transform.position += vibration;
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
        
        if(!spriteRenderer.enabled || !childObject.activeInHierarchy)
        {
            Debug.Log("Enabling sprite renderer and child object");
            spriteRenderer.enabled = true;
            childObject.SetActive(true);
        }
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
        float radius = 0.5f; //radius/diameter of the circle

        int layerToIgnore = 3;
        LayerMask layerMask = ~(1 << layerToIgnore); 
        colliders = Physics2D.OverlapCircleAll(center, radius, layerMask);

        if (spriteRenderer.enabled)
        {
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
    }
    
    private IEnumerator FadeOutVolume(AudioSource audioSource)
    {
        float startVolume = 1f;
        Vector3 lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        while (audioSource.volume > 0)
        {
            Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float distanceToCurrentFrame = Vector3.Distance(currentMousePosition, transform.position);

            if (distanceToCurrentFrame > 8f) 
            {
                audioSource.volume -= startVolume * Time.deltaTime / fadeOutTime;
            }
            else if (distanceToCurrentFrame <= 1f)
            {
                audioSource.volume = 1;
            }
            else
            {
                audioSource.volume += startVolume * Time.deltaTime / fadeOutTime;
            }

            lastMousePosition = currentMousePosition;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
    
    private void Circlecalculate() 
    {
        
        Vector2 orbPosition = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        float distance = Vector2.Distance(circleCenter, orbPosition);
        
        float normalizedDistance = distance / circleRadius;

        mouseSensivity = mouseSensivityCurve.Evaluate(normalizedDistance);

        
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

            if (!isOutsideOfCircle)
            {
                ConnectedVibrationIntensity += vibrationAdd;
                isOutsideOfCircle = true;
                isInsideTheCircle = false;
            }

        }
        else if(!isInsideTheCircle)
        {
            isOutsideCircle = false;

            ConnectedVibrationIntensity -= vibrationAdd;

            isInsideTheCircle = true;
            isOutsideOfCircle = false;
  
        }
    }
    
    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(circleCenter, circleRadius);
    }
    
}
