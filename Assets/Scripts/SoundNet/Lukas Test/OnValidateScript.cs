using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnValidateScript : MonoBehaviour
{
    private EdgeCollider2D edgeCollider;
    public GameObjectPair[] gameObjectPair;
    private Dictionary<GameObjectPair, GameObject> lines = new Dictionary<GameObjectPair, GameObject>();
    private List<GameObject> allgameObjectsPair = new List<GameObject>();
    private GameObject[] connectedOrbs;
    [SerializeField] private GameObject nonConnectedOrb;
    [SerializeField] private GameObject lineContainer;

    private void OnValidate()
    {
        if (!Application.isEditor || Application.isPlaying)
        {
            return;
        }

        connectedOrbs = GameObject.FindGameObjectsWithTag("SoundOrbConnected");

        foreach (var pair in gameObjectPair)
        {
            allgameObjectsPair.Add(pair.Object1);
            allgameObjectsPair.Add(pair.Object2);
        }

        List<GameObject> orbsToRemove = new List<GameObject>();

        foreach (var orb in connectedOrbs)
        {
            if (!allgameObjectsPair.Contains(orb))
            {
                orbsToRemove.Add(orb);
            }
        }

        foreach (var orb in orbsToRemove)
        {
            var index = Array.IndexOf(connectedOrbs, orb);
            if (index != -1)
            {
                List<GameObject> tempOrbs = new List<GameObject>(connectedOrbs);
                tempOrbs.RemoveAt(index);
                connectedOrbs = tempOrbs.ToArray();
            }
        }

        foreach (var pair in gameObjectPair)
        {
            if (!lines.ContainsKey(pair) || lines[pair] == null)
            {
                GameObject lineObject = new GameObject("Line");
                lineObject.tag = "CanBeDestroyed";
                lineObject.transform.SetParent(transform);
                lineObject.AddComponent<LineRenderer>();
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

        Removegameobjectpairs();
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

        lineObject.transform.SetParent(lineContainer.transform);
    }

    private void Removegameobjectpairs()
    {
        List<GameObjectPair> pairsToRemove = new List<GameObjectPair>();
        List<GameObjectPair> tempPairs = new List<GameObjectPair>(gameObjectPair);

        foreach (var line in lines)
        {
            if (!Array.Exists(gameObjectPair, pair => pair.Equals(line.Key)))
            {
                if (line.Value != null)
                {
                    line.Value.SetActive(false);
                }
                pairsToRemove.Add(line.Key);
            }
        }

        foreach (var pair in pairsToRemove)
        {
            lines.Remove(pair);
        }
        foreach (var pair in gameObjectPair)
        {
            GameObject lineObject;
            if (!lines.TryGetValue(pair, out lineObject) || lineObject == null || !lineObject.activeInHierarchy)
            {
                pairsToRemove.Add(pair);
            }
            else
            {
                Drawline(pair.Object1, pair.Object2, lineObject);
            }
        }

        foreach (var pair in pairsToRemove)
        {
            allgameObjectsPair.Remove(pair.Object1);
            allgameObjectsPair.Remove(pair.Object2);
            tempPairs.Remove(pair);
        }

        gameObjectPair = tempPairs.ToArray();
    }
}