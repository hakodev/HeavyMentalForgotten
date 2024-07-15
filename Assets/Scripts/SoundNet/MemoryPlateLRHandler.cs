using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPlateLRHandler : MonoBehaviour
{
    [Header("Lines Settings")] 
    [SerializeField] private GameObject[] linePoints;
    private LineRenderer lineRenderer;
    private List <GameObject> memoryPlateFilledPlates;
    
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    
    private void Update()
    {
        memoryPlateFilledPlates = MemoryPlateHandler.filledPlates;
        LineRendererHandler();
        
    }
    
    private void LineRendererHandler()
    {
        if(memoryPlateFilledPlates.Count == 3)
        {
            lineRenderer.positionCount = 4;
            lineRenderer.SetPosition(0, memoryPlateFilledPlates[0].transform.position);
            lineRenderer.SetPosition(1, memoryPlateFilledPlates[1].transform.position);
            lineRenderer.SetPosition(2, memoryPlateFilledPlates[2].transform.position);
            lineRenderer.SetPosition(3, memoryPlateFilledPlates[0].transform.position);
        }
    }
}
