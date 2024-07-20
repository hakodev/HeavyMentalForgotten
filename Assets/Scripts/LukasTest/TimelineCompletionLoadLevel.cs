using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineCompletionLoadLevel : MonoBehaviour
{
    private PlayableDirector timeline;

    private float elapsedTime;
    [SerializeField] private float minimalTimeBeforeProgression;

    private void Awake()
    {
        timeline = GetComponent<PlayableDirector>();
    }

    private void Start()
    {
        if (timeline.state != PlayState.Playing)
        {
            Debug.Log("Timeline is not playing. Playing now.");
            timeline.Play();
        }
    }


    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (timeline.state != PlayState.Playing && elapsedTime >= minimalTimeBeforeProgression)
        {
            GameManager.Ins.LoadNextLevel(GameManager.Ins.NextLevelLayerA);
        }
    }
}
