using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectedOrbBool : MonoBehaviour
{
    private DisconnectedSoundOrbHandler orbHandler;

    private void Awake()
    {
        orbHandler = GetComponent<DisconnectedSoundOrbHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // orbHandler.isHovering = true;
        // orbHandler.followMouse = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
