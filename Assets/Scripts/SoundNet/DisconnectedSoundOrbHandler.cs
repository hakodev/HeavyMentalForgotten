using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DisconnectedSoundOrbHandler : MonoBehaviour
{
    [SerializeField] private AnimationCurve fadeCurve;

    [Header("Not Connected")]
    [SerializeField] private float duration;
    [SerializeField] private float speedOfMovement = 5f;
    private float time;
    private Vector3 targetPosition;
    private SpriteRenderer spriteRenderer;
    public bool isPlacedOnSnap = false;

    [Header("Audio Source")]
    private AudioSource audioSource;
    public AudioClip notConnectedAudio;
    
    [Header("Hover Color Stats, Not Connected")]
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;
    public bool isHovering;
    private float hoverTime;
    [SerializeField] private float ConnectedColorTransitionSpeed;
    private bool followMouse;

    

    // The code is still roughly written and needs to be properly formed once approved
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        targetPosition = this.gameObject.transform.position;
    }

    private void Update()
    {
        
        
        //Reset fade effect timer
        if (followMouse)
        {
            time = 0f;
        }
        
        StartCoroutine(Notconnected());


        //Follow Mouse
        if (followMouse)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z;
            transform.position = mousePosition;
        }
    }

    private void OnMouseDown()
    {
        isHovering = true;
        followMouse = true;
        StartCoroutine(Notconnectedhover());
    }

    private void OnMouseOver()
    {
        if (this.gameObject.CompareTag("SoundOrbConnected"))
        {
            isHovering = true;
        }
        
        if(this.gameObject.CompareTag("SoundOrbDisconnected"))
        {
            isHovering = true;
            
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(notConnectedAudio);
            }
        }
    }

    private void OnMouseUp()
    {
        isHovering = false;
        followMouse = false;
    }
    
    private void OnMouseExit()
    {
        isHovering = false;
    }
    
    private IEnumerator Notconnected()
    {
        while (time < duration)
        {
            if (isPlacedOnSnap)
            {
                break;
            }
            
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
            
            time += Time.deltaTime / 50f;
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
            audioSource.PlayOneShot(notConnectedAudio);
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
