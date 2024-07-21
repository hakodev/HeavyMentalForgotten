using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoverOverDisconnectedOrbTriggers : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DisconnectedSoundOrbHandler disconnectedSoundOrbHandler;
    //public DisconnectedSoundOrbHandler disconnectedSoundOrbHandler;

    [SerializeField]
    private bool activatedDisconnectedHover;
    [SerializeField]
    private GameObject objectToActivate;

    //attached GameObject will setActive if hovering. If it is inactive, then it can be set active again.


    private void Awake()
    {
        disconnectedSoundOrbHandler = GetComponent<DisconnectedSoundOrbHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //disconnectedSoundOrbHandler = connectedSoundOrbHandler.nonConnectedOrbReference.gameObject.GetComponent<DisconnectedSoundOrbHandler>();

    }

    // Update is called once per frame
    void Update()
    {
        if (disconnectedSoundOrbHandler.isHovering && activatedDisconnectedHover == false)
        {
            objectToActivate.SetActive(true);
            activatedDisconnectedHover = true;
        }
        else if (objectToActivate.activeInHierarchy == false && activatedDisconnectedHover == true)
        {
            activatedDisconnectedHover = false;
        }
    }
}

