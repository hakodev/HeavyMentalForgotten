using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSystemList : MonoBehaviour
{
    [Header("Tools for easier Design")]
    public bool triggerOnStart;
    public bool triggerOnPlayerEnter;
    public bool triggerOnEvent;
    public bool triggerOnCustomEnter;
    public string customTagTrigger;
    public bool enableReset;


    [Header("System")]
    [SerializeField]
    private int currentEvent = -1;

    public DialoguesEvents[] events;
    [System.Serializable]
    public struct DialoguesEvents
    {
        public string name; //just for overview, no relevance in code
        public float enterDelay;
        public UnityEvent DialogueEvent;
        public float exitDelay;
    }

    private float timeSinceEnter=0;
    private float timeSinceExit=0;
    private bool entering = false;


    void Start()
    {
        entering = true;
        if (triggerOnStart == true) 
        {
            currentEvent = 0;
            Debug.Log("Triggered on Start");
        }
    }

    /*---------------------------------------------------------------------------------------------
                                                SYSTEM
    ---------------------------------------------------------------------------------------------*/
    void Update()
    {
        if (currentEvent < events.Length && currentEvent > -1) 
        {
            if (entering == true)
            {
                Debug.Log("Entering");
                timeSinceEnter += Time.deltaTime;
                if (timeSinceEnter >= events[currentEvent].enterDelay)
                {
                    Debug.Log("Entered after " + timeSinceEnter + " Seconds. Invoked event: "+ events[currentEvent].name + "\nCurrentEvent = " + currentEvent);
                    events[currentEvent].DialogueEvent.Invoke();
                    entering = false;
                    timeSinceEnter = 0;
                }
            }
            else
            {
                Debug.Log("Exiting");
                timeSinceExit += Time.deltaTime;
                if (timeSinceExit >= events[currentEvent].exitDelay)
                {
                    currentEvent++;
                    Debug.Log("Exited after " + timeSinceExit + " Seconds. \nCurrentEvent = " + currentEvent);
                    entering = true;
                    timeSinceExit = 0;

                }
            }
        } 
        else
        {
            Debug.Log("DialogueEventSystem inactive. CurrentEvent = " + currentEvent);
        }
    }


    /*---------------------------------------------------------------------------------------------
                                     Triggers for starting the system
    ---------------------------------------------------------------------------------------------*/


    public void StartSystem()                    //Trigger from any Event (or code)
    {
        if (triggerOnEvent == true && currentEvent == -1)
        {
            currentEvent = 0;
            Debug.Log("System triggered.");
        }
    }


    private void OnTriggerEnter2D (Collider2D other)
    {
        if (triggerOnPlayerEnter == true && currentEvent == -1)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                currentEvent = 0;
                Debug.Log("Triggered due to Player entering trigger");
            }
        }
        if (triggerOnCustomEnter == true && currentEvent == -1)
        {
            if (other.gameObject.CompareTag(customTagTrigger))
            {
                currentEvent = 0;
                Debug.Log("Triggered due to " + customTagTrigger + " entering trigger");
            }
        }
    }




    /*
    ----------------------------------------------------------------------------------------------------
    -----------------------------------------------IN DISCUSSION----------------------------------------
    ----------------------------------------------------------------------------------------------------
     */


    public void ResetSystem()                    //Reset from any Event / code
    {
        if (enableReset == true)
        {
            currentEvent = -1;
            Debug.Log("System resetted.");
        }
    }

    /*
    public void StopSystem()                    //Stop from any Event / code
    {  
        currentEvent = events.Length +1;
        Debug.Log("System stopped.");

    }
   */

}
