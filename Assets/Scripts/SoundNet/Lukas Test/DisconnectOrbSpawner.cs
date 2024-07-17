using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectOrbSpawner : MonoBehaviour
{
    [SerializeField] public GameObject orbPrefab;
    [SerializeField] public bool shouldSpawnOrb = false;

    // Update is called once per frame
    void Update()
    {
        if (shouldSpawnOrb)
        {
            Instantiate(orbPrefab, transform.position, Quaternion.identity);
            shouldSpawnOrb = false;
        }
    }
}
