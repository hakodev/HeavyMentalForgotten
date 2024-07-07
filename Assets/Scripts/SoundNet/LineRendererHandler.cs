using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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

public class LineRendererHandler : MonoBehaviour
{    
    private EdgeCollider2D edgeCollider;
    private LineRenderer lineRenderer;
    
    private Dictionary<GameObjectPair, GameObject> lines = new Dictionary<GameObjectPair, GameObject>();
    [SerializeField] private List<GameObjectPair> gameObjectPair = new List<GameObjectPair>();
    [SerializeField] private GameObject lineContainer;
    [SerializeField] private Material lRMaterial;
    
    private void Update()
    {
        for (int i = gameObjectPair.Count - 1; i >= 0; i--)
        {
            if (gameObjectPair[i].Object1 == null || gameObjectPair[i].Object2 == null)
            {
                gameObjectPair.RemoveAt(i);
            }
        }
        
        foreach (var pair in gameObjectPair)
        {
            if (!lines.ContainsKey(pair) || lines[pair] == null)
            {
                GameObject lineObject = new GameObject("Line");
                lineObject.tag = "CanBeDestroyed";
                lineObject.transform.SetParent(transform);
                lineRenderer = lineObject.AddComponent<LineRenderer>();
                lineRenderer.material = lRMaterial;
                edgeCollider = lineObject.AddComponent<EdgeCollider2D>();
                lineObject.AddComponent<DestroyOrbHandler>();
                Drawline(pair.Object1, pair.Object2, lineObject);
                edgeCollider.edgeRadius = 0.15f;

                lines[pair] = lineObject;
            }
            else
            {
                GameObject lineObject = lines[pair];
                Drawline(pair.Object1, pair.Object2, lineObject);
            }
        }
    }

    private void Drawline(GameObject obj1, GameObject obj2, GameObject lineObject)
    {
        LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();
        EdgeCollider2D edgeCollider = lineObject.GetComponent<EdgeCollider2D>();

        lineRenderer.startWidth = 0.20f;
        lineRenderer.endWidth = 0.20f;

        lineRenderer.SetPosition(0, obj1.transform.position);
        lineRenderer.SetPosition(1, obj2.transform.position);

        Vector2 localPos1 = lineObject.transform.InverseTransformPoint(obj1.transform.position);
        Vector2 localPos2 = lineObject.transform.InverseTransformPoint(obj2.transform.position);

        edgeCollider.points = new Vector2[] { localPos1, localPos2};

        // Set the z position of the EdgeCollider2D to 0.103
        Vector3 colliderPosition = edgeCollider.transform.position;
        colliderPosition.z = 0.103f;
        edgeCollider.transform.position = colliderPosition;

        lineObject.transform.SetParent(lineContainer.transform);
    }



}
