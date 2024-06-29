using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedSoundOrbHandler : MonoBehaviour
{
    [Header("Sound Orb Connected Stats")]
    [SerializeField] private AnimationCurve fadeCurve;
    [SerializeField] private Color connectedStartColor;
    [SerializeField] private Color connectedEndColor;
    public AudioClip connectedAudioClip;
    [SerializeField] private float ConnectedColorTransitionSpeed;
    [SerializeField] private float ConnectedVibrationIntensity;
    [SerializeField] private float vibrationSpeed;
    [SerializeField] private GameObject nonConnectedOrb;
    public GameObject[] connectedOrbs;
    [SerializeField] private float delay;
    private bool followMouse;
    private bool isHovering;
    private SpriteRenderer spriteRenderer;
    public Collider2D[] colliders;
    private bool hasSpawned = false;
    private float time; //must be same as delay in Net Regen
    public GameObject childObject;

    private void Awake()
    {
        time = delay;
        spriteRenderer = GetComponent<SpriteRenderer>();
        connectedOrbs = GameObject.FindGameObjectsWithTag("SoundOrbConnected");
        childObject = this.gameObject.transform.GetChild(0).gameObject;
    }

    void Update()
    {
        time -= Time.deltaTime;
        
        if (spriteRenderer != null && !spriteRenderer.enabled)
        {
            StartCoroutine(EnableAfterDelay());
        }
        
        Connected();
        
        //Follow Mouse
        if (followMouse)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z;
            transform.position = mousePosition;
        }
        
        if(time <= 0)
        {
            Spawner();
        }

    }
    private void OnMouseOver()
    {
        isHovering = true;
        
        foreach (GameObject orb in connectedOrbs)
        {
            StartCoroutine(Fadeinout(orb, connectedStartColor, connectedEndColor));

            AudioSource orbAudioSource = orb.GetComponent<AudioSource>();
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

    private void OnMouseExit()
    {
        isHovering = false;
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
        spriteRenderer.enabled = true;
        childObject.SetActive(true);
    }

    private void Spawner()
    {
        Debug.Log("Spawner called");
        
        if (colliders.Length >= 2)
        {
            hasSpawned = false;
        }
        
        Vector2 center = this.gameObject.transform.position;
        float radius = 0.3f; //radius/diameter of the circle

        colliders = Physics2D.OverlapCircleAll(center, radius);

        if (colliders.Length == 1 && !hasSpawned)
        {
            GameObject nonConnectedOrbSpawner = Instantiate(nonConnectedOrb, this.gameObject.transform.position, Quaternion.identity);
            nonConnectedOrbSpawner.GetComponent<DisconnectedSoundOrbHandler>().notConnectedAudio = connectedAudioClip;
            hasSpawned = true;
            time = 4.1f;
            StartCoroutine(EnableAfterDelay());
            childObject.SetActive(false);
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

    }
    
}
