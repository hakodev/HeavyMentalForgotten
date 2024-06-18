using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundNet : MonoBehaviour
{
    [SerializeField] private AnimationCurve fadeCurve;

    [Header("Not Connected")]
    [SerializeField] private float duration;
    [SerializeField] private float speedOfMovement = 5f;
    private float time;
    private Vector3 targetPosition;
    private SpriteRenderer spriteRenderer;

    [Header("Audio Source")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    
    [Header("Hover Color Stats, Not Connected")]
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;
    private bool isHovering;
    private float hoverTime;
    
    [Header("Sound Orb Connected Stats")]
    [SerializeField] private Color connectedStartColor;
    [SerializeField] private Color connectedEndColor;
    [SerializeField] private AudioClip connectedAudioClip;
    [SerializeField] private float ConnectedColorTransitionSpeed;
    [SerializeField] private float ConnectedVibrationIntensity; //Keep it low, its very sensitive
    [SerializeField] private float vibrationSpeed;
    [SerializeField] private List<Vector3> lineRendererPositions = new List<Vector3>(); // WIP for line renderer removal
    
    private GameObject[] connectedOrbs;
    private bool followMouse;
    [SerializeField] private LineRenderer lineRenderer;
    
    // The code is still roughly written and needs to be properly formed once approved
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        targetPosition = this.gameObject.transform.position;
    }

    private void Start()
    {
        connectedOrbs = GameObject.FindGameObjectsWithTag("SoundOrbConnected");
        
        lineRenderer.positionCount = connectedOrbs.Length; 
        lineRenderer.SetPosition(0, transform.position);

        for (int i = 0; i < connectedOrbs.Length; i++)
        {
            lineRenderer.SetPosition(i, connectedOrbs[i].transform.position); 
        }
    }

    private void Update()
    {
        if (followMouse)
        {
            time = 0f;
        }
        
        //Saving all the positions of line renderer to a list
        lineRendererPositions.Clear();
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRendererPositions.Add(lineRenderer.GetPosition(i));
        }
        
        if (this.gameObject.CompareTag("SoundOrbConnected"))
        {
            Connected();
        }
        
        if (this.gameObject.CompareTag("SoundOrbDisconnected") && !isHovering)
        {
            StartCoroutine(Notconnected());
        }
        
        if (followMouse)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z;
            transform.position = mousePosition;
        }
    }

    private void OnMouseOver()
    {
        if (this.gameObject.CompareTag("SoundOrbDisconnected"))
        {
            isHovering = true;
            followMouse = true;
            StartCoroutine(Notconnectedhover());
        }
        else if (this.gameObject.CompareTag("SoundOrbConnected"))
        {
            isHovering = true;
            
            foreach (GameObject orb in connectedOrbs)
            {
                StartCoroutine(Fadeinout(orb, connectedStartColor, connectedEndColor));
                
                if (audioSource != null)
                {
                    Debug.Log("AudioSource on orb " + orb.name + " is playing."); 
                    audioSource.PlayOneShot(connectedAudioClip);
                }
            }
        }
    }

    private void OnMouseExit()
    {
        isHovering = false;
        followMouse = false;
    }

    private IEnumerator Notconnected()
    {
        while (time < duration)
        {
            if (!followMouse)
            {
                //Fade Effect - Just fading out
                float alpha = Mathf.Lerp(1, 0, time / duration);
                Color newColor = spriteRenderer.color;
                newColor.a = alpha;
                spriteRenderer.color = newColor;
            }
            else
            {
                Color newColor = spriteRenderer.color;
                newColor.a = 1;
                spriteRenderer.color = newColor;
            }
            
            //Moving
            if (this.gameObject.transform.position == targetPosition)
            {
                float randomRadius = UnityEngine.Random.Range(1f, 5f);
                Vector2 randomPosition = UnityEngine.Random.insideUnitCircle * randomRadius;
                targetPosition = this.gameObject.transform.position + new Vector3(randomPosition.x, randomPosition.y, 0);
            }

            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, targetPosition, 
                speedOfMovement * Time.deltaTime);
            
            time += Time.deltaTime / 60f;
            yield return null;
        }
        
        if (time >= duration)
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator Notconnectedhover()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(audioClip);
        }
        
        StartCoroutine(Fadeinout(this.gameObject, startColor, endColor));
        
        while (true)
        {
            if(!isHovering)
            {
                spriteRenderer.color = startColor;
                break;
            }
            yield return null;
        }
    }

    private void Connected()
    {
        lineRenderer.SetPosition(0, transform.position);
        
        for (int i = 0; i < connectedOrbs.Length; i++)
        {
            Vector3 vibration = new Vector3(UnityEngine.Random.Range(-ConnectedVibrationIntensity, ConnectedVibrationIntensity), 
                UnityEngine.Random.Range(-ConnectedVibrationIntensity, ConnectedVibrationIntensity), 0) * vibrationSpeed * Time.deltaTime;
            
            connectedOrbs[i].transform.position += vibration;
            lineRenderer.SetPosition(i, connectedOrbs[i].transform.position);
        }
    }
    
    private IEnumerator Fadeinout(GameObject orb, Color firstColor, Color lastColor)
    {
        float curveEvaulate = 0;
        SpriteRenderer orbRenderer = orb.GetComponent<SpriteRenderer>();

        while (true)
        {
            float curveValue = fadeCurve.Evaluate(curveEvaulate);
            Color newColor = Color.Lerp(firstColor, lastColor, curveValue);
            orbRenderer.color = newColor;

            if(!isHovering)
            {
                orbRenderer.color = firstColor;
                break;
            }

            curveEvaulate += Time.deltaTime / ConnectedColorTransitionSpeed;
            if (curveEvaulate > 1)
            {
                curveEvaulate = 0;
            }

            yield return null;
        }
    }
}
