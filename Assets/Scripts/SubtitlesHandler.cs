using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubtitlesHandler : MonoBehaviour
{
    private float subtitleTime;
    private TextMeshProUGUI textMeshPro;
    
    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        textMeshPro.text = "";
    }
    

    public void PlaySubtitles(string subtitles)
    {
        StartCoroutine(DisplaySubtitles(subtitles));
    }
    

    public void SetSubTime(float setTime)
    {
        subtitleTime = setTime;
    }
    private IEnumerator DisplaySubtitles(string subtitles)
    {
        textMeshPro.enabled = true;
        textMeshPro.text = subtitles;
        yield return new WaitForSeconds(subtitleTime);
        textMeshPro.text = "";

    }
}
