using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TextFade : MonoBehaviour
{
    [SerializeField]
    private float TextDurationUntilFade;
    [SerializeField]
    private float fadeOutDuration;
    [SerializeField]
    private float fadeInDuration;

    private TMP_Text TMP;
    private float elapsedTime;
    private bool fadingOut;

    private void Awake()
    {
        TMP = gameObject.GetComponent<TMP_Text>();
    }
    
    // Start is called before the first frame update
    void OnEnable()
    {
        TMP.DOFade(0f, 0f);
        elapsedTime = 0;
        //fadingOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsedTime == 0)
        {
            TMP.DOFade(1f, fadeInDuration);
        }

        elapsedTime += Time.deltaTime;

        if (elapsedTime > TextDurationUntilFade) // && fadingOut == false
        {
            TMP.DOFade(0f, fadeOutDuration).OnComplete(() => gameObject.SetActive(false));
            //fadingOut = true;
        }
        /*if (elapsedTime > TextDurationUntilFade + fadeOutDuration)
        {
            gameObject.SetActive(false);
        }*/

    }
}
