using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GameObjectPair
{
    public GameObject Object1;
    public GameObject Object2;

    public GameObjectPair(GameObject object1, GameObject object2)
    {
        Object1 = object1;
        Object2 = object2;
    }
}
public class DrawLineRenderer : MonoBehaviour
{
    private EdgeCollider2D edgeCollider;
    [SerializeField] private GameObjectPair[] gameObjectPair;
    [SerializeField] private DestroyOnClick destroyOnClick;
    private Dictionary<GameObjectPair, GameObject> lines = new Dictionary<GameObjectPair, GameObject>();

    private void Update()
    {
        foreach (var pair in gameObjectPair)
        {
            if (!lines.ContainsKey(pair) || lines[pair] == null)
            {
                GameObject lineObject = new GameObject("Line");
                lineObject.tag = "CanBeDestroyed";
                lineObject.transform.SetParent(transform);
                lineObject.AddComponent<LineRenderer>();
                edgeCollider = lineObject.AddComponent<EdgeCollider2D>();
                lineObject.AddComponent<Rigidbody2D>();
                lineObject.AddComponent<DestroyOnClick>();
                DrawLine(pair.Object1, pair.Object2, lineObject);
                edgeCollider.edgeRadius = 0.15f;

                // Add the line to the dictionary
                lines[pair] = lineObject;
            }
            else
            {
                // If the line already exists, just update it
                GameObject lineObject = lines[pair];
                DrawLine(pair.Object1, pair.Object2, lineObject);
            }
        }
    }


    private void DrawLine(GameObject obj1, GameObject obj2, GameObject lineObject)
    {
        LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();
        EdgeCollider2D edgeCollider = lineObject.GetComponent<EdgeCollider2D>();

        lineRenderer.startWidth = 0.20f;
        lineRenderer.endWidth = 0.20f;

        lineRenderer.SetPosition(0, obj1.transform.position);
        lineRenderer.SetPosition(1, obj2.transform.position);

        Vector2 localPos1 = lineObject.transform.InverseTransformPoint(obj1.transform.position);
        Vector2 localPos2 = lineObject.transform.InverseTransformPoint(obj2.transform.position);

        edgeCollider.points = new Vector2[] { localPos1, localPos2 };

        // Calculate the distance between the two points
        float lineLength = Vector3.Distance(obj1.transform.position, obj2.transform.position);
    }
    
}
