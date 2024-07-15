using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LighterActivation : MonoBehaviour
{
    [SerializeField]
    public bool onFire { get; private set; }
    
    // will be set to true if the mouse wheel is pressed, will be set to false if the lighter is properly activated or the inputDelay is 0 again.
    public bool isEmmittingSpark { get; private set; } 

    //-------------------------------------------------------

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


    //StoryRelevant
    [Header("STORY RELEVANT:")]
    [Header("Bools need to be false")]
    private bool wentOutside;
    [SerializeField]
    private bool burnedPaper;
    [Header("Put the system in here which is responsible for burning the paper etc")]
    public GameObject DialogueEventListForBurningPaper;

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
        if (Input.GetMouseButtonDown(2)                     //middle mouse button
            || Input.GetKeyDown(KeyCode.UpArrow))           //ArrowKeys can be used as well, just if there is no mouse (but pshh! - it's rather a secret)
        {
            Debug.Log("Mouse wheel pressed!");
            flameLight.intensity = maxLightIntensity;
            flameParticle.Play();
            audioSource.Play();
            inputTimeLeft = inputDelayThreshold;            //InputTimeLeft is set to a value of inputDelayThreshold. While there is still time remaining, the input of mousescroll still work together.
            isEmmittingSpark = true;
        }
        if ((Input.GetAxis("Mouse ScrollWheel") < -wheelThreshold || Input.GetAxis("Mouse ScrollWheel") > wheelThreshold) && inputTimeLeft > 0  // you can theoretically scroll in both directions - for player who don't realise or mix up the direction (happened to me), the feeling is more or less the same
            || Input.GetKeyDown(KeyCode.DownArrow))
        {
            isEmmittingSpark = false;
            onFire = true;
            Debug.Log("Mouse wheel scrolled down!");
            flameLight.intensity = maxLightIntensity;
        }
        else if (inputTimeLeft <= 0 && (onFire == false || !(Input.GetMouseButton(2)    //if any of these keys/buttons is pressed, the result will be false for all of them together
            || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))))
                {
            isEmmittingSpark = false;
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

        StoryEvents();
    }

    public void playerWentOutside()
    {
        wentOutside = true;
    }


    //Story related
    private void StoryEvents()
    {
        if (onFire && !wentOutside)
        {
            //light the area and free it from The Darkness !
        }
        else if (onFire && wentOutside && !burnedPaper)
        {
            //start the system which is responsible for the burning of paper. Needs to be allowed in the respective object component.
            DialogueEventListForBurningPaper.GetComponent<EventSystemList>().StartSystem(); 
            burnedPaper = true;
        }
    }
}
