using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineCompletionLoadLevel : MonoBehaviour
{
    private PlayableDirector timeline;

    private void Awake()
    {
        timeline = GetComponent<PlayableDirector>();
    }
    

    // Update is called once per frame
    void Update()
    {
        if (timeline.state != PlayState.Playing)
        {
            GameManager.Ins.LoadNextLevel(GameManager.Ins.NextLevelLayerA);
        }
    }
}
