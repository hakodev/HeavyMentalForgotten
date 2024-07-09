using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedOrbBool : MonoBehaviour
{
    [Header("References")]
    private ConnectedSoundOrbHandler connectedSoundOrbHandler;

    private void Awake()
    {
        connectedSoundOrbHandler = GetComponent<ConnectedSoundOrbHandler>();
    }

    void Start()
    {
        //This is how you call them, Please keep this script attached to the connected orb prefab
        
        // connectedSoundOrbHandler.isOutsideOfCircle = true;  //if true the orb is outside of the circle
        // connectedSoundOrbHandler.isInsideTheCircle = true; // if true the orb is inside the circle
        // connectedSoundOrbHandler.isHovering = false; // if true the mouse is hovering over the orb
        // connectedSoundOrbHandler.followMouse = true; // if true the orb follows the mouse AKA grabbed
    }

    void Update()
    {
        
    }
}
