using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LineRendererPair
{
    public GameObject memoryPlate;
    public GameObject Object1;
    public GameObject Object2;
}

public class MemoryPlateLRHandler : MonoBehaviour
{
    [Header("Lines Settings")] 
    [SerializeField] private LineRendererPair[] linePointsPairs;
    private LineRenderer lineRenderer;
    private List <GameObject> memoryPlateFilledPlates;
    [SerializeField] private int memoryPlateFillRequired;
    
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
        if(memoryPlateFilledPlates.Count == memoryPlateFillRequired)
        {

            if (memoryPlateFilledPlates.Count >= 3)
            {
                lineRenderer.positionCount = 5;
                lineRenderer.SetPosition(0, memoryPlateFilledPlates[0].transform.position);
                lineRenderer.SetPosition(1, memoryPlateFilledPlates[1].transform.position);
                lineRenderer.SetPosition(2, memoryPlateFilledPlates[2].transform.position);
                lineRenderer.SetPosition(3, memoryPlateFilledPlates[0].transform.position);
            }

            foreach (var pair in linePointsPairs)
            {
                LineRenderer lr = pair.Object1.GetComponent<LineRenderer>();

                int numPoints = 25;
                lr.positionCount = numPoints;

                Vector3 p0 = pair.memoryPlate.transform.position;
                Vector3 p2 = pair.Object2.transform.position;
                Vector3 p1 = pair.Object1.transform.position;

                for (int i = 0; i < numPoints; i++)
                {
                    float pointsCalculate = i / (float)(numPoints - 1);
                    lr.SetPosition(i, CalculateBezierPoint(pointsCalculate, p0, p1, p2));
                }
            }
        }
    }
    
    Vector3 CalculateBezierPoint(float points, Vector3 pointZero, Vector3 pointOne, Vector3 pointTwo)
    {
        float u = 1 - points;
        float tt = points * points;
        float uu = u * u;

        Vector3 bezierPoint = uu * pointZero + 2 * u * points * pointOne + tt * pointTwo;

        return bezierPoint;
    }
}
