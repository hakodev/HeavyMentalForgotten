using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LighterActivation : MonoBehaviour
{
    [SerializeField]
    private bool onFire;

    public float inputDelayThreshold;
    private float inputTimeLeft;

    public float wheelThreshold; //between 0 and 1

    private float maxLightIntensity;
    public float lightIntensityDivider; //more than 1, smaller values will create weird behaviour or problems

    public ParticleSystem flameParticle;
    public Light2D flameLight;

    private AudioSource audioSource;
    //public AudioClip flameAudioClip;
    //public AudioClip clickAudioClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        onFire = false;
        flameParticle.Stop();
        maxLightIntensity = flameLight.intensity;

        if (lightIntensityDivider <= 1)
        {
            Debug.LogWarning("lightIntensityDivider is " + lightIntensityDivider + " - It should be more than 1.");
            lightIntensityDivider = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inputTimeLeft > 0)
        {
            inputTimeLeft -= Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(2)) //middle mouse button
        {
            Debug.Log("Mouse wheel pressed!");
            flameLight.intensity = maxLightIntensity;
            flameParticle.Play();
            audioSource.Play();

            if (Input.GetAxis("Mouse ScrollWheel") < -wheelThreshold)
            {
                onFire = true;
                Debug.Log("Mouse wheel scrolled down!");
                inputTimeLeft = inputDelayThreshold;
                flameLight.intensity = maxLightIntensity;
            }
        }      
        else if ((!Input.GetMouseButton(2) || onFire == false) && inputTimeLeft <= 0)
                {
            onFire = false;
        }

        if (onFire)
        {
            if (!flameParticle.isPlaying)
            { 
                flameParticle.Play();
                flameParticle.loop = true;
            } 
        } 
        else
        {
            flameLight.intensity = flameLight.intensity / lightIntensityDivider;
            flameParticle.loop = false;
            flameParticle.Stop();
        }


    }
}
