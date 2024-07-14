using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class TransitionImages : MonoBehaviour
{
    [SerializeField]
    private int currentImage;

    public TranstionImages[] Images;
    [System.Serializable]
    public struct TranstionImages
    {
        public string name; //just for overview, no relevance in code
        public SpriteRenderer ImageSprite;
        public float fadeInTime;
        public float timeBetweenFades;
        public float fadeOutTime;
        public UnityEvent TriggerEventAtImage;
        public float timeInTotal;
        [Header("Only after FadeOut started")]
        public float earlierTransitionInSec;
    }

    private float timeSinceLastImage;
    //[SerializeField]
    private bool fading;
    private bool startedCoroutine;

    [Header("Will load next Level A, but only \nafter list is finished and this waiting time")]
    [SerializeField]
    private float waitingTimeAfterFinishing;


    private void OnValidate()
    {
        for (int i = 0; i < Images.Length; i++)
        {
            Images[i].timeInTotal = Images[i].fadeInTime + Images[i].timeBetweenFades + Images[i].fadeOutTime;

            if (Images[i].earlierTransitionInSec < 0)
            {
                Images[i].earlierTransitionInSec = 0;
            }
            else if (Images[i].earlierTransitionInSec >= Images[i].fadeOutTime)
            {
                Images[i].earlierTransitionInSec = Images[i].fadeOutTime - 0.1f; //add this so that the fadeOutStarts before the next image is started
            }  
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Images.Length; i++)
        {
            Images[i].ImageSprite.DOFade(0f, 0f);
    
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentImage < Images.Length)
        {
            if (timeSinceLastImage == 0)
            {
                Images[currentImage].TriggerEventAtImage.Invoke();
            }

            float imageTotalTime = Images[currentImage].timeBetweenFades + Images[currentImage].fadeInTime + Images[currentImage].fadeOutTime;
            float timeToFadeIn = Images[currentImage].fadeInTime;
            float timeToFadeOut = Images[currentImage].timeBetweenFades + Images[currentImage].fadeInTime;

            timeSinceLastImage += Time.deltaTime;

            /*if (!startedCoroutine)
            {
                startedCoroutine = true;
                StartCoroutine(Fading(timeToFadeIn, imageTotalTime, timeToFadeOut, currentImage));

            }*/


            if (timeSinceLastImage > timeToFadeIn && timeSinceLastImage < timeToFadeOut)
            {
                fading = false;
            }
            else if (timeSinceLastImage >= timeToFadeOut)
            {
                Images[currentImage].ImageSprite.DOFade(0f, timeToFadeIn);
            }
            else if (timeSinceLastImage <= timeToFadeIn)
            {
                Images[currentImage].ImageSprite.DOFade(1f, timeToFadeIn);
            }

            
            float imageTotalTimeUntilNext = imageTotalTime - Images[currentImage].earlierTransitionInSec;
            if (timeSinceLastImage >= imageTotalTimeUntilNext)
            {
                timeSinceLastImage = 0;
                fading = false;
                currentImage++;
            }

        } 
        else
        {
            timeSinceLastImage += Time.deltaTime;
            if (timeSinceLastImage >= waitingTimeAfterFinishing)
            {
                GameManager.Ins.LoadNextLevel(GameManager.Ins.NextLevelLayerA);
            }
        }
    }
}

    // not used
    /*
    IEnumerator Fading(float pTimeToFadeIn, float pImageTotalTime,  float pTimeToFadeOut, int pIndex)
    {
        
        float pTimeSinceLastImage = 0;
        while (true)
        {
            pTimeSinceLastImage += Time.deltaTime;

            if (timeSinceLastImage > pTimeToFadeIn && timeSinceLastImage < pTimeToFadeOut)
            {
                fading = false;
            }
            else if (timeSinceLastImage >= pTimeToFadeOut)
            {
                Images[currentImage].ImageSprite.DOFade(0f, pTimeToFadeIn);
                fading = true;
            }
            else if (timeSinceLastImage <= pTimeToFadeIn)
            {
                Images[currentImage].ImageSprite.DOFade(1f, pTimeToFadeIn);
                fading = true;
            }
            if (timeSinceLastImage >= Images[currentImage].timeUntilNextImage)
            {
                break;
            }
        }

        currentImage++;
        timeSinceLastImage = 0;
        fading = false;
        startedCoroutine = false;

        yield return null;
    }
}
*/
