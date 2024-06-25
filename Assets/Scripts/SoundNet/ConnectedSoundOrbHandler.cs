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
    public GameObject[] connectedOrbs;
    private bool followMouse;
    private bool isHovering;

    private void Awake()
    {
        connectedOrbs = GameObject.FindGameObjectsWithTag("SoundOrbConnected");

    }
    void Update()
    {
        Connected();
        
        //Follow Mouse
        if (followMouse)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z;
            transform.position = mousePosition;
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

}
