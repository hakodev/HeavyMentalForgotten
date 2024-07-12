using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointerHandler : MonoBehaviour
{
    [SerializeField] private GameObject mousePointer;
    [SerializeField] private bool showCursor = false;

    void Update()
    {
        if (!showCursor)
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }
        
       
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0;
        mousePointer.transform.position = mousePosition;
    }
}
