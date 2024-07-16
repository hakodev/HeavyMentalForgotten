using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoverOverConnectedOrbTriggers : MonoBehaviour
{

    [Header("References")]
    private ConnectedSoundOrbHandler connectedSoundOrbHandler;

    [SerializeField]
    private bool activated;
    [SerializeField]
    private GameObject objectToActivate;

    //attached GameObject will setActive if hovering. If it is inactive, then it can be set active again.


    private void Awake()
    {
        connectedSoundOrbHandler = GetComponent<ConnectedSoundOrbHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (connectedSoundOrbHandler.isHovering && activated == false)
        {
            objectToActivate.SetActive(true);
            activated = true;
        }
        else if (objectToActivate.activeInHierarchy == false && activated == true)
        {
            activated = false;
        }
    }
}
